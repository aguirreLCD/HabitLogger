using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        string? readInputResult = "";
        string? menuSelection = "";

        while (menuSelection != "0")
        {
            Console.Clear();

            Console.WriteLine("Welcome to the Habit Logger App in C#");
            Console.WriteLine("------------------------\n");

            // Ask the user to choose an option.
            Console.WriteLine("Your main menu options are:");
            Console.WriteLine("------------------------\n");

            Console.WriteLine("1. To display the current database, type: 1");
            Console.WriteLine("2. To display the current users, type: 2");
            Console.WriteLine("3. To display the current habits, type: 3");
            Console.WriteLine("4. To display the habits by selected user, type: 4");

            Console.WriteLine("5. To create a new user and habit, type: 5");
            // Console.WriteLine("6. To create a new user, type: 6");
            // Console.WriteLine("7. To create a new habit, type: 7");

            var currentDate = DateTime.Now;
            Console.WriteLine($"{Environment.NewLine}Hello! on {currentDate:d} at {currentDate:t}");

            Console.WriteLine("Enter your option (or type 0 to Exit the program)");
            Console.WriteLine();

            readInputResult = Console.ReadLine();

            var acceptableMenuOption = "1 2 3 4 5".Split();

            if (readInputResult != null)
            {
                // validate for menu options
                while (!acceptableMenuOption.Contains(readInputResult))
                {
                    Console.WriteLine("Enter your option (or type 0 to exit the program)");
                    Console.WriteLine();
                    readInputResult = Console.ReadLine();
                }

                if (!String.IsNullOrEmpty(readInputResult))
                {
                    menuSelection = readInputResult.ToLower();
                }
            }

            const string dataBaseFile = "consolehabits.db";

            var connectionString = $"Data Source={dataBaseFile}";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // C->Create
                CreateHabitsTable(connection);

                // call to CRUD Methods
                switch (menuSelection)
                {
                    case "1": // database file
                        DisplayAllTable(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "2": // users
                        DisplayAllUsers(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "3": // habits
                        DisplayAllHabits(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "4": // display habits by selected user
                        DisplayAllHabitsByUser(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "5": // create new user and habit
                        CreateNewUser(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    default: // create new user and habit
                        CreateNewUser(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                }

                // U -> Update
                // UpdateHabit(connection);
                // UpdateAllHabits(connection);

                // D -> Delete
                // DeleteHabit(connection);
                // DeleteAllHabits(connection);
                // DisplayAllHabitsByUser(connection);
            }
        }

        // // Clean up
        // File.Delete(dataBaseFile);
        // Console.WriteLine("The DataBase file was deleted.");


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

            // Console.WriteLine("The Habit's Table was created.");
        }

        // static void CreateName(SqliteConnection connection)
        // {
        //     using (var transaction = connection.BeginTransaction())
        //     {
        //         Console.Write("Name: ");
        //         string? name = Console.ReadLine();

        //         var insertCommand = connection.CreateCommand();

        //         insertCommand.CommandText =
        //         @"
        //         INSERT INTO habits (name)
        //         VALUES ($name);
        //    ";

        //         insertCommand.Parameters.AddWithValue("$name", name);

        //         insertCommand.ExecuteNonQuery();

        //         transaction.Commit();

        //         Console.WriteLine($"Name: {name} inserted.");

        //     }
        // }

        // static void CreateHabit(SqliteConnection connection)
        // {
        //     Console.Write("Habit: ");
        //     var habit = Console.ReadLine();

        //     var command = connection.CreateCommand();

        //     command.CommandText =
        //     @"
        // INSERT INTO habits (habit)
        // VALUES ($habit);
        // ";

        //     command.Parameters.AddWithValue("$habit", habit);

        //     command.ExecuteNonQuery();

        //     Console.WriteLine($"Habit: {habit} inserted.");
        // }

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
                SELECT name
                FROM habits;
            ";

            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("\nCurrent Users in Database:");

                while (reader.Read())
                {
                    Console.WriteLine($"Name: {reader["name"]}");
                }
            }
        }

        static void CreateNewUser(SqliteConnection connection)
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

        static void DisplayAllHabitsByUser(SqliteConnection connection)
        {

            Console.Write("Type the name you want to show user's habits: ");
            var name = Console.ReadLine();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                SELECT habit
                FROM habits
                WHERE name = $name;
            ";

            command.Parameters.AddWithValue("$name", name);

            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine($"\nCurrent {name}'s habits:");

                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["habit"]}");
                }
            }
        }
    }
}