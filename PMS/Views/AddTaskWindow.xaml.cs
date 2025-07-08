using System.Windows;
using PMS.Models;
using PMS.ViewModels;

namespace PMS.Views
{
    public partial class AddTaskWindow : Window
    {
        public TaskModel CreatedTask { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();
            DataContext = new AddTaskViewModel();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var vm = (AddTaskViewModel)DataContext;
            // create TaskModel from ViewModel data
            CreatedTask = new TaskModel(
                title: vm.Title,
                description: vm.Description,
                dueDate: vm.DueDate,
                state: vm.State,
                priority: vm.Priority);

            DialogResult = true;
        }
    }
}