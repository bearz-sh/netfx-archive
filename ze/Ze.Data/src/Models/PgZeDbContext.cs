using Microsoft.EntityFrameworkCore;

namespace Ze.Data.Models;

public class PgZeDbContext : ZeDbContext
{
    public PgZeDbContext(DbContextOptions<PgZeDbContext> options)
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