namespace Bearz.Virtual.Environment;

public interface IEnvironmentPath : IEnumerable<string>
{
    void Add(string path, bool prepend = false);

    void Remove(string path);

    bool Has(string path);

    void Set(string paths);

    string Get();
}