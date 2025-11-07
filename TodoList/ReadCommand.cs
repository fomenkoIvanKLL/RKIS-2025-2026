namespace TodoList;

public class ReadCommand : ICommand
{
	public string[] parts { get; set; }
	public TodoList todoList { get; set; }

	public void Execute()
	{
		if (parts.Length < 2 || !int.TryParse(parts[1], out var taskNumber))
		{
			Console.WriteLine("Ошибка: укажите номер задачи");
			return;
		}

		var index = taskNumber - 1;
		try
		{
			var item = todoList.GetItem(index);
			Console.WriteLine($"Задача {taskNumber}:");
			Console.WriteLine(item.GetFullInfo());
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine("Ошибка: неверный номер задачи");
		}
	}
}