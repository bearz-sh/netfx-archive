using System;
using System.IO;
using System.Management.Automation;

using Bearz.Std;
using Bearz.Text;

namespace Ze.PowerShell.Core;

[Alias("rmev")]
[Cmdlet(VerbsCommon.Remove, "EnvironmentVariable")]
public class RemoveEnvironmentVariableCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    protected override void ProcessRecord()
    {
        if (this.Target == EnvironmentVariableTarget.Machine && !Env.IsUserElevated)
        {
            throw new UnauthorizedAccessException("You must be an administrator or root to remove machine environment variables.");
        }

        if (Env.IsWindows())
        {
            Env.Unset(this.Name, this.Target);
        }

        if (this.Target == EnvironmentVariableTarget.Process)
        {
            Env.Unset(this.Name);
        }

        if (this.Target == EnvironmentVariableTarget.Machine)
        {
            var files = new string[] { "/etc/environment" };

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    var sb = StringBuilderCache.Acquire();
                    var found = false;
                    foreach (var line in File.ReadAllLines(file))
                    {
                        if (line.StartsWith(this.Name + "="))
                        {
                            found = true;
                            continue;
                        }

                        sb.AppendLine(line);
                    }

                    if (found)
                    {
                        File.WriteAllText(file, StringBuilderCache.GetStringAndRelease(sb));
                    }
                }
            }
        }

        throw new PlatformNotSupportedException("Non-Windows platforms only support setting process environment variables.");
    }
}