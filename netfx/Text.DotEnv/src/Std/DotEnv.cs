using Bearz.Text.DotEnv;

namespace Bearz.Std;

public static class DotEnv
{
    public static void Config()
        => DotEnvFile.Config();

    public static void Config(DotEnvSettings settings)
        => DotEnvFile.Config(settings);

    public static void Config(Stream stream)
        => DotEnvFile.Config(stream);

    public static void Config(Stream stream, DotEnvSettings settings)
        => DotEnvFile.Config(stream, settings);

    public static void Config(string data)
        => DotEnvFile.Config(data);

    public static void Config(string data, DotEnvSettings settings)
        => DotEnvFile.Config(data, settings);

    public static void Config(TextReader reader)
        => DotEnvFile.Config(reader);

    public static void Config(TextReader reader, DotEnvSettings settings)
        => DotEnvFile.Config(reader, settings);

    public static void Config(IReadOnlyDictionary<string, string> values)
        => DotEnvFile.Config(values);

    public static void Config(IReadOnlyDictionary<string, string> values, DotEnvSettings settings)
        => DotEnvFile.Config(values, settings);

    public static IReadOnlyDictionary<string, string> Parse(DotEnvSettings settings)
        => DotEnvFile.Parse(settings);

    public static IReadOnlyDictionary<string, string> Parse(string data)
        => DotEnvFile.Parse(data);

    public static IReadOnlyDictionary<string, string> Parse(string data, DotEnvSettings settings)
        => DotEnvFile.Parse(data, settings);

    public static IReadOnlyDictionary<string, string> Parse(Stream stream)
        => DotEnvFile.Parse(stream);

    public static IReadOnlyDictionary<string, string> Parse(Stream stream, DotEnvSettings settings)
        => DotEnvFile.Parse(stream, settings);

    public static IReadOnlyDictionary<string, string> Parse(TextReader reader)
        => DotEnvFile.Parse(reader);

    public static IReadOnlyDictionary<string, string> Parse(TextReader reader, DotEnvSettings settings)
        => DotEnvFile.Parse(reader, settings);
}