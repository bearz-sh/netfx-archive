using System.Collections.Generic;

namespace Ze.PowerShell.Module;

public static class PSManifestKeys
{
    public const string RootModule = "RootModule";

    public const string ModuleVersion = "ModuleVersion";

    public const string Guid = "GUID";

    public const string Author = "Author";

    public const string CompanyName = "CompanyName";

    public const string Copyright = "Copyright";

    public const string Description = "Description";

    public const string PowerShellVersion = "PowerShellVersion";

    public const string PowerShellHostName = "PowerShellHostName";

    public const string PowerShellHostVersion = "PowerShellHostVersion";

    public const string DotNetFrameworkVersion = "DotNetFrameworkVersion";

    public const string ClrVersion = "CLRVersion";

    public const string ProcessorArchitecture = "ProcessorArchitecture";

    public const string RequiredModules = "RequiredModules";

    public const string RequiredAssemblies = "RequiredAssemblies";

    public const string ScriptsToProcess = "ScriptsToProcess";

    public const string TypesToProcess = "TypesToProcess";

    public const string FormatsToProcess = "FormatsToProcess";

    public const string NestedModules = "NestedModules";

    public const string FunctionsToExport = "FunctionsToExport";

    public const string CmdletsToExport = "CmdletsToExport";

    public const string VariablesToExport = "VariablesToExport";

    public const string AliasesToExport = "AliasesToExport";

    public const string DscResourcesToExport = "DscResourcesToExport";

    public const string ModuleList = "ModuleList";

    public const string FileList = "FileList";

    public const string PrivateData = "PrivateData";

    public const string Tags = "Tags";

    public const string LicenseUri = "LicenseUri";

    public const string ProjectUri = "ProjectUri";

    public const string IconUri = "IconUri";

    public const string ReleaseNotes = "ReleaseNotes";

    public const string RequireLicenseAcceptance = "RequireLicenseAcceptance";

    public const string ExternalModuleDependencies = "ExternalModuleDependencies";

    public const string Prerelease = "Prerelease";

    public const string HelpInfoUri = "HelpInfoURI";

    public const string DefaultCommandPrefix = "DefaultCommandPrefix";

    private static readonly List<string> Keys = new List<string>
    {
        RootModule,
        ModuleVersion,
        Guid,
        Author,
        CompanyName,
        Copyright,
        Description,
        PowerShellVersion,
        PowerShellHostName,
        PowerShellHostVersion,
        DotNetFrameworkVersion,
        ClrVersion,
        ProcessorArchitecture,
        RequiredModules,
        RequiredAssemblies,
        ScriptsToProcess,
        TypesToProcess,
        FormatsToProcess,
        NestedModules,
        FunctionsToExport,
        CmdletsToExport,
        VariablesToExport,
        AliasesToExport,
        DscResourcesToExport,
        ModuleList,
        FileList,
        PrivateData,
        Tags,
        LicenseUri,
        ProjectUri,
        IconUri,
        ReleaseNotes,
        RequireLicenseAcceptance,
        ExternalModuleDependencies,
        Prerelease,
        HelpInfoUri,
        DefaultCommandPrefix,
    };

    public static IReadOnlyList<string> Values => Keys;
}