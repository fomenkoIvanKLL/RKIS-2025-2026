namespace TodoList;

public class AddCommand : ICommand
{
	public string[] parts { get; set; }
	public TodoList todoList { get; set; }

	public void Execute()
	{
		var multiline = Program.HasFlag(parts, "--multiline", "-m");

		if (multiline)
			AddMultilineTask();
		else
		{
			if (parts.Length < 2)
			{
				Console.WriteLine("Ошибка: укажите текст задачи");
				return;
			}

			var taskText = string.Join(" ", parts, 1, parts.Length - 1);
			AddSingleTask(taskText);
		}
	}

	private void AddSingleTask(string taskText)
	{
		if (string.IsNullOrWhiteSpace(taskText))
		{
			Console.WriteLine("Ошибка: текст задачи не может быть пустым");
			return;
		}

		var newItem = new TodoItem(taskText);
		todoList.Add(newItem);
		Console.WriteLine($"Задача добавлена: {taskText}");
	}

	private void AddMultilineTask()
	{
		Console.WriteLine("Введите текст задачи (для завершения введите 'end'):");
		var taskText = "";
		while (true)
		{
			Console.Write("> ");
			var line = Console.ReadLine();
			if (line == null)
				continue;
			if (line == "end")
				break;
			taskText += line + "\n";
		}

		taskText = taskText.TrimEnd('\n');
		if (string.IsNullOrWhiteSpace(taskText))
		{
			Console.WriteLine("Ошибка: текст задачи не может быть пустым");
			return;
		}

		AddSingleTask(taskText);
	}
}