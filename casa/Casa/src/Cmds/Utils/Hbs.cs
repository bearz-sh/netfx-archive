using Bearz.Extra.Strings;
using Bearz.Secrets;
using Bearz.Std;

using Casa.Domain;

using Microsoft.VisualBasic;

using Path = Bearz.Std.Path;

namespace Casa.Cmds.Utils;

public static class Hbs
{
    public static string RenderTemplate(string template, object context)
    {
        var tpl = HandlebarsDotNet.Handlebars.Compile(template);
        return tpl(context);
    }

    public static string RenderFile(string path, object context)
    {
        var tpl = HandlebarsDotNet.Handlebars.Compile(Fs.ReadTextFile(path));
        return tpl(context);
    }

    public static bool RenderDockerEnvFile(
        string templateName,
        string? envName,
        Domain.Settings settings,
        Domain.Environments environments)
    {
        var templateDirectory = Path.Join(Paths.DockerTemplatesDir, templateName);
        var envTemplate = Path.Join(templateDirectory, ".env.hbs");
        if (!Path.Exists(envTemplate))
        {
            Console.Error.WriteLine("Template file not found: " + envTemplate);
            return false;
        }

        envName ??= settings.Get("env.name");
        if (string.IsNullOrWhiteSpace(envName))
        {
            Console.Error.WriteLine("No environment name specified");
            return false;
        }

        var appDir = Path.Join(Paths.DockerAppsDir, $"{envName}-{templateName}");

        var env = environments.GetOrCreate(envName);
        var global = environments.GetOrCreate("global");
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var next in global.Variables)
        {
            data.Add(next.Name, next.Value);
        }

        foreach (var next in env.Variables)
        {
            data.Add(next.Name, next.Value);
        }

        foreach (var secret in global.Secrets)
        {
            data.Add(secret.Name, secret.Value);
        }

        foreach (var secret in env.Secrets)
        {
            data.Add(secret.Name, secret.Value);
        }

        var defaultData = new Dictionary<string, string>()
        {
            ["CASA_PATHS_DATA"] = Path.Join(Paths.DataDir, templateName),
            ["CASA_PATHS_LOG"] = Path.Join(Paths.LogDir, templateName),
            ["CASA_PATHS_CONFIG"] = Path.Join(Paths.EtcDir, templateName),
            ["CASA_PATHS_ETC"] = Path.Join(Paths.EtcDir, templateName),
            ["NAME"] = $"{envName}-{templateName}",
            ["NET_HOST_IP"] = "127.0.0.1",
            ["NET_DOMAIN"] = "casa.local",
            ["VNET_NAME"] = "docker-vnet",
        };

        foreach (var kvp in defaultData)
        {
            if (!data.ContainsKey(kvp.Key))
            {
                data.Add(kvp.Key, kvp.Value);
            }
        }

        Register(global, env, data);

        var dest = Path.Join(appDir, ".env");
        var content = RenderFile(envTemplate, data);
        Fs.WriteTextFile(dest, content);
        return true;
    }

    public static bool RenderDockerTemplates(
        string templateName,
        string? envName,
        Domain.Settings settings,
        Domain.Environments environments)
    {
        envName ??= settings.Get("env.name");
        if (string.IsNullOrWhiteSpace(envName))
        {
            Console.Error.WriteLine("No environment name specified");
            return false;
        }

        var templateDirectory = Path.Join(Paths.DockerTemplatesDir, templateName);
        var appDir = Path.Join(Paths.DockerAppsDir, $"{envName}-{templateName}");

        var env = environments.GetOrCreate(envName);
        var global = environments.GetOrCreate("global");
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var next in global.Variables)
        {
            data.Add(next.Name, next.Value);
        }

        foreach (var next in env.Variables)
        {
            data.Add(next.Name, next.Value);
        }

        foreach (var secret in global.Secrets)
        {
            data.Add(secret.Name, secret.Value);
        }

        foreach (var secret in env.Secrets)
        {
            data.Add(secret.Name, secret.Value);
        }

        var defaultData = new Dictionary<string, string>()
        {
            ["CASA_PATHS_DATA"] = Path.Join(Paths.DataDir, templateName),
            ["CASA_PATHS_LOG"] = Path.Join(Paths.LogDir, templateName),
            ["CASA_PATHS_CONFIG"] = Path.Join(Paths.EtcDir, templateName),
            ["CASA_PATHS_ETC"] = Path.Join(Paths.EtcDir, templateName),
            ["NAME"] = $"{envName}-{templateName}",
            ["NET_HOST_IP"] = "127.0.0.1",
            ["NET_DOMAIN"] = "casa.local",
            ["VNET_NAME"] = "docker-vnet",
        };

        foreach (var kvp in defaultData)
        {
            if (!data.ContainsKey(kvp.Key))
            {
                data.Add(kvp.Key, kvp.Value);
            }
        }

        Register(global, env, data);

        var paths = Fs.ReadDirectory(templateDirectory, "*", SearchOption.AllDirectories);
        var templates = new List<string>();

        foreach (var path in paths)
        {
            if (Fs.Stat(path).HasFlag(FileAttribute.Directory))
                continue;

            if (Path.Basename(path) == ".env.hbs")
                continue;

            if (Path.Extension(path) == ".hbs")
            {
                templates.Add(path);
                continue;
            }

            var relative = Path.RelativePath(templateDirectory, path);
            var dest = Path.Join(appDir, relative);
            var dir = Path.Dirname(dest)!;
            if (!Fs.DirectoryExits(dir))
                Fs.MakeDirectory(dir);

            Fs.CopyFile(path, dest);
        }

        foreach (var template in templates)
        {
            var relative = Path.RelativePath(templateDirectory, template);
            var dest = Path.Join(appDir, relative.Substring(0, relative.Length - 4));
            var dir = Path.Dirname(dest)!;
            if (!Fs.DirectoryExits(dir))
                Fs.MakeDirectory(dir);

            var content = RenderFile(template, data);
            Fs.WriteTextFile(dest, content);
        }

        return true;
    }

    public static void Register(Domain.Environment global, Domain.Environment currentEnv, Dictionary<string, string> env)
    {
        HandlebarsDotNet.Handlebars.RegisterHelper("env", (c, args) =>
        {
            if (args.Length == 0)
                throw new InvalidOperationException("env helper requires at least one argument");

            var name = args[0]?.ToString();
            if (name is null)
                throw new InvalidOperationException("name must not be null");

            string defaultValue = string.Empty;
            if (args.Length > 1)
                defaultValue = args[1].ToString()!;

            if (!env.TryGetValue(name, out var value))
                return defaultValue;

            return value;
        });

        HandlebarsDotNet.Handlebars.RegisterHelper("env-bool", (c, args) =>
        {
            if (args.Length == 0)
                throw new InvalidOperationException("env helper requires at least one argument");

            var name = args[0]?.ToString();
            if (name is null)
                throw new InvalidOperationException("name must not be null");

            bool defaultValue = false;
            if (args.Length > 1)
                defaultValue = (bool)args[1];

            if (!env.TryGetValue(name, out var value))
                return defaultValue;

            if (value.EqualsIgnoreCase("true") || value.EqualsIgnoreCase("1") || value.EqualsIgnoreCase("yes"))
                return true;

            return false;
        });

        HandlebarsDotNet.Handlebars.RegisterHelper("new-password", (c, args) =>
        {
            if (args.Length == 0)
                throw new InvalidOperationException("env helper requires at least one argument");

            var keyValue = args[0];
            if (keyValue is null)
                throw new InvalidOperationException("keyValue must not be null");

            var key = (string)keyValue;

            var pw = global.GetSecret(key) ?? currentEnv.GetSecret(key);
            if (pw is not null)
                return pw;

            int length = 16;
            string specialChars = "!@#%^&()_|-+";

            if (args.Length > 1 && !int.TryParse(args[1]?.ToString(), out length))
                throw new InvalidOperationException("length must be a number");

            if (args.Length > 2 && args[2] is string)
                specialChars = args[2].ToString()!;

            var pg = new SecretGenerator()
                .Add(SecretCharacterSets.LatinAlphaUpperCase)
                .Add(SecretCharacterSets.LatinAlphaLowerCase)
                .Add(SecretCharacterSets.Digits)
                .Add(specialChars);

            pw = pg.GenerateAsString(length);
            currentEnv.SetSecret(key, pw);

            return pw;
        });
    }

    public static void Register(Domain.Environments environments, Settings settings)
    {
        var global = environments.GetOrCreate("global");
        var envName = settings.Get("env.name");
        var env1 = environments.GetOrCreate(envName);

        var env = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var next in global.Variables)
        {
            env.Add(next.Name, next.Value);
        }

        foreach (var next in env1.Variables)
        {
            env.Add(next.Name, next.Value);
        }

        foreach (var secret in global.Secrets)
        {
            env.Add(secret.Name, secret.Value);
        }

        foreach (var secret in env1.Secrets)
        {
            env.Add(secret.Name, secret.Value);
        }


        HandlebarsDotNet.Handlebars.RegisterHelper("env", (c, args) =>
        {
            if (args.Length == 0)
                throw new InvalidOperationException("env helper requires at least one argument");

            var name = args[0]?.ToString();
            if (name is null)
                throw new InvalidOperationException("name must not be null");

            string defaultValue = string.Empty;
            if (args.Length > 1)
                defaultValue = args[1].ToString()!;

            if (!env.TryGetValue(name, out var value))
                return defaultValue;

            return value;
        });

        HandlebarsDotNet.Handlebars.RegisterHelper("env-bool", (c, args) =>
        {
            if (args.Length == 0)
                throw new InvalidOperationException("env helper requires at least one argument");

            var name = args[0]?.ToString();
            if (name is null)
                throw new InvalidOperationException("name must not be null");

            bool defaultValue = false;
            if (args.Length > 1)
                defaultValue = (bool)args[1];

            if (!env.TryGetValue(name, out var value))
                return defaultValue;

            if (value.EqualsIgnoreCase("true") || value.EqualsIgnoreCase("1") || value.EqualsIgnoreCase("yes"))
                return true;

            return false;
        });

        HandlebarsDotNet.Handlebars.RegisterHelper("new-password", (c, args) =>
        {
            if (args.Length == 0)
                throw new InvalidOperationException("env helper requires at least one argument");

            var keyValue = args[0];
            if (keyValue is null)
                throw new InvalidOperationException("keyValue must not be null");

            var key = (string)keyValue;

            var pw = global.GetSecret(key) ?? env1.GetSecret(key);
            if (pw is not null)
                return pw;

            int length = 16;
            string specialChars = "!@#%^&()_|-+";

            if (args.Length > 1 && !int.TryParse(args[1]?.ToString(), out length))
                throw new InvalidOperationException("length must be a number");

            if (args.Length > 2 && args[2] is string)
                specialChars = args[2].ToString()!;

            var pg = new SecretGenerator()
                .Add(SecretCharacterSets.LatinAlphaUpperCase)
                .Add(SecretCharacterSets.LatinAlphaLowerCase)
                .Add(SecretCharacterSets.Digits)
                .Add(specialChars);

            pw = pg.GenerateAsString(length);
            env1.SetSecret(key, pw);

            return pw;
        });
    }
}