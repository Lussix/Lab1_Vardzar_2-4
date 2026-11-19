using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Lab1_Vardzar_2_4.Models;
using SQLite;

namespace Lab1_Vardzar_2_4.Services;

public class SyncService : ISyncService
{
    private const string ApiUrl = "https://jsonplaceholder.typicode.com/posts";

    private readonly SQLiteAsyncConnection _db;
    private readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(10) };
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _initialized;

    public event EventHandler? StateChanged;

    public SyncService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);

        Connectivity.Current.ConnectivityChanged += async (s, e) =>
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
            if (IsOnline) await SyncAsync();
        };
    }

    public bool IsOnline => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

    private async Task InitAsync()
    {
        if (_initialized) return;
        await _db.CreateTableAsync<SyncQueueItem>();
        _initialized = true;
    }

    public async Task EnqueueAsync(string entityType, string operation, object payload)
    {
        await InitAsync();

        await _db.InsertAsync(new SyncQueueItem
        {
            EntityType = entityType,
            Operation = operation,
            PayloadJson = JsonSerializer.Serialize(payload, payload.GetType()),
            CreatedAt = DateTime.UtcNow
        });

        Debug.WriteLine($"[SYNC] у чергу додано: {operation} {entityType}");
        StateChanged?.Invoke(this, EventArgs.Empty);

        if (IsOnline)
            _ = SyncAsync();  
    }

    public async Task<int> GetPendingCountAsync()
    {
        await InitAsync();
        return await _db.Table<SyncQueueItem>().CountAsync();
    }

    public async Task SyncAsync()
    {
        if (!IsOnline) return;
        if (!await _lock.WaitAsync(0)) return;  

        try
        {
            await InitAsync();
            var items = await _db.Table<SyncQueueItem>().OrderBy(i => i.Id).ToListAsync();

            foreach (var item in items)
            {
                try
                {
                    var response = await SendAsync(item);
                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"[SYNC] сервер відповів {(int)response.StatusCode}, зупиняємось");
                        break;
                    }

                    await _db.DeleteAsync(item);   
                    Debug.WriteLine($"[SYNC] відправлено: {item.Operation} {item.EntityType}");
                }
                catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
                {
                    Debug.WriteLine("[SYNC] немає зв'язку з сервером, лишаємо в черзі");
                    break;
                }
            }
        }
        finally
        {
            _lock.Release();
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private Task<HttpResponseMessage> SendAsync(SyncQueueItem item)
    {
        var content = new StringContent(item.PayloadJson, Encoding.UTF8, "application/json");

        return item.Operation switch
        {
            "Insert" => _http.PostAsync(ApiUrl, content),
            "Update" => _http.PutAsync($"{ApiUrl}/1", content),
            "Delete" => _http.DeleteAsync($"{ApiUrl}/1"),
            _ => throw new InvalidOperationException($"Невідома операція: {item.Operation}")
        };
    }
}