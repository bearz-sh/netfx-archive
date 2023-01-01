using System;
using System.Linq;

using Bearz.Std;

using Path = System.IO.Path;

namespace Casa;

public static class Paths
{
    private static string? s_homeDir;

    public static string HomeDir
    {
        get
        {
            if (s_homeDir == null)
            {
                var home = Env.Get("CASA_HOME");
                home ??= Path.Combine(Env.Directory(SpecialDirectory.Opt), "casa");
                s_homeDir = home;
            }

            return s_homeDir;
        }
    }

    public static string EtcDir { get;  } = Path.Combine(HomeDir, "etc");

    public static string LogDir { get;  } = Path.Combine(HomeDir, "var", "log");

    public static string DataDir { get;  } = Path.Combine(HomeDir, "var", "data");

    public static string DockerTemplatesDir { get; } = Path.Combine(HomeDir, "var", "docker", "templates");

    public static string DockerAppsDir { get; } = Path.Combine(HomeDir, "var", "docker", "apps");
}