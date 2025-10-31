namespace TodoList;

public class UpdateCommand: ICommand
{
    public string[] parts { get; set; }
    public TodoList todoList { get; set; }

    public void Execute()
    {
        if (parts.Length < 3)
        {
            Console.WriteLine("Ошибка: укажите номер и новый текст задачи");
            return;
        }
        if (!int.TryParse(parts[1], out int taskNumber))
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
            return;
        }
        int index = taskNumber - 1;
        try
        {
            TodoItem item = todoList.GetItem(index);
            string newText = string.Join(" ", parts, 2, parts.Length - 2);
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Ошибка: новый текст задачи не может быть пустым");
                return;
            }
            if (newText.StartsWith("\"") && newText.EndsWith("\""))
                newText = newText.Substring(1, newText.Length - 2);
            item.UpdateText(newText);
            Console.WriteLine($"Задача обновлена: '{item.Text}'");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
}