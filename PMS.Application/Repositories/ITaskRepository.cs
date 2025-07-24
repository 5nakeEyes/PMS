using PMS.Domain.Models;

namespace PMS.Application.Repositories

{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> GetAllAsync();
        Task<TaskModel?> GetByIdAsync(int id);
        Task AddAsync(TaskModel task);
        Task UpdateAsync(TaskModel task);
        Task DeleteAsync(int id);
    }
}
