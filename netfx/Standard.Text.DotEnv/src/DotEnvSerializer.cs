using System.Text;

using Std.Text.DotEnv.Serialization;

namespace Std.Text.DotEnv;

public static class DotEnvSerializer
{
    public static Task<IReadOnlyDictionary<string, string>> DeserializeAsync(TextReader reader, CancellationToken cancellationToken)
        => Parser.ParseAsync(reader, cancellationToken);

    public static Task<IReadOnlyDictionary<string, string>> DeserializeAsync(TextReader reader)
        => Parser.ParseAsync(reader);

    public static Task<IReadOnlyDictionary<string, string>> DeserializeAsync(string text, CancellationToken cancellationToken)
    {
        using var reader = new StringReader(text);
        return Parser.ParseAsync(reader, cancellationToken);
    }

    public static Task<IReadOnlyDictionary<string, string>> DeserializeAsync(string text)
    {
        using var reader = new StringReader(text);
        return Parser.ParseAsync(reader);
    }

    public static Task<IReadOnlyDictionary<string, string>> DeserializeAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream, new UTF8Encoding(false));
        return Parser.ParseAsync(reader, cancellationToken);
    }

    public static Task<IReadOnlyDictionary<string, string>> DeserializeAsync(Stream stream)
    {
        using var reader = new StreamReader(stream, new UTF8Encoding(false));
        return Parser.ParseAsync(reader);
    }

    public static IReadOnlyDictionary<string, string> Deserialize(TextReader reader)
        => Parser.Parse(reader);

    public static IReadOnlyDictionary<string, string> Deserialize(string text)
    {
        using var reader = new StringReader(text);
        return Parser.Parse(reader);
    }

    public static IReadOnlyDictionary<string, string> Deserialize(Stream stream, DotEnvSettings settings)
    {
        using var reader = new StreamReader(stream, settings.Encoding);
        return Parser.Parse(reader);
    }
}