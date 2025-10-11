using System;
using System.IO;

namespace TodoList
{
    class Program
    {
        private const int InitialCapacity = 2;
        private const string TasksFile = "tasks.txt";
        
        private static string[] tasks = new string[InitialCapacity];
        private static bool[] statuses = new bool[InitialCapacity];
        private static DateTime[] dates = new DateTime[InitialCapacity];
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
                case "done": MarkAsDone(parts); break;
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
            Console.WriteLine("done <num>    - отметить задачу как выполненную");
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
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
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

            Console.WriteLine("\n№  Статус      Дата                Задача");
            Console.WriteLine("--------------------------------------------");
            
            for (int i = 0; i < taskCount; i++)
            {
                string status = statuses[i] ? "Сделано   " : "Не сделано";
                string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                Console.WriteLine($"{i + 1,-2} {status} {date} {tasks[i]}");
            }
            Console.WriteLine();
        }

        static void MarkAsDone(string[] parts)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= taskCount)
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача '{tasks[index]}' отмечена как выполненная");
            SaveTasksToFile();
        }

        static void ExpandArrays()
        {
            int newSize = tasks.Length * 2;

            string[] newTasks = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];
            
            Array.Copy(tasks, newTasks, taskCount);
            Array.Copy(statuses, newStatuses, taskCount);
            Array.Copy(dates, newDates, taskCount);
            
            tasks = newTasks;
            statuses = newStatuses;
            dates = newDates;
        }

        static void SaveTasksToFile()
        {
            try
            {
                using StreamWriter writer = new StreamWriter(TasksFile);
                for (int i = 0; i < taskCount; i++)
                    writer.WriteLine($"{tasks[i]}|{statuses[i]}|{dates[i]}");
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

                    string[] parts = line.Split('|');
                    if (parts.Length >= 3)
                    {
                        if (taskCount >= tasks.Length)
                            ExpandArrays();

                        tasks[taskCount] = parts[0];
                        statuses[taskCount] = bool.Parse(parts[1]);
                        dates[taskCount] = DateTime.Parse(parts[2]);
                        taskCount++;
                    }
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