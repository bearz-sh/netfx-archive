using Microsoft.EntityFrameworkCore;

using Ze.Data.Models;

namespace Ze.Domain;

public class Settings
{
    private readonly DbContext db;
    private readonly Dictionary<string, string> settings = new(StringComparer.OrdinalIgnoreCase);

    [CLSCompliant(false)]
    public Settings(DbContext db)
    {
        this.db = db;
        var dbSettings = this.db.Set<Setting>().ToList();
        foreach (var setting in dbSettings)
        {
            this.settings[setting.Name] = setting.Value;
        }
    }

    public bool Delete(string name)
    {
        name = name.ToSettingName();
        if (!this.settings.ContainsKey(name))
        {
            return false;
        }

        this.db.Set<Setting>().Remove(new Setting { Name = name });
        this.db.SaveChanges();
        this.settings.Remove(name);
        return true;
    }

    public string? Get(string name)
    {
        name = name.ToSettingName();
        if (this.settings.TryGetValue(name, out var value))
            return value;

        return null;
    }

    public void Set(string name, string value)
    {
        name = name.ToSettingName();
        if (this.settings.TryGetValue(name, out var existingValue))
        {
            if (existingValue == value)
                return;

            var setting = this.db.Set<Setting>().Single(s => s.Name == name);
            setting.Value = value;
            this.db.SaveChanges();
        }
        else
        {
            var setting = new Setting { Name = name, Value = value };
            this.db.Set<Setting>().Add(setting);
            this.db.SaveChanges();
        }

        this.settings[name] = value;
    }
}