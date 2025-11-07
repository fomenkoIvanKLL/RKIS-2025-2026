namespace TodoList.commands;

public class CommandParser
{
	public static Profile? userProfile { get; set; }

	public static ICommand Parse(string input, TodoList todoList, Profile? profile)
	{
		var flags = ParseFlags(input);
		var parts = input.Split(' ');
		var command = parts[0].ToLower();
		
		switch (command)
		{
			case "help": return new HelpCommand();
			case "add":
				return new AddCommand
				{
					todoList = todoList,
					multiline = flags.Contains("-m") || flags.Contains("--multi"),
					parts = parts
				};
			case "view":
				return new ViewCommand
				{
					todoList = todoList,
					ShowIndex = flags.Contains("--index") || flags.Contains("-i"),
					ShowStatus = flags.Contains("--status") || flags.Contains("-s"),
					ShowDate = flags.Contains("--update-date") || flags.Contains("-d"),
					ShowAll = flags.Contains("--all") || flags.Contains("-a")
				};
			case "read":
				return new ReadCommand
				{
					todoList = todoList,
					parts = parts
				};
			case "done":
				return new DoneCommand
				{
					todoList = todoList,
					parts = parts
				};
			case "delete":
				return new DeleteCommand
				{
					todoList = todoList,
					parts = parts
				};
			case "update":
				return new UpdateCommand
				{
					todoList = todoList,
					parts = parts
				};
			case "profile":
				return new ProfileCommand
				{
					parts = parts
				};
			case "exit": return new ExitCommand();
			default: return new UnknownCommand();
		}
	}
	public static string[] ParseFlags(string command)
	{
		var parts = command.Split(' ');
		var flags = new List<string>();

		foreach (var part in parts)
			if (part.StartsWith("--"))
				flags.Add(part);
			else if (part.StartsWith("-"))
				for (var i = 1; i < part.Length; i++)
					flags.Add("-" + part[i]);

		return flags.ToArray();
	}
}