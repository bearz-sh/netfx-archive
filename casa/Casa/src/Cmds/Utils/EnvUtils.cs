using Casa.Domain;

using Environment = Casa.Domain.Environment;

namespace Casa.Cmds.Utils;

public static class EnvUtils
{
    public static Environment? FindEnvironment(string? envName, Settings settings, Domain.Environments environments)
    {
        if (string.IsNullOrWhiteSpace(envName))
            envName = settings.Get("env.name");

        if (string.IsNullOrWhiteSpace(envName))
        {
            Console.Error.WriteLine("No environment specified and the current environment was not set");

            return null;
        }

        var env = environments.Get(envName);
        if (env is null)
        {
            Console.Error.WriteLine("No environment found with the name '{0}'", envName);

            return null;
        }

        return env;
    }
}