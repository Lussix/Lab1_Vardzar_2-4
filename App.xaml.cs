using System.Diagnostics;
using Lab1_Vardzar_2_4.Services;

namespace Lab1_Vardzar_2_4;

public partial class App : Application
{
    private readonly IAppStateService _state;

    public App(IAppStateService state)
    {
        InitializeComponent();
        _state = state;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        Debug.WriteLine("[LIFECYCLE] CreateWindow: вікно створено");

        window.Created += (s, e) => Debug.WriteLine("[LIFECYCLE] Created");
        window.Activated += (s, e) => Debug.WriteLine("[LIFECYCLE] Activated: у фокусі");
        window.Deactivated += (s, e) => Debug.WriteLine("[LIFECYCLE] Deactivated: втратило фокус");

        window.Stopped += (s, e) =>
        {
            Debug.WriteLine("[LIFECYCLE] Stopped: застосунок згорнуто");
            _state.Save();
        };
        window.Resumed += (s, e) =>
        {
            Debug.WriteLine("[LIFECYCLE] Resumed: застосунок повернувся");
            _state.Restore();
        };
        window.Destroying += (s, e) =>
            Debug.WriteLine("[LIFECYCLE] Destroying: застосунок закривається");

        return window;
    }
}