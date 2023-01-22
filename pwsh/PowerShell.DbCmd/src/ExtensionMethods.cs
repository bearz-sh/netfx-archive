using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace Ze.PowerShell.DbCmd;

[SuppressMessage("Major Code Smell", "S112:General exceptions should never be thrown")]
internal static class ExtensionMethods
{
    public static void AddParameters(this DbCommand command, object? parameters, bool update = false, string? parameterPrefix = "@",  PSCmdlet? cmdlet = null)
    {
        if (parameters == null)
        {
            return;
        }

        parameterPrefix ??= "@";
        if (parameters is PSObject psObject)
        {
            foreach (var t in psObject.Properties)
            {
                var name = t.Name;
                if (!name.StartsWith(parameterPrefix))
                    name = $"{parameterPrefix}{name}";

                if (update)
                {
                    command.Parameters[name].Value = t.Value;
                }
                else
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = name;
                    parameter.Value = t.Value;
                    command.Parameters.Add(parameter);
                }
            }
        }

        if (parameters is DbParameter singleDbParameter)
        {
            if (update)
                command.Parameters[singleDbParameter.ParameterName] = singleDbParameter;
            else
                command.Parameters.Add(singleDbParameter);
            return;
        }

        if (parameters is DbParameterCollection dbParameterCollection)
        {
            foreach (DbParameter parameter in dbParameterCollection)
            {
                if (update)
                    command.Parameters[parameter.ParameterName] = parameter;
                else
                    command.Parameters.Add(parameter);
            }

            return;
        }

        if (parameters is IDictionary dictionary)
        {
            foreach (var key in dictionary.Keys)
            {
                if (key is not string name)
                    continue;

                var value = dictionary[key];
                if (value is DbParameter dbParameter)
                {
                    if (update)
                    {
                        command.Parameters[name] = dbParameter;
                    }
                    else
                    {
                        command.Parameters.Add(dbParameter);
                    }
                }

                if (!name.StartsWith(parameterPrefix))
                    name = $"{parameterPrefix}{name}";

                if (update)
                {
                    command.Parameters[name].Value = dictionary[key];
                }
                else
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = name;
                    parameter.Value = dictionary[key];
                    command.Parameters.Add(parameter);
                }
            }

            return;
        }

        if (parameters is IDictionary<string, DbParameter> dbParameters)
        {
            foreach (var key in dbParameters.Keys)
            {
                if (update)
                {
                    command.Parameters[key] = dbParameters[key];
                }
                else
                {
                    command.Parameters.Add(dbParameters[key]);
                }
            }

            return;
        }

        if (parameters is IDictionary<string, object?> genericDictionary)
        {
            foreach (var kvp in genericDictionary)
            {
                var parameter = command.CreateParameter();
                var name = kvp.Key;
                if (!name.StartsWith(parameterPrefix))
                    name = $"{parameterPrefix}{kvp.Key}";
                parameter.ParameterName = name;
                parameter.Value = kvp.Value;
                command.Parameters.Add(parameter);
                if (update)
                {
                    command.Parameters[name].Value = genericDictionary[name];
                }
                else
                {
                    parameter.ParameterName = name;
                    parameter.Value = genericDictionary[name];
                    command.Parameters.Add(parameter);
                }
            }
        }

        if (parameters is IList list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item is DbParameter parameter)
                {
                    if (update)
                    {
                        command.Parameters[parameter.ParameterName].Value = parameter.Value;
                    }
                    else
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                else
                {
                    var name = $"{parameterPrefix}{i}";
                    if (update)
                    {
                        command.Parameters[name].Value = item;
                    }
                    else
                    {
                        parameter = command.CreateParameter();
                        parameter.ParameterName = name;
                        parameter.Value = item;
                        command.Parameters.Add(parameter);
                    }
                }
            }

            return;
        }

        if (parameters is IList<DbParameter> genericList)
        {
            for (var i = 0; i < genericList.Count; i++)
            {
                var parameter = genericList[i];
                if (update)
                {
                    command.Parameters[parameter.ParameterName].Value = parameter.Value;
                }
                else
                {
                    command.Parameters.Add(parameter);
                }
            }

            return;
        }

        if (parameters is IList<object> objectList)
        {
            for (var i = 0; i < objectList.Count; i++)
            {
                var item = objectList[i];
                if (item is DbParameter parameter)
                {
                    if (update)
                    {
                        command.Parameters[parameter.ParameterName].Value = parameter.Value;
                    }
                    else
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                else
                {
                    var name = $"{parameterPrefix}{i}";
                    if (update)
                    {
                        command.Parameters[name].Value = item;
                    }
                    else
                    {
                        parameter = command.CreateParameter();
                        parameter.ParameterName = name;
                        parameter.Value = item;
                        command.Parameters.Add(parameter);
                    }
                }
            }
        }
        else
        {
            throw new Exception("Parameters must be a hashtable");
        }
    }

    public static DbConnection CreateRequiredConnection(this DbProviderFactory factory)
    {
        var connection = factory.CreateConnection();
        if (connection is null)
            throw new NullReferenceException($"The DbProviderFactory {factory.GetType().FullName} returned a null {nameof(DbConnection)}.");

        return connection;
    }

    public static DbCommandBuilder CreateRequiredCommandBuilder(this DbProviderFactory factory)
    {
        var command = factory.CreateCommandBuilder();
        if (command is null)
            throw new NullReferenceException($"The DbProviderFactory {factory.GetType().FullName} returned a null {nameof(DbCommand)}.");

        return command;
    }

    public static DbCommand CreateRequiredCommand(this DbProviderFactory factory)
    {
        var command = factory.CreateCommand();
        if (command is null)
            throw new NullReferenceException($"The DbProviderFactory {factory.GetType().FullName} returned a null {nameof(DbCommand)}.");

        return command;
    }
}