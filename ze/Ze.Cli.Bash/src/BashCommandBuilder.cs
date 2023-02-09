using Bearz.Cli.Execution;

namespace Ze.Cli.Bash;

public partial class BashCommandBuilder : ICliCommandBuilder
{
    ICliCommand ICliCommandBuilder.Build()
    {
        return this.Build();
    }
}