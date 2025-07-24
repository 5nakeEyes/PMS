using PMS.Domain.Models;

namespace PMS.Infrastructure.Services
{
    public interface ITaskStorageService
    {
        Task<List<TaskModel>> LoadAsync();
        Task SaveAsync(IEnumerable<TaskModel> tasks);
    }
}