using System;

namespace TodoList
{
    class Program
    {
        private const int InitialCapacity = 2;
        private static string[] tasks = new string[InitialCapacity];
        private static bool[] statuses = new bool[InitialCapacity];
        private static DateTime[] dates = new DateTime[InitialCapacity];
        private static int taskCount = 0;
        private static string userName = "";
        private static string userSurname = "";
        private static DateTime userBirthDate = DateTime.MinValue;

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
                case "done": MarkAsDone(parts); break;
                case "delete": DeleteTask(parts); break;
                case "update": UpdateTask(parts); break;
                case "profile": ProfileCommand(parts); break;
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
            Console.WriteLine("done <num>    - отметить задачу как выполненную");
            Console.WriteLine("delete <num>  - удалить задачу");
            Console.WriteLine("update <num> \"<text>\" - обновить текст задачи");
            Console.WriteLine("profile       - управление profile пользователя");
            Console.WriteLine("exit          - выйти из программы");
        }

        static void ProfileCommand(string[] parts)
        {
            if (parts.Length == 1)
            {
                ShowProfile();
                return;
            }
            string subCommand = parts[1].ToLower();
            switch (subCommand)
            {
                case "установить":
                case "set":
                    if (parts.Length >= 5)
                        SetProfile(parts[2], parts[3], parts[4]);
                    else
                        Console.WriteLine("Ошибка: используйте формат: profile установить <имя> <фамилия> <дата_рождения>");
                    break;
                case "показать":
                case "show":
                    ShowProfile();
                    break;
                case "очистить":
                case "clear":
                    ClearProfile();
                    break;
                default:
                    Console.WriteLine("Неизвестная подкоманда профиля");
                    break;
            }
        }

        static void SetProfile(string name, string surname, string birthDateStr)
        {
            if (DateTime.TryParse(birthDateStr, out DateTime birthDate))
            {
                userName = name;
                userSurname = surname;
                userBirthDate = birthDate;
                Console.WriteLine($"Профиль установлен: {userName} {userSurname}, дата рождения: {userBirthDate:dd.MM.yyyy}");
            }
            else
            {
                Console.WriteLine("Ошибка: неверный формат даты. Используйте формат ДД.ММ.ГГГГ");
            }
        }

        static void ShowProfile()
        {
            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Профиль не установлен");
            }
            else
            {
                Console.WriteLine("=== Профиль пользователя ===");
                Console.WriteLine($"Имя: {userName}");
                Console.WriteLine($"Фамилия: {userSurname}");
                Console.WriteLine($"Дата рождения: {userBirthDate:dd.MM.yyyy}");
                if (userBirthDate != DateTime.MinValue)
                {
                    int age = DateTime.Now.Year - userBirthDate.Year;
                    if (DateTime.Now < userBirthDate.AddYears(age)) age--;
                    Console.WriteLine($"Возраст: {age} лет");
                }
            }
        }

        static void ClearProfile()
        {
            userName = "";
            userSurname = "";
            userBirthDate = DateTime.MinValue;
            Console.WriteLine("Профиль очищен");
        }

        static void AddTask(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: укажите текст задачи");
                return;
            }
            string taskText = string.Join(" ", parts, 1, parts.Length - 1);
            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                return;
            }
            if (taskCount >= tasks.Length)
                ExpandArrays();
            tasks[taskCount] = taskText;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
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
            Console.WriteLine("№  Статус      Дата                Задача");
            Console.WriteLine("--------------------------------------------");
            for (int i = 0; i < taskCount; i++)
            {
                string status = statuses[i] ? "Сделано   " : "Не сделано";
                string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                Console.WriteLine($"{i + 1,-2} {status} {date} {tasks[i]}");
            }
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
            if (statuses[index])
            {
                Console.WriteLine("Задача уже отмечена как выполненная");
                return;
            }
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача '{tasks[index]}' отмечена как выполненная");
        }

        static void DeleteTask(string[] parts)
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
            string deletedTask = tasks[index];
            for (int i = index; i < taskCount - 1; i++)
            {
                tasks[i] = tasks[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }
            taskCount--;
            Console.WriteLine($"Задача удалена: {deletedTask}");
        }

        static void UpdateTask(string[] parts)
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
            if (index < 0 || index >= taskCount)
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }
            string newText = string.Join(" ", parts, 2, parts.Length - 2);
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Ошибка: новый текст задачи не может быть пустым");
                return;
            }
            if (newText.StartsWith("\"") && newText.EndsWith("\""))
                newText = newText.Substring(1, newText.Length - 2);
            string oldText = tasks[index];
            tasks[index] = newText;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача обновлена: '{oldText}' -> '{newText}'");
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

        static void ExitProgram()
        {
            Console.WriteLine("Выход из программы. До свидания!");
            Environment.Exit(0);
        }
    }
}