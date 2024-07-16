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

