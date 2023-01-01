using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Casa.Data.Model;

public class CasaDbContextFactory : IDesignTimeDbContextFactory<CasaDbContext>
{
    public CasaDbContext CreateDbContext(string[] args)
    {
        Console.WriteLine(args);
        if (args.Length == 0)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CasaDbContext>();
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseSqlite("Data Source=Casa.db");
            return new CasaDbContext(optionsBuilder.Options);
        }

        var type = args[0];
        Console.WriteLine(type);
        var connectionString = args.Length > 1 ? args[1] : null;
        switch (type)
        {
            case "sqlite":
                {
                    connectionString ??= "Data Source=Casa.db";
                    var optionsBuilder = new DbContextOptionsBuilder<SqliteCasaDbContext>();
                    optionsBuilder.UseSnakeCaseNamingConvention();
                    optionsBuilder.UseSqlite(connectionString);
                    return new SqliteCasaDbContext(optionsBuilder.Options);
                }

            case "pg":
                {
                    connectionString ??=
                        "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;";
                    var optionsBuilder = new DbContextOptionsBuilder<PgCasaDbContext>();
                    optionsBuilder.UseSnakeCaseNamingConvention();
                    optionsBuilder.UseNpgsql(connectionString);
                    return new PgCasaDbContext(optionsBuilder.Options);
                }

            default:
                throw new InvalidOperationException($"Unknown database type: {type}");
        }
    }
}