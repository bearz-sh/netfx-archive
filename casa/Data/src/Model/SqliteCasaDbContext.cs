using Microsoft.EntityFrameworkCore;

namespace Casa.Data.Model;

public class SqliteCasaDbContext : CasaDbContext
{
    public SqliteCasaDbContext(DbContextOptions<SqliteCasaDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Casa.db");
        }

        base.OnConfiguring(optionsBuilder);
    }
}