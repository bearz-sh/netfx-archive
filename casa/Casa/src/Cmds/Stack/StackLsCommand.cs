using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Extra.Collections;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;
using Path = Bearz.Std.Path;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackLsCommandHandler))]
public class StackLsCommand : Command
{
    public StackLsCommand()
        : base("ls", "List running compose projects")
    {
        this.AddOption(new Option<bool>(new[] { "all", "a" }, "show all stopped compose projects."));
        this.AddOption(new Option<string?>(new[] { "filter", "f" }, "filter compose projects by name."));
        this.AddOption(new Option<bool>(new[] { "quiet", "q" }, "only display IDs."));
        this.AddOption(new Option<bool>(new[] { "templates", "t" }, "display available templates. Negates all other options."));
    }
}

public class StackLsCommandHandler : ICommandHandler
{
    public bool All { get; set; }

    public string? Filter { get; set; }

    public bool Quiet { get; set; }

    public bool Templates { get; set; }

    public int Invoke(InvocationContext context)
    {
        if (this.Templates)
        {
            var list = new List<string>();
            var files = Fs.ReadDirectory(Paths.DockerTemplatesDir, "docker-compose.yml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var dir = Path.Basename(Path.Dirname(file));
                if (dir != null)
                {
                    list.Add(dir);
                }
            }

            files = Fs.ReadDirectory(Paths.DockerTemplatesDir, "docker-compose.yaml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var dir = Path.Basename(Path.Dirname(file));
                if (dir != null)
                {
                    list.Add(dir);
                }
            }

            list.Sort();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            return 0;
        }

        var exe = "docker";
        var args = new CommandArgs { "compose", "ls" };
        if (this.All)
            args.Add("--all");

        if (this.Quiet)
            args.Add("--quiet");

        if (!string.IsNullOrWhiteSpace(this.Filter))
        {
            args.Add("--filter");
            args.Add(this.Filter);
        }

        if (EnvUtils.UseSudoForDocker())
        {
            exe = "sudo";
            args.Unshift("docker");
        }

        var cmd = Process.CreateCommand(exe, new CommandStartInfo()
        {
            Args = args,
            StdOut = Stdio.Inherit,
            StdErr = Stdio.Inherit,
        });

        var r = cmd.Output();

        return r.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
        throw new NotImplementedException();
    }
}