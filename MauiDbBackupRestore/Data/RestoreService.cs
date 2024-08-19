namespace MauiDbBackupRestore.Data;
public class RestoreService
{
    private static readonly HashSet<string> SqliteDbExtensions = new HashSet<string>([".db", ".db2", ".db3", ".sqlite", ".sqlite2", ".sqlite3"], StringComparer.OrdinalIgnoreCase);
    public async Task<bool> RestoreAsync()
    {
        var result = await App.Current.MainPage.DisplayAlert("First Launch", "Do you have a database backup file to restore?", "Yes", "No");
        if (result)
        {
            // User selected yes, 
            // User has a db backup file
            // Show user and option to choose the backup file

            var platformFileTypes = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                [DevicePlatform.WinUI] = SqliteDbExtensions,
            };

            var fileTypes = DeviceInfo.Platform == DevicePlatform.WinUI
                            ? new FilePickerFileType(platformFileTypes)
                            : null;

            var fileResult = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select database file to restore",
                FileTypes = fileTypes
            });

            if (fileResult != null)
            {
                if (DeviceInfo.Platform != DevicePlatform.WinUI)
                {
                    // Validate the selected file extension
                    if (!SqliteDbExtensions.Any(ext => fileResult.FullPath.Contains(ext)))
                    {
                        // Invalid file selected
                        await App.Current.MainPage.DisplayAlert("Invalid Backup File", "Selected backup file is invalid", "Ok");

                        // Handle this case as per your need
                        // you can
                        // either show the FIlePicker again
                        // or restore the deafult initial db

                        return false;
                    }
                }

                using var dbFileStream = await fileResult.OpenReadAsync();

                return await RestoreDbFileAsync(dbFileStream);
            }
            else
            {
                // User cancelled the file picker

                // Handle this case as per your need
                // you can
                // either show the FilePicker again
                // or restore the deafult initial db
            }
        }
        else
        {
            // User selected No
            // User does not have backup file
            // Restoreour initial db from Resources/raw folder

            var dbFileStream = await FileSystem.OpenAppPackageFileAsync(DataContext.DatabaseName);

            return await RestoreDbFileAsync(dbFileStream);
        }
        return false;

        static async Task<bool> RestoreDbFileAsync(Stream dbFileStream)
        {
            var pathToRestoreDb = DataContext.DbPath;
            using var fs = File.Create(pathToRestoreDb);

            await dbFileStream.CopyToAsync(fs);

            return true;
        }
    }
}
