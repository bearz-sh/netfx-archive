#if !NETLEGACY

namespace Bearz.Extensions.Secrets;

public class AesGcmFileVault : ISecretsVault, ISecretVaultMetaData
{
    public Task<IEnumerable<string>> ListNamesAsync()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> ListNames()
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetSecretAsync(string name)
    {
        throw new NotImplementedException();
    }

    public string? GetSecret(string name)
    {
        throw new NotImplementedException();
    }

    public Task SetSecretAsync(string name, string secret)
    {
        throw new NotImplementedException();
    }

    public void SetSecret(string name, string secret)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSecretAsync(string name)
    {
        throw new NotImplementedException();
    }

    public void DeleteSecret(string name)
    {
        throw new NotImplementedException();
    }

    public ISecretRecord CreateRecord(string name)
    {
        throw new NotImplementedException();
    }

    public Task<ISecretRecord?> GetSecretRecordAsync(string name)
    {
        throw new NotImplementedException();
    }

    public ISecretRecord? GetSecretRecord(string name)
    {
        throw new NotImplementedException();
    }

    public void SetSecretRecord(ISecretRecord record)
    {
        throw new NotImplementedException();
    }

    public Task SetSecretRecordAsync(ISecretRecord record)
    {
        throw new NotImplementedException();
    }
}

#endif