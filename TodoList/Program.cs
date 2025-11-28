using TodoList.commands;

namespace TodoList;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== Todo List Application ===");
        Console.WriteLine("Практическую работу 10 сделали: Фоменко и Мартиросьян");
        Console.WriteLine("Введите 'help' для списка команд");
        
        FileManager.EnsureAllData();
        AppInfo.Todos = FileManager.LoadTodos();
        AppInfo.CurrentProfile = FileManager.LoadProfile();
        
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;
            
            var command = CommandParser.Parse(input);
            command.Execute();
            FileManager.SaveTodos(AppInfo.Todos);
        }
    }
}