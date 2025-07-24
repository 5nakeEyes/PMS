using Microsoft.Extensions.DependencyInjection;
using PMS.Presentation.ViewModels;
using System.Windows;

namespace PMS.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
            : this(App.Services.GetRequiredService<TaskViewModel>())
        {
        }

        public MainWindow(TaskViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}