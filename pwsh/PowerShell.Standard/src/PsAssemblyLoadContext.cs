using System.Reflection;
using System.Runtime.Loader;

namespace Ze.Powershell.Standard;

public class PsAssemblyLoadContext : AssemblyLoadContext
{
    private readonly string directoryPath;

    public PsAssemblyLoadContext(string directoryPath)
    {
        this.directoryPath = directoryPath;
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string assemblyPath = Path.Combine(
            this.directoryPath,
            $"{assemblyName.Name}.dll");

        return File.Exists(assemblyPath) ?
            this.LoadFromAssemblyPath(assemblyPath) : null;
    }
}