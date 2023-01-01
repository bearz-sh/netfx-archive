using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Secrets;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretNewCommandHandler))]
public class SecretsNewCommand : Command
{
    public SecretsNewCommand()
        : base("new", "creates a new secret")
    {
        this.AddOption(new Option<int>(new[] { "--length", "-l" }, () => 16, "the length of the secret"));
    }
}

public class SecretNewCommandHandler : ICommandHandler
{
    public int Length { get; set; }

    public int Invoke(InvocationContext context)
    {
        var pg = new SecretGenerator()
            .AddDefaults();

        if (this.Length < 1)
            this.Length = 16;

        Console.WriteLine(pg.GenerateAsString(this.Length));
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}