This is a project to learn how to:

- perform CRUD operations against a real database;
- create and run a .NET console application by using Visual Studio Code and the .NET CLI.
- use SQLite;
- use SQLite with C# in VSCode;
- debug;
- test;
- publish;

# Using Visual Studio Code:

## Create a Console Application: Habit Logger App

```
Create a new folder => HabitLogger
The folder name becomes the project name and the namespace by default;
In Visual Studio Code,
choose Create new project
Search for Console Application
Choose a Project Name
Run your application by running
dotnet build
dotnet run
```

### Create .vscode files to configure build and debug:

```
Command + P:
.NET: Generate Assets for Build and Debug
```

To handle terminal input while debugging, you can use the integrated terminal:
In launch.json file, configure console:

"console": "integratedTerminal",

### Create a .gitignore template

```
dotnet new gitignore
```

### Using SQLite with C# in VSCode

- Installing Necessary Package

Microsoft.Data.Sqlite is a lightweight ADO NET provider for SQLite and you can install it with:

```
dotnet add package Microsoft.Data.Sqlite
```

https://github.com/dotnet/docs/blob/main/samples/snippets/standard/data/sqlite/DateAndTimeSample/Program.cs

SQLite doesn't support primitive DateTime and TimeSpan values. Instead, it provides date and time functions to help you perform operations based on strings and Julian day values;

By default, Microsoft.Data.Sqlite uses strings, but it can also read DateTime values from Julian day values;

And it can read TimeSpan values from values in days;

To write values in days or as Julian day values, set SqliteType to Real;

## Work in progress...

- [x] Create Database and Database connection;
- [x] Create Table;
- [x] Create CRUD Methods:
- [x] Create habit;
- [x] Display habits;
- [x] Update habit;
- [x] Delete habit;
- [] Handle errors:
- [] Handle deleted database;
- [] Handle deleted habit;
- [] Handle updated habit;
- [] Test;
- [] Publish;

## Learning topics:

- Connecting to a Sqlite database;
- ADO.NET;
- .NET data access;
- Microsoft Data SQLite;
- DB Design;
- SQL;
- Testing;
- Git branches;

- Batching:
- SQLite doesn't natively support batching SQL statements together into a single command. Microsoft.Data.Sqlite does, however, implement statement Batching as a convenience to make it behave more like other ADO.NET providers.

- Transactions:
- Transactions let you group multiple SQL statements into a single unit of work that is committed to the database as one atomic unit;

<!-- WHERE habit = '% {$habit} %'; -->

- LINQ: Runs SQL statements written like language constructs to query collections directly from inside .Net code
