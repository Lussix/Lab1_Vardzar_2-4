using Lab1_Vardzar_2_4.Services;
using Lab1_Vardzar_2_4.ViewModels;
using Lab1_Vardzar_2_4.Views;
using Microsoft.Extensions.Logging;

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

            builder.Services.AddTransient<StudentDetailViewModel>();
            builder.Services.AddTransient<StudentDetailPage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "students.db3");
            builder.Services.AddSingleton<IStudentRepository>(_ => new StudentRepository(dbPath));

            return builder.Build();
        }
    }
}
