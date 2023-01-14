using Bearz.Extra.Collections;
using Bearz.Std;

using Ze.Cli;

namespace Ze.Cli.Docker;

public static partial class Docker
{
    public static ExecutableInfo Executable { get; set; } = new ExecutableInfo
    {
        Name = "docker",
        Windows = new[] { "%Program Files%\\Docker\\Docker\\resources\\bin\\docker.exe", },
    };

    public static class Network
    {
        public static CommandOutput Create(Action<NetworkCreateCommandBuilder> configure)
        {
            var builder = new NetworkCreateCommandBuilder();
            configure(builder);
            return Create(builder.Build());
        }

        public static CommandOutput Create(NetworkCreateCommand command)
            => Cli.Call(Executable, command);

        public static Task<CommandOutput> CreateAsync(
            Action<NetworkCreateCommandBuilder> configure,
            CancellationToken cancellationToken = default)
        {
            var builder = new NetworkCreateCommandBuilder();
            configure(builder);
            return CreateAsync(builder.Build(), cancellationToken);
        }

        public static Task<CommandOutput> CreateAsync(NetworkCreateCommand command, CancellationToken cancellationToken = default)
            => Cli.CallAsync(Executable, command, cancellationToken);

        public static CommandOutput List(Action<NetworkListCommandBuilder> configure)
        {
            var builder = new NetworkListCommandBuilder();
            configure(builder);
            return List(builder.Build());
        }

        public static CommandOutput List(NetworkListCommand command)
            => Cli.Call(Executable, command);

        public static Task<CommandOutput> ListAsync(
            Action<NetworkListCommandBuilder> configure,
            CancellationToken cancellationToken = default)
        {
            var builder = new NetworkListCommandBuilder();
            configure(builder);
            return ListAsync(builder.Build(), cancellationToken);
        }

        public static Task<CommandOutput> ListAsync(NetworkListCommand command, CancellationToken cancellationToken = default)
            => Cli.CallAsync(Executable, command, cancellationToken);

        public static CommandOutput Remove(Action<NetworkRemoveCommandBuilder> configure)
        {
            var builder = new NetworkRemoveCommandBuilder();
            configure(builder);
            return Remove(builder.Build());
        }

        public static CommandOutput Remove(NetworkRemoveCommand command)
            => Cli.Call(Executable, command);

        public static Task<CommandOutput> RemoveAsync(
            Action<NetworkRemoveCommandBuilder> configure,
            CancellationToken cancellationToken = default)
        {
            var builder = new NetworkRemoveCommandBuilder();
            configure(builder);
            return RemoveAsync(builder.Build(), cancellationToken);
        }

        public static Task<CommandOutput> RemoveAsync(NetworkRemoveCommand command, CancellationToken cancellationToken = default)
            => Cli.CallAsync(Executable, command, cancellationToken);
    }
}