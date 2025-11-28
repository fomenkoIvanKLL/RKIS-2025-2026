namespace TodoList.commands;

public class UndoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.UndoStack.Count == 0)
        {
            Console.WriteLine("Нет действий для отмены");
            return;
        }

        var command = AppInfo.UndoStack.Pop();
        command.Unexecute();
        AppInfo.RedoStack.Push(command);
        FileManager.SaveTodos(AppInfo.Todos);
    }

    public void Unexecute()
    {
        // Команда undo не требует отмены
    }
}