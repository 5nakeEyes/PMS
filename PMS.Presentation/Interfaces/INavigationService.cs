namespace PMS.Presentation.Interfaces
{
    public interface INavigationService
    {
        Task NavigateToAsync(string route, object? parameter = null);
        Task GoBackAsync();
    }
}