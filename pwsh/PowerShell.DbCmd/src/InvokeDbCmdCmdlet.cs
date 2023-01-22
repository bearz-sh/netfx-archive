using System;
using System.Collections.Generic;
using System.Data;
using System.Management.Automation;

namespace Ze.PowerShell.DbCmd;

[OutputType(typeof(PSObject[]), typeof(PSObject), typeof(int), typeof(object), typeof(List<PSObject[]>))]
public class InvokeDbCmdCmdlet : DbCmdlet
{
    [Parameter(ParameterSetName = "Connection")]
    [Parameter(ParameterSetName = "Transaction")]
    [Parameter(ParameterSetName = "Provider")]
    public ExecutionType ExecutionType { get; set; } = ExecutionType.NonQuery;

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

            switch (this.ExecutionType)
            {
                case ExecutionType.Reader:
                    {
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
                    }

                    break;
                case ExecutionType.Scalar:
                    {
                        var result = cmd.ExecuteScalar();
                        this.WriteObject(result);
                    }

                    break;

                case ExecutionType.NonQuery:
                    {
                        var result = cmd.ExecuteNonQuery();
                        this.WriteObject(result);
                    }

                    break;
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