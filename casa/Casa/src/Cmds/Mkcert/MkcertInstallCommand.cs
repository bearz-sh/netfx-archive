using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Std;

using Command = System.CommandLine.Command;
using Path = System.IO.Path;

namespace Casa.Cmds.Mkcert;

[CommandHandler(typeof(MkcertInstallCommandHandler))]
public class MkcertInstallCommand : Command
{
    public MkcertInstallCommand()
        : base("install", "installs the root ca certificate into the system CA root store")
    {
    }
}

public class MkcertInstallCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        var cmd = Process.CreateCommand(
            "mkcert",
            new CommandStartInfo()
            {
                Args = new CommandArgs() { "-install" }, StdOut = Stdio.Inherit, StdErr = Stdio.Inherit,
            });

        var r = cmd.Output();
        if (r.ExitCode != 0)
        {
            return r.ExitCode;
        }

        cmd = Process.CreateCommand(
            "mkcert",
            new CommandStartInfo()
            {
                Args = new CommandArgs() { "-CAROOT" }, StdOut = Stdio.Piped, StdErr = Stdio.Piped,
            });

        r = cmd.Output();
        if (r.ExitCode != 0)
        {
            return r.ExitCode;
        }

        var root = r.StdOut.FirstOrDefault();
        var pem = Path.Join(root, "rootCA.pem");
        var key = Path.Join(root, "rootCA-key.pem");

        var dir = Path.Join(Paths.EtcDir, "ssl", "certs");
        if (!Fs.DirectoryExits(dir))
            Fs.MakeDirectory(dir);

        var pemDest = Path.Join(dir, "ca.pem");
        var keyDest = Path.Join(dir, "ca.key");
        if (!Fs.FileExists(pemDest))
            Fs.CopyFile(pem, pemDest);

        if (!Fs.FileExists(keyDest))
            Fs.CopyFile(key, keyDest);

        return r.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}