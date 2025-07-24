using PMS.Application.Repositories;
using PMS.Domain.Models;
using PMS.Infrastructure.Services;

namespace PMS.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ITaskStorageService _storage;

        public TaskRepository(ITaskStorageService storage) =>
            _storage = storage;

        public Task<List<TaskModel>> GetAllAsync() =>
            _storage.LoadAsync();

        public async Task<TaskModel?> GetByIdAsync(int id) =>
            (await GetAllAsync()).FirstOrDefault(t => t.Id == id);

        public async Task AddAsync(TaskModel task)
        {
            var list = await GetAllAsync();

            var usedIds = list
                .Select(t => t.Id)
                .OrderBy(x => x)
                .ToList();

            int newId = 0;
            foreach (var id in usedIds)
            {
                if (id == newId)
                {
                    newId++;
                }
                else if (id > newId)
                {
                    break;
                }
            }

            task.Id = newId;
            list.Add(task);

            await _storage.SaveAsync(list);
        }

        public async Task UpdateAsync(TaskModel task)
        {
            var list = await GetAllAsync();
            var idx = list.FindIndex(t => t.Id == task.Id);
            if (idx >= 0)
                list[idx] = task;

            await _storage.SaveAsync(list);
        }

        public async Task DeleteAsync(int id)
        {
            var list = await GetAllAsync();
            if (list.RemoveAll(t => t.Id == id) > 0)
                await _storage.SaveAsync(list);
        }
    }
}