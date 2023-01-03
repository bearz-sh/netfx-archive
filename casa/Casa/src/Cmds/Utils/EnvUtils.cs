using Bearz.Std;

using Casa.Domain;

using Environment = Casa.Domain.Environment;

namespace Casa.Cmds.Utils;

public static class EnvUtils
{
    public static bool UseSudo()
    {
        if (OperatingSystem.IsWindows() || OperatingSystem.IsBrowser())
            return false;

        if (Env.Get("SUDO_USER") != null || Env.Get("SUDO_UID") != null)
            return false;

        return true;
    }

    public static bool UseSudoForDocker()
    {
        var rootless = Env.Get("DOCKER_ROOTLESS");
        if (UseSudo())
        {
            return rootless is not "1";
        }

        return false;
    }
    
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