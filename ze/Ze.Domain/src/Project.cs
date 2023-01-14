using Bearz.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

using DbPipeline = Ze.Data.Models.Pipeline;
using DbProject = Ze.Data.Models.Project;
using DbProjectRepo = Ze.Data.Models.ProjectRepository;

namespace Ze.Domain;

public class Project
{
    private readonly DbProject dbProject;

    private readonly DbContext db;

    private readonly HashSet<ProjectRepository> repositories = new();

    private readonly HashSet<Pipeline> pipelines = new();

    private readonly IEncryptionProvider cipher;

    private ProjectRepository? defaultRepository;

    internal Project(DbContext db, DbProject dbProject, IEncryptionProvider cipher)
    {
        this.cipher = cipher;
        this.db = db;
        this.dbProject = dbProject;
        foreach (var repo in dbProject.Repositories)
        {
            var r = new ProjectRepository(repo);
            this.repositories.Add(r);
            if (repo.Default)
                this.defaultRepository = r;
        }
    }

    public string Name
    {
        get => this.dbProject.Name;
        set
        {
            this.dbProject.Name = value;
            this.dbProject.Name = value.ToEnvName();
        }
    }

    public ProjectRepository? DefaultRepository => this.defaultRepository;

    public IEnumerable<ProjectRepository> Repos => this.repositories;

    public IEnumerable<Pipeline> Pipelines => this.pipelines;

    public Pipeline AddPipeline(string name)
    {
        var dbPipeline = new DbPipeline
        {
            Name = name,
            Project = this.dbProject,
        };

        this.db.Add(dbPipeline);
        var pipeline = new Pipeline(this.db, dbPipeline, this.cipher);
        this.pipelines.Add(pipeline);
        return pipeline;
    }

    public ProjectRepository AddRepository(string name, string url, string? directory = null, bool defaultRepo = false)
    {
        if (this.repositories.Count == 0)
            defaultRepo = true;

        if (defaultRepo)
        {
            foreach (var r in this.dbProject.Repositories)
            {
                r.Default = false;
            }
        }

        var dbRepo = new DbProjectRepo
        {
            Name = name,
            Url = url,
            Directory = directory,
            Project = this.dbProject,
            Default = defaultRepo,
        };

        var pr = new ProjectRepository(dbRepo);

        this.dbProject.Repositories.Add(dbRepo);
        this.repositories.Add(pr);

        if (defaultRepo)
            this.defaultRepository = pr;

        return pr;
    }

    public void Save()
    {
        this.db.SaveChanges();
    }
}