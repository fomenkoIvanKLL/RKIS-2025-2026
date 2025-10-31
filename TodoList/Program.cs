using System;

namespace TodoList
{
    class Program
    {
        private static TodoList todoList = new TodoList();
        private static Profile userProfile = null;

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
                ProcessCommand(input);
            }
        }

        static void ProcessCommand(string input)
        {
            string[] parts = input.Split(' ');
            string command = parts[0].ToLower();
            switch (command)
            {
                case "help": ShowHelp(); break;
                case "add": AddTask(parts); break;
                case "view": ViewTasks(parts); break;
                case "read": ReadTask(parts); break;
                case "done": MarkAsDone(parts); break;
                case "delete": DeleteTask(parts); break;
                case "update": UpdateTask(parts); break;
                case "profile": ProfileCommand(parts); break;
                case "exit": ExitProgram(); break;
                default: Console.WriteLine($"Неизвестная команда: {command}"); break;
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

        static void ProfileCommand(string[] parts)
        {
            if (parts.Length == 1)
            {
                if (userProfile != null)
                    Console.WriteLine(userProfile.GetInfo());
                else
                    Console.WriteLine("Профиль не установлен");
                return;
            }
            string subCommand = parts[1].ToLower();
            switch (subCommand)
            {
                case "установить":
                case "set":
                    if (parts.Length >= 5 && int.TryParse(parts[4], out int birthYear))
                    {
                        userProfile = new Profile(parts[2], parts[3], birthYear);
                        Console.WriteLine($"Профиль установлен: {userProfile.GetInfo()}");
                    }
                    else
                        Console.WriteLine("Ошибка: используйте формат: profile установить <имя> <фамилия> <год_рождения>");
                    break;
                case "показать":
                case "show":
                    if (userProfile != null)
                        Console.WriteLine(userProfile.GetInfo());
                    else
                        Console.WriteLine("Профиль не установлен");
                    break;
                case "очистить":
                case "clear":
                    userProfile = null;
                    Console.WriteLine("Профиль очищен");
                    break;
                default:
                    Console.WriteLine("Неизвестная подкоманда профиля");
                    break;
            }
        }
    }
}