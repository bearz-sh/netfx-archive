namespace Bearz.Extensions.Secrets;

public interface ISecretVaultMetaData
{
    ISecretRecord CreateRecord(string name);

    Task<ISecretRecord?> GetSecretRecordAsync(string name, CancellationToken cancellationToken = default);

    ISecretRecord? GetSecretRecord(string name);

    void SetSecretRecord(ISecretRecord record);

    Task SetSecretRecordAsync(ISecretRecord record, CancellationToken cancellationToken = default);
}