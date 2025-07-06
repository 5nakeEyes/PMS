using PMS.Models;
using PMS.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PMS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // stwórz i pokaż dialog
            var dlg = new Views.AddTaskWindow
            {
                Owner = this
            };

            if (dlg.ShowDialog() == true)
            {
                // pobierz nowy TaskModel i dodaj do kolekcji
                var newTask = dlg.CreatedTask;
                var vm = (TaskViewModel)DataContext;
                vm.Tasks.Add(newTask);
                vm.SelectedTask = newTask;
            }
        }
    }
}