using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDbBackupRestore.Data;
public class DataContext
{
    public const string DatabaseName = "MauiDb.db3";
    public static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, DatabaseName);

    private SQLiteAsyncConnection _connection;

    public DataContext()
    {     
        SetupConnection();
    }

    public async Task CloseConnectionAsync() => await _connection.CloseAsync();
    public void SetupConnection() =>
        _connection = new SQLiteAsyncConnection(DbPath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);

    public async Task<User[]> GetUsersAsync() => 
        await _connection.Table<User>().ToArrayAsync();

    public async Task UpdateUserAsync(User user) => 
        await _connection.UpdateAsync(user);
}
