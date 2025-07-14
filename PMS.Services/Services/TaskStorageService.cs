using PMS.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMS.Services
{
    public class TaskStorageService : ITaskStorageService
    {
        private readonly string _path;

        public TaskStorageService()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PMS");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _path = Path.Combine(folder, "tasks.json");
        }

        public async Task<List<TaskModel>> LoadAsync()
        {
            if (!File.Exists(_path))
                return new List<TaskModel>();

            try
            {
                using var stream = File.OpenRead(_path);
                var result = await JsonSerializer.DeserializeAsync<List<TaskModel>>(stream)
                                 .ConfigureAwait(false);
                return result ?? new List<TaskModel>();
            }
            catch
            {
                return new List<TaskModel>();
            }
        }

        public async Task SaveAsync(IEnumerable<TaskModel> tasks)
        {
            var dir = Path.GetDirectoryName(_path)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var options = new JsonSerializerOptions { WriteIndented = true };
            using var stream = File.Create(_path);
            await JsonSerializer.SerializeAsync(stream, tasks, options)
                                .ConfigureAwait(false);
        }
    }
}
