using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Std;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Age;

[SubCommandHandler(typeof(AgeCommandHandler))]
public class AgeCommand : Command
{
    public AgeCommand()
        : base("age", "Provides sub commands to use to call the age cli")
    {
        this.AddCommand(new AgeKeygenCommand());
        this.AddArgument(new Argument<string>("input", "the input file to encrypt or decrypt"));
        this.AddOption(new Option<bool>(new[] { "encrypt", "e" }, "encrypt the input file. Default if omitted"));
        this.AddOption(new Option<bool>(new[] { "decrypt", "d" }, "decrypt the input file"));
        this.AddOption(new Option<string?>(new[] { "output", "o" }, "the output file"));
        this.AddOption(new Option<bool>(new[] { "armor", "a" }, "Encrypt to a PEM encoded format."));
        this.AddOption(new Option<string[]?>(
            new[] { "recipient", "r" },
            "Encrypt to the specified recipient. Can be specified multiple times."));
        this.AddOption(new Option<string[]?>(
            new[] { "recipient-file", "R" },
            "Encrypt to the specified recipient. Can be specified multiple times."));
        this.AddOption(new Option<bool>(new[] { "passphrase", "p" }, "Encrypt with the specified passphrase."));
        this.AddOption(new Option<string[]?>(new[] { "identity", "i" }, "Use the given identity file."));
    }
}

public class AgeCommandHandler : ICommandHandler
{
    public string Input { get; set; } = string.Empty;

    public bool Encrypt { get; set; } = true;

    public bool Decrypt { get; set; }

    public string? Output { get; set; }

    public bool Armor { get; set; }

    public string[]? Recipient { get; set; }

    public string[]? RecipientFile { get; set; }

    public bool Passphrase { get; set; }

    public string[]? Identity { get; set; }

    public int Invoke(InvocationContext context)
    {
        var args = new CommandArgs();

        if (this.Decrypt)
            args.Add("--decrypt");
        else if (this.Encrypt)
            args.Add("--encrypt");

        if (this.Armor)
            args.Add("--armor");

        if (this.Passphrase)
            args.Add("--passphrase");

        if (this.Recipient is not null)
        {
            foreach (var r in this.Recipient)
            {
                args.Add("--recipient", r);
            }
        }
        else if (Env.Get("AGE_RECIPIENT_KEY") is not null)
        {
            args.Add("--recipient", Env.Get("AGE_RECIPIENT_KEY")!);
        }

        if (this.RecipientFile is not null)
        {
            foreach (var r in this.RecipientFile)
                args.Add("--recipient-file", r);
        }
        else if (Env.Get("AGE_RECIPIENT_FILE") is not null)
        {
            args.Add("--recipient-file", Env.Get("AGE_RECIPIENT_FILE")!);
        }

        if (this.Identity is not null)
        {
            foreach (var r in this.Identity)
                args.Add("--identity", r);
        }

        if (!string.IsNullOrWhiteSpace(this.Output))
            args.Add("--output", this.Output);

        args.Add(this.Input);

        var cmd = Process.CreateCommand(
            "age",
            new CommandStartInfo() { Args = args, StdOut = Stdio.Inherit, StdErr = Stdio.Inherit, });

        var re = cmd.Output();
        return re.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}