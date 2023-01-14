using Bearz.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

using DbEnvironment = Ze.Data.Models.Environment;
using DbSecret = Ze.Data.Models.EnvironmentSecret;
using DbVariable = Ze.Data.Models.EnvironmentVariable;

namespace Ze.Domain;

public class Environment
{
    private readonly Dictionary<string, EnvironmentSecret> secrets = new(StringComparer.OrdinalIgnoreCase);
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
            this.secrets.Add(secret.Name, new EnvironmentSecret(secret, this.cipher));
        }

        foreach (var variable in model.Variables)
        {
            this.environmentVariables.Add(variable.Name, new EnvironmentVariable(variable));
        }
    }

    public string Name => this.model.Name;

    public IEnumerable<EnvironmentSecret> Secrets => this.secrets.Values;

    public IEnumerable<EnvironmentVariable> Variables => this.environmentVariables.Values;

    public void DeleteSecret(EnvironmentVariableName variableName)
    {
        if (!this.secrets.TryGetValue(variableName, out var secret))
        {
            return;
        }

        this.db.Remove(secret.Model);
        this.db.SaveChanges();
        this.secrets.Remove(variableName);
    }

    public void DeleteVariable(EnvironmentVariableName variableName)
    {
        if (!this.environmentVariables.TryGetValue(variableName, out var variable))
        {
            return;
        }

        this.db.Remove(variable.Model);
        this.db.SaveChanges();
        this.environmentVariables.Remove(variableName);
    }

    public string? GetSecret(EnvironmentVariableName variableName)
    {
        return this.secrets.TryGetValue(variableName, out var secret)
            ? secret.Value
            : null;
    }

    public string? GetVariable(EnvironmentVariableName variableName)
    {
        return this.environmentVariables.TryGetValue(variableName, out var variable)
            ? variable.Value
            : null;
    }

    public void SetSecret(EnvironmentVariableName variableName, string value)
    {
        if (!this.secrets.TryGetValue(variableName, out var secret))
        {
            var dbSecret = new DbSecret
            {
                Name = variableName,
                EnvironmentId = this.model.Id,
            };
            secret = new EnvironmentSecret(dbSecret, this.cipher);
            this.db.Add(dbSecret);
        }

        secret.Value = value;
        this.db.SaveChanges();
        this.secrets[variableName] = secret;
    }

    public void SetSecret(EnvironmentVariableName variableName, string value, DateTime? expiresAt)
    {
        if (!this.secrets.TryGetValue(variableName, out var secret))
        {
            var dbSecret = new DbSecret
            {
                Name = variableName,
                EnvironmentId = this.model.Id,
            };
            secret = new EnvironmentSecret(dbSecret, this.cipher);
            this.db.Add(dbSecret);
        }

        secret.Value = value;
        secret.ExpiresAt = expiresAt;

        this.db.SaveChanges();
        this.secrets[variableName] = secret;
    }

    public void SetSecret(EnvironmentVariableName variableName, string value, DateTime? expiresAt,  Dictionary<string, string> tags)
    {
        if (!this.secrets.TryGetValue(variableName, out var secret))
        {
            var dbSecret = new DbSecret
            {
                Name = variableName,
                EnvironmentId = this.model.Id,
            };
            secret = new EnvironmentSecret(dbSecret, this.cipher);
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
        this.db.SaveChanges();
        this.secrets[variableName] = secret;
    }

    public void SetVariable(EnvironmentVariableName variableName, string value)
    {
        if (this.environmentVariables.TryGetValue(variableName, out var variable))
        {
            variable.Value = value;
        }
        else
        {
            var dbVariable = new DbVariable
            {
                Name = variableName,
                Value = value,
                EnvironmentId = this.model.Id,
            };

            this.db.Add(dbVariable);
            variable = new EnvironmentVariable(dbVariable);
        }

        this.db.SaveChanges();
        this.environmentVariables[variableName] = variable;
    }

    public void SetVariable(EnvironmentVariable variable)
    {
        if (!this.environmentVariables.TryGetValue(variable.Name, out var model))
        {
            var entity = new DbVariable { Environment = this.model };
            model = new EnvironmentVariable(entity);

            this.environmentVariables.Add(variable.Name, model);
        }

        model.Value = variable.Value;

        this.db.SaveChanges();

        this.environmentVariables[variable.Name] = model;
    }

    protected void Save()
    {
        this.db.SaveChanges();
    }
}