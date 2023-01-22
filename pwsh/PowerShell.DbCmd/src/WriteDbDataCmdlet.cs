using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Management.Automation;

namespace Ze.PowerShell.DbCmd;

[Cmdlet(VerbsCommunications.Write, "DbData")]
[OutputType(typeof(int[]), typeof(int), typeof(void))]
public class WriteDbDataCmdlet : DbCmdlet
{
    private object? stachedParamaters;

    [Parameter(ParameterSetName = "Connection")]
    [Parameter(ParameterSetName = "Transaction")]
    [Parameter(ParameterSetName = "Provider")]
    public SwitchParameter PassThru { get; set; }

    protected override void BeginProcessing()
    {
        this.stachedParamaters = this.Parameters;
        this.Parameters = null;
    }

    protected override void ProcessRecord()
    {
        var cmd = this.GenerateCommand();
        var connection = cmd.Connection;
        var transaction = cmd.Transaction;
        bool close = false;
        bool commit = false;

        try
        {
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                close = true;
                cmd.Connection.Open();
            }

            if (this.UseTransaction && transaction is null)
            {
                transaction = connection.BeginTransaction();
                commit = true;
            }

            if (this.stachedParamaters is IList<PSObject> psObjects)
            {
                for (var i = 0; i < psObjects.Count; i++)
                {
                    var next = psObjects[i];
                    cmd.AddParameters(next, i != 0, this.ParameterPrefix, this);

                    if (this.PassThru.ToBool())
                    {
                        this.WriteObject(cmd.ExecuteNonQuery());
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                if (commit)
                    transaction.Commit();

                return;
            }

            if (this.stachedParamaters is IList<Hashtable> hashTables)
            {
                for (var i = 0; i < hashTables.Count; i++)
                {
                    var next = hashTables[i];
                    cmd.AddParameters(next, i != 0, this.ParameterPrefix, this);

                    if (this.PassThru.ToBool())
                    {
                        this.WriteObject(cmd.ExecuteNonQuery());
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                if (commit)
                    transaction.Commit();

                return;
            }

            if (this.stachedParamaters is IList<DbParameterCollection> dbParameterCollections)
            {
                for (var i = 0; i < dbParameterCollections.Count; i++)
                {
                    var next = dbParameterCollections[i];
                    cmd.AddParameters(next, i != 0, this.ParameterPrefix, this);

                    if (this.PassThru.ToBool())
                    {
                        this.WriteObject(cmd.ExecuteNonQuery());
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                if (commit)
                    transaction.Commit();

                return;
            }

            cmd.AddParameters(this.Parameters, false, this.ParameterPrefix, this);
            if (this.PassThru.ToBool())
            {
                this.WriteObject(cmd.ExecuteNonQuery());
            }
            else
            {
                cmd.ExecuteNonQuery();
            }

            if (commit)
                transaction.Commit();
        }
        catch
        {
            transaction?.Rollback();
            throw;
        }
        finally
        {
            if (commit)
                transaction?.Dispose();

            if (close)
                connection.Close();

            cmd.Dispose();

            if (this.ConnectionOwned)
                connection.Dispose();
        }
    }
}