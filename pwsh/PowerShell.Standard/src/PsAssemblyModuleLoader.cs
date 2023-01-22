using System.Management.Automation;
using System.Reflection;
using System.Runtime.Loader;

using Bearz.Extra.Strings;

namespace Ze.Powershell.Standard;

public abstract class PsAssemblyModuleLoader : IModuleAssemblyInitializer, IModuleAssemblyCleanup
{
    static PsAssemblyModuleLoader()
    {
        var dependencyPath = Path.GetFullPath(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);

        AssemblyLoader = new PsAssemblyLoadContext(dependencyPath);
    }

    public static AssemblyLoadContext AssemblyLoader { get; private set; }

    public abstract IReadOnlyList<string> AssemblyNames { get; }

    public void OnImport()
    {
        AssemblyLoadContext.Default.Resolving += this.ResolveAssembly;
    }

    public void OnRemove(PSModuleInfo psModuleInfo)
    {
        AssemblyLoadContext.Default.Resolving -= this.ResolveAssembly;
    }

    protected virtual Assembly? ResolveAssembly(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve)
    {
        if (!this.AssemblyNames.Any(o => o.EqualsIgnoreCase(assemblyToResolve.Name)))
        {
            return null;
        }

        return AssemblyLoader.LoadFromAssemblyName(assemblyToResolve);
    }
}