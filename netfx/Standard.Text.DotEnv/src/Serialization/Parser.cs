using Std.Collections.Generic;

namespace Std.Text.DotEnv.Serialization;

internal static class Parser
{
    public static IReadOnlyDictionary<string, string> Parse(TextReader reader)
    {
        var nodes = DotEnvLexer.Scan(reader);
        var result = new OrderedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var key = string.Empty;
        foreach (var node in nodes)
        {
            if (node is EnvCommentNode)
                continue;

            if (key.Length == 0)
            {
                if (node is not EnvNameNode nameNode)
                    throw new InvalidOperationException("EnvNameNode expected");
                key = nameNode.Value.ToString();
                continue;
            }

            if (node is not EnvScalarNode scalarNode)
            {
                throw new InvalidOperationException("ScalarNode expected");
            }

            result.Add(key, scalarNode.Value.ToString());
            key = string.Empty;
        }

        return result;
    }

    public static async Task<IReadOnlyDictionary<string, string>> ParseAsync(
        TextReader reader,
        CancellationToken cancellationToken = default)
    {
        var nodes = await DotEnvLexer.ScanAsync(reader, cancellationToken);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var key = string.Empty;
        foreach (var node in nodes)
        {
            if (node is EnvCommentNode)
                continue;

            if (key.Length == 0)
            {
                if (node is not EnvNameNode nameNode)
                    throw new InvalidOperationException("EnvNameNode expected");
                key = nameNode.Value.ToString();
                continue;
            }

            if (node is not EnvScalarNode scalarNode)
            {
                throw new InvalidOperationException("ScalarNode expected");
            }

            result.Add(key, scalarNode.Value.ToString());
            key = string.Empty;
        }

        return result;
    }
}