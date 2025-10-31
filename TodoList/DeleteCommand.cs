namespace TodoList;

public class DeleteCommand
{
    public string[] parts { get; set; }
    public TodoList todoList { get; set; }

    public void Execute()
    {
        if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
        {
            Console.WriteLine("Ошибка: укажите номер задачи");
            return;
        }
        int index = taskNumber - 1;
        try
        {
            TodoItem item = todoList.GetItem(index);
            todoList.Delete(index);
            Console.WriteLine($"Задача удалена: {item.Text}");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
}