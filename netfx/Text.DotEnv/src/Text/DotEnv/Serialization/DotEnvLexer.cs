using System.Text;

namespace Bearz.Text.DotEnv.Serialization;

internal static class DotEnvLexer
{
    private enum Capture
    {
        None,
        SingleQuote,
        DoubleQuote,
        Brackets,
        Backtick,
    }

    private enum Segment
    {
        Key,
        Value,
        Comment,
    }

    public static IReadOnlyList<EnvNode> Scan(TextReader reader)
    {
        var list = new List<EnvNode>();
        var eof = false;
        var buffer = new StringBuilder();
        int lineNumber = 0;
        var capture = Capture.None;
        var segment = Segment.Key;
        while (!eof)
        {
            var nextLine = reader.ReadLine();

            if (nextLine is null)
            {
                eof = true;
                continue;
            }

            lineNumber++;

            if (string.IsNullOrWhiteSpace(nextLine))
            {
                if (capture == Capture.None)
                    continue;

                buffer.AppendLine(nextLine);
                continue;
            }

            int startColumn = 0;
            bool keyHasSpace = false;
            int multiLine = 0;
            int multiLineColumn = 0;

            for (var i = 0; i < nextLine.Length; i++)
            {
                var c = nextLine[i];
                if (segment == Segment.Key)
                {
                    if (c == '#' && buffer.Length == 0)
                    {
                        list.Add(new EnvCommentNode(lineNumber, i + 1, nextLine.Substring(i + 1).AsSpan()));
                        segment = Segment.Comment;
                        break;
                    }

                    if (c == '=')
                    {
                        if (buffer.Length == 0)
                            throw new ParseException($"The environment variable name is missing on line {lineNumber}");

                        var name = buffer.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(name))
                            throw new ParseException($"The environment variable name is missing on line {lineNumber}");

                        list.Add(new EnvNameNode(lineNumber, startColumn, buffer.ToString().AsSpan()));
                        buffer.Clear();
                        segment = Segment.Value;
                        multiLineColumn = i + 1;
                        continue;
                    }

                    if (char.IsLetterOrDigit(c) || c == '_')
                    {
                        if (keyHasSpace)
                        {
                            throw new ParseException(
                                $"The environment variable name is already terminated by a space {lineNumber}");
                        }

                        buffer.Append(c);
                        continue;
                    }

                    if (c == ' ' && buffer.Length != 0)
                    {
                        keyHasSpace = true;
                    }
                }
                else
                {
                    if (buffer.Length == 0)
                    {
                        switch (c)
                        {
                            case '\"':
                                multiLine = lineNumber;
                                multiLineColumn = i + 1;
                                capture = Capture.DoubleQuote;
                                break;
                            case '\'':
                                multiLine = lineNumber;
                                multiLineColumn = i + 1;
                                capture = Capture.SingleQuote;
                                break;
                            case '`':
                                multiLine = lineNumber;
                                multiLineColumn = i + 1;
                                capture = Capture.Backtick;
                                break;
                            case '{':
                                multiLine = lineNumber;
                                multiLineColumn = i + 1;
                                capture = Capture.Brackets;
                                buffer.Append(c);
                                break;
                            default:
                                buffer.Append(c);
                                break;
                        }

                        continue;
                    }

                    if (capture != Capture.None)
                    {
                        bool eov = false;

                        switch (capture)
                        {
                            case Capture.Backtick:
                                if (c == '`')
                                {
                                    // escape \`
                                    if (buffer[^1] == '\\')
                                    {
                                        buffer.Remove(buffer.Length - 1, 1);
                                        buffer.Append(c);
                                        continue;
                                    }

                                    eov = true;
                                }

                                break;
                            case Capture.Brackets:
                                // json must end with } on a new line
                                if (c == '}' && i == 0)
                                {
                                    eov = true;
                                }

                                break;
                            case Capture.DoubleQuote:
                                if (c == '"')
                                {
                                    // escape \"
                                    if (buffer[^1] == '\\')
                                    {
                                        buffer.Remove(buffer.Length - 1, 1);
                                        buffer.Append(c);
                                        continue;
                                    }

                                    eov = true;
                                }

                                break;
                            case Capture.SingleQuote:
                                if (c == '\'')
                                {
                                    // escape \'
                                    if (buffer[^1] == '\\')
                                    {
                                        buffer.Remove(buffer.Length - 1, 1);
                                        buffer.Append(c);
                                        continue;
                                    }

                                    eov = true;
                                }

                                break;
                        }

                        if (eov)
                        {
                            if (capture == Capture.Brackets)
                            {
                                buffer.Append('}');
                            }

                            segment = Segment.Key;
                            var value = buffer.ToString();
                            var node = new EnvScalarNode(
                                new Mark(multiLine, multiLineColumn),
                                new Mark(lineNumber, i + 1),
                                value.AsSpan());
                            buffer.Clear();

                            list.Add(node);
                            break;
                        }
                    }

                    buffer.Append(c);
                }
            }

            if (segment == Segment.Value)
            {
                if (capture != Capture.None)
                {
                    buffer.AppendLine();
                    continue;
                }

                // ReSharper disable once RedundantAssignment
                segment = Segment.Key;

                // trim whitespace to match npms` dotenv package for single line values
                var value = buffer.ToString().Trim();
                buffer.Clear();
                list.Add(new EnvScalarNode(
                    new Mark(lineNumber, multiLineColumn),
                    new Mark(lineNumber, nextLine.Length),
                    value.AsSpan()));
                continue;
            }

            if (segment == Segment.Comment)
            {
                segment = Segment.Key;
                continue;
            }

            // if we get here and the buffer isn't whitespace e.g. empty line
            // the we have a key without a value because we lack and equal symbol.
            if (buffer.Length != 0 && !string.IsNullOrWhiteSpace(buffer.ToString()))
            {
                throw new ParseException($"Environment Variable Name on line {lineNumber} is missing an equals");
            }
        }

        return list;
    }

    public static async Task<IReadOnlyList<EnvNode>> ScanAsync(
        TextReader reader,
        CancellationToken cancellationToken = default)
    {
        var list = new List<EnvNode>();
        var eof = false;
        var buffer = new StringBuilder();
        int lineNumber = 0;
        var capture = Capture.None;
        var segment = Segment.Key;
        while (!eof)
        {
#if NETLEGACY || NET6_0
            var nextLine = await reader.ReadLineAsync()
                .ConfigureAwait(false);
#else
            var nextLine = await reader.ReadLineAsync(cancellationToken)
                .ConfigureAwait(false);
#endif

            if (nextLine is null)
            {
                eof = true;
                continue;
            }

            lineNumber++;

            if (string.IsNullOrWhiteSpace(nextLine))
            {
                if (capture == Capture.None)
                    continue;

                buffer.AppendLine(nextLine);
                continue;
            }

            int startColumn = 0;
            bool keyHasSpace = false;
            int multiLine = 0;
            int multiLineColumn = 0;

            for (var i = 0; i < nextLine.Length; i++)
            {
                var c = nextLine[i];
                if (segment == Segment.Key)
                {
                    if (c == '#' && buffer.Length == 0)
                    {
                        list.Add(new EnvCommentNode(lineNumber, i + 1, nextLine.Substring(i + 1).AsSpan()));
                        segment = Segment.Comment;
                        break;
                    }

                    if (c == '=')
                    {
                        if (buffer.Length == 0)
                            throw new ParseException($"The environment variable name is missing on line {lineNumber}");

                        var name = buffer.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(name))
                            throw new ParseException($"The environment variable name is missing on line {lineNumber}");

                        list.Add(new EnvNameNode(lineNumber, startColumn, buffer.ToString().AsSpan()));
                        buffer.Clear();
                        segment = Segment.Value;
                        multiLineColumn = i + 1;
                        continue;
                    }

                    if (char.IsLetterOrDigit(c) || c == '_')
                    {
                        if (keyHasSpace)
                        {
                            throw new ParseException(
                                $"The environment variable name is already terminated by a space {lineNumber}");
                        }

                        buffer.Append(c);
                        continue;
                    }

                    if (c == ' ' && buffer.Length != 0)
                    {
                        keyHasSpace = true;
                    }
                }
                else
                {
                    if (buffer.Length == 0)
                    {
                        switch (c)
                        {
                            case '\"':
                                multiLine = lineNumber;
                                multiLineColumn = i + 1;
                                capture = Capture.DoubleQuote;
                                break;
                            case '\'':
                                multiLine = lineNumber;
                                multiLineColumn = i + 1;
                                capture = Capture.SingleQuote;
                                break;
                            case '`':
                                multiLine = lineNumber;
                                multiLineColumn = i + 1;
                                capture = Capture.Backtick;
                                break;
                            case '{':
                                multiLine = lineNumber;
                                multiLineColumn = i + 1;
                                capture = Capture.Brackets;
                                buffer.Append(c);
                                break;
                            default:
                                buffer.Append(c);
                                break;
                        }

                        continue;
                    }

                    if (capture != Capture.None)
                    {
                        bool eov = false;

                        switch (capture)
                        {
                            case Capture.Backtick:
                                if (c == '`')
                                {
                                    // escape \`
                                    if (buffer[^1] == '\\')
                                    {
                                        buffer.Remove(buffer.Length - 1, 1);
                                        buffer.Append(c);
                                        continue;
                                    }

                                    eov = true;
                                }

                                break;
                            case Capture.Brackets:
                                // json must end with } on a new line
                                if (c == '}' && i == 0)
                                {
                                    eov = true;
                                }

                                break;
                            case Capture.DoubleQuote:
                                if (c == '"')
                                {
                                    // escape \"
                                    if (buffer[^1] == '\\')
                                    {
                                        buffer.Remove(buffer.Length - 1, 1);
                                        buffer.Append(c);
                                        continue;
                                    }

                                    eov = true;
                                }

                                break;
                            case Capture.SingleQuote:
                                if (c == '\'')
                                {
                                    // escape \'
                                    if (buffer[^1] == '\\')
                                    {
                                        buffer.Remove(buffer.Length - 1, 1);
                                        buffer.Append(c);
                                        continue;
                                    }

                                    eov = true;
                                }

                                break;
                        }

                        if (eov)
                        {
                            if (capture == Capture.Brackets)
                            {
                                buffer.Append('}');
                            }

                            segment = Segment.Key;
                            var value = buffer.ToString();
                            var node = new EnvScalarNode(
                                new Mark(multiLine, multiLineColumn),
                                new Mark(lineNumber, i + 1),
                                value.AsSpan());
                            buffer.Clear();

                            list.Add(node);
                            break;
                        }
                    }

                    buffer.Append(c);
                }
            }

            if (segment == Segment.Value)
            {
                if (capture != Capture.None)
                {
                    buffer.AppendLine();
                    continue;
                }

                // ReSharper disable once RedundantAssignment
                segment = Segment.Key;

                // trim whitespace to match npms` dotenv package for single line values
                var value = buffer.ToString().Trim();
                buffer.Clear();
                list.Add(new EnvScalarNode(
                    new Mark(lineNumber, multiLineColumn),
                    new Mark(lineNumber, nextLine.Length),
                    value.AsSpan()));
                continue;
            }

            if (segment == Segment.Comment)
            {
                segment = Segment.Key;
                continue;
            }

            if (buffer.Length != 0 && !string.IsNullOrWhiteSpace(buffer.ToString()))
            {
                throw new ParseException($"Environment Variable Name on line {lineNumber} is missing an equals");
            }
        }

        return list;
    }
}