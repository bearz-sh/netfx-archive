using System;
using System.Collections.Generic;
using System.Management.Automation;

using Bearz.Extra.Strings;
using Bearz.Std;

namespace Ze.PowerShell.Core;

[Alias("getev")]
[Cmdlet(VerbsCommon.Get, "EnvironmentVariable")]
[OutputType(typeof(string))]
public class GetEnvironmentVariableCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true)]
    public string? Name { get; set; } = string.Empty;

    [Parameter(Position = 1)]
    public string Default { get; set; } = string.Empty;

    [Parameter]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    protected override void ProcessRecord()
    {
        if (this.Name.IsNullOrWhiteSpace())
        {
            if (Env.IsWindows())
            {
                var variables = Environment.GetEnvironmentVariables(this.Target);
                foreach (var key in variables.Keys)
                {
                    if (key is string name)
                    {
                        this.WriteObject(new KeyValuePair<string, object?>(name, variables[name]));
                    }
                }

                return;
            }

            if (this.Target == EnvironmentVariableTarget.Process)
            {
                var variables = Environment.GetEnvironmentVariables(this.Target);
                foreach (var key in variables.Keys)
                {
                    if (key is string name)
                    {
                        this.WriteObject(new KeyValuePair<string, object?>(name, variables[name]));
                    }
                }

                return;
            }

            throw new PlatformNotSupportedException("Non-Windows platforms only support setting process environment variables.");
        }

        if (Env.IsWindows())
        {
            var value = Env.Get(this.Name, this.Target) ?? this.Default ?? string.Empty;
            this.WriteObject(value);
        }

        if (this.Target == EnvironmentVariableTarget.Process)
        {
            var value = Environment.GetEnvironmentVariable(this.Name) ?? this.Default ?? string.Empty;
            this.WriteObject(value);
        }

        throw new PlatformNotSupportedException("Non-Windows platforms only support setting process environment variables.");
    }
}