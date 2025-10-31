namespace TodoList;

public class ExitCommand: ICommand
{
    public void Execute()
    {
        Console.WriteLine("Выход из программы. До свидания!");
        Environment.Exit(0);
    }
}