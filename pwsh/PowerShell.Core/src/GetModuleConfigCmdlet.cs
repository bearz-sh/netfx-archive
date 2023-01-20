using System;
using System.Collections;
using System.IO;
using System.Management.Automation;
using System.Text.Json;

using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.Get, "ModuleConfig")]
[OutputType(typeof(Hashtable), typeof(PSObject), typeof(Array[]))]
public class GetModuleConfigCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    [ValidateNotNullOrEmpty]
    public string? ModuleName { get; set; }

    [Parameter(Position = 1)]
    public string? Query { get; set; }

    protected override void ProcessRecord()
    {
        if (this.ModuleName.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.ModuleName));

        var moduleName = this.ModuleName;
        var config = Utils.GetModuleConfig(moduleName);
        if (config is null)
        {
            var data = Env.Directory(SpecialDirectory.HomeData);
            var dest = System.IO.Path.Combine(data, moduleName, "module.config.json");
            if (File.Exists(dest))
            {
                var json = File.ReadAllText(dest);
                config = JsonSerializer.Deserialize<Hashtable>(
                    json,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        AllowTrailingCommas = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                    });

                config ??= new Hashtable();
            }
            else
            {
                config = new Hashtable();
            }
        }

        if (this.Query.IsNullOrWhiteSpace())
        {
            this.WriteObject(config, false);
            return;
        }

        var segments = this.Query.Split('.');
        object? current = config;

        foreach (var segment in segments)
        {
            if (current is null)
            {
                this.WriteObject(null);
                break;
            }

            if (current is IDictionary dictionary)
            {
                current = dictionary[segment];
                continue;
            }

            if (current is IList list)
            {
                if (int.TryParse(segment, out var index))
                {
                    if (index >= list.Count)
                    {
#pragma warning disable S112
                        throw new IndexOutOfRangeException($"Index {index} was outside the bounds of the array.");
#pragma warning restore S112
                    }

                    current = list[index];
                    continue;
                }

                throw new InvalidOperationException($"Cannot index into a list with a non-numeric value: {segment}.");
            }

            this.WriteObject(null);
            return;
        }

        this.WriteObject(current);
    }
}