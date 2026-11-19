namespace Lab1_Vardzar_2_4.Services;

public interface ISyncService
{
    bool IsOnline { get; }
    event EventHandler? StateChanged;

    Task EnqueueAsync(string entityType, string operation, object payload);
    Task<int> GetPendingCountAsync();
    Task SyncAsync();
}