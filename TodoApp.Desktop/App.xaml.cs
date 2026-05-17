using System.Windows;
using TodoApp.Desktop.Views;

namespace TodoApp.Desktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var loginWindow = new LoginWindow();
        loginWindow.Show();
    }
}