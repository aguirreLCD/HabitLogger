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
            InsertHabit(connection);

            // R -> Read
            DisplayAllHabits(connection);

            // U -> Update
            // UpdateHabit(connection);
            // UpdateAllHabits(connection);

            // D -> Delete
            // DeleteHabit(connection);
            // DeleteAllHabits(connection);

        }

        // Clean up
        // File.Delete(dataBaseFile);
        // Console.WriteLine("The DataBase file was deleted.");
    }

    static void CreateHabitsTable(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
        @"
        CREATE TABLE IF NOT EXISTS habits (
        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        habit TEXT NOT NULL);   
        ";

        command.ExecuteNonQuery();

        Console.WriteLine("The Habit's Table was created.");
    }


    static void InsertHabit(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
        @"
        INSERT INTO habits (name, habit)
        VALUES ('lili', 'exercise'),
        ('ginho', 'run');
        ";

        command.ExecuteNonQuery();
        Console.WriteLine("Habits inserted.");
    }

    static void DisplayAllHabits(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
        @"
        SELECT id, name, habit
        FROM habits
        ";

        using (var reader = command.ExecuteReader())
        {
            Console.WriteLine("Current Habits in Database:");

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var habit = reader.GetString(2);
                Console.WriteLine($" ID: {id}, Name: {name}, Habit: {habit}");
            }
        }
    }
}