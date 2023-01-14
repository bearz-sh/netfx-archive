using Microsoft.EntityFrameworkCore;

using DbProjectRepo = Ze.Data.Models.ProjectRepository;

namespace Ze.Domain;

public class ProjectRepository
{
    private readonly DbProjectRepo repo;

    internal ProjectRepository(DbProjectRepo repo)
    {
        this.repo = repo;
    }

    public string Url
    {
        get => this.repo.Url;
        set => this.repo.Url = value;
    }

    public string Name
    {
        get => this.repo.Name;
        set => this.repo.Name = value;
    }

    public string? Directory
    {
        get => this.repo.Directory;
        set => this.repo.Directory = value;
    }

    internal DbProjectRepo Model => this.repo;
}