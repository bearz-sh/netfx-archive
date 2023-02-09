using System.Runtime.InteropServices;

using Bearz.Diagnostics;
using Bearz.Std;

using Path = System.IO.Path;

namespace Bearz.Cli.Execution;

public class ExecutableInfo
{
    internal static readonly List<IPreCliHook> PreCliHooks = new List<IPreCliHook>();

    internal static readonly List<IPostCliHook> PostCliHooks = new List<IPostCliHook>();

    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    public IReadOnlyList<string> Windows { get; set; } = Array.Empty<string>();

    public IReadOnlyList<string> MacOs { get; set; } = Array.Empty<string>();

    public IReadOnlyList<string> Linux { get; set; } = Array.Empty<string>();

    public static void RegisterPreCliHook(IPreCliHook hook)
    {
        PreCliHooks.Add(hook);
    }

    public static void RegisterPostCliHook(IPostCliHook hook)
    {
        PostCliHooks.Add(hook);
    }

    public virtual string? Find()
    {
        if (!string.IsNullOrEmpty(this.Location))
            return this.Location;

        var exe = Process.Which(this.Name, null, true);
        if (exe is not null)
        {
            this.Location = exe;
            return exe;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            foreach (var path in this.Windows)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                this.Location = exe;
                return exe;
            }

            return null;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            foreach (var path in this.MacOs)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                this.Location = exe;
                return exe;
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            foreach (var path in this.Linux)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                this.Location = exe;
                return exe;
            }
        }

        return null;
    }

    public virtual string FindOrThrow()
    {
        var exe = this.Find();
        if (exe is null)
        {
            throw new NotFoundOnPathException(this.Name);
        }

        return exe;
    }
}