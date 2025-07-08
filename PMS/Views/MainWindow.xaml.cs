using PMS.ViewModels;
using System.Windows;

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
            //create and show dialogue
            var dlg = new Views.AddTaskWindow
            {
                Owner = this
            };

            if (dlg.ShowDialog() == true)
            {
                // download the new TaskModel and add it to the collection
                var newTask = dlg.CreatedTask;
                var vm = (TaskViewModel)DataContext;
                vm.Tasks.Add(newTask);
                vm.SelectedTask = newTask;
            }
        }
    }
}