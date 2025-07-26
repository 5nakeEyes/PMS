using PMS.Domain.Models;
using System.Text.Json;

namespace PMS.Infrastructure.Services
{
    public class TaskStorageService : ITaskStorageService
    {
        private const string FileName = "tasks.json";
        private readonly string _filePath;

        public TaskStorageService()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PMS");
            Directory.CreateDirectory(folder);
            _filePath = Path.Combine(folder, FileName);
        }

        public async Task<List<TaskModel>> LoadAsync()
        {
            if (!File.Exists(_filePath))
                return new List<TaskModel>();

            using var stream = File.OpenRead(_filePath);
            return await JsonSerializer
                .DeserializeAsync<List<TaskModel>>(stream)
                .ConfigureAwait(false)
                   ?? new List<TaskModel>();
        }

        public Task SaveAsync(IEnumerable<TaskModel> tasks)
            => SaveToFileAsync(tasks);

        private async Task SaveToFileAsync(IEnumerable<TaskModel> tasks)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, tasks, options)
                                .ConfigureAwait(false);
        }
    }
}