namespace Lab1_Vardzar_2_4.Services;

public static class ThemeService
{
    private const string Key = "app_theme";

    public static void Apply(AppTheme theme)
    {
        if (Application.Current is null) return;

        Application.Current.UserAppTheme = theme;
        Preferences.Set(Key, theme.ToString());
    }

    public static void LoadSaved()
    {
        if (Application.Current is null) return;

        var saved = Preferences.Get(Key, AppTheme.Unspecified.ToString());
        if (Enum.TryParse<AppTheme>(saved, out var theme))
            Application.Current.UserAppTheme = theme;
    }
}