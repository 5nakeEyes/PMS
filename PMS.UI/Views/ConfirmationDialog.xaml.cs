using PMS.UI.ViewModels;
using System.Windows;

namespace PMS.UI.Views
{
    public partial class ConfirmationDialog : Window
    {
        public ConfirmationDialog()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object s, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ConfirmationDialogViewModel vm)
            {
                vm.RequestClose += result =>
                {
                    DialogResult = result;
                };
            }
        }
    }
}
