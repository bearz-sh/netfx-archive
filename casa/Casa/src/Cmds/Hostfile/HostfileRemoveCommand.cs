using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Extra.Strings;
using Bearz.Std;
using Bearz.Text;

using HandlebarsDotNet.Extensions;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Hostfile;

[CommandHandler(typeof(HostfileRemoveCommandHandler))]
public class HostfileRemoveCommand : Command
{
    public HostfileRemoveCommand()
        : base("remove", "Remove a host entry")
    {
        this.AddAlias("rm");
        this.AddArgument(new Argument<string>("host", "The host to remove"));
    }
}

public class HostfileRemoveCommandHandler : ICommandHandler
{
    public string Host { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        if (!Env.IsUserElevated)
        {
            Console.Error.WriteLine("User must be elevated as administrator or root to run this command");
            return -1;
        }

        var hostfile = "/etc/hosts";
        if (OperatingSystem.IsWindows())
        {
            var systemRoot = Env.Directory(SpecialDirectory.Windows);
            hostfile = $"{systemRoot}\\System32\\drivers\\etc\\hosts";
        }

        var sb = StringBuilderCache.Acquire();
        var removed = false;
        foreach (var line in File.ReadAllLines(hostfile))
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (removed)
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

            var ip = parts[0];
            if (ip.Contains("::") || ip.Count(o => o == '.') == 3)
            {
                foreach (var part in parts)
                {
                    if (part.EqualsIgnoreCase(this.Host))
                    {
                        // multiple hosts on a single line
                        if (parts.Length > 2)
                        {
                            var hosts = new List<string>(parts.Skip(1));
                            hosts.Remove(part);
                            sb.Append(ip).Append(' ').AppendJoin(' ', hosts);
                            removed = true;
                            break;
                        }

                        removed = true;
                        break;
                    }
                }

                if (removed)
                    continue;

                sb.Append(line);
            }
            else
            {
                sb.Append(line);
            }
        }

        Fs.WriteTextFile(hostfile, StringBuilderCache.GetStringAndRelease(sb));
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}