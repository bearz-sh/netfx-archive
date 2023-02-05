using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ze.PowerShell.Module;

public static class PSModuleManifest
{
    public static void UpdateManifestFromAssembly(Assembly assembly, Dictionary<string, object> manifest)
    {
        var moduleName = assembly.GetName().Name;

        var cmdlets = assembly.ExportedTypes.Where(o => o.IsAssignableFrom(typeof(PSCmdlet)) && o.IsPublic)
            .ToList();
        if (cmdlets.Any())
        {
            var aliases = new List<string>();
            var cmdletNames = new List<string>();
            var assemblyVersion = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
            if (assemblyVersion is not null && !manifest.ContainsKey(PSManifestKeys.ModuleVersion) && Version.TryParse(assemblyVersion.Version, out var version))
            {
                manifest.Add(PSManifestKeys.ModuleVersion, version.ToString());
            }

            var assemblyDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            if (assemblyDescription is not null && !manifest.ContainsKey(PSManifestKeys.Description))
            {
                manifest[PSManifestKeys.Description] = assemblyDescription.Description;
            }

            foreach (var cmdlet in cmdlets)
            {
                var attr = cmdlet.GetCustomAttribute<CmdletAttribute>();
                if (attr is not null)
                {
                    cmdletNames.Add($"{attr.VerbName}-{attr.NounName}");
                }

                var aliasAttr = cmdlet.GetCustomAttribute<AliasAttribute>();
                if (aliasAttr is not null)
                {
                    aliases.AddRange(aliasAttr.AliasNames);
                }
            }

            var assemblyGuid = assembly.GetCustomAttribute<GuidAttribute>();
            if (assemblyGuid is not null)
            {
                manifest[PSManifestKeys.Guid] = assemblyGuid.Value;
            }

            manifest[PSManifestKeys.RootModule] = $"{moduleName}.dll";
            manifest[PSManifestKeys.FunctionsToExport] = cmdletNames.ToArray();
            manifest[PSManifestKeys.AliasesToExport] = aliases.ToArray();
        }
    }
}