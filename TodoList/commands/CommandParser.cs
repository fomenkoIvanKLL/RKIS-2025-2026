namespace TodoList.commands;

public class CommandParser
{
	public static Profile? userProfile { get; set; }

	public static ICommand Parse(string input, TodoList todoList, Profile? profile)
	{
		var parts = input.Split(' ');
		var command = parts[0].ToLower();
		switch (command)
		{
			case "help": return new HelpCommand();
			case "add":
				return new AddCommand
				{
					todoList = todoList,
					parts = parts
				};
			case "view":
				return new ViewCommand
				{
					todoList = todoList,
					parts = parts
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
}