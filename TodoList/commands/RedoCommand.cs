namespace TodoList.commands;

public class RedoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.RedoStack.Count == 0)
        {
            Console.WriteLine("Нет действий для повторения");
            return;
        }

        var command = AppInfo.RedoStack.Pop();
        command.Execute();
        AppInfo.UndoStack.Push(command);
        FileManager.SaveTodos(AppInfo.Todos);
    }

    public void Unexecute()
    {
        // Команда redo не требует отмены
    }
}