namespace Bearz.Extensions.Secrets;

public interface ISecretsVault
{
    Task<IEnumerable<string>> ListNamesAsync();

    IEnumerable<string> ListNames();

    Task<string?> GetSecretAsync(string name);

    string? GetSecret(string name);

    Task SetSecretAsync(string name, string secret);

    void SetSecret(string name, string secret);

    Task DeleteSecretAsync(string name);

    void DeleteSecret(string name);
}