namespace TodoList;

public class ViewCommand : ICommand
{
	public string[] parts { get; set; }
	public TodoList todoList { get; set; }

	public void Execute()
	{
		var showIndex = Program.HasFlag(parts, "--index", "-i");
		var showStatus = Program.HasFlag(parts, "--status", "-s");
		var showDate = Program.HasFlag(parts, "--update-date", "-d");
		var showAll = Program.HasFlag(parts, "--all", "-a");

		if (showAll)
		{
			showIndex = true;
			showStatus = true;
			showDate = true;
		}

		todoList.View(showIndex, showStatus, showDate);
	}
}