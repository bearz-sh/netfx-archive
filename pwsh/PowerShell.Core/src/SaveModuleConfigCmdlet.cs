using System.Collections;
using System.Management.Automation;
using System.Text.Json;

using Bearz.Std;

using Path = System.IO.Path;

namespace Ze.PowerShell.Core;

public class SaveModuleConfigCmdlet : PSCmdlet
{
    public string? ModuleName { get; set; }

    protected override void ProcessRecord()
    {
        var moduleName = this.ModuleName ?? this.MyInvocation.MyCommand.ModuleName;
        var config = Utils.GetModuleConfig(moduleName);
        if (config is null)
        {
            config = new Hashtable();
        }

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var data = Env.Directory(SpecialDirectory.HomeData);
        var dest = Path.Combine(data, moduleName, "module.config.json");
        var dir = Path.GetDirectoryName(dest);
        if (!System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir!);
        }

        System.IO.File.WriteAllText(dest, json);
    }
}