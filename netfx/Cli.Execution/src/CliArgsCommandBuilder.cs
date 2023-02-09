using Bearz.Std;

namespace Bearz.Cli.Execution;

public partial class CliArgsCommandBuilder : ICliCommandBuilder
{
    public CliArgsCommandBuilder WithCwd(string? cwd)
    {
        this.Object.Value.CliStartInfo.Cwd = cwd;
        return this;
    }

    public CliArgsCommandBuilder WithSudo(bool useSudo)
    {
        this.Object.Value.CliStartInfo.UseSudo = useSudo;
        return this;
    }

    public CliArgsCommandBuilder WithEnv(IDictionary<string, string> env)
    {
        this.Object.Value.CliStartInfo.Env = new Dictionary<string, string>(env, StringComparer.OrdinalIgnoreCase);
        return this;
    }

    public CliArgsCommandBuilder WithStdio(Stdio stdio)
    {
        this.Object.Value.CliStartInfo.StdOut = stdio;
        this.Object.Value.CliStartInfo.StdErr = stdio;
        this.Object.Value.CliStartInfo.StdIn = stdio;
        return this;
    }

    public CliArgsCommandBuilder WithArgs(CommandArgs args)
    {
        this.Object.Value.CliStartInfo.Args = args;
        return this;
    }

    public CliArgsCommandBuilder SetEnvVar(string name, string? value)
    {
        this.Object.Value.CliStartInfo.Env ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        this.Object.Value.CliStartInfo.Env[name] = value ?? string.Empty;
        return this;
    }

    public CliArgsCommandBuilder AddArg(string arg0)
    {
        this.Object.Value.CliStartInfo.Args.Add(arg0);
        return this;
    }

    public CliArgsCommandBuilder AddArg(string arg0, string arg1)
    {
        this.Object.Value.CliStartInfo.Args.Add(arg0, arg1);
        return this;
    }

    public CliArgsCommandBuilder AddArg(string arg0, string arg1, string arg2)
    {
        this.Object.Value.CliStartInfo.Args.Add(arg0, arg1, arg2);
        return this;
    }

    public CliArgsCommandBuilder AddArg(string arg0, string arg1, string arg2, string arg3)
    {
        this.Object.Value.CliStartInfo.Args.Add(arg0, arg1, arg2, arg3);
        return this;
    }

    public CliArgsCommandBuilder AddArg(string arg0, string arg1, string arg2, string arg3, string arg4)
    {
        this.Object.Value.CliStartInfo.Args.Add(arg0, arg1, arg2, arg3, arg4);
        return this;
    }

    ICliCommand ICliCommandBuilder.Build()
    {
        return this.Build();
    }
}