using System.CommandLine.Invocation;
using System.Linq;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Std;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Hostfile;

[CommandHandler(typeof(HostfileListCommandHandler))]
public class HostfileListCommand : Command
{
    public HostfileListCommand()
        : base("list", "lists all entries in the hostfile")
    {
        this.AddAlias("ls");
    }
}

public class HostfileListCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        var hostfile = "/etc/hosts";
        if (OperatingSystem.IsWindows())
        {
            var systemRoot = Env.Directory(SpecialDirectory.Windows);
            hostfile = $"{systemRoot}\\System32\\drivers\\etc\\hosts";
        }

        var dictionary = new Dictionary<string, string>();

        foreach (var line in File.ReadAllLines(hostfile))
        {
            if (line.Length == 0 || line.StartsWith("#"))
                continue;

            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length > 1)
            {
                var ip = parts[0];
                if (ip.Count(o => o == '.') != 3 && !ip.Contains("::"))
                {
                    continue;
                }

                for (var i = 1; i < parts.Length; i++)
                {
                    var host = parts[i];
                    if (!dictionary.ContainsKey(host))
                        dictionary.Add(host, ip);
                }
            }
        }

        var pad = dictionary.Keys.Max(o => o.Length) + 4;
        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"{kvp.Key.PadRight(pad)}{kvp.Value}");
        }

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}