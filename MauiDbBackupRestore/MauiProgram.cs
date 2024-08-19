using CommunityToolkit.Maui;
using MauiDbBackupRestore.Data;
using Microsoft.Extensions.Logging;

namespace MauiDbBackupRestore;
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
            })
            .UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<DataContext>()
            .AddSingleton<MainPage>();

        return builder.Build();
    }
}
