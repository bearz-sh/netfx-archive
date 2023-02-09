using Bearz.Extra.Collections;
using Bearz.Extra.Strings;
using Bearz.Std;

using FluentBuilder;

namespace Bearz.Cli.Execution;

[AutoGenerateBuilder]
public class CliArgsCommand : ICliCommand
{
    public string? CommandName { get; set; }

    public CommandArgs Args => this.CliStartInfo.Args;

    public CliStartInfo CliStartInfo { get; } = new CliStartInfo();

    public static implicit operator CliArgsCommand(string value)
    {
        var cmd = new CliArgsCommand { CliStartInfo = { Args = CommandArgs.From(value) } };
        return cmd;
    }

    public CommandStartInfo Build()
    {
        if (!this.CommandName.IsNullOrWhiteSpace())
            this.Args.Unshift(this.CommandName);

        return new CommandStartInfo()
        {
            Args = this.CliStartInfo.Args,
            Cwd = this.CliStartInfo.Cwd,
            Env = this.CliStartInfo.Env,
            StdIn = this.CliStartInfo.StdIn,
            StdOut = this.CliStartInfo.StdOut,
            StdErr = this.CliStartInfo.StdErr,
        };
    }
}