using SQLite;

namespace MauiDbBackupRestore.Data;
public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
}
