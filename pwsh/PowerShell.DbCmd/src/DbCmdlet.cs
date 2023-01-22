using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Management.Automation;

using Bearz.Extra.Strings;

namespace Ze.PowerShell.DbCmd;

public abstract class DbCmdlet : PSCmdlet
{
    [Parameter(ParameterSetName = "Connection", Mandatory = true, ValueFromPipeline = true)]
    public DbConnection? Connection { get; set; }

    [Parameter(ParameterSetName = "Transaction", Mandatory = true, ValueFromPipeline = true)]
    public DbTransaction? Transaction { get; set; }

    [Parameter(ParameterSetName = "Provider")]
    public string? ProviderName { get; set; }

    [Parameter(ParameterSetName = "Connection", Position = 0)]
    [Parameter(ParameterSetName = "Transaction", Position = 0)]
    [Parameter(ParameterSetName = "Provider", Position = 0)]
    public string? Query { get; set; }

    [Parameter(ParameterSetName = "Connection", Position = 1)]
    [Parameter(ParameterSetName = "Transaction", Position = 1)]
    [Parameter(ParameterSetName = "Provider", Position = 1)]
    public object? Parameters { get; set; }

    [Parameter(ParameterSetName = "Connection")]
    [Parameter(ParameterSetName = "Transaction")]
    [Parameter(ParameterSetName = "Provider")]
    public string? ParameterPrefix { get; set; } = "@";

    [Parameter(ParameterSetName = "Provider")]
    public string? ConnectionString { get; set; }

    [Parameter(ParameterSetName = "Provider")]
    public string? ConnectionStringName { get; set; }

    [Parameter(ParameterSetName = "Connection")]
    [Parameter(ParameterSetName = "Transaction")]
    [Parameter(ParameterSetName = "Provider")]
    public CommandType CommandType { get; set; } = CommandType.Text;

    [Parameter(ParameterSetName = "Connection")]
    [Parameter(ParameterSetName = "Transaction")]
    [Parameter(ParameterSetName = "Provider")]
    public SwitchParameter UseTransaction { get; set; }

    protected bool ConnectionOwned { get; set; } = false;

    protected virtual DbCommand GenerateCommand()
    {
        DbCommand command;
        if (this.Transaction is not null)
        {
            var connection = this.Transaction.Connection;
            if (connection is null)
                throw new InvalidOperationException("Transaction is not associated with a connection.");

            command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = this.Transaction;
        }
        else if (this.Connection is not null)
        {
            command = this.Connection.CreateCommand();
            command.Connection = this.Connection;
            if (this.UseTransaction)
            {
                command.Transaction = this.Connection.BeginTransaction();
            }
        }
        else
        {
            this.ConnectionStringName ??= "default";
            var cs = this.ConnectionString ?? DbCmdModuleState.GetConnectionString(this.ConnectionStringName);
            var factory = DbCmdModuleState.GetFactory(this.ProviderName);
            var connection = factory.CreateRequiredConnection();
            command = connection.CreateCommand();
            command.Connection = connection;
            connection.ConnectionString = cs;
            this.ConnectionOwned = true;
            if (this.UseTransaction)
            {
                command.Transaction = connection.BeginTransaction();
            }
        }

        command.CommandType = this.CommandType;
        command.CommandText = this.Query;
        command.AddParameters(this.Parameters, false, this.ParameterPrefix, this);
        return command;
    }
}