namespace TodoList;

public class ProfileCommand(): ICommand
{
    public string[] parts { get; set; }

    public void Execute()
    {
        if (parts.Length == 1)
        {
            if (CommandParser.userProfile != null)
                Console.WriteLine(CommandParser.userProfile.GetInfo());
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
                    CommandParser.userProfile = new Profile(parts[2], parts[3], birthYear);
                    Console.WriteLine($"Профиль установлен: {CommandParser.userProfile.GetInfo()}");
                }
                else
                    Console.WriteLine("Ошибка: используйте формат: profile установить <имя> <фамилия> <год_рождения>");
                break;
            case "показать":
            case "show":
                if (CommandParser.userProfile != null)
                    Console.WriteLine(CommandParser.userProfile.GetInfo());
                else
                    Console.WriteLine("Профиль не установлен");
                break;
            case "очистить":
            case "clear":
                CommandParser.userProfile = null;
                Console.WriteLine("Профиль очищен");
                break;
            default:
                Console.WriteLine("Неизвестная подкоманда профиля");
                break;
        }
    }
}