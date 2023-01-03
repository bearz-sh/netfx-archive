using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Std;

using Command = System.CommandLine.Command;
using Path = Bearz.Std.Path;

namespace Casa.Cmds.Mkcert;

[CommandHandler(typeof(MkcertNewCommandHandler))]
public class MkcertNewCommand : Command
{
    public MkcertNewCommand()
        : base("new", "Create a new certificate for the specified hostnames")
    {
        this.AddArgument(new Argument<string[]>("hostnames", "The hostnames to create a certificate for"));
        this.AddOption(new Option<string?>(new[] { "name", "n" }, "The name of the certificate"));
    }
}

public class MkcertNewCommandHandler : ICommandHandler
{
    public string[] Hostnames { get; set; } = Array.Empty<string>();

    public string? Name { get; set; }

    public int Invoke(InvocationContext context)
    {
        if (this.Hostnames.Length == 0)
        {
            Console.Error.WriteLine("No hostnames specified");
            return -1;
        }

        var name = this.Name;
        if (string.IsNullOrWhiteSpace(this.Name))
        {
            name = this.Hostnames[0];
            if (name.StartsWith("*."))
            {
                name = "star." + name.Substring(2);
            }
        }

        var pemFile = Path.Join(Paths.EtcDir, "ssl", "certs", $"{name}.pem");
        var keyFile = Path.Join(Paths.EtcDir, "ssl", "certs", $"{name}.key");

        var dir = Path.Dirname(pemFile);
        if (!Fs.DirectoryExits(dir))
            Fs.MakeDirectory(dir!);

        var args = new CommandArgs { "-cert-file", pemFile, "-key-file", keyFile };

        foreach (var h in this.Hostnames)
        {
            args.Add($"\"{h}\"");
        }

        var cmd = Process.CreateCommand(
            "mkcert",
            new CommandStartInfo()
            {
                Args = args, StdOut = Stdio.Inherit, StdErr = Stdio.Inherit,
            });

        var r = cmd.Output();
        return r.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}