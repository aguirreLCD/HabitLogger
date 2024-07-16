using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        const string dataBaseFile = "consolehabits.db";

        // A connection string is used to specify how to connect to the database.
        var connectionString = $"Data Source={dataBaseFile}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // call to CRUD Methods

            // C -> Create
            CreateHabitsTable(connection);
            // InsertHabit(connection);

            // R -> Read
            // DisplayAllHabits(connection);

            // U -> Update
            // UpdateHabit(connection);
            // UpdateAllHabits(connection);

            // D -> Delete
            // DeleteHabit(connection);
            // DeleteAllHabits(connection);

        }

        // Clean up
        File.Delete(dataBaseFile);
        Console.WriteLine("The DataBase file was deleted.");
    }

    static void CreateHabitsTable(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
        @"
        CREATE TABLE IF NOT EXISTS habits (
        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        habit TEXT NOT NULL,
        measurement TEXT NOT NULL,
        goal TEXT NOT NULL,
        started TEXT NOT NULL,
        finished TEXT NULL,
        )
        ";

        // DateTime
        // day, REAL,
        // week, REAL,
        // month, REAL,
        command.ExecuteNonQuery();
        Console.WriteLine("The Habit's Table was created.");
    }

}