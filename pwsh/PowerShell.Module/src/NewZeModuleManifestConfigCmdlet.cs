/*

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Bearz.Extra.Strings;
using Bearz.Std;
using Bearz.Text;

using Humanizer;

using static System.IO.Path;

namespace Ze.PowerShell.Module;


public class NewZeModuleManifestConfigCmdlet : PSCmdlet
{
    /// <summary>
    /// <para type="description">The destination path for the manifest cmdlet.</para>
    /// </summary>
    [Parameter(Position = 0)]
    public string? Path { get; set; }

    public string[]? ExcludedDirectories { get; set; }

    public string[]? ExcludedFiles { get; set; }

    public string? RootModule { get; set; }

    public string? ModuleVersion { get; set; }

    public Guid? Guid { get; set; }

    public string? Author { get; set; }

    public string? CompanyName { get; set; }

    public string? Copyright { get; set; }

    public string? Description { get; set; }

    public string? PowerShellVersion { get; set; }

    public string? PowerShellHostName { get; set; }

    public string? PowerShellHostVersion { get; set; }

    public string? DotNetFrameworkVersion { get; set; }

    public string? ClrVersion { get; set; }

    public string? ProcessArchitecture { get; set; }

    public object[]? RequiredModules { get; set; }

    public Hashtable? RequiredAssemblies { get; set; }

    public string[]? ScriptsToProcess { get; set; }

    public string[]? TypesToProcess { get; set; }

    public string[]? FormatsToProcess { get; set; }

    public string[]? NestedModules { get; set; }

    public string[]? FunctionsToExport { get; set; }

    public string[]? CmdletsToExport { get; set; }

    public string[]? VariablesToExport { get; set; }

    public string[]? AliasesToExport { get; set; }

    public string[]? DscResourcesToExport { get; set; }

    public object[]? ModuleList { get; set; }

    public string[]? FileList { get; set; }

    public object? PrivateData { get; set; }

    public string[]? Tags { get; set; }

    public string? LicenseUri { get; set; }

    public string? ProjectUri { get; set; }

    public string? IconUri { get; set; }

    public string? ReleaseNotes { get; set; }

    public string? PreRelease { get; set; }

    public bool RequireLicenseAcceptance { get; set; }

    public string[]? ExternalModuleDependencies { get; set; }

    public string? HelpInfoUri { get; set; }

    public string? DefaultCommandPrefix { get; set; }

    protected override void ProcessRecord()
    {
        var dictionary = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        var destination = this.Path;
        if (destination.IsNullOrWhiteSpace())
        {
            destination = this.SessionState.Path.CurrentLocation.Path;
        }

        if (destination!.EndsWith(".json"))
        {
            destination = System.IO.Path.GetDirectoryName(destination)!;
        }

        var csproj = Directory.EnumerateFiles(destination, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
        var psm1 = Directory.EnumerateFiles(destination, "*.psm1", SearchOption.TopDirectoryOnly).FirstOrDefault();
        var psd1 = Directory.EnumerateFiles(destination, "*.psd1", SearchOption.TopDirectoryOnly).FirstOrDefault();
        string? moduleName = null;

        if (csproj is not null)
        {
            moduleName = GetFileNameWithoutExtension(csproj);
            var xml = XDocument.Load(csproj);
            string? buildPropsFile = null;
            var target = destination;
            while (!target.IsNullOrWhiteSpace())
            {
                var file = Combine(target, "Directory.Build.props");
                if (File.Exists(file))
                {
                    buildPropsFile = file;
                    break;
                }

                target = GetDirectoryName(target);
            }

            var properties = new Dictionary<string, string?>();

            if (buildPropsFile is not null)
            {
                var buildProps = XDocument.Load(buildPropsFile);
                var propertyGroups = buildProps.Descendants("PropertyGroup").ToList();
                foreach (var propertyGroup in propertyGroups)
                {
                    foreach (var property in propertyGroup.Elements())
                    {
                        if (property.Value.Contains("$("))
                        {
                            Regex.Replace(property.Value, "$(.*)", m =>
                            {
                                if (m.Value.IsNullOrWhiteSpace())
                                    return string.Empty;

                                if (properties.TryGetValue(m.Value, out var value))
                                    return value;

                                return m.Value;
                            });
                        }

                    0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000    properties[property.Name.LocalName] = property.Value;
                    }
                }
            }
        } else if (psm1 is not null)
        {
            moduleName = GetFileNameWithoutExtension(psm1);
        } else if (psd1 is not null)
        {
            moduleName = GetFileNameWithoutExtension(psd1);
        }

        var moduleName = System.IO.Path.GetFileName(destination);

        var readme = System.IO.Path.Combine(destination, "README");
        var description = System.IO.Path.Combine(destination, "DESCRIPTION");
        var changelog = System.IO.Path.Combine(destination, "CHANGELOG");
        var bin = Combine(destination, "bin");

        if (File.Exists(csproj))
        {
            var xml = System.Xml.Linq.XDocument.Load(csproj);
            var propertyGroups = xml.Descendants("PropertyGroup").ToList();
            var versionElement = propertyGroups.Descendants("Version").FirstOrDefault();
            var descriptionElement = propertyGroups.Descendants("Description").FirstOrDefault();
            var packageTags = propertyGroups.Descendants("PackageTags").FirstOrDefault();
            var packageReleaseNotes = propertyGroups.Descendants("PackageReleaseNotes").FirstOrDefault();
            var packageLicenseExpression = propertyGroups.Descendants("PackageLicenseExpression").FirstOrDefault();
            var packageLicenseFile = propertyGroups.Descendants("PackageLicenseFile").FirstOrDefault();
            var packageProjectUrl = propertyGroups.Descendants("PackageProjectUrl").FirstOrDefault();
            var packageIcon = propertyGroups.Descendants("PackageIconUrl").FirstOrDefault();
            var packageRequireLicenseAcceptance = propertyGroups.Descendants("PackageRequireLicenseAcceptance").FirstOrDefault();
            var packageVersion = propertyGroups.Descendants("PackageVersion").FirstOrDefault();
            var packageAuthors = propertyGroups.Descendants("Authors").FirstOrDefault();
            var packageCompany = propertyGroups.Descendants("Company").FirstOrDefault();
            var packageCopyright = propertyGroups.Descendants("Copyright").FirstOrDefault();
            var packageVersionSuffix = propertyGroups.Descendants("VersionSuffix").FirstOrDefault();

            if (!dictionary.ContainsKey(PSManifestKeys.ModuleVersion) &&
                versionElement != null &&
                Version.TryParse(versionElement.Value, out var version))
            {
                dictionary.Add(PSManifestKeys.ModuleVersion, version.ToString());
            }

            if (!dictionary.ContainsKey(PSManifestKeys.ModuleVersion) &&
                packageVersion != null &&
                Version.TryParse(packageVersion.Value, out version))
            {
                dictionary.Add(PSManifestKeys.ModuleVersion, version.ToString());
            }

            if (!dictionary.ContainsKey(PSManifestKeys.Description) &&
                descriptionElement != null &&
                !descriptionElement.Value.IsNullOrWhiteSpace())
            {
                dictionary[PSManifestKeys.Description] = descriptionElement.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.Tags) &&
                packageTags != null &&
                !packageTags.Value.IsNullOrWhiteSpace())
            {
                dictionary[PSManifestKeys.Tags] = packageTags.Value.Split(';');
            }

            if (!dictionary.ContainsKey(PSManifestKeys.ReleaseNotes) &&
                packageReleaseNotes != null &&
                !packageReleaseNotes.Value.IsNullOrWhiteSpace())
            {
                dictionary[PSManifestKeys.ReleaseNotes] = packageReleaseNotes.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.LicenseUri) &&
                packageLicenseExpression != null &&
                !packageLicenseExpression.Value.IsNullOrWhiteSpace())
            {
                // todo: handle license expression
                dictionary[PSManifestKeys.LicenseUri] = packageLicenseExpression.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.LicenseUri) &&
                packageLicenseFile != null &&
                !packageLicenseFile.Value.IsNullOrWhiteSpace())
            {
                // todo: handle license file uri
                dictionary[PSManifestKeys.LicenseUri] = packageLicenseFile.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.ProjectUri) &&
                packageProjectUrl != null &&
                !packageProjectUrl.Value.IsNullOrWhiteSpace()
                && Uri.IsWellFormedUriString(packageProjectUrl.Value, UriKind.Absolute))
            {
                dictionary[PSManifestKeys.ProjectUri] = packageProjectUrl.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.IconUri) &&
                packageIcon != null &&
                !packageIcon.Value.IsNullOrWhiteSpace()
                && Uri.IsWellFormedUriString(packageIcon.Value, UriKind.Absolute))
            {
                dictionary[PSManifestKeys.IconUri] = packageIcon.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.RequireLicenseAcceptance) &&
                packageRequireLicenseAcceptance != null &&
                bool.TryParse(packageRequireLicenseAcceptance.Value, out var requireLicenseAcceptance))
            {
                dictionary[PSManifestKeys.RequireLicenseAcceptance] = requireLicenseAcceptance;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.Author) &&
                packageAuthors != null &&
                !packageAuthors.Value.IsNullOrWhiteSpace())
            {
                dictionary[PSManifestKeys.Author] = packageAuthors.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.CompanyName) &&
                packageCompany != null &&
                !packageCompany.Value.IsNullOrWhiteSpace())
            {
                dictionary[PSManifestKeys.CompanyName] = packageCompany.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.Copyright) &&
                packageCopyright != null &&
                !packageCopyright.Value.IsNullOrWhiteSpace())
            {
                dictionary[PSManifestKeys.Copyright] = packageCopyright.Value;
            }

            if (!dictionary.ContainsKey(PSManifestKeys.Prerelease) &&
                packageVersionSuffix != null &&
                !packageVersionSuffix.Value.IsNullOrWhiteSpace())
            {
                dictionary[PSManifestKeys.Prerelease] = packageVersionSuffix.Value;
            }

            if (File.Exists(bin))
            {
                string? dllPath = null;
                var debug = Combine(bin, "Debug");
                var release = Combine(bin, "Release");
                var targets = new List<string>();
                if (Directory.Exists(debug))
                {
                    foreach (var dir in Directory.EnumerateDirectories(debug))
                    {
                        var childDirectory = GetFileName(dir);
                        if (childDirectory.StartsWith("net"))
                        {
                            try
                            {
                                _ = new FrameworkName(childDirectory);
                                targets.Add(childDirectory);
                            }
                            catch
                            {
                                Debug.WriteLine("Invalid framework: " + childDirectory);
                            }
                        }
                    }
                }
                else if (Directory.Exists(release))
                {
                    foreach (var dir in Directory.EnumerateDirectories(debug))
                    {

                        var childDirectory = GetFileName(dir);
                        if (childDirectory.StartsWith("net"))
                        {
                            try
                            {
                                _ = new FrameworkName(childDirectory);
                                targets.Add(childDirectory);
                            }
                            catch
                            {
                                Debug.WriteLine("Invalid framework: " + childDirectory);
                            }
                        }
                    }

                    if (targets.Count == 0)
                    {
                        var t = Combine(release, $"{moduleName}.dll");
                        if (File.Exists(t))
                            dllPath = t;
                    }
                }
                else
                {
                    foreach (var dir in Directory.EnumerateDirectories(bin))
                    {
                        var childDirectory = GetFileName(dir);
                        if (childDirectory.StartsWith("net"))
                        {
                            try
                            {
                                _ = new FrameworkName(childDirectory);
                                targets.Add(childDirectory);
                            }
                            catch
                            {
                                Debug.WriteLine("Invalid framework: " + childDirectory);
                            }
                        }
                    }

                    if (targets.Count == 0)
                    {
                        var t = Combine(release, $"{moduleName}.dll");
                        if (File.Exists(t))
                            dllPath = t;
                    }
                }

                if (dllPath.IsNullOrWhiteSpace() && targets.Count > 0)
                {
                    var t = Combine(targets[0], $"{moduleName}.dll");
                    if (File.Exists(t))
                        dllPath = t;
                }
            }
        }

        foreach (var kvp in this.MyInvocation.BoundParameters)
        {
            if (kvp.Key.Equals("Path", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            dictionary.Add(kvp.Key, kvp.Value);
        }

        if (!dictionary.ContainsKey(PSManifestKeys.RootModule) && File.Exists(psm1))
        {
            dictionary[PSManifestKeys.RootModule] = psm1;
        }

        if (!dictionary.ContainsKey(PSManifestKeys.Description))
        {
            if (File.Exists(csproj))
            {
            }

            var attempts = new List<string>() { description, $"{description}.txt", };
            if (Env.IsWindows())
            {

            }

            attempts = new List<string>()
            {
                Combine(description, "readme"),
                Combine(description, "readme.txt"),
                Combine(description, "readme.md"),
                Combine(description, "README"),
                Combine(description, "README.txt"),
                Combine(description, "README.md"),
            };

            foreach (var attempt in attempts)
            {
                if (File.Exists(attempt))
                {
                    var sb = StringBuilderCache.Acquire();
                    if (attempt.EndsWith(".md"))
                    {
                        var lines = File.ReadAllLines(attempt);
                        var followsHeader = false;
                        foreach (var line in lines)
                        {
                            if (line.StartsWith("#") || line.StartsWith("ReadME", StringComparison.OrdinalIgnoreCase))
                            {
                                followsHeader = true;
                                continue;
                            }

                            if (line.IsNullOrWhiteSpace())
                            {
                                if (followsHeader)
                                    continue;
                                else
                                    break;
                            }

                            sb.AppendLine(line);
                        }

                        var text = sb.ToString();
                        dictionary[PSManifestKeys.Description] = text.Truncate(256, "...");
                        break;
                    }
                }

            }
        }

        base.ProcessRecord();
    }
}
*/