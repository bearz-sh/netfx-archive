using Bearz.Std;

using Path = System.IO.Path;

namespace Ze;

public static class ZePaths
{
    private static string? s_homeDir;

    private static string? s_etcDir;

    private static string? s_logDir;

    private static string? s_dataDir;

    private static string? s_certsDir;

    /// <summary>
    /// Gets the ze home directory where data and configuration data is stored.
    /// </summary>
    public static string HomeDir
    {
        get
        {
            if (s_homeDir != null)
            {
                return s_homeDir;
            }

            var home = Env.Get("ZE_HOME") ?? Env.Get("ZE_HOME_DIR");
            home ??= Path.Combine(Env.Directory(SpecialDirectory.Opt), "ze");
            s_homeDir = home;

            return s_homeDir;
        }
    }

    /// <summary>
    /// Gets the ze etc directory where configuration data is stored.
    /// </summary>
    public static string EtcDir
    {
        get
        {
            if (s_etcDir is not null)
                return s_etcDir;

            var etc = Env.Get("ZE_ETC_DIR");
            etc ??= Path.Combine(HomeDir, "etc");
            s_etcDir = etc;

            return s_etcDir;
        }
    }

    public static string LogDir
    {
        get
        {
            if (s_logDir is not null)
                return s_logDir;

            var log = Env.Get("ZE_LOG_DIR");
            log ??= Path.Combine(HomeDir, "log");
            s_logDir = log;

            return s_logDir;
        }
    }

    public static string DataDir
    {
        get
        {
            if (s_dataDir is not null)
                return s_dataDir;

            var data = Env.Get("ZE_DATA_DIR");
            data ??= Path.Combine(HomeDir, "data");
            s_dataDir = data;

            return s_dataDir;
        }
    }

    public static string CertsDir
    {
        get
        {
            if (s_certsDir is not null)
                return s_certsDir;

            var certs = Env.Get("ZE_CERTS_DIR");
            certs ??= Path.Combine(HomeDir, "certs");
            s_certsDir = certs;

            return s_certsDir;
        }
    }
}