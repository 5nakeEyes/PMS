using System.Windows;
using PMS.Presentation.ViewModels;

namespace PMS.UI.Views
{
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow(AddTaskViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.RequestClose += result => DialogResult = result;
        }
    }
}
