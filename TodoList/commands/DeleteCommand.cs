namespace TodoList.commands;

public class DeleteCommand : ICommand
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
			todoList.Delete(index);
			Console.WriteLine($"Задача удалена: {item.Text}");
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine("Ошибка: неверный номер задачи");
		}
	}
}