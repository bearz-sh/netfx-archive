using Bearz.Text;

using Casa.Data.Model;

using Microsoft.EntityFrameworkCore;

namespace Casa.Domain;

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
        name = NormalizeKey(name);
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
        name = NormalizeKey(name);
        if (this.settings.TryGetValue(name, out var value))
            return value;

        return null;
    }

    public void Set(string name, string value)
    {
        name = NormalizeKey(name);
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

    private static string NormalizeKey(ReadOnlySpan<char> name)
    {
        var sb = StringBuilderCache.Acquire();
        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToUpperInvariant(c));
            }

            if (c is '_' or '-' or '/' or ':' or '.')
            {
                sb.Append(':');
            }
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}