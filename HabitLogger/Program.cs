﻿using Microsoft.Data.Sqlite;

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

            var acceptableMenuOption = "1 2 3 4 5 6 7 8".Split();

            if (readInputResult != null)
            {
                // validate for menu options
                while (!acceptableMenuOption.Contains(readInputResult))
                {
                    //The application should only be terminated when the user inserts 0.
                    if (readInputResult == "0")
                    {
                        Console.WriteLine("Exiting program...");
                        return;
                    }

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

            // SqliteConnection() => Represents a connection to a SQLite database.
            using (var connection = new SqliteConnection(connectionString))
            {
                // Open() =>  Opens a connection to the database using the value of ConnectionString.
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
                        DisplayByGoal(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "5": // insert - create new habit
                        CreateNewHabit(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "6": // update - update one habit
                        UpdateHabit(connection, 1);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "7": // delete - delete one habit
                        // DeleteHabitById(connection, 3);
                        DeleteHabit(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "8": // delete - delete habit by ID
                        DeleteHabitById(connection, 1);
                        // DeleteHabit(connection);

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
            // Creates a new command associated with the connection.
            var command = connection.CreateCommand();

            // Gets or sets the SQL to execute against the database.
            // The SQL to execute against the database.
            try
            {
                command.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS habits (
                id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                habit TEXT,
                quantity TEXT);   
            ";

                //Executes the CommandText against the database.
                command.ExecuteNonQuery();
            }
            catch (SqliteException message)
            {

                Console.WriteLine(message);
                // Console.WriteLine(errorCode);

            }
            // Console.WriteLine("The Habit's Table was created.");
        }


        static void DisplayAllTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            try
            {
                command.CommandText =
                @"
                SELECT *
                FROM habits;
            ";

                // ExecuteReader() => Executes the CommandText against the database and returns a data reader.
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nCurrent Habits to keep track:\n");
                    Console.Write("ID:\t\t");
                    Console.Write("Habit:");
                    Console.Write("\t\t\tGoal:");
                    Console.WriteLine();

                    //.Read() => Advances to the next row in the result set.
                    while (reader.Read())
                    {
                        Console.WriteLine();
                        Console.Write($"{reader["id"]}:\t\t");
                        Console.Write($"{reader["habit"]}:\t\t");
                        Console.Write($"{reader["quantity"]}\n");
                    }
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message);
            }
        }

        static void DisplayAllHabits(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            try
            {
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
            catch (SqliteException message)
            {
                Console.WriteLine(message);
            }
        }

        static void CreateNewHabit(SqliteConnection connection)
        {
            try
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
            catch (SqliteException message)
            {
                Console.WriteLine(message);
            }
        }

        static void DisplayByHabit(SqliteConnection connection)
        {
            Console.Write("Type the habit you want to check: ");
            var habit = Console.ReadLine();

            var command = connection.CreateCommand();

            try
            {
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
            catch (SqliteException message)
            {
                Console.WriteLine(message);
            }
        }

        static void DisplayByGoal(SqliteConnection connection)
        {
            Console.Write("Type the goal to search for related habit: ");
            var quantity = Console.ReadLine();

            var command = connection.CreateCommand();

            try
            {
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
            catch (SqliteException message)
            {
                Console.WriteLine(message);
            }
        }

        static void UpdateHabit(SqliteConnection connection, int id)
        {
            try
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

                    // COMMIT makes all data changes in a transaction permanent.
                    updateTransaction.Commit();

                    Console.WriteLine();
                    Console.WriteLine($"Habit: {habit}\t {quantity} updated.");
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message);
            }
        }

        static void DeleteHabitById(SqliteConnection connection, int id)
        {
            try
            {
                using (var updateTransaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                    DELETE FROM habits
                    WHERE id = $id;
                ";
                    command.Parameters.AddWithValue("$id", id);

                    command.ExecuteNonQuery();

                    // COMMIT makes all data changes in a transaction permanent.
                    updateTransaction.Commit();

                    Console.WriteLine();
                    Console.WriteLine($"Habit: {id} deleted.");
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message);
            }
        }


        static void DeleteHabit(SqliteConnection connection)
        {
            try
            {
                using (var deleteTransaction = connection.BeginTransaction())
                {
                    Console.Write("habit to delete: ");
                    var habit = Console.ReadLine();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                    DELETE FROM habits
                    WHERE habit = $habit;
                ";

                    command.Parameters.AddWithValue("$habit", habit);

                    command.ExecuteNonQuery();

                    // COMMIT makes all data changes in a transaction permanent.
                    deleteTransaction.Commit();

                    Console.WriteLine();
                    Console.WriteLine($"Habit: {habit} deleted.");
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message);
            }

        }
    }
}