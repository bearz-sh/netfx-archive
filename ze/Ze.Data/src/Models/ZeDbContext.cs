using Microsoft.EntityFrameworkCore;

namespace Ze.Data.Models;

public class ZeDbContext : DbContext
{
#pragma warning disable CS8618
    public ZeDbContext(DbContextOptions options)
#pragma warning restore CS8618
        : base(options)
    {
    }

    public DbSet<EnvironmentSecret> EnvironmentSecrets { get; set; }

    public DbSet<EnvironmentVariable> EnvironmentVariables { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public DbSet<Environment> Environments { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<ProjectRepository> ProjectRepositories { get; set; }

    public DbSet<Pipeline> Pipelines { get; set; }

    public DbSet<PipelineRun> PipelineRuns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("casa_db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var setting = modelBuilder.Entity<Setting>();
        setting.Property(o => o.Name).IsRequired()
            .HasMaxLength(256);
        setting.Property(o => o.Value)
            .IsRequired()
            .HasMaxLength(512);

        var envVar = modelBuilder.Entity<EnvironmentVariable>();
        envVar.Property(o => o.Name).IsRequired()
            .HasMaxLength(256);
        envVar.Property(o => o.Value).IsRequired()
            .HasMaxLength(4096);

        var secret = modelBuilder.Entity<EnvironmentSecret>();
        secret.Property(o => o.Name).IsRequired()
            .HasMaxLength(256);
        secret.Property(o => o.Value).IsRequired()
            .HasMaxLength(4096);

        var env = modelBuilder.Entity<Environment>();
        env.Property(o => o.Name).IsRequired()
            .HasMaxLength(128)
            .IsRequired();
        env.Property(o => o.Slug)
            .HasMaxLength(128)
            .IsRequired();
        env.HasMany(o => o.Secrets)
            .WithOne(o => o.Environment)
            .HasForeignKey(o => o.EnvironmentId);

        env.HasMany(o => o.Variables)
            .WithOne(o => o.Environment)
            .HasForeignKey(o => o.EnvironmentId);

        var project = modelBuilder.Entity<Project>();
        project.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(128);

        project.Property(o => o.Slug)
            .IsRequired()
            .HasMaxLength(128);

        project.HasMany(o => o.Repositories)
            .WithOne(o => o.Project);

        project.HasMany(o => o.Pipelines)
            .WithOne(o => o.Project);

        var repo = modelBuilder.Entity<ProjectRepository>();
        repo.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(256);

        repo.Property(o => o.Url)
            .IsRequired()
            .HasMaxLength(2048);

        repo.Property(o => o.Directory)
            .HasMaxLength(1048);

        var pipeline = modelBuilder.Entity<Pipeline>();
        pipeline.Property(o => o.Name)
            .HasMaxLength(128)
            .IsRequired();

        pipeline.Property(o => o.Slug)
            .HasMaxLength(128)
            .IsRequired();

        pipeline.Property(o => o.DefinitionFile)
            .HasMaxLength(1048);

        pipeline.HasMany(o => o.Runs)
            .WithOne(o => o.Pipeline);

        var pipelineRun = modelBuilder.Entity<PipelineRun>();
        pipelineRun.Property(o => o.CommitRef)
            .HasMaxLength(256);
        pipelineRun.Property(o => o.BranchName)
            .HasMaxLength(256);

        pipelineRun.Property(o => o.CommitEmail)
            .HasMaxLength(256);

        base.OnModelCreating(modelBuilder);
    }
}