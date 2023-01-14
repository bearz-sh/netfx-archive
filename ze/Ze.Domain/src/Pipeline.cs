using Bearz.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

using DbPipeline = Ze.Data.Models.Pipeline;
using DbPipelineRun = Ze.Data.Models.PipelineRun;

namespace Ze.Domain;

public class Pipeline
{
    private readonly DbPipeline pipeline;

    private readonly DbContext db;

    private readonly IEncryptionProvider cipher;

    private readonly HashSet<PipelineRun> runs = new();

    internal Pipeline(DbContext db, DbPipeline pipeline, IEncryptionProvider cipher)
    {
        this.pipeline = pipeline;
        this.db = db;
        this.cipher = cipher;
        foreach (var run in pipeline.Runs)
        {
            this.runs.Add(new PipelineRun(db, run, cipher));
        }
    }

    public string Name
    {
        get => this.pipeline.Name;

        set
        {
            this.pipeline.Name = value;
            this.pipeline.Slug = value.ToEnvName();
        }
    }

    public string? Description
    {
        get => this.pipeline.Description;
        set => this.pipeline.Description = value;
    }

    public string? DefinitionFile
    {
        get => this.pipeline.DefinitionFile;
        set => this.pipeline.DefinitionFile = value;
    }

    public string? JsonDefinition
    {
        get => this.pipeline.JsonDefinition;
        set => this.pipeline.JsonDefinition = value;
    }

    public IEnumerable<PipelineRun> Runs => this.runs;

    internal DbPipeline Model => this.pipeline;

    public void Save()
    {
        this.db.SaveChanges();
    }

    public PipelineRun AddRun()
    {
        var now = DateTime.UtcNow;
        var startOfToday = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);

        var rev = this.Runs.Count(o => o.CreatedAt >= startOfToday);
        rev++;

        var dbRun = new DbPipelineRun()
        {
            Pipeline = this.Model,
            CreatedAt = DateTime.UtcNow,
            Revision = rev,
        };

        var run = new PipelineRun(this.db, dbRun, this.cipher);
        this.db.Set<DbPipelineRun>().Add(dbRun);
        this.runs.Add(run);
        this.Save();
        return run;
    }
}