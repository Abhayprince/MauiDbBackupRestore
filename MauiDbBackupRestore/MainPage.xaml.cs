using MauiDbBackupRestore.Data;

namespace MauiDbBackupRestore;

public partial class MainPage : ContentPage
{
    private readonly DataContext _dataContext;

    public MainPage(DataContext dataContext)
    {
        InitializeComponent();
        _dataContext = dataContext;
        LoadDataAsync();
    }
    private User? _user;
    private async void LoadDataAsync()
    {
        var users = await _dataContext.GetUsersAsync();
        if (users != null)
        {
            _user = users.FirstOrDefault();
            nameLbl.Text = _user?.Name ?? "Unknown";
            nameEntry.Text = nameLbl.Text;
        }
    }

    private async void backupBtn_Clicked(object sender, EventArgs e)
    {
        if (_user != null)
        {
            _user.Name = nameEntry.Text;
            await _dataContext.UpdateUserAsync(_user);
        }

        // There is no point of Adding this Backup Service to DI
        // Because this is going to be used on demand only at one or two places
        var backupService = new BackupService();
        await backupService.BackupAsync(_dataContext);
    }
}

