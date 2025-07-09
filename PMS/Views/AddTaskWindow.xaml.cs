using System.Windows;
using PMS.ViewModels;

namespace PMS.Views
{
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow(AddTaskViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.RequestClose += result => this.DialogResult = result;
        }
    }
}