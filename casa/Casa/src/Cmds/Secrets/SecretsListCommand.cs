using System.CommandLine;
using System.CommandLine.Invocation;

namespace Casa.Cmds.Secrets;

public class SecretsListCommand : Command
{
    public SecretsListCommand()
        : base("list", "lists the secrets available in the current or given environment")
    {
    }
}

public class SecretListCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
        throw new NotImplementedException();
    }
}