using System.CommandLine.Invocation;
using System.Diagnostics;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Extra.Collections;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;
using Path = System.IO.Path;
using Process = Bearz.Std.Process;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackUpCommandHandler))]
public class StackUpCommand : Command
{
    public StackUpCommand()
        : base("up", "runs the docker compose up command")
    {
    }
}

public class StackUpCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public StackUpCommandHandler(Domain.Settings settings, Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string? Env { get; set; }

    public string Stack { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        var appDir = Path.Join(Paths.DockerAppsDir, this.Stack);
        if (!Directory.Exists(appDir) && !Hbs.RenderDockerTemplates(this.Stack, this.Env, this.settings, this.environments))
            return -1;

        var dockerComposeFile = Path.Join(appDir, "docker-compose.yml");
        if (!Fs.FileExists(dockerComposeFile))
        {
            dockerComposeFile = Path.ChangeExtension(dockerComposeFile, ".yaml");
        }

        if (!Fs.FileExists(dockerComposeFile))
        {
            Console.Error.WriteLine($"No docker-compose.yml file found in {appDir}");
            return -1;
        }

        if (!Hbs.RenderDockerEnvFile(this.Stack, this.Env, this.settings, this.environments))
            return -1;

        var exe = "docker";
        var args = new CommandArgs { "compose", "up", "--wait", "--no-recreate" };

        if (EnvUtils.UseSudoForDocker())
        {
            exe = "sudo";
            args.Unshift("docker");
        }

        var cmd = Process.CreateCommand(exe, new CommandStartInfo()
        {
            Args = args,
            Cwd = appDir,
            StdOut = Stdio.Inherit,
            StdErr = Stdio.Inherit,
        });

        var r = cmd.Output();

        return r.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}