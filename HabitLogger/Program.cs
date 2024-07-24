﻿using System.Data;
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
            // instance of a SQLite connection class
            // using statement => to use garbage colector properly
            using (var connection = new SqliteConnection(connectionString))
            {
                try
                {
                    // Open() =>  Opens a connection to the database using the value of ConnectionString.
                    connection.Open();
                    // C->Create
                    //It should also create a table in the database, where the habit will be logged.
                    CreateHabitsTable(connection);
                }
                catch (SqliteException message)
                {
                    Console.WriteLine(message.Message);
                    Console.WriteLine(message.ErrorCode);   // Console.WriteLine(errorCode);
                }

                // call to CRUD Methods
                switch (menuSelection)
                {
                    case "1": // show table in database
                        DisplayAllTable(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "2": // show all habits
                        SeedHabitsTable(connection);
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
                        UpdateHabit(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "7": // delete - delete one habit
                        // DeleteHabitById(connection, 3);
                        DeleteHabit(connection);

                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    case "8": // delete - delete all database 
                              // DeleteHabitById(connection, 12);
                        DeleteDatabase(connection);
                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;

                    default:
                        Console.WriteLine("\n\rPress the Enter key to continue.");
                        readInputResult = Console.ReadLine();
                        break;
                }
            }
            // Clean up
            // File.Delete(dataBaseFile);
            // Console.WriteLine("The DataBase file was deleted.");

        }

        static void CreateHabitsTable(SqliteConnection connection)
        {
            // Creates a new command associated with the connection.
            var createTableCommand = connection.CreateCommand();

            // Gets or sets the SQL to execute against the database.
            // The SQL to execute against the database.
            try
            {
                createTableCommand.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS habits (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    habit TEXT,
                    goal TEXT);   
                ";

                // Executes the CommandText against the database.
                // do not return any values from DB
                // not querying DB
                createTableCommand.ExecuteNonQuery();
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
            // Console.WriteLine("The Habit's Table was created.");
        }


        static void SeedHabitsTable(SqliteConnection connection)
        {
            using (var transaction = connection.BeginTransaction())
            {
                var command = connection.CreateCommand();

                command.CommandText =
                @"
                    INSERT INTO habits (habit, goal)
                    VALUES ('Hello World', 'days'),
                            ('Learn SQLite', 'days'),
                            ('Learn .NET', 'days'),
                            ('Testing', 'days'),
                            ('Debug', 'days');
                ";

                command.ExecuteNonQuery();
                transaction.Commit();

                Console.WriteLine("Data inserted on Habit's Table");
            }
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
                    // Console.Write("ID:\t\t");
                    // Console.Write("Habit:\t\t\t");
                    // Console.Write("Goal:");
                    // Console.WriteLine();
                    Console.WriteLine("{0,-5} {1,-10} {2,15}\n", "ID", "Habit", "Goal");

                    //.Read() => Advances to the next row in the result set.
                    while (reader.Read())
                    {
                        // Console.WriteLine();
                        // Console.WriteLine("{0,-5} {1,0} {2,0}\n", $"{reader["id"]}", $"{reader["habit"]}", $"{reader["goal"]}");
                        Console.Write("{0,-5}", $"{reader["id"]}");
                        Console.Write("{0,-10}", $"{reader["habit"]}");
                        Console.Write("{0,15}", $"{reader["goal"]}");

                        Console.WriteLine();
                        // Console.Write($"{reader["id"]}");
                        // Console.Write($"{reader["habit"]}");
                        // Console.Write($"{reader["goal"]}");
                    }
                    Console.WriteLine();
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }

        static void DisplayAllHabits(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            try
            {
                command.CommandText =
                @"
                    SELECT habit, goal
                    FROM habits;
                ";

                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nCurrent Logged Habits:\n");
                    Console.Write("Habit:\t\t\t");
                    Console.Write("Goal:");
                    Console.WriteLine();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine();
                            Console.Write($"{reader["habit"]}");
                            Console.Write($"\t\t{reader["goal"]}");
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("No habits found.");
                    }
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
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

                    Console.Write("New goal: ");
                    var goal = Console.ReadLine();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        INSERT INTO habits (habit, goal)
                        VALUES ($habit, $goal);
                    ";

                    command.Parameters.AddWithValue("$habit", habit);
                    command.Parameters.AddWithValue("$goal", goal);

                    command.ExecuteNonQuery();
                    transaction.Commit();

                    Console.WriteLine();
                    Console.WriteLine($"Habit: {habit}\t{goal} inserted.");
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
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
                    SELECT habit, goal
                    FROM habits
                    WHERE habit = $habit;
                ";

                command.Parameters.AddWithValue("$habit", habit);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine();
                        Console.Write("Habit:");
                        Console.Write("\t\t\tGoal:");
                        Console.WriteLine();

                        while (reader.Read())
                        {
                            var habitSearched = reader.GetString(0);
                            var goal = reader.GetString(1);

                            Console.WriteLine();
                            Console.Write($"{habitSearched}");
                            Console.Write($"\t\t\t{goal}");
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("No logged habits found.");
                    }
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }

        static void DisplayByGoal(SqliteConnection connection)
        {
            Console.Write("Type the goal to search for: ");
            var goal = Console.ReadLine();

            var command = connection.CreateCommand();
            try
            {
                command.CommandText =
                @"
                    SELECT habit, goal
                    FROM habits
                    WHERE goal = $goal;
                ";

                command.Parameters.AddWithValue("$goal", goal);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine();
                        Console.Write("Habit:");
                        Console.Write("\t\tGoal:");
                        Console.WriteLine();

                        while (reader.Read())
                        {
                            var habitSearched = reader.GetString(0);
                            var goalSearched = reader.GetString(1);

                            Console.WriteLine();
                            // Console.Write($"{reader["habit"]}");
                            Console.Write($"{habitSearched}");
                            // Console.Write($"\t\t{reader["goal"]}");
                            Console.Write($"\t\t{goalSearched}");
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("No logged habits found with this goal.");
                    }
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }

        static void UpdateHabit(SqliteConnection connection)
        {
            try
            {
                using (var updateTransaction = connection.BeginTransaction())
                {
                    Console.Write("Type the habit you want to update: ");
                    var inputHabit = Console.ReadLine();

                    Console.Write("Type the new habit: ");
                    var newHabit = Console.ReadLine();

                    Console.Write("Type the new goal: ");
                    var newGoal = Console.ReadLine();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        UPDATE habits
                        SET habit = $newHabit, goal = $newGoal
                        WHERE habit = $inputHabit;
                    ";

                    command.Parameters.AddWithValue("$inputHabit", inputHabit);
                    command.Parameters.AddWithValue("$newHabit", newHabit);
                    command.Parameters.AddWithValue("$newGoal", newGoal);

                    command.ExecuteNonQuery();

                    // COMMIT makes all data changes in a transaction permanent.
                    updateTransaction.Commit();

                    Console.WriteLine();
                    Console.WriteLine($"Habit: {newHabit}\t {newGoal} updated.");
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
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
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }


        static void DeleteHabit(SqliteConnection connection)
        {
            try
            {
                using (var deleteTransaction = connection.BeginTransaction())
                {
                    Console.Write("Type the habit you want to delete: ");
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
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }

        static void DeleteDatabase(SqliteConnection connection)
        {
            try
            {
                using (var deleteAllTransactions = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        DELETE FROM habits;
                    ";

                    command.ExecuteNonQuery();
                    deleteAllTransactions.Commit();
                    Console.WriteLine("The DataBase file was deleted.");
                }
            }
            catch (SqliteException message)
            {
                Console.WriteLine(message.Message);
                Console.WriteLine(message.ErrorCode);
                throw;
            }
        }
    }
}