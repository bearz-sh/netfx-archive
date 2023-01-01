using Bearz.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

using DbEnvironment = Casa.Data.Model.Environment;

namespace Casa.Domain;

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

    public DbEnvironment Create(string name)
    {
        var lowered = name.ToLower();
        var entity = new DbEnvironment { Name = name, LoweredName = lowered, };
        this.db.Set<DbEnvironment>().Add(entity);
        this.db.SaveChanges();
        return entity;
    }

    public bool Delete(string name)
    {
        var lowered = name.ToLower();
        var entity = this.db.Set<DbEnvironment>().FirstOrDefault(x => x.LoweredName == lowered);
        if (entity is null)
            return false;

        this.db.Set<DbEnvironment>().Remove(entity);
        this.db.SaveChanges();
        return true;
    }

    public Environment GetOrCreate(string name)
    {
        var lowered = name.ToLower();
        var entity = this.db.Set<DbEnvironment>().FirstOrDefault(o => o.LoweredName == lowered);
        if (entity is not null)
            return new Environment(this.db, entity, this.cipher);

        entity = new DbEnvironment { Name = name, LoweredName = lowered, };
        this.db.Set<DbEnvironment>().Add(entity);
        this.db.SaveChanges();
        return new Environment(this.db, entity, this.cipher);
    }

    public Environment? Get(string name)
    {
        var lowered = name.ToLower();
        var entity = this.db.Set<DbEnvironment>().FirstOrDefault(o => o.LoweredName == lowered);
        if (entity is not null)
            return new Environment(this.db, entity, this.cipher);

        return null;
    }

    public IEnumerable<string> ListNames()
    {
        return this.db.Set<DbEnvironment>().Select(o => o.Name);
    }
}