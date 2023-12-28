using Microsoft.Data.Sqlite;

string connectionString = @"Data Source = habitlogger.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS drinking_water (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
DATE TEXT,
Quantity INTEGER)";

    tableCmd.ExecuteNonQuery();
    connection.Close();
}