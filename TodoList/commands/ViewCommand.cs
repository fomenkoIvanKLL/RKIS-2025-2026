namespace TodoList.commands;

public class ViewCommand : ICommand
{
	public bool ShowIndex { get; set; }
	public bool ShowStatus { get; set; }
	public bool ShowDate { get; set; }
	public bool ShowAll { get; set; }
	public TodoList todoList { get; set; }

	public void Execute()
	{
		if (ShowAll)
		{
			ShowIndex = true;
			ShowStatus = true;
			ShowDate = true;
		}

		todoList.View(ShowIndex, ShowStatus, ShowDate);
	}
}