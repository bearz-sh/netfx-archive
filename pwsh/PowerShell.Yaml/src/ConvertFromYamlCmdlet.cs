using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Management.Automation;
using System.Text.RegularExpressions;

using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Ze.PowerShell.Yaml;

public class ConvertFromYamlCmdlet : PSCmdlet
{
    public string? InputObject { get; set; }

    public SwitchParameter AsHashtable { get; set; }

    public SwitchParameter Merge { get; set; }

    public SwitchParameter All { get; set; }

    protected override void ProcessRecord()
    {
        if (string.IsNullOrWhiteSpace(this.InputObject))
        {
            this.WriteObject(null);
            return;
        }

        var result = PsYamlReader.ReadYaml(
            this.InputObject,
            this.Merge.ToBool(),
            this.All.ToBool(),
            this.AsHashtable.ToBool());
        this.WriteObject(result);
    }
}