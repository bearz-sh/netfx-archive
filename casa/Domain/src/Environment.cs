using Bearz.Security.Cryptography;
using Bearz.Text;

using Microsoft.EntityFrameworkCore;

using DbEnvironment = Casa.Data.Model.Environment;
using DbSecret = Casa.Data.Model.Secret;
using DbVariable = Casa.Data.Model.EnvironmentVariable;

namespace Casa.Domain;

public class Environment
{
    private readonly Dictionary<string, Secret> secrets = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, EnvironmentVariable> environmentVariables = new(StringComparer.OrdinalIgnoreCase);
    private readonly DbEnvironment model;
    private readonly DbContext db;
    private readonly IEncryptionProvider cipher;

    internal Environment(DbContext db, DbEnvironment model, IEncryptionProvider cipher)
    {
        this.db = db;
        this.model = model;
        this.cipher = cipher;
        foreach (var secret in model.Secrets)
        {
            this.secrets.Add(secret.Name, new Secret(secret, this.cipher));
        }

        foreach (var variable in model.Variables)
        {
            this.environmentVariables.Add(variable.Name, new EnvironmentVariable(variable));
        }
    }

    public string Name => this.model.Name;

    public IEnumerable<Secret> Secrets => this.secrets.Values;

    public IEnumerable<EnvironmentVariable> Variables => this.environmentVariables.Values;

    public void DeleteSecret(string name)
    {
        name = NormalizeEnvName(name);
        if (this.secrets.TryGetValue(name, out var secret))
        {
            this.db.Remove(secret.Model);
            this.db.SaveChanges();
            this.secrets.Remove(name);
        }
    }

    public void DeleteVariable(string name)
    {
        name = NormalizeEnvName(name);
        if (this.environmentVariables.TryGetValue(name, out var variable))
        {
            this.db.Remove(variable.Model);
            this.db.SaveChanges();
            this.environmentVariables.Remove(name);
        }
    }

    public string? GetSecret(string name)
    {
        name = NormalizeEnvName(name);

        if (this.secrets.TryGetValue(name, out var secret))
        {
            return secret.Value;
        }

        return null;
    }

    public string? GetVariable(string name)
    {
        name = NormalizeEnvName(name);

        if (this.environmentVariables.TryGetValue(name, out var variable))
        {
            return variable.Value;
        }

        return null;
    }

    public void SetSecret(string name, string value)
    {
        name = NormalizeEnvName(name);
        if (!this.secrets.TryGetValue(name, out var secret))
        {
            var dbSecret = new DbSecret
            {
                Name = name,
                EnvironmentId = this.model.Id,
            };
            secret = new Secret(dbSecret, this.cipher);
            this.db.Add(dbSecret);
        }

        secret.Value = value;
        this.Save();
        this.secrets[name] = secret;
    }

    public void SetSecret(string name, string value, DateTime? expiresAt)
    {
        name = NormalizeEnvName(name);
        if (!this.secrets.TryGetValue(name, out var secret))
        {
            var dbSecret = new DbSecret
            {
                Name = name,
                EnvironmentId = this.model.Id,
            };
            secret = new Secret(dbSecret, this.cipher);
            this.db.Add(dbSecret);
        }

        secret.Value = value;
        secret.ExpiresAt = expiresAt;

        this.Save();
        this.secrets[name] = secret;
    }

    public void SetSecret(string name, string value, DateTime? expiresAt,  Dictionary<string, string> tags)
    {
        name = NormalizeEnvName(name);
        if (!this.secrets.TryGetValue(name, out var secret))
        {
            var dbSecret = new DbSecret
            {
                Name = name,
                EnvironmentId = this.model.Id,
            };
            secret = new Secret(dbSecret, this.cipher);
            this.db.Add(dbSecret);
        }

        secret.Value = value;
        secret.ExpiresAt = expiresAt;
        secret.Tags.Clear();

        foreach (var kvp in tags)
        {
            secret.Tags[kvp.Key] = kvp.Value;
        }

        secret.Model.Tags = secret.Tags;
        this.Save();
        this.secrets[name] = secret;
    }

    public void SetVariable(string name, string value)
    {
        name = NormalizeEnvName(name);
        if (this.environmentVariables.TryGetValue(name, out var variable))
        {
            variable.Value = value;
        }
        else
        {
            var dbVariable = new DbVariable
            {
                Name = name,
                Value = value,
                EnvironmentId = this.model.Id,
            };

            this.db.Add(dbVariable);
            variable = new EnvironmentVariable(dbVariable);
        }

        this.Save();
        this.environmentVariables[name] = variable;
    }

    public void SetVariable(EnvironmentVariable variable)
    {
        variable.Name = NormalizeEnvName(variable.Name);
        if (!this.environmentVariables.TryGetValue(variable.Name, out var model))
        {
            var entity = new DbVariable();
            entity.Environment = this.model;
            model = new EnvironmentVariable(entity);

            this.environmentVariables.Add(variable.Name, model);
        }

        model.Value = variable.Value;

        this.Save();

        this.environmentVariables[variable.Name] = model;
    }

    protected void Save()
    {
        this.db.SaveChanges();
    }

    private static string NormalizeEnvName(ReadOnlySpan<char> name)
    {
        var sb = StringBuilderCache.Acquire();
        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToUpperInvariant(c));
            }

            if (c is '_' or '-' or '/' or ':' or '.' or ' ')
            {
                sb.Append('_');
            }
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}