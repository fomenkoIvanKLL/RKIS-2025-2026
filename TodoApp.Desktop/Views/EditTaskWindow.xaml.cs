using System;
using System.Windows;
using TodoApp.Desktop.ViewModels;
using TodoApp.Models;

namespace TodoApp.Desktop.Views;

public partial class EditTaskWindow : Window
{
    public EditTaskWindow(TodoItem task)
    {
        InitializeComponent();
        var vm = new EditTaskViewModel(task);
        DataContext = vm;
        vm.CloseRequest += (saved) =>
        {
            DialogResult = saved;
            Close();
        };
        // Заполнение ComboBox значениями enum TodoStatus
        StatusComboBox.ItemsSource = Enum.GetValues(typeof(TodoStatus));
        // Устанавливаем выбранный статус (биндинг уже есть, но для синхронизации)
        StatusComboBox.SelectedItem = vm.SelectedStatus;
    }
}