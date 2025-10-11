using System;
using System.IO;

namespace TodoList
{
    class Program
    {
        private const int InitialCapacity = 2;
        private const int ArrayExpansionMultiplier = 2;
        private const string TasksFileName = "tasks.txt";
        private const string ProfileFileName = "profile.txt";
        private const char TaskSeparator = '|';
        private const char CommandSeparator = ' ';
        
        private static string[] tasks;
        private static bool[] taskStatuses;
        private static DateTime[] taskDates;
        private static int taskCount;

        private static string userName = "";
        private static string userSurname = "";
        private static DateTime userBirthDate = DateTime.MinValue;

        static void Main(string[] args)
        {
            InitializeApplication();
            ShowWelcomeMessage();
            
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                    
                ProcessCommand(input);
            }
        }

        static void InitializeApplication()
        {
            tasks = new string[InitialCapacity];
            taskStatuses = new bool[InitialCapacity];
            taskDates = new DateTime[InitialCapacity];
            taskCount = 0;
            
            LoadTasksFromFile();
            LoadProfileFromFile();
        }

        static void ShowWelcomeMessage()
        {
            Console.WriteLine("=== Todo List Application ===");
            Console.WriteLine("Практическую работу 4 сделали: Фоменко и Мартиросьян");
            Console.WriteLine("Введите 'help' для списка команд");
        }

        static void ProcessCommand(string input)
        {
            string[] commandParts = input.Split(CommandSeparator);
            string command = commandParts[0].ToLower();

            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                case "add":
                    HandleAddCommand(commandParts);
                    break;
                case "view":
                    HandleViewCommand();
                    break;
                case "done":
                    HandleDoneCommand(commandParts);
                    break;
                case "delete":
                    HandleDeleteCommand(commandParts);
                    break;
                case "update":
                    HandleUpdateCommand(commandParts);
                    break;
                case "profile":
                    HandleProfileCommand(commandParts);
                    break;
                case "exit":
                    HandleExitCommand();
                    break;
                default:
                    ShowUnknownCommandError(command);
                    break;
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("\nДоступные команды:");
            Console.WriteLine("help          - показать эту справку");
            Console.WriteLine("add <task>    - добавить новую задачу");
            Console.WriteLine("view          - показать все задачи");
            Console.WriteLine("done <num>    - отметить задачу как выполненную");
            Console.WriteLine("delete <num>  - удалить задачу");
            Console.WriteLine("update <num> \"<text>\" - обновить текст задачи");
            Console.WriteLine("profile       - управление профилем пользователя");
            Console.WriteLine("exit          - выйти из программы\n");
        }

        static void ShowUnknownCommandError(string command)
        {
            Console.WriteLine($"Неизвестная команда: {command}");
        }

        static void HandleAddCommand(string[] commandParts)
        {
            if (commandParts.Length < 2)
            {
                Console.WriteLine("Ошибка: укажите текст задачи");
                return;
            }

            string taskText = string.Join(CommandSeparator.ToString(), commandParts, 1, commandParts.Length - 1);
            AddNewTask(taskText);
        }

        static void HandleViewCommand()
        {
            DisplayAllTasks();
        }

        static void HandleDoneCommand(string[] commandParts)
        {
            if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            MarkTaskAsCompleted(taskNumber);
        }

        static void HandleDeleteCommand(string[] commandParts)
        {
            if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            DeleteTaskByNumber(taskNumber);
        }

        static void HandleUpdateCommand(string[] commandParts)
        {
            if (commandParts.Length < 3)
            {
                Console.WriteLine("Ошибка: укажите номер и новый текст задачи");
                Console.WriteLine("Пример: update 1 \"Новый текст задачи\"");
                return;
            }

            if (!int.TryParse(commandParts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            string newText = string.Join(CommandSeparator.ToString(), commandParts, 2, commandParts.Length - 2);
            
            if (newText.StartsWith("\"") && newText.EndsWith("\""))
            {
                newText = newText.Substring(1, newText.Length - 2);
            }

            UpdateTaskText(taskNumber, newText);
        }

        static void HandleProfileCommand(string[] commandParts)
        {
            if (commandParts.Length == 1)
            {
                ShowUserProfile();
                return;
            }

            string subCommand = commandParts[1].ToLower();
            
            switch (subCommand)
            {
                case "установить":
                case "set":
                    if (commandParts.Length >= 5)
                    {
                        SetUserProfile(commandParts[2], commandParts[3], commandParts[4]);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: используйте формат: profile установить <имя> <фамилия> <дата_рождения>");
                        Console.WriteLine("Пример: profile установить Иван Иванов 15.05.1990");
                    }
                    break;
                case "показать":
                case "show":
                    ShowUserProfile();
                    break;
                case "очистить":
                case "clear":
                    ClearUserProfile();
                    break;
                default:
                    Console.WriteLine("Неизвестная подкоманда профиля");
                    Console.WriteLine("Доступные подкоманды: установить, показать, очистить");
                    break;
            }
        }

        static void HandleExitCommand()
        {
            ExitApplication();
        }

        static void AddNewTask(string taskText)
        {
            EnsureArrayCapacity();
            
            tasks[taskCount] = taskText;
            taskStatuses[taskCount] = false;
            taskDates[taskCount] = DateTime.Now;
            taskCount++;

            Console.WriteLine($"Задача добавлена: {taskText}");
            SaveTasksToFile();
        }

        static void DisplayAllTasks()
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
                string status = taskStatuses[i] ? "Сделано   " : "Не сделано";
                string date = taskDates[i].ToString("dd.MM.yyyy HH:mm");
                Console.WriteLine($"{i + 1,-2} {status} {date} {tasks[i]}");
            }
            Console.WriteLine();
        }

        static void MarkTaskAsCompleted(int taskNumber)
        {
            int taskIndex = taskNumber - 1;
            if (!IsValidTaskIndex(taskIndex))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            taskStatuses[taskIndex] = true;
            taskDates[taskIndex] = DateTime.Now;
            
            Console.WriteLine($"Задача '{tasks[taskIndex]}' отмечена как выполненная");
            SaveTasksToFile();
        }

        static void DeleteTaskByNumber(int taskNumber)
        {
            int taskIndex = taskNumber - 1;
            if (!IsValidTaskIndex(taskIndex))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            string deletedTask = tasks[taskIndex];
            RemoveTaskAtIndex(taskIndex);
            
            Console.WriteLine($"Задача удалена: {deletedTask}");
            SaveTasksToFile();
        }

        static void UpdateTaskText(int taskNumber, string newText)
        {
            int taskIndex = taskNumber - 1;
            if (!IsValidTaskIndex(taskIndex))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            string oldText = tasks[taskIndex];
            tasks[taskIndex] = newText;
            taskDates[taskIndex] = DateTime.Now;
            
            Console.WriteLine($"Задача обновлена: '{oldText}' -> '{newText}'");
            SaveTasksToFile();
        }

        static void SetUserProfile(string name, string surname, string birthDateString)
        {
            if (DateTime.TryParse(birthDateString, out DateTime birthDate))
            {
                userName = name;
                userSurname = surname;
                userBirthDate = birthDate;
                
                Console.WriteLine($"Профиль установлен: {userName} {userSurname}, дата рождения: {userBirthDate:dd.MM.yyyy}");
                SaveProfileToFile();
            }
            else
            {
                Console.WriteLine("Ошибка: неверный формат даты. Используйте формат ДД.ММ.ГГГГ");
            }
        }

        static void ShowUserProfile()
        {
            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Профиль не установлен");
                Console.WriteLine("Для установки профиля используйте: profile установить <имя> <фамилия> <дата_рождения>");
            }
            else
            {
                Console.WriteLine("\n=== Профиль пользователя ===");
                Console.WriteLine($"Имя: {userName}");
                Console.WriteLine($"Фамилия: {userSurname}");
                Console.WriteLine($"Дата рождения: {userBirthDate:dd.MM.yyyy}");
                
                if (userBirthDate != DateTime.MinValue)
                {
                    int age = DateTime.Now.Year - userBirthDate.Year;
                    if (DateTime.Now < userBirthDate.AddYears(age)) age--;
                    Console.WriteLine($"Возраст: {age} лет");
                }
                Console.WriteLine();
            }
        }

        static void ClearUserProfile()
        {
            userName = "";
            userSurname = "";
            userBirthDate = DateTime.MinValue;
            Console.WriteLine("Профиль очищен");
            SaveProfileToFile();
        }

        static bool IsValidTaskIndex(int index)
        {
            return index >= 0 && index < taskCount;
        }

        static void EnsureArrayCapacity()
        {
            if (taskCount >= tasks.Length)
            {
                ExpandArrays();
            }
        }

        static void RemoveTaskAtIndex(int index)
        {
            for (int i = index; i < taskCount - 1; i++)
            {
                tasks[i] = tasks[i + 1];
                taskStatuses[i] = taskStatuses[i + 1];
                taskDates[i] = taskDates[i + 1];
            }
            
            tasks[taskCount - 1] = null;
            taskStatuses[taskCount - 1] = false;
            taskDates[taskCount - 1] = DateTime.MinValue;
            
            taskCount--;
        }

        static void ExpandArrays()
        {
            int newSize = tasks.Length * ArrayExpansionMultiplier;

            string[] newTasks = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];
            
            for (int i = 0; i < taskCount; i++)
            {
                newTasks[i] = tasks[i];
                newStatuses[i] = taskStatuses[i];
                newDates[i] = taskDates[i];
            }
            
            tasks = newTasks;
            taskStatuses = newStatuses;
            taskDates = newDates;
            
            Console.WriteLine($"Массивы расширены до {newSize} элементов");
        }

        static void SaveTasksToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(TasksFileName))
                {
                    for (int i = 0; i < taskCount; i++)
                    {
                        writer.WriteLine($"{tasks[i]}{TaskSeparator}{taskStatuses[i]}{TaskSeparator}{taskDates[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }

        static void LoadTasksFromFile()
        {
            if (!File.Exists(TasksFileName))
                return;

            try
            {
                string[] lines = File.ReadAllLines(TasksFileName);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split(TaskSeparator);
                    if (parts.Length >= 3)
                    {
                        if (taskCount >= tasks.Length)
                        {
                            ExpandArrays();
                        }

                        tasks[taskCount] = parts[0];
                        taskStatuses[taskCount] = bool.Parse(parts[1]);
                        taskDates[taskCount] = DateTime.Parse(parts[2]);
                        taskCount++;
                    }
                }
                
                if (taskCount > 0)
                {
                    Console.WriteLine($"Загружено {taskCount} задач из файла");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
            }
        }

        static void SaveProfileToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ProfileFileName))
                {
                    writer.WriteLine(userName);
                    writer.WriteLine(userSurname);
                    writer.WriteLine(userBirthDate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении профиля: {ex.Message}");
            }
        }

        static void LoadProfileFromFile()
        {
            if (!File.Exists(ProfileFileName))
                return;

            try
            {
                string[] lines = File.ReadAllLines(ProfileFileName);
                if (lines.Length >= 3)
                {
                    userName = lines[0];
                    userSurname = lines[1];
                    if (DateTime.TryParse(lines[2], out DateTime birthDate))
                    {
                        userBirthDate = birthDate;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке профиля: {ex.Message}");
            }
        }

        static void ExitApplication()
        {
            Console.WriteLine("Сохранение данных...");
            SaveTasksToFile();
            SaveProfileToFile();
            Console.WriteLine("Выход из программы. До свидания!");
            Environment.Exit(0);
        }
    }
}