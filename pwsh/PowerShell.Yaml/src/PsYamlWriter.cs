using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Management.Automation;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeResolvers;

namespace Ze.PowerShell.Yaml;

public static class PsYamlWriter
{
    public static void WriteYaml(object? input, SerializationOptions options, TextWriter writer)
    {
        var normalizedObject = Visit(input);
        if (normalizedObject is null)
        {
            return;
        }

        var builder = new SerializerBuilder();

        var o = options;
        if (o.HasFlag(SerializationOptions.Roundtrip))
            builder.EnsureRoundtrip();

        if (o.HasFlag(SerializationOptions.DisableAliases))
            builder.DisableAliases();

        if (o.HasFlag(SerializationOptions.JsonCompatible))
            builder.JsonCompatible();

        if (o.HasFlag(SerializationOptions.DefaultToStaticType))
            builder.WithTypeResolver(new StaticTypeResolver());

        using var sw = new StringWriter();
        var serializer = builder.Build();
        serializer.Serialize(sw, normalizedObject);
    }

    public static object? Visit(object? value)
    {
        switch (value)
        {
            case OrderedDictionary od:
                return Visit(od);
            case IDictionary d:
                return Visit(d);

            case PSObject pso:
                return Visit(pso);

            case IList l:
                return Visit(l);

            default:
                return value;
        }
    }

    public static object Visit(IList value)
    {
        var list = new List<object?>();
        foreach (var item in value)
        {
            list.Add(Visit(item));
        }

        return list;
    }

    public static object Visit(IDictionary value)
    {
        var dict = new Dictionary<string, object?>();
        foreach (var key in value.Keys)
        {
            if (key is string name)
            {
                dict.Add(name, Visit(value[key]));
            }
        }

        return dict;
    }

    public static object Visit(OrderedDictionary dictionary)
    {
        var ordered = new OrderedDictionary();
        foreach (var key in dictionary.Keys)
        {
            ordered.Add(key, Visit(dictionary[key]));
        }

        return ordered;
    }

    public static object Visit(PSObject value)
    {
        var dict = new OrderedDictionary();
        foreach (var prop in value.Properties)
        {
            dict.Add(prop.Name, Visit(prop.Value));
        }

        return dict;
    }
}