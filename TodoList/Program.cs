using System;
using System.IO;

namespace TodoList
{
    class Program
    {
        private const int InitialCapacity = 2;
        private const string TasksFile = "tasks.txt";
        
        private static string[] tasks = new string[InitialCapacity];
        private static int taskCount = 0;

        static void Main(string[] args)
        {
            Initialize();
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

        static void Initialize()
        {
            LoadTasksFromFile();
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
            Console.WriteLine("\nДоступные команды:");
            Console.WriteLine("help          - показать эту справку");
            Console.WriteLine("add <task>    - добавить новую задачу");
            Console.WriteLine("view          - показать все задачи");
            Console.WriteLine("exit          - выйти из программы\n");
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
            SaveTasksToFile();
        }

        static void ViewTasks()
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            Console.WriteLine("\nСписок задач:");
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
            Console.WriteLine();
        }

        static void ExpandArrays()
        {
            int newSize = tasks.Length * 2;
            string[] newTasks = new string[newSize];
            Array.Copy(tasks, newTasks, taskCount);
            tasks = newTasks;
        }

        static void SaveTasksToFile()
        {
            try
            {
                using StreamWriter writer = new StreamWriter(TasksFile);
                for (int i = 0; i < taskCount; i++)
                    writer.WriteLine($"{tasks[i]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }

        static void LoadTasksFromFile()
        {
            if (!File.Exists(TasksFile)) return;

            try
            {
                string[] lines = File.ReadAllLines(TasksFile);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    if (taskCount >= tasks.Length)
                        ExpandArrays();

                    tasks[taskCount] = line;
                    taskCount++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
            }
        }

        static void ExitProgram()
        {
            Console.WriteLine("Сохранение данных...");
            SaveTasksToFile();
            Console.WriteLine("Выход из программы. До свидания!");
            Environment.Exit(0);
        }
    }
}