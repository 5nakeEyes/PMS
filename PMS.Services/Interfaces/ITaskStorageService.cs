using PMS.Core.Models;

namespace PMS.Services
{
    public interface ITaskStorageService
    {
        Task<List<TaskModel>> LoadAsync();
        Task SaveAsync(IEnumerable<TaskModel> tasks);
    }
}