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

            // C->Create
            CreateHabitsTable(connection);

            BulkInsert(connection);

            // CreateName(connection);

            // CreateHabit(connection);

            // // R -> Read
            // Console.WriteLine("\nDataBase file:");
            DisplayAllTable(connection);

            // Console.WriteLine("\nUsers:");
            DisplayAllUsers(connection);

            // Console.WriteLine("\nHabits:");
            DisplayAllHabits(connection);

            // U -> Update
            // UpdateHabit(connection);
            // UpdateAllHabits(connection);

            // D -> Delete
            // DeleteHabit(connection);
            // DeleteAllHabits(connection);

        }

        // // Clean up
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
        name TEXT NULL,
        habit TEXT);   
        ";

        command.ExecuteNonQuery();

        Console.WriteLine("The Habit's Table was created.");
    }

    static void CreateName(SqliteConnection connection)
    {
        using (var transaction = connection.BeginTransaction())
        {
            Console.Write("Name: ");
            string? name = Console.ReadLine();

            var insertCommand = connection.CreateCommand();

            insertCommand.CommandText =
            @"
                INSERT INTO habits (name)
                VALUES ($name);
           ";

            insertCommand.Parameters.AddWithValue("$name", name);

            insertCommand.ExecuteNonQuery();

            transaction.Commit();

            Console.WriteLine($"Name: {name} inserted.");

        }
    }

    static void CreateHabit(SqliteConnection connection)
    {
        Console.Write("Habit: ");
        var habit = Console.ReadLine();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
        INSERT INTO habits (habit)
        VALUES ($habit);
        ";

        command.Parameters.AddWithValue("$habit", habit);
        // command.ExecuteScalar();

        command.ExecuteNonQuery();

        Console.WriteLine($"Habit: {habit} inserted.");
    }

    static void DisplayAllTable(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT *
            FROM habits;
        ";

        using (var reader = command.ExecuteReader())
        {
            Console.WriteLine("\nCurrent Database:");

            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}, Habit: {reader["habit"]}");
            }
        }
    }

    static void DisplayAllHabits(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT habit
            FROM habits;
        ";

        using (var reader = command.ExecuteReader())
        {
            Console.WriteLine("\nCurrent Habits in Database:");

            while (reader.Read())
            {
                Console.WriteLine($"\tHabit: {reader["habit"]}");
            }
        }
    }

    static void DisplayAllUsers(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT id, name
            FROM habits;
        ";

        using (var reader = command.ExecuteReader())
        {
            Console.WriteLine("\nCurrent Users in Database:");

            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}");
            }
        }
    }
    static void BulkInsert(SqliteConnection connection)
    {
        using (var transaction = connection.BeginTransaction())
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Habit: ");
            var habit = Console.ReadLine();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                INSERT INTO habits (name, habit)
                VALUES ($name, $habit);
            ";

            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$habit", habit);

            command.ExecuteNonQuery();

            transaction.Commit();

            Console.WriteLine();
            Console.WriteLine($"Name: {name} inserted.");
            Console.WriteLine($"Habit: {habit} inserted.");
        }
    }
}