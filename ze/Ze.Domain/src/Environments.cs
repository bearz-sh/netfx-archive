using Bearz.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

using DbEnvironment = Ze.Data.Models.Environment;
using DbSecret = Ze.Data.Models.EnvironmentSecret;
using DbVariable = Ze.Data.Models.EnvironmentVariable;

namespace Ze.Domain;

public class Environments
{
    private readonly DbContext db;
    private readonly IEncryptionProvider cipher;

    [CLSCompliant(false)]
    public Environments(DbContext db, IEncryptionProvider cipher)
    {
        this.db = db;
        this.cipher = cipher;
    }

    public Environment Create(string name)
    {
        var lowered = name.ToEnvName();
        var entity = new DbEnvironment { Name = name, Slug = lowered, };
        this.db.Set<DbEnvironment>().Add(entity);
        this.db.SaveChanges();
        return new Environment(this.db, entity, this.cipher);
    }

    public bool Delete(string name)
    {
        var lowered = name.ToEnvName();
        var entity = this.db.Set<DbEnvironment>()
            .Include(o => o.Secrets)
            .Include(o => o.Variables)
            .AsSplitQuery().FirstOrDefault(x => x.Slug == lowered);
        if (entity is null)
            return false;

        var secrets = this.db.Set<DbSecret>();
        var variables = this.db.Set<DbVariable>();
        variables.RemoveRange(entity.Variables);
        secrets.RemoveRange(entity.Secrets);

        this.db.Set<DbEnvironment>().Remove(entity);
        this.db.SaveChanges();
        return true;
    }

    public Environment GetOrCreate(string name)
    {
        var lowered = name.ToEnvName();
        var entity = this.db.Set<DbEnvironment>()
            .Include(o => o.Variables)
            .Include(o => o.Secrets)
            .AsSplitQuery()
            .FirstOrDefault(o => o.Slug == lowered);
        if (entity is not null)
            return new Environment(this.db, entity, this.cipher);

        entity = new DbEnvironment { Name = name, Slug = lowered, };
        this.db.Set<DbEnvironment>().Add(entity);
        this.db.SaveChanges();
        return new Environment(this.db, entity, this.cipher);
    }

    public Environment? Get(string name)
    {
        var lowered = name.ToEnvName();
        var entity = this.db.Set<DbEnvironment>()
            .Include(o => o.Variables)
            .Include(o => o.Secrets)
            .AsSplitQuery()
            .FirstOrDefault(o => o.Slug == lowered);

        return entity is not null ?
            new Environment(this.db, entity, this.cipher)
            : null;
    }

    public IEnumerable<string> ListNames()
    {
        return this.db.Set<DbEnvironment>().Select(o => o.Name);
    }
}