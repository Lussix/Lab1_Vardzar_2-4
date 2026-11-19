using Microsoft.Extensions.Logging;
using Lab1_Vardzar_2_4.Services;
using Lab1_Vardzar_2_4.ViewModels;
using Lab1_Vardzar_2_4.Views;

namespace Lab1_Vardzar_2_4
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IAppStateService, AppStateService>();

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "students.db3");
            builder.Services.AddSingleton<ICryptoService, CryptoService>();
            builder.Services.AddSingleton<IStudentRepository>(sp =>
                new StudentRepository(dbPath, sp.GetRequiredService<ICryptoService>()));
            builder.Services.AddSingleton<ISyncService>(_ => new SyncService(dbPath));

            builder.Services.AddSingleton<StudentViewModel>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddTransient<StudentDetailViewModel>();
            builder.Services.AddTransient<StudentDetailPage>();

            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();

            return builder.Build();
        }
    }
}