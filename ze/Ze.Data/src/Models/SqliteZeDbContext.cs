using Microsoft.EntityFrameworkCore;

namespace Ze.Data.Models;

public class SqliteZeDbContext : ZeDbContext
{
    public SqliteZeDbContext(DbContextOptions<SqliteZeDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Ze.db");
        }

        base.OnConfiguring(optionsBuilder);
    }
}