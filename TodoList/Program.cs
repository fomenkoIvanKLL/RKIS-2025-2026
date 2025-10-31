using System;

namespace TodoList
{
    class Program
    {
        private static TodoList todoList = new TodoList();
        private static Profile? userProfile = null;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Todo List Application ===");
            Console.WriteLine("Практическую работу 6 сделали: Фоменко и Мартиросьян");
            Console.WriteLine("Введите 'help' для списка команд");
            
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                ICommand command = CommandParser.Parse(input, todoList, userProfile);
                command.Execute();
            }
        }

        public static bool HasFlag(string[] parts, string fullFlag, string shortFlag)
        {
            foreach (string part in parts)
            {
                if (part == fullFlag || part == shortFlag)
                    return true;
                if (part.StartsWith("-") && part.Length > 1 && !part.StartsWith("--"))
                {
                    if (part.Contains(shortFlag.Replace("-", "")))
                        return true;
                }
            }
            return false;
        }
    }
}