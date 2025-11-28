using System.Collections.Generic;
using TodoList.commands;

namespace TodoList;

public static class AppInfo
{
    public static TodoList Todos { get; set; } = new TodoList();
    public static Profile CurrentProfile { get; set; } = new Profile("Default", "User", 2000);
    public static Stack<ICommand> UndoStack { get; set; } = new Stack<ICommand>();
    public static Stack<ICommand> RedoStack { get; set; } = new Stack<ICommand>();
}