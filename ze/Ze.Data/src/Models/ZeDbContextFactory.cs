using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ze.Data.Models;

public class ZeDbContextFactory : IDesignTimeDbContextFactory<ZeDbContext>
{
    public ZeDbContext CreateDbContext(string[] args)
    {
        Console.WriteLine(args);
        if (args.Length == 0)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ZeDbContext>();
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseSqlite("Data Source=Ze.db");
            return new ZeDbContext(optionsBuilder.Options);
        }

        var type = args[0];
        Console.WriteLine(type);
        var connectionString = args.Length > 1 ? args[1] : null;
        switch (type)
        {
            case "sqlite":
                {
                    connectionString ??= "Data Source=Ze.db";
                    var optionsBuilder = new DbContextOptionsBuilder<SqliteZeDbContext>();
                    optionsBuilder.UseSnakeCaseNamingConvention();
                    optionsBuilder.UseSqlite(connectionString);
                    return new SqliteZeDbContext(optionsBuilder.Options);
                }

            case "pg":
                {
                    connectionString ??=
                        "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;";
                    var optionsBuilder = new DbContextOptionsBuilder<PgZeDbContext>();
                    optionsBuilder.UseSnakeCaseNamingConvention();
                    optionsBuilder.UseNpgsql(connectionString);
                    return new PgZeDbContext(optionsBuilder.Options);
                }

            default:
                throw new InvalidOperationException($"Unknown database type: {type}");
        }
    }
}