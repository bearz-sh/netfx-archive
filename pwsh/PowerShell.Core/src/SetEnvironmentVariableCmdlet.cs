using System;
using System.Collections;
using System.Management.Automation;

using Bearz.Extra.Strings;
using Bearz.Secrets;
using Bearz.Std;

namespace Ze.PowerShell.Core;

[Alias("setev")]
[Cmdlet(VerbsCommon.Set, "EnvironmentVariable")]
public class SetEnvironmentVariableCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "NameValue")]
    public string Name { get; set; } = string.Empty;

    [Parameter(Position = 1, Mandatory = true, ParameterSetName = "NameValue")]
    public string Value { get; set; } = string.Empty;

    [Parameter(Position = 0, ParameterSetName = "Values")]
    public IDictionary? Values { get; set; }

    [Parameter(ParameterSetName = "Values")]
    [Parameter(ParameterSetName = "NameValue")]
    public SwitchParameter AsSecret { get; set; }

    [Parameter(ParameterSetName = "Values", Position = 2)]
    [Parameter(ParameterSetName = "NameValue", Position = 3)]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    protected override void ProcessRecord()
    {
        if (this.Name.IsNullOrWhiteSpace() && this.Values is null)
            throw new PSArgumentNullException(nameof(this.Name), "-Name or -Values must be set");

        if (this.Target == EnvironmentVariableTarget.Machine && !Env.IsUserElevated)
        {
            throw new UnauthorizedAccessException("You must be an administrator or root to remove machine environment variables.");
        }

        if (this.Values != null)
        {
            foreach (var key in this.Values.Keys)
            {
                if (key is not string name)
                    continue;

                var value = this.Values[key] as string;
                if (value == null)
                    continue;

                if (this.AsSecret.ToBool())
                {
                    SecretMasker.Default.Add(value);
                }

                if (Env.IsWindows())
                {
                    Env.Set(name, value, this.Target);
                    return;
                }

                if (this.Target == EnvironmentVariableTarget.Process)
                {
                    Env.Set(name, value);
                }

                throw new PlatformNotSupportedException("Non-Windows platforms only support setting process environment variables.");
            }

            return;
        }

        if (this.AsSecret.ToBool())
        {
            SecretMasker.Default.Add(this.Value);
        }

        if (Env.IsWindows())
        {
            Env.Set(this.Name, this.Value, this.Target);
            return;
        }

        if (this.Target == EnvironmentVariableTarget.Process)
        {
            Env.Set(this.Name, this.Value);
        }

        throw new PlatformNotSupportedException("Non-Windows platforms only support setting process environment variables.");
    }
}