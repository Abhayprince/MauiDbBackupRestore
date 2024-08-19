using CommunityToolkit.Maui.Storage;

namespace MauiDbBackupRestore.Data;
public class BackupService
{
    public async Task BackupAsync(DataContext dataContext)
    {
        try
        {
            await dataContext.CloseConnectionAsync();

            using var dbFileStream = File.OpenRead(DataContext.DbPath);

            // Show user an option to choose the location to save the database backup 
            var fileSaveResult = await FileSaver.Default.SaveAsync(DataContext.DatabaseName, dbFileStream);
            if (fileSaveResult.IsSuccessful)
            {
                await App.Current.MainPage.DisplayAlert("Backup Success", $"Backup succesfully saved to - {fileSaveResult.FilePath}", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Backup Error", $"Error in backup - {fileSaveResult.Exception.Message}", "Ok");
            }
        }
        finally
        {
            dataContext.SetupConnection();
        }
    }
}
