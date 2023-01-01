using Microsoft.EntityFrameworkCore;

namespace Casa.Data.Model;

public class PgCasaDbContext : CasaDbContext
{
    public PgCasaDbContext(DbContextOptions<PgCasaDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseNpgsql();
        }

        base.OnConfiguring(optionsBuilder);
    }
}