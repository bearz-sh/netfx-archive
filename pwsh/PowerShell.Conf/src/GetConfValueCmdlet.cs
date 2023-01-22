using System;
using System.Management.Automation;

using Bearz.Extra.Strings;

using Microsoft.Extensions.Configuration;

namespace Ze.PowerShell.Conf;

[Cmdlet(VerbsCommon.Get, "ConfValue")]
[OutputType(typeof(string), typeof(object))]
public class GetConfValueCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public IConfiguration? InputObject { get; set; }

    [Parameter(Position = 1)]
    [ValidateNotNullOrEmpty]
    public string? Key { get; set; }

    public Type? Type { get; set; }

    [Parameter(Position = 2)]
    public object? DefaultValue { get; set; }

    protected override void ProcessRecord()
    {
        if (this.InputObject is null)
            throw new PSArgumentNullException(nameof(this.InputObject));

        if (this.Key.IsNullOrWhiteSpace())
            throw new PSArgumentNullException(nameof(this.Key));

        if (this.Type is not null)
        {
            this.WriteObject(this.InputObject.GetValue(this.Type, this.Key, this.DefaultValue));
            return;
        }

        var v = this.InputObject.GetValue<string>(this.Key, this.DefaultValue?.ToString() ?? string.Empty);
        this.WriteObject(v);
    }
}