using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Management.Automation;
using System.Text.RegularExpressions;

using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Ze.PowerShell.Yaml;

public static class PsYamlReader
{
    private const string YamlDateTimePattern = @"  
# From the YAML spec: https://yaml.org/type/timestamp.html
[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9] # (ymd)
|[0-9][0-9][0-9][0-9] # (year)
 -[0-9][0-9]? # (month)
 -[0-9][0-9]? # (day)
 ([Tt]|[ \t]+)[0-9][0-9]? # (hour)
 :[0-9][0-9] # (minute)
 :[0-9][0-9] # (second)
 (\.[0-9]*)? # (fraction)
 (([ \t]*)Z|[-+][0-9][0-9]?(:[0-9][0-9])?)? # (time zone)
@";

    public static object? ReadYaml(string? yaml, bool merge, bool all, bool asHashtable)
    {
        if (yaml.IsNullOrWhiteSpace())
            return null;

        using var sr = new System.IO.StringReader(yaml);
        IParser parser = new Parser(sr);
        if (merge)
            parser = new MergingParser(parser);

        var stream = new YamlStream();
        stream.Load(parser);

        if (stream.Documents.Count == 0)
            return null;

        if (stream.Documents.Count == 1 || !all)
        {
            return Visit(stream.Documents[0].RootNode, asHashtable);
        }

        var list = new List<object?>();
        foreach (var doc in stream.Documents)
        {
            list.Add(Visit(doc.RootNode, asHashtable));
        }

        return list.ToArray();
    }

    private static object? Visit(YamlNode node, bool asHashtable)
    {
        switch (node)
        {
            case YamlScalarNode scalar:
                return Visit(scalar);
            case YamlSequenceNode sequence:
                return Visit(sequence, asHashtable);
            case YamlMappingNode mapping:
                return Visit(mapping, asHashtable);
            default:
                throw new NotSupportedException();
        }
    }

    private static object Visit(YamlSequenceNode sequence, bool asHashtable)
    {
        var list = new List<object?>();
        foreach (var item in sequence.Children)
        {
            list.Add(Visit(item, asHashtable));
        }

        return list.ToArray();
    }

    private static object Visit(YamlMappingNode node, bool asHashtable)
    {
        if (asHashtable)
        {
            var ordered = new OrderedDictionary();
            foreach (var child in node.Children)
            {
                if (child.Key is YamlScalarNode label)
                {
                    if (label.Value is null)
                        continue;

                    var value = Visit(child.Value, true);
                    ordered.Add(label.Value, value);
                }
            }

            return ordered;
        }

        var obj = new PSObject();
        foreach (var child in node.Children)
        {
            if (child.Key is YamlScalarNode label)
            {
                if (label.Value is null)
                    continue;

                var value = Visit(child.Value, true);
                obj.Properties.Add(new PSNoteProperty(label.Value, value));
            }
        }

        return obj;
    }

    private static object? Visit(YamlScalarNode node)
    {
        if (node.Value is null)
            return null;

        if (node.Value.Length == 0)
            return null;

        if (node.Value.Equals("null", StringComparison.OrdinalIgnoreCase) || node.Value == "~")
            return null;

        if (string.IsNullOrWhiteSpace(node.Value))
            return node.Value;

        if (!node.Tag.Value.IsNullOrWhiteSpace())
        {
            switch (node.Tag.Value)
            {
                case "tag:yaml.org,2002:bool":
                    {
                        if (bool.TryParse(node.Value, out var b))
                            return b;
                    }

                    break;
                case "tag:yaml.org,2002:int":
                    {
                        if (short.TryParse(node.Value, out var s))
                            return s;

                        if (int.TryParse(node.Value, out var i))
                            return i;

                        if (long.TryParse(node.Value, out var l))
                            return l;
                    }

                    break;

                case "tag:yaml.org,2002:float":
                    {
                        if (double.TryParse(node.Value, out var d))
                            return d;
                    }

                    break;
                case "tag:yaml.org,2002:timestamp":
                    {
                        if (DateTime.TryParse(node.Value, out var dt))
                            return dt;
                    }

                    break;

                case "tag:yaml.org,2002:binary":
                    {
                        return Convert.FromBase64String(node.Value.Trim());
                    }
            }
        }

        if (node.Style == ScalarStyle.Plain)
        {
            if (bool.TryParse(node.Value, out var bit))
                return bit;

            bool isNumeric = double.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number);
            if (isNumeric)
            {
                if (short.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture,  out var s))
                    return s;

                if (int.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var integer))
                    return integer;

                if (long.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var l))
                    return l;

                if (decimal.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture,  out var dec))
                    return dec;

                return number;
            }

            if (Regex.IsMatch(node.Value, YamlDateTimePattern, RegexOptions.IgnorePatternWhitespace) &&
                DateTime.TryParse(node.Value, out var dt))
                return dt;
        }

        return node.Value;
    }
}