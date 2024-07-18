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

            Console.WriteLine("1. To display the all current database, type: 1");
            Console.WriteLine("2. To display the all current habits, type: 2");
            Console.WriteLine("3. To display Info by specific habit, type: 3");
            Console.WriteLine("4. To create new habit, type: 4");

            var currentDate = DateTime.Now;
            Console.WriteLine($"{Environment.NewLine}Hello! on {currentDate:d} at {currentDate:t}");

            Console.WriteLine("Enter your option (or type 0 to Exit the program)");
            Console.WriteLine();

            readInputResult = Console.ReadLine();

            var acceptableMenuOption = "1 2 3 4".Split();

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

            // When the application starts, it should create a sqlite database, if one isn’t present.
            const string dataBaseFile = "consolehabits.db";

            var connectionString = $"Data Source={dataBaseFile}";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // C->Create
                //It should also create a table in the database, where the habit will be logged.
                CreateHabitsTable(connection);

                // call to CRUD Methods
                switch (menuSelection)
                {
                    case "1": // show database file
                        DisplayAllTable(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "2": // show all habits
                        DisplayAllHabits(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "3": // display info by habit
                        DisplayByHabit(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "4": // create new habit
                        CreateNewHabit(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    default:
                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;
                }
            }
            // // Clean up
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
                habit TEXT,
                quantity TEXT);   
            ";

            command.ExecuteNonQuery();
            // Console.WriteLine("The Habit's Table was created.");
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
                    Console.WriteLine($"ID: {reader["id"]}, Habit: {reader["habit"]} - {reader["quantity"]}");
                }
            }
        }

        static void DisplayAllHabits(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText =
            @"
                SELECT habit, quantity
                FROM habits;
            ";

            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("\nCurrent Habits in Database:");

                while (reader.Read())
                {
                    Console.WriteLine($"\tHabit: {reader["habit"]}, Quantity: {reader["quantity"]}");
                }
            }
        }

        static void CreateNewHabit(SqliteConnection connection)
        {
            using (var transaction = connection.BeginTransaction())
            {
                Console.Write("New Habit: ");
                var habit = Console.ReadLine();

                Console.Write("Quantity: ");
                var quantity = Console.ReadLine();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    INSERT INTO habits (habit, quantity)
                    VALUES ($habit, $quantity);
                ";

                command.Parameters.AddWithValue("$habit", habit);
                command.Parameters.AddWithValue("$quantity", quantity);

                command.ExecuteNonQuery();

                transaction.Commit();

                Console.WriteLine();
                Console.WriteLine($"Habit: {habit} - {quantity} inserted.");
            }
        }


        static void DisplayByHabit(SqliteConnection connection)
        {

            Console.Write("Type the habit you want to check: ");
            var habit = Console.ReadLine();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                SELECT habit, quantity
                FROM habits
                WHERE habit = $habit;
            ";

            command.Parameters.AddWithValue("$habit", habit);

            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine($"\nCurrent habits:");

                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["habit"]} - {reader["quantity"]} ");
                }
            }
        }


    }
}