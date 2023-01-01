using System.ComponentModel.Composition.Hosting;
using System.Security.Cryptography;
using System.Text;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Security.Cryptography;
using Bearz.Std;
using Bearz.Text;

using Casa.Data.Model;
using Casa.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Path = System.IO.Path;

namespace Casa;

public static class Startup
{
    public static void Configure(ConsoleApplicationBuilder builder)
    {
        builder.UseDefaults();
        var db = Path.Join(Paths.DataDir, "casa", "casa.db");
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>()
        {
            ["Logging:LogLevel:Microsoft"] = "Warning",
        });

        builder.Configuration.AddEnvironmentVariables("CASA_");
        builder.Configuration.AddJsonFile(Path.Join(Paths.EtcDir, "casa.json"), true, false);
        builder.Services.AddSqlite<CasaDbContext>("Data Source=" + db, null, (c) =>
            c.UseSnakeCaseNamingConvention());
        builder.Services.TryAddSingleton<DbContext>(s => s.GetRequiredService<CasaDbContext>());
        builder.Services.TryAddSingleton<IEncryptionProvider>(s =>
        {
            var cfg = s.GetRequiredService<IConfiguration>();
            var key = cfg.GetValue<string>("cipherKey") ?? Env.Get("CASA_CIPHER_KEY") ?? throw new InvalidOperationException("Missing cipherKey");
            var salt = cfg.GetValue<string>("cipherSalt") ?? Env.Get("CASA_CIPHER_SALT") ?? throw new InvalidOperationException("Missing cipherSalt");
            using var pbkdf2 = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes(salt), 100000, HashAlgorithmName.SHA256);
            var bytes = pbkdf2.GetBytes(32);
            return new AesGcmEncryptionProvider(bytes);
        });
        builder.Services.TryAddSingleton<Settings>();
        builder.Services.TryAddSingleton<Environments>();

        builder.UseComposition((cat) =>
        {
            cat.Catalogs.Add(new AssemblyCatalog(typeof(Startup).Assembly));
        });
    }
}