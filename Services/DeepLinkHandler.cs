using System.Diagnostics;
using Lab1_Vardzar_2_4.ViewModels;

namespace Lab1_Vardzar_2_4.Services;

public static class DeepLinkHandler
{
    public static StudentViewModel? Main { get; set; }

    private static Guid? _pendingId;

    public static void Handle(string? uriText)
    {
        Debug.WriteLine($"[DEEPLINK] отримано: {uriText}");

        if (!Uri.TryCreate(uriText, UriKind.Absolute, out var uri)) return;
        if (!Guid.TryParse(uri.AbsolutePath.Trim('/'), out var id)) return;

        _pendingId = id;
        MainThread.BeginInvokeOnMainThread(async () => await TryProcessAsync());
    }

    public static async Task TryProcessAsync()
    {
        if (_pendingId is not Guid id || Main is null || Shell.Current is null)
            return;

        _pendingId = null;

        var student = Main.Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            Debug.WriteLine("[DEEPLINK] студента з таким Id немає");
            return;
        }

        await Task.Delay(300);  

        await Shell.Current.GoToAsync("studentdetail",
            new Dictionary<string, object> { { "SelectedStudent", student } });
    }
}