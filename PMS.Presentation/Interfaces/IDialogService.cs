namespace PMS.Presentation.Interfaces
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmAsync(string title, string message);
        Task<bool> ShowDialogAsync<TViewModel>(TViewModel viewModel);
    }
}