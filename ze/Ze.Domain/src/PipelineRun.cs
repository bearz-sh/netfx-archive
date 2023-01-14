using Bearz.Security.Cryptography;
using Bearz.Text;

using Microsoft.EntityFrameworkCore;

using DbPipelineRun = Ze.Data.Models.PipelineRun;

namespace Ze.Domain;

public class PipelineRun
{
    private readonly DbPipelineRun run;

    private readonly DbContext db;

    private readonly IEncryptionProvider cipher;

    internal PipelineRun(DbContext db, DbPipelineRun run, IEncryptionProvider cipher)
    {
        this.db = db;
        this.run = run;
        this.cipher = cipher;
    }

    public string? Id
    {
        get => this.run.ExternalId;
        set => this.run.ExternalId = value;
    }

    public int Revision
    {
        get => this.run.Revision;
        internal set => this.run.Revision = value;
    }

    public DateTime? CreatedAt => this.run.CreatedAt;

    public DateTime? StartedAt
    {
        get => this.run.StartedAt;
        internal set => this.run.StartedAt = value;
    }

    public DateTime? CompletedAt
    {
        get => this.run.CompletedAt;
        internal set => this.run.CompletedAt = value;
    }

    public int Status
    {
        get => this.run.Status;
        set => this.run.Status = value;
    }

    public string? CommitRef
    {
        get => this.run.CommitRef;
        set => this.run.CommitRef = value;
    }

    public string? CommitEmail
    {
        get => this.run.CommitEmail;
        set => this.run.CommitEmail = value;
    }

    public string? State
    {
        get
        {
            if (string.IsNullOrWhiteSpace(this.run.StateJson))
                return null;

            var bytes = Convert.FromBase64String(this.run.StateJson);
            var decrypted = this.cipher.Decrypt(bytes);
            return Encodings.Utf8NoBom.GetString(decrypted);
        }

        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                this.run.StateJson = null;
                return;
            }

            var bytes = Encodings.Utf8NoBom.GetBytes(value);
            var encrypted = this.cipher.Encrypt(bytes);
            this.run.StateJson = Convert.ToBase64String(encrypted);
        }
    }

    internal DbPipelineRun Model => this.run;

    public void Start()
    {
        this.StartedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        this.CompletedAt = DateTime.UtcNow;
    }

    public void Save()
    {
        this.db.SaveChanges();
    }
}