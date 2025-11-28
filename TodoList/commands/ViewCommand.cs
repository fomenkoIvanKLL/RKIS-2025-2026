namespace TodoList.commands;

public class ViewCommand : ICommand
{
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }
    public bool ShowAll { get; set; }

    public void Execute()
    {
        if (ShowAll)
        {
            ShowIndex = true;
            ShowStatus = true;
            ShowDate = true;
        }

        AppInfo.Todos.View(ShowIndex, ShowStatus, ShowDate);
    }

    public void Unexecute()
    {
        // Команда view не требует отмены
    }
}