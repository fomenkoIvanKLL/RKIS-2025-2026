using System;

namespace TodoList
{
    class Program
    {
        private const int InitialCapacity = 2;
        private static string[] tasks = new string[InitialCapacity];
        private static int taskCount = 0;

        static void Main(string[] args)
        {
            ShowWelcome();
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                ProcessCommand(input);
            }
        }

        static void ShowWelcome()
        {
            Console.WriteLine("=== Todo List Application ===");
            Console.WriteLine("Практическую работу 4 сделали: Фоменко и Мартиросьян");
            Console.WriteLine("Введите 'help' для списка команд");
        }

        static void ProcessCommand(string input)
        {
            string[] parts = input.Split(' ');
            string command = parts[0].ToLower();
            switch (command)
            {
                case "help": ShowHelp(); break;
                case "add": AddTask(parts); break;
                case "view": ViewTasks(); break;
                case "exit": ExitProgram(); break;
                default: Console.WriteLine($"Неизвестная команда: {command}"); break;
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help          - показать эту справку");
            Console.WriteLine("add <task>    - добавить новую задачу");
            Console.WriteLine("view          - показать все задачи");
            Console.WriteLine("exit          - выйти из программы");
        }

        static void AddTask(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: укажите текст задачи");
                return;
            }
            string taskText = string.Join(" ", parts, 1, parts.Length - 1);
            if (taskCount >= tasks.Length)
                ExpandArrays();
            tasks[taskCount] = taskText;
            taskCount++;
            Console.WriteLine($"Задача добавлена: {taskText}");
        }

        static void ViewTasks()
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }
            Console.WriteLine("Список задач:");
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }

        static void ExpandArrays()
        {
            int newSize = tasks.Length * 2;
            string[] newTasks = new string[newSize];
            Array.Copy(tasks, newTasks, taskCount);
            tasks = newTasks;
        }

        static void ExitProgram()
        {
            Console.WriteLine("Выход из программы. До свидания!");
            Environment.Exit(0);
        }
    }
}