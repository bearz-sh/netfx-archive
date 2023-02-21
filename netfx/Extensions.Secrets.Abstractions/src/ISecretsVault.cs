namespace Bearz.Extensions.Secrets;

public interface ISecretsVault
{
    Task<IEnumerable<string>> ListNamesAsync(CancellationToken cancellationToken = default);

    IEnumerable<string> ListNames();

    Task<string?> GetSecretAsync(string name, CancellationToken cancellationToken = default);

    string? GetSecret(string name);

    Task SetSecretAsync(string name, string secret,  CancellationToken cancellationToken = default);

    void SetSecret(string name, string secret);

    Task DeleteSecretAsync(string name, CancellationToken cancellationToken = default);

    void DeleteSecret(string name);
}