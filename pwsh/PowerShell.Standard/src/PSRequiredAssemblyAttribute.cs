namespace Ze.Powershell.Standard;

// ReSharper disable once InconsistentNaming
[AttributeUsage(AttributeTargets.Assembly)]
public class PSRequiredAssemblyAttribute : Attribute
{
    public PSRequiredAssemblyAttribute(string assemblyName)
    {
        this.Name = assemblyName;
    }

    public string Name { get; set; }
}