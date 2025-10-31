namespace TodoList;

public class ViewCommand: ICommand
{
    public string[] parts { get; set; }
    public TodoList todoList { get; set; }
    
    public void Execute()
    {
        bool showIndex = Program.HasFlag(parts, "--index", "-i");
        bool showStatus = Program.HasFlag(parts, "--status", "-s");
        bool showDate = Program.HasFlag(parts, "--update-date", "-d");
        bool showAll = Program.HasFlag(parts, "--all", "-a");

        if (showAll)
        {
            showIndex = true;
            showStatus = true;
            showDate = true;
        }

        todoList.View(showIndex, showStatus, showDate);
    }
}