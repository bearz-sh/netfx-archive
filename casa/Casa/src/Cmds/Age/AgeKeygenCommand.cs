using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Std;

using Command = System.CommandLine.Command;
using Path = Bearz.Std.Path;

namespace Casa.Cmds.Age;

public class AgeKeygenCommand : Command
{
    public AgeKeygenCommand()
        : base("keygen", "Generate a new age keypair")
    {
        this.AddOption(new Option<bool>(new[] { "stdout", "s" }, "Print the key to stdout instead of writing to a file. Ignored if --output is set."));
        this.AddOption(new Option<string?>(new[] { "output", "o" }, "The file to write the keypair to. Defaults to the casa etc directory."));
    }
}

public class AgeKeygenCommandHandler : ICommandHandler
{
    public string? Output { get; set; }

    public bool StdOut { get; set; }

    public int Invoke(InvocationContext context)
    {
        var args = new CommandArgs();
        if (!string.IsNullOrWhiteSpace(this.Output))
        {
            args.Add("--output");
            args.Add(this.Output);
        }
        else if (!this.StdOut)
        {
            args.Add("--output");
            args.Add(Path.Join(Paths.EtcDir, "age.key"));
        }

        var cmd = Process.CreateCommand(
            "age-keygen",
            new CommandStartInfo() { Args = args, StdOut = Stdio.Inherit, StdErr = Stdio.Inherit, });

        var r = cmd.Output();
        return r.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}