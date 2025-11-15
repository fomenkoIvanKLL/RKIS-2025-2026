namespace TodoList.commands;

public class SetStatusCommand : ICommand
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
			var status = Enum.Parse<TodoStatus>(parts[2], true);
			var item = todoList.GetItem(index);
			item.SetStatus(status);
			Console.WriteLine($"Поставлен новый статус({status}) для задачи '{item.Text}' ");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка: {ex.Message}");
		}
	}
}