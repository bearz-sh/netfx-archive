using Ze.Cli.Apt;

// ReSharper disable once CheckNamespace
namespace Ze.Cli;

public static class AptModule
{
    public static AptExe Apt { get; } = AptExe.Default;
}