namespace Bearz.Extensions.Secrets;

public interface ISecretVaultMetaData
{
    ISecretRecord CreateRecord(string name);

    Task<ISecretRecord?> GetSecretRecordAsync(string name);

    ISecretRecord? GetSecretRecord(string name);

    void SetSecretRecord(ISecretRecord record);

    Task SetSecretRecordAsync(ISecretRecord record);
}