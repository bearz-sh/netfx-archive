using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Extra.Strings;
using Bearz.Std;
using Bearz.Text;

using HandlebarsDotNet.Extensions;

using Microsoft.Extensions.Primitives;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Hostfile;

[CommandHandler(typeof(HostfileSetCommandHandler))]
public class HostfileSetCommand : Command
{
    public HostfileSetCommand()
        : base("set", "Set a host entry")
    {
        this.AddArgument(new Argument<string>("host", "The host name"));
        this.AddArgument(new Argument<string>("ip", "The IP address"));
    }
}

public class HostfileSetCommandHandler : ICommandHandler
{
    public string Host { get; set; } = string.Empty;

    public string Ip { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        var hostfile = "/etc/hosts";
        if (OperatingSystem.IsWindows())
        {
            var systemRoot = Env.Directory(SpecialDirectory.Windows);
            hostfile = $"{systemRoot}\\System32\\drivers\\etc\\hosts";
        }

        var sb = StringBuilderCache.Acquire();
        var replaced = false;
        foreach (var line in File.ReadAllLines(hostfile))
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (replaced)
            {
                sb.Append(line);
                continue;
            }

            if (line.Length == 0)
                continue;

            if (line.StartsWith("#"))
            {
                sb.AppendLine(line);
                continue;
            }

            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length < 2)
            {
                sb.Append(line);
                continue;
            }

            // is the first segment an ip address?
            var ip = parts[0];
            if (ip.Contains("::") || ip.Count(o => o == '.') == 3)
            {
                foreach (var part in parts)
                {
                    if (part.EqualsIgnoreCase(this.Host))
                    {
                        var hosts = string.Join(" ", parts.Skip(1));
                        sb.Append(this.Ip).Append(' ').Append(hosts);
                        replaced = true;
                        break;
                    }
                }

                if (replaced)
                    continue;
            }

            sb.Append(line);
        }

        if (!replaced)
        {
            sb.AppendLine();
            sb.Append(this.Ip).Append(' ').Append(this.Host);
        }

        Fs.WriteTextFile(hostfile, StringBuilderCache.GetStringAndRelease(sb));
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}