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

            // view table - view
            Console.WriteLine("1. To display the all current table in database, type: 1");

            // view habit - view
            Console.WriteLine("2. To display the all current (logged) habits, type: 2");

            // track by habit - search
            Console.WriteLine("3. To search specific habit, type: 3");

            // track by goal - search
            Console.WriteLine("4. To search habits related to your goal, type: 4");

            // register one habit - insert
            Console.WriteLine("5. To create new habit, type: 5");

            // update one habit - update
            Console.WriteLine("6. To update an habit, type: 6");

            // delete one habit - delete
            Console.WriteLine("7. To delete an habit, type: 7");

            // delete all habits - delete
            Console.WriteLine("8. To delete all habits, type: 8");


            var currentDate = DateTime.Now;
            Console.WriteLine($"{Environment.NewLine}Hello! on {currentDate:d} at {currentDate:t}");

            Console.WriteLine("Enter your option (or type 0 to Exit the program)");
            Console.WriteLine();

            readInputResult = Console.ReadLine();

            var acceptableMenuOption = "1 2 3 4 5 6".Split();

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
                    case "1": // show table in database
                        DisplayAllTable(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "2": // show all habits
                        DisplayAllHabits(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "3": // search and display info by habit - track by habit
                        DisplayByHabit(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "4": // search and display info by unit of measurement - track by goal
                        DisplayByQuantity(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "5": // insert - create new habit
                        CreateNewHabit(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "6": // update - update one habit
                        UpdateHabit(connection, 3);

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
                Console.WriteLine("\nCurrent Habits to keep track:\n");
                Console.Write("ID:\t\t");
                Console.Write("Habit:");
                Console.Write("\t\t\tGoal:");
                Console.WriteLine();

                while (reader.Read())
                {
                    Console.WriteLine();
                    Console.Write($"{reader["id"]}:\t\t");
                    Console.Write($"{reader["habit"]}:\t\t");
                    Console.Write($"{reader["quantity"]}\n");
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
                Console.WriteLine("\nCurrent Logged Habits:\n");
                Console.Write("Habit:");
                Console.Write("\t\t\tGoal:");
                Console.WriteLine();

                while (reader.Read())
                {
                    Console.WriteLine();
                    Console.Write($"{reader["habit"]}:\t\t");
                    Console.Write($"{reader["quantity"]}\n");
                    // Console.Write("Edit\t");
                    // Console.Write("Delete\n");
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
                Console.WriteLine($"Habit: {habit}\t{quantity} inserted.");
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
                Console.Write("Habit:");
                Console.Write("\t\t\tGoal:");
                Console.WriteLine();

                while (reader.Read())
                {
                    Console.WriteLine();
                    Console.Write($"{reader["habit"]}:\t\t");
                    Console.Write($"{reader["quantity"]}\n");
                }
            }
        }

        static void DisplayByQuantity(SqliteConnection connection)
        {
            Console.Write("Type the quantity to search for related habit: ");
            var quantity = Console.ReadLine();

            var command = connection.CreateCommand();

            command.CommandText =
            @"
                SELECT habit, quantity
                FROM habits
                WHERE quantity = $quantity;
            ";

            command.Parameters.AddWithValue("$quantity", quantity);

            using (var reader = command.ExecuteReader())
            {
                Console.Write("Habit:");
                Console.Write("\t\t\tGoal:");
                Console.WriteLine();

                while (reader.Read())
                {
                    Console.WriteLine();
                    Console.Write($"{reader["habit"]}:\t\t");
                    Console.Write($"{reader["quantity"]}\n");
                }
            }
        }

        static void UpdateHabit(SqliteConnection connection, int id)
        {
            using (var updateTransaction = connection.BeginTransaction())
            {
                Console.Write("Update habit to: ");
                var habit = Console.ReadLine();

                Console.Write("Update habit goal to: ");
                var quantity = Console.ReadLine();

                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    UPDATE habits
                    SET habit = $habit, quantity = $quantity
                    WHERE id = $id;
                ";

                command.Parameters.AddWithValue("$habit", habit);
                command.Parameters.AddWithValue("$quantity", quantity);
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteNonQuery();

                updateTransaction.Commit();

                Console.WriteLine();
                Console.WriteLine($"Habit: {habit}\t {quantity} updated.");
            }

        }
    }
}