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

    static void DisplayAllHabits(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT *
            FROM habits;
        ";

        using (var reader = command.ExecuteReader())
        {
            Console.WriteLine("Current Habits in Database:");

            while (reader.Read())
            {
                // var id = reader.GetInt32(0);
                // var name = reader.GetString(0);
                // var habit = reader.GetString(2);
                // Console.WriteLine($"ID: {reader["id"]}: {reader["name"]}: {reader["habit"]}");
                Console.WriteLine($"\tID: {reader["id"]}");

                Console.WriteLine($"\tName: {reader["name"]}");

                Console.WriteLine($"\tHabit: {reader["habit"]}");
            }

            // reader.NextResult();

            // while (reader.Read())
            // {
            //     // var id = reader.GetInt32(0);
            //     // var name = reader.GetString(0);
            //     // var habit = reader.GetString(0);
            //     // Console.WriteLine($"Name: {name}");
            //     Console.Write($"\tName: {reader["name"]}");

            // }

            // reader.NextResult();

            // while (reader.Read())
            // {
            //     // var id = reader.GetInt32(0);
            //     // var name = reader.GetString(0);
            //     // var habit = reader.GetString(0);
            //     // Console.WriteLine($"ID: {id}");
            //     Console.Write($"\tHabit: {reader["habit"]}");

            // }
        }
    }

    static void BulkInsert(SqliteConnection connection)
    {
        // #region snippet_BulkInsert
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

            Console.WriteLine($"Name: {name} inserted.");
            Console.WriteLine($"Habit: {habit} inserted.");
        }
    }
}