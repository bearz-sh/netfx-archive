using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

using Bearz.Extra.Strings;

using HandlebarsDotNet;

namespace Ze.PowerShell.Templates.Handlebars;

[Alias("hbs", "render_hbs")]
[Cmdlet(VerbsData.Expand, "HandlebarTemplate")]
[OutputType(typeof(void))]
public class ExpandHandlebarTemplateCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public string[] Template { get; set; } = Array.Empty<string>();

    [Parameter(Position = 2)]
    public string? Destination { get; set; }

    [Parameter(Position = 1)]
    public object? Data { get; set; }

    [Parameter(ValueFromPipeline = true)]
    public IHandlebars? Handlebar { get; set; }

    protected override void ProcessRecord()
    {
        var data = this.Data;
        if (data is not null)
        {
            var kvStore = new Dictionary<string, object?>();
            if (data is IDictionary dictionary)
            {
                foreach (var key in dictionary.Keys)
                {
                    if (key is string name)
                    {
                        kvStore[name] = dictionary[key];
                    }
                }
            }

            if (data is Dictionary<string, object?> genericDictionary)
            {
                kvStore = genericDictionary;
            }
            else if (data is IDictionary<string, object?> dictionary2)
            {
                foreach (var kvp in dictionary2)
                {
                    kvStore[kvp.Key] = kvp.Value;
                }
            }
            else if (data is PSObject psObject)
            {
                foreach (var prop in psObject.Properties)
                {
                    kvStore[prop.Name] = prop.Value;
                }
            }

            data = kvStore;
        }

        foreach (var template in this.Template)
        {
            var file = Path.GetFullPath(template);
            if (!File.Exists(file))
            {
                throw new FileNotFoundException(file);
            }

            var templateContent = File.ReadAllText(template);
            var tpl = this.Handlebar == null ?
                HandlebarsDotNet.Handlebars.Compile(templateContent) :
                this.Handlebar.Compile(templateContent);

            var content = tpl(data);

            var dest = this.Destination;
            if (dest.IsNullOrWhiteSpace())
            {
                dest = Path.GetDirectoryName(template);
            }

            var target = Path.Combine(dest!, Path.GetFileNameWithoutExtension(template));
            File.WriteAllText(target, content);
        }
    }
}