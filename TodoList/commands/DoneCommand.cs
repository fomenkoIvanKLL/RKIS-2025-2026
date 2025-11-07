namespace TodoList.commands;

public class DoneCommand : ICommand
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
			item.MarkDone();
			Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная");
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine("Ошибка: неверный номер задачи");
		}
	}
}