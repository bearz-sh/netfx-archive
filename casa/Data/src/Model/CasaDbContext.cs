using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Casa.Data.Model;

public class CasaDbContext : DbContext
{
#pragma warning disable CS8618
    public CasaDbContext(DbContextOptions options)
#pragma warning restore CS8618
        : base(options)
    {
    }

    public DbSet<Secret> Secrets { get; set; }

    public DbSet<EnvironmentVariable> EnvironmentVariables { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public DbSet<Environment> Environments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("casa_db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var setting = modelBuilder.Entity<Setting>();
        setting.Property(o => o.Name).IsRequired();
        setting.Property(o => o.Value).IsRequired();

        var envVar = modelBuilder.Entity<EnvironmentVariable>();
        envVar.Property(o => o.Name).IsRequired();
        envVar.Property(o => o.Value).IsRequired();

        var secret = modelBuilder.Entity<Secret>();
        secret.Property(o => o.Name).IsRequired();
        secret.Property(o => o.Value).IsRequired();

        var env = modelBuilder.Entity<Environment>();
        env.Property(o => o.Name).IsRequired();
        env.HasMany(o => o.Secrets).WithOne(o => o.Environment).HasForeignKey(o => o.EnvironmentId);
        env.HasMany(o => o.Variables).WithOne(o => o.Environment).HasForeignKey(o => o.EnvironmentId);

        base.OnModelCreating(modelBuilder);
    }
}