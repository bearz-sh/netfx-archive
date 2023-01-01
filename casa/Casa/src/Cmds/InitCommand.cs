using System.CommandLine.Invocation;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Text.Json;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Secrets;
using Bearz.Std;
using Bearz.Std.Unix;

using Casa.Data.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Command = System.CommandLine.Command;
using Path = System.IO.Path;

namespace Casa.Cmds;

[SubCommandHandler(typeof(InitCommandHandler))]
public class InitCommand : Command
{
    public InitCommand()
        : base("init", "Initializes casa file system directories and configuration")
    {
    }
}

public class InitCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        var mode = UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute |
                   UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                   UnixFileMode.OtherRead;

        var directories = new string[]
        {
            Paths.HomeDir,
            Paths.EtcDir,
            Path.Join(Paths.EtcDir, "casa"),
            Path.Join(Paths.HomeDir, "var"),
            Paths.DataDir,
            Path.Join(Paths.DataDir, "casa"),
            Paths.LogDir,
            Path.Join(Paths.LogDir, "casa"),
            Paths.DockerTemplatesDir,
            Paths.DockerAppsDir,
        };

        foreach (var dir in directories)
        {
            if (!Fs.DirectoryExits(dir))
                Fs.MakeDirectory(dir, mode);
        }

        if (Bearz.Std.Env.IsLinux())
        {
            var uid = -1;
            var sudoIdString = Env.Get("SUDO_UID");
            if (!string.IsNullOrEmpty(sudoIdString))
            {
                int.TryParse(sudoIdString, out uid);
            }
            else
            {
                if (UnixUser.EffectiveUserId.HasValue)
                    uid = UnixUser.EffectiveUserId.Value;
            }

            if (uid > -1)
            {
                foreach (var dir in directories)
                {
                    Fs.Chown(dir, uid, 0);
                }
            }
        }

        var appSettings = Path.Join(Paths.EtcDir, "casa.json");
        if (!Fs.FileExists(appSettings))
        {
            var pg = new SecretGenerator().AddDefaults();
            var secret = pg.GenerateAsString(20);
            var salt = pg.GenerateAsString(20);
            var json = new
            {
                cipherKey = secret,
                cipherSalt = salt,
                database = new
                {
                    kind = "sqlite",
                    connectionString = $"Data Source={Path.Join(Paths.DataDir, "casa", "casa.db")}",
                },
            };

            var settingsContents = JsonSerializer.Serialize(
                json,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true,
                });

            Console.WriteLine("Writing settings to {0}", appSettings);
            Fs.WriteTextFile(appSettings, settingsContents);
        }

        var cfg = new ConfigurationManager()
            .AddJsonFile(appSettings, true, false)
            .Build();

        var connectionString = cfg.GetValue<string>("database:connectionString");
        var kind = cfg.GetValue<string>("database:kind");
        if (kind == "sqlite")
        {
            var options = new DbContextOptionsBuilder<SqliteCasaDbContext>();
            options.UseSnakeCaseNamingConvention();
            options.UseSqlite(connectionString);
            var db = new SqliteCasaDbContext(options.Options);
            db.Database.Migrate();
        }
        else
        {
            Console.WriteLine("No Db");
        }

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}