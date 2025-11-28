namespace TodoList.commands;

public class SetStatusCommand : ICommand
{
    public string[] parts { get; set; }
    public TodoItem StatusItem { get; private set; }
    public TodoStatus OldStatus { get; private set; }
    public TodoStatus NewStatus { get; private set; }

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
            StatusItem = AppInfo.Todos.GetItem(index);
            OldStatus = StatusItem.Status;
            NewStatus = Enum.Parse<TodoStatus>(parts[2], true);
            
            StatusItem.SetStatus(NewStatus);
            Console.WriteLine($"Поставлен новый статус({NewStatus}) для задачи '{StatusItem.Text}'");
            AppInfo.UndoStack.Push(this);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    public void Unexecute()
    {
        if (StatusItem != null)
        {
            StatusItem.SetStatus(OldStatus);
            Console.WriteLine($"Отменена смена статуса. Восстановлен статус: {OldStatus}");
        }
    }
}