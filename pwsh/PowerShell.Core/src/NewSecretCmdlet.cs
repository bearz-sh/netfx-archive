using System.Management.Automation;
using System.Security;

using Bearz.Extra.Strings;
using Bearz.Secrets;

namespace Ze.PowerShell.Core;

[Cmdlet(VerbsCommon.New, "Secret")]
[OutputType(typeof(string), typeof(char[]), typeof(SecureString), typeof(byte[]))]
public class NewSecretCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    public int Length { get; set; } = 16;

    [Parameter(Position = 1, ValueFromPipelineByPropertyName = true)]
    public string? Characters { get; set; }

    [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
    public ScriptBlock? Validator { get; set; } = null!;

    [Parameter]
    public SwitchParameter AsString { get; set; }

    [Parameter]
    public SwitchParameter AsBytes { get; set; }

    [Parameter]
    public SwitchParameter AsSecureString { get; set; }

    protected override void ProcessRecord()
    {
        var pg = new SecretGenerator();
        if (this.Characters.IsNullOrWhiteSpace())
        {
            pg.AddDefaults();
        }
        else
        {
            pg.Add(this.Characters);
        }

        if (this.Validator is not null)
        {
            pg.SetValidator((chars) =>
            {
                var r = this.Validator.Invoke(chars);
                if (r.Count == 1)
                {
                    return r[0].BaseObject is bool b && b;
                }

                return false;
            });
        }

        if (this.AsString.ToBool())
        {
            this.WriteObject(pg.GenerateAsString(this.Length));
            return;
        }

        if (this.AsBytes.ToBool())
        {
            var bytes = pg.GenerateAsBytes(this.Length);
            this.WriteObject(bytes, false);
            return;
        }

        if (this.AsSecureString.ToBool())
        {
            this.WriteObject(pg.GenerateAsSecureString(this.Length));
            return;
        }

        var secret = pg.Generate(this.Length);
        this.WriteObject(secret, false);
    }
}