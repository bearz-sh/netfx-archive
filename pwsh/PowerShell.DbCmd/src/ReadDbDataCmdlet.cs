using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Management.Automation;

namespace Ze.PowerShell.DbCmd;

[Cmdlet(VerbsCommunications.Read, "DbData")]
[OutputType(typeof(List<PSObject[]>), typeof(PSObject[]), typeof(PSObject))]
public class ReadDbDataCmdlet : DbCmdlet
{

    protected override void ProcessRecord()
    {
        var cmd = this.GenerateCommand();
        var connection = cmd.Connection;
        if (connection is null)
            throw new InvalidOperationException("The connection for the command is null");
        var transaction = cmd.Transaction;
        bool close = false;
        bool commit = false;

        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                close = true;
                connection.Open();
            }

            if (this.UseTransaction && transaction is null)
            {
                transaction = connection.BeginTransaction();
                commit = true;
            }

            using var dr = cmd.ExecuteReader();
            var multiple = new List<PSObject[]>();
            do
            {
                var set = new List<PSObject>();
                while (dr.Read())
                {
                    var row = new PSObject();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        row.Members.Add(new PSNoteProperty(dr.GetName(i), dr.GetValue(i)));
                    }

                    set.Add(row);
                }

                multiple.Add(set.ToArray());
            }
            while (dr.NextResult());

            if (multiple.Count > 1)
            {
                this.WriteObject(multiple);
            }
            else if (multiple.Count == 1)
            {
                var set = multiple[0];
                if (set.Length == 1)
                {
                    this.WriteObject(set[0]);
                }
                else
                {
                    this.WriteObject(set);
                }
            }

            if (commit)
                transaction?.Commit();
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