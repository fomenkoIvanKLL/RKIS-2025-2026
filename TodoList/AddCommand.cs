namespace TodoList;

public class AddCommand: ICommand
{
    public string[] parts { get; set; }
    public TodoList todoList { get; set; }

    public void Execute()
    {
        bool multiline = Program.HasFlag(parts, "--multiline", "-m");
            
        if (multiline)
        {
            AddMultilineTask();
        }
        else
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: укажите текст задачи");
                return;
            }
            string taskText = string.Join(" ", parts, 1, parts.Length - 1);
            AddSingleTask(taskText);
        }
    }

    void AddSingleTask(string taskText)
    {
        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            return;
        }
        TodoItem newItem = new TodoItem(taskText);
        todoList.Add(newItem);
        Console.WriteLine($"Задача добавлена: {taskText}");
    }

    void AddMultilineTask()
    {
        Console.WriteLine("Введите текст задачи (для завершения введите 'end'):");
        string taskText = "";
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null)
                continue;
            if (line == "end")
                break;
            taskText += line + "\n";
        }
        taskText = taskText.TrimEnd('\n');
        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            return;
        }
        AddSingleTask(taskText);
    }
}