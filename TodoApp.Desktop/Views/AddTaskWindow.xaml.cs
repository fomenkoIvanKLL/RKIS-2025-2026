using System;
using System.Windows;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views;

public partial class AddTaskWindow : Window
{
    public AddTaskWindow()
    {
        InitializeComponent();
        var vm = new AddTaskViewModel(Guid.Empty); // будет заменён в MainViewModel
        DataContext = vm;
        vm.CloseRequest += (saved) => { DialogResult = saved; Close(); };
        // Заполнение статусов
        var combo = FindName("StatusComboBox") as System.Windows.Controls.ComboBox;
        if (combo != null) combo.ItemsSource = Enum.GetValues(typeof(TodoStatus));
    }
}