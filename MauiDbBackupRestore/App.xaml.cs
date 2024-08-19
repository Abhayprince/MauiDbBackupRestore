using MauiDbBackupRestore.Data;

namespace MauiDbBackupRestore;

public partial class App : Application
{
    private const string DbRestoredKey = "DbRestored";
    public App()
    {
        InitializeComponent();

        if (!Preferences.Default.ContainsKey(DbRestoredKey))
        {
            // This is the first run
            // We need to restore the database

            MainPage = new InitPage();
            MainPage.Loaded += MainPage_Loaded;
        }
        else
        {
            // This is not first run
            // Database is restored already
            // No need to do anything
            // Let the app continue

            MainPage = new AppShell();
        }
    }

    private async void MainPage_Loaded(object? sender, EventArgs e)
    {
        await HandleFirstRunAsync();
        MainPage = new AppShell();
    }

    private async Task HandleFirstRunAsync()
    {
        // Ask user if he has some database backup
        // if yes, show an option to choose the bakup file to restore
        // else, restore our initial database from Resources/raw folder


        // There is no point of Adding this RestoreService to DI
        // Because this is going to be used on here at this one place only, that too once only, when we launch app first time

        var restoreService = new RestoreService();

        var isSucessfull = await restoreService.RestoreAsync();
        if (isSucessfull)
            Preferences.Default.Set(DbRestoredKey, true);
    }
}
