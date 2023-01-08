using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretsImportCommandHandler))]
public class SecretsImportCommand : Command
{
    public SecretsImportCommand()
        : base("import", "Import secrets from a file")
    {
        this.AddOption(new Option<string?>(new[] { "env", "e" }));
    }
}

public class SecretsImportCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public SecretsImportCommandHandler(Domain.Settings settings, Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string? Env { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        var env = EnvUtils.FindEnvironment(this.Env, this.settings, this.environments);
        if (env is null)
        {
            Console.Error.WriteLine("Could not determine the environment to use.");
            return 1;
        }

        if (string.IsNullOrWhiteSpace(this.FilePath))
        {
            Console.Error.WriteLine("File path of the file to import is required");
            return 1;
        }

        var envVars = ImportUtil.Import(this.FilePath);
        foreach (var kvp in envVars)
        {
            env.SetSecret(kvp.Key, kvp.Value);
        }

        return 1;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}