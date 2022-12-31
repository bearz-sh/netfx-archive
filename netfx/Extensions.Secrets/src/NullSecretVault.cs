using Bearz.Extensions.Secrets;

namespace Bearz.Extensions.Secrets;

public class NullSecretVault : ISecretsVault, ISecretVaultMetaData
{
    public static readonly NullSecretVault Instance = new NullSecretVault();

    public Task<IEnumerable<string>> ListNamesAsync()
        => Task.FromResult(this.ListNames());

    public IEnumerable<string> ListNames()
    {
        return Array.Empty<string>();
    }

    public Task<string?> GetSecretAsync(string name)
        => Task.FromResult<string?>(null);

    public string? GetSecret(string name)
        => null;

    public Task SetSecretAsync(string name, string secret)
        => Task.CompletedTask;

    public void SetSecret(string name, string secret)
    {
        // noop
    }

    public Task DeleteSecretAsync(string name)
        => Task.CompletedTask;

    public void DeleteSecret(string name)
    {
        // noop;
    }

    public ISecretRecord CreateRecord(string name)
        => new MemorySecretVault.MemorySecretRecord(name);

    public Task<ISecretRecord?> GetSecretRecordAsync(string name)
        => Task.FromResult<ISecretRecord?>(null);

    public ISecretRecord? GetSecretRecord(string name)
        => null;

    public void SetSecretRecord(ISecretRecord record)
    {
        // noop
    }

    public Task SetSecretRecordAsync(ISecretRecord record)
        => Task.CompletedTask;
}