using Bearz.Extensions.Secrets;
using Bearz.Extra.Strings;
using Bearz.Secrets;

using HandlebarsDotNet;

namespace Bearz.Templates.Handlebars;

public static class SecretHelpers
{
    public static void GetSecretValue(ISecretsVault vault, EncodedTextWriter writer, Context context, Arguments arguments)
    {
        if (arguments.Length == 0) throw new InvalidOperationException("conf helper requires at least one argument");

        var key = arguments[0].ToString();
        if (key.IsNullOrWhiteSpace())
            throw new InvalidOperationException("key must not be null or whitespace");

        var secret = vault.GetSecret(key);
        if (secret is not null)
        {
            writer.WriteSafeString(secret);
            return;
        }

        int length = 16;
        string special = "~`#@|:;^-_/";
        bool excludeUpper = false;
        bool excludeLower = false;
        bool excludeNumber = false;

        for (var i = 2; i < arguments.Length; i++)
        {
            var arg = arguments[i];
            if (i == 2)
            {
                length = arg.AsInt32(16);
                continue;
            }

            if (i == 3)
            {
                special = arg.AsString("~`#@|:;^-_/");
                continue;
            }

            if (i == 4)
            {
                excludeUpper = arg.AsBool();
                continue;
            }

            if (i == 5)
            {
                excludeLower = arg.AsBool();
                continue;
            }

            if (i == 6)
            {
                excludeNumber = arg.AsBool();
                break;
            }
        }

        var sg = new SecretGenerator();
        if (!excludeNumber)
            sg.Add(SecretCharacterSets.Digits);

        if (!excludeLower)
            sg.Add(SecretCharacterSets.LatinAlphaLowerCase);

        if (!excludeUpper)
            sg.Add(SecretCharacterSets.LatinAlphaUpperCase);

        sg.Add(special.IsNullOrWhiteSpace() ? SecretCharacterSets.SpecialSafe : special);

        secret = sg.GenerateAsString(length);
        vault.SetSecret(key, secret);

        writer.WriteSafeString(secret);
    }

    [CLSCompliant(false)]
    public static void RegisterSecretHelpers(this IHandlebars? hb, ISecretsVault vault)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("secret", (w, c, a) => GetSecretValue(vault, w, c, a));
            return;
        }

        hb.RegisterHelper("secret", (w, c, a) => GetSecretValue(vault, w, c, a));
    }
}