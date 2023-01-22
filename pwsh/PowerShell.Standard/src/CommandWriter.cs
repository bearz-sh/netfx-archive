using Bearz.Extra.Strings;
using Bearz.Secrets;
using Bearz.Std;

namespace Ze.Powershell.Standard;

public static class CommandWriter
{
    public static Action<string>? WriteMessage { get; set; }

    public static void Write(string command, CommandArgs? args = null)
    {
        args ??= new CommandArgs();
        var message = $"{command} {args}";
        message = SecretMasker.Default.Mask(message);

        if (WriteMessage is not null)
        {
            WriteMessage(message);
            return;
        }

        if (Env.Get("TF_BUILD")?.EqualsIgnoreCase("true") == true)
        {
            Console.WriteLine($"##[command]{message}");
        }
        else if (Env.Get("GITHUB_ACTIONS")?.EqualsIgnoreCase("true") == true)
        {
            Console.WriteLine($"::notice::{message}");
        }
        else
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}