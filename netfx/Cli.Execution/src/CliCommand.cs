using System.ComponentModel;
using System.Reflection;

using Bearz.Diagnostics;
using Bearz.Extra.Strings;
using Bearz.Std;
using Bearz.Text;

namespace Bearz.Cli.Execution;

public abstract class CliCommand : ICliCommand
{
    protected CliCommand(string? name)
    {
        this.CommandName = name;
    }

    public string? CommandName { get; private set; }

    public CliStartInfo CliStartInfo { get; protected set; } = new CliStartInfo();

    protected char[] CliParameterPrefix { get; set; } = new[] { '-', '-' };

    protected string CliArrayDelimiter { get; set; } = ",";

    protected char CliAssignmentChar { get; set; } = ' ';

    protected bool AppendCliArguments { get; set; } = false;

    protected Func<string, string> FormatCliOptionName { get; set; } = CliFormatter.FormatPosixOptionName;

    protected IReadOnlyCollection<string> JoinCliArgumentNames { get; set; } = Array.Empty<string>();

    protected IReadOnlyCollection<string> CliArgumentsNames { get; set; } = Array.Empty<string>();

    protected IReadOnlyCollection<string> CliExcludedParameters { get; set; } = Array.Empty<string>();

    public virtual CommandStartInfo Build()
    {
        var t = this.GetType();
        var command = new CommandArgs();
        var properties = t.GetRuntimeProperties()
            .Where(o => o.Name != nameof(this.CommandName) && o.Name != nameof(this.CliStartInfo))
            .ToList();
        var arguments = new CommandArgs();
        var spaceAssignment = this.CliAssignmentChar == ' ';

        // the cli argument names must be in the order required by the command
        // so that when this loop is run, the arguments are added in the correct order.
        foreach (var argName in this.CliArgumentsNames)
        {
            var removal = new List<PropertyInfo>();
            foreach (var property in properties)
            {
                removal.Add(property);
                if (property.Name.Equals(argName, StringComparison.OrdinalIgnoreCase))
                {
                    var value = property.GetValue(this);
                    if (value != null)
                    {
                        if (value is IEnumerable<string> enumerable)
                        {
                            foreach (var item in enumerable)
                            {
                                if (!item.IsNullOrWhiteSpace())
                                    arguments.Add(item);
                            }

                            continue;
                        }

                        var str = value.ToString();
                        if (!str.IsNullOrWhiteSpace())
                            arguments.Add(str);
                    }
                }
            }

            foreach (var property in removal)
            {
                properties.Remove(property);
            }
        }

        foreach (var prop in properties)
        {
            if (this.CliExcludedParameters.Contains(prop.Name))
                continue;

            var value = prop.GetValue(this);
            if (value == null)
            {
                continue;
            }

            if (value is IEnumerable<string> enumerable)
            {
                if (this.JoinCliArgumentNames.Count > 0 && this.JoinCliArgumentNames.Contains(prop.Name))
                {
                    if (!spaceAssignment)
                        command.Add($"{this.FormatCliOptionName(prop.Name)}{this.CliAssignmentChar}{string.Join(this.CliArrayDelimiter, enumerable)}");
                    else
                        command.Add(this.FormatCliOptionName(prop.Name), string.Join(this.CliArrayDelimiter, enumerable));
                }
                else
                {
                    if (!spaceAssignment)
                    {
                        foreach (var item in enumerable)
                        {
                            command.Add($"{this.FormatCliOptionName(prop.Name)}{this.CliAssignmentChar}{item}");
                        }
                    }
                    else
                    {
                        foreach (var item in enumerable)
                        {
                            command.Add(this.FormatCliOptionName(prop.Name), item);
                        }
                    }
                }
            }

            if (value is bool bit)
            {
                if (!bit)
                {
                    continue;
                }

                command.Add(this.FormatCliOptionName(prop.Name));
                continue;
            }

            var str = value.ToString();
            if (str is null)
                continue;

            if (prop.PropertyType.IsEnum)
            {
                var field = prop.PropertyType.GetField(value.ToString()!);
                var descriptionField = field?.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionField is not null)
                {
                    str = descriptionField.Description;
                }
            }

            if (!spaceAssignment)
                command.Add($"{this.FormatCliOptionName(prop.Name)}{this.CliAssignmentChar}{str}");
            else
                command.Add(this.FormatCliOptionName(prop.Name), str);
        }

        if (arguments.Count > 0)
        {
            if (this.AppendCliArguments)
            {
                command.AddRange(arguments);
            }
            else
            {
                command.InsertRange(0, arguments);
            }
        }

        if (!this.CommandName.IsNullOrWhiteSpace())
            command.Insert(0, this.CommandName);

        return new CommandStartInfo()
        {
            Args = command,
            Cwd = this.CliStartInfo.Cwd,
            Env = this.CliStartInfo.Env,
            StdIn = this.CliStartInfo.StdIn,
            StdOut = this.CliStartInfo.StdOut,
            StdErr = this.CliStartInfo.StdErr,
        };
    }
}