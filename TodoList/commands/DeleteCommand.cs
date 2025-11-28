namespace TodoList.commands;

public class DeleteCommand : ICommand
{
    public string[] parts { get; set; }
    public TodoItem DeletedItem { get; private set; }
    public int DeletedIndex { get; private set; }

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
            DeletedItem = AppInfo.Todos.GetItem(index);
            DeletedIndex = index;
            AppInfo.Todos.Delete(index);
            Console.WriteLine($"Задача удалена: {DeletedItem.Text}");
            AppInfo.UndoStack.Push(this);
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }

    public void Unexecute()
    {
        if (DeletedItem != null)
        {
            AppInfo.Todos.items.Insert(DeletedIndex, DeletedItem);
            Console.WriteLine($"Отменено удаление задачи: {DeletedItem.Text}");
        }
    }
}