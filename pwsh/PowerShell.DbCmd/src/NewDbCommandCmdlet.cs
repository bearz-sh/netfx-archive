using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Management.Automation;

namespace Ze.PowerShell.DbCmd;

[OutputType(typeof(DbCommand), typeof(PSObject), typeof(Collection<PSObject>))]
public class NewDbCommandCmdlet : DbCmdlet
{
    [Parameter(ParameterSetName = "Connection", Position = 0)]
    [Parameter(ParameterSetName = "Transaction", Position = 0)]
    [Parameter(ParameterSetName = "Provider", Position = 0)]
    public ScriptBlock? Do { get; set; }

    protected override void ProcessRecord()
    {
        var command = this.GenerateCommand();
        if (this.Do is null)
        {
            this.WriteObject(command);
            return;
        }

        var connection = command.Connection;
        var transaction = command.Transaction;
        bool close = false;
        if (this.ConnectionOwned || connection.State != ConnectionState.Open)
        {
            close = true;
            connection.Open();
        }

        try
        {
            var variables = new List<PSVariable>()
            {
                new PSVariable("_", command),
                new PSVariable("Command", command),
            };

            var result = this.Do.InvokeWithContext(new Dictionary<string, ScriptBlock>(), variables);
            this.WriteObject(result);

            if (transaction is not null && (this.ConnectionOwned || close))
            {
                transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            if (this.Transaction is not null)
            {
                this.Transaction.Rollback();
            }

            this.WriteError(new ErrorRecord(ex, "DbCmdError", ErrorCategory.NotSpecified, null));
        }
        finally
        {
            if (close)
                connection.Close();

            if (this.ConnectionOwned)
                connection.Dispose();
        }
    }
}