namespace TodoList.commands;

public class ProfileCommand : ICommand
{
    public string[] parts { get; set; }
    public Profile OldProfile { get; private set; }
    public Profile NewProfile { get; private set; }

    public void Execute()
    {
        if (parts.Length == 1)
        {
            Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
            return;
        }

        var subCommand = parts[1].ToLower();
        switch (subCommand)
        {
            case "установить":
            case "set":
                if (parts.Length >= 5 && int.TryParse(parts[4], out var birthYear))
                {
                    OldProfile = AppInfo.CurrentProfile;
                    NewProfile = new Profile(parts[2], parts[3], birthYear);
                    AppInfo.CurrentProfile = NewProfile;
                    FileManager.SaveProfile(AppInfo.CurrentProfile);
                    Console.WriteLine($"Профиль установлен: {AppInfo.CurrentProfile.GetInfo()}");
                    AppInfo.UndoStack.Push(this);
                }
                else
                    Console.WriteLine("Ошибка: используйте формат: profile установить <имя> <фамилия> <год_рождения>");

                break;
            case "показать":
            case "show":
                Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
                break;
            default:
                Console.WriteLine("Неизвестная подкоманда профиля");
                break;
        }
    }

    public void Unexecute()
    {
        if (OldProfile != null && NewProfile != null)
        {
            AppInfo.CurrentProfile = OldProfile;
            FileManager.SaveProfile(AppInfo.CurrentProfile);
            Console.WriteLine($"Отменена смена профиля. Восстановлен профиль: {OldProfile.GetInfo()}");
        }
    }
}