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

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help                          - показать эту справку");
            Console.WriteLine("add [--multiline|-m] <task>   - добавить задачу (многострочный режим с флагом)");
            Console.WriteLine("view [--index|-i] [--status|-s] [--update-date|-d] [--all|-a]");
            Console.WriteLine("                              - показать задачи с флагами отображения");
            Console.WriteLine("read <idx>                    - прочитать полный текст задачи");
            Console.WriteLine("done <num>                    - отметить задачу как выполненную");
            Console.WriteLine("delete <num>                  - удалить задачу");
            Console.WriteLine("update <num> \"<text>\"       - обновить текст задачи");
            Console.WriteLine("profile                       - управление профилем пользователя");
            Console.WriteLine("exit                          - выйти из программы");
            Console.WriteLine();
            Console.WriteLine("Флаги для команды view:");
            Console.WriteLine("  --index, -i       - показывать индекс задачи");
            Console.WriteLine("  --status, -s      - показывать статус задачи");
            Console.WriteLine("  --update-date, -d - показывать дату изменения");
            Console.WriteLine("  --all, -a         - показывать все данные");
            Console.WriteLine("Комбинации флагов: view -is, view --index --status и т.д.");
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

        static void ViewTasks(string[] parts)
        {
            bool showIndex = HasFlag(parts, "--index", "-i");
            bool showStatus = HasFlag(parts, "--status", "-s");
            bool showDate = HasFlag(parts, "--update-date", "-d");
            bool showAll = HasFlag(parts, "--all", "-a");

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }

            todoList.View(showIndex, showStatus, showDate);
        }

        static void ReadTask(string[] parts)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }
            int index = taskNumber - 1;
            try
            {
                TodoItem item = todoList.GetItem(index);
                Console.WriteLine($"Задача {taskNumber}:");
                Console.WriteLine(item.GetFullInfo());
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
            }
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

        static void ExitProgram()
        {
            Console.WriteLine("Выход из программы. До свидания!");
            Environment.Exit(0);
        }
    }
}