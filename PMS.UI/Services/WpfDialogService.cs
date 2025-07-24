using Microsoft.Extensions.DependencyInjection;
using PMS.Presentation.Interfaces;
using PMS.Presentation.ViewModels;
using PMS.UI.Views;
using System.Windows;

namespace PMS.UI.Services
{
    public class WpfDialogService : IDialogService
    {
        private readonly IServiceProvider _provider;

        public WpfDialogService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task<bool> ShowConfirmAsync(string title, string message)
        {
            var result = MessageBox.Show(message, title,
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            return Task.FromResult(result == MessageBoxResult.Yes);
        }

        public Task<bool> ShowDialogAsync<TViewModel>(TViewModel viewModel)
        {
            if (viewModel is AddTaskViewModel addVm)
            {
                // Opcja A: używamy ActivatorUtilities, żeby DI przekazało nasz viewModel
                /*var window = ActivatorUtilities.CreateInstance<AddTaskWindow>(
                    _provider,
                    addVm
                );*/

                // Opcja B: albo po prostu new AddTaskWindow(addVm);
                var window = new AddTaskWindow(addVm);

                var dialogResult = window.ShowDialog() == true;
                return Task.FromResult(dialogResult);
            }

            return Task.FromResult(false);
        }
    }
}