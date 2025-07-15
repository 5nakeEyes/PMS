using PMS.Core.Models;
using System.Text.Json;

namespace PMS.Services
{
    public class TaskStorageService : ITaskStorageService
    {
        private const int CurrentSchemaVersion = 2;
        private static readonly string OldFileName = $"tasksV{CurrentSchemaVersion - 1}.json";
        private static readonly string NewFileName = $"tasksV{CurrentSchemaVersion}.json";

        private readonly string _oldPath;
        private readonly string _newPath;

        public TaskStorageService()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PMS");
            Directory.CreateDirectory(folder);
            _oldPath = Path.Combine(folder, OldFileName);
            _newPath = Path.Combine(folder, NewFileName);
        }

        public async Task<List<TaskModel>> LoadAsync()
        {
            if (File.Exists(_newPath))
                return await LoadFromFileAsync(_newPath);

            if (File.Exists(_oldPath))
            {
                var legacyModels = await LoadFromFileAsync(_oldPath);

                var migrated = MigrateV1toV2(legacyModels);

                await SaveToFileAsync(_newPath, migrated);

                /*
                var verify = await LoadFromFileAsync(_newPath);
                if (AreEqual(legacyModels, verify))
                    File.Delete(_oldPath);
                */

                return migrated;
            }

            return new List<TaskModel>();
        }

        public Task SaveAsync(IEnumerable<TaskModel> tasks)
            => SaveToFileAsync(_newPath, tasks);

        private async Task<List<TaskModel>> LoadFromFileAsync(string path)
        {
            using var stream = File.OpenRead(path);
            using var doc = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);
            var root = doc.RootElement;

            var version = root.GetProperty("schemaVersion").GetInt32();
            var tasksElem = root.GetProperty("tasks");

            if (version == CurrentSchemaVersion)
            {
                return JsonSerializer
                    .Deserialize<List<TaskModel>>(tasksElem.GetRawText())
                    ?? new List<TaskModel>();
            }

            if (version == CurrentSchemaVersion - 1)
            {
                var oldList = JsonSerializer
                    .Deserialize<List<TaskModel>>(tasksElem.GetRawText())
                    ?? new List<TaskModel>();

                return MigrateV1toV2(oldList);
            }

            throw new NotSupportedException(
                $"Nieobsługiwana wersja pliku: {version}");
        }

        private async Task SaveToFileAsync(string path, IEnumerable<TaskModel> tasks)
        {
            var container = new StorageFile
            {
                schemaVersion = CurrentSchemaVersion,
                tasks = new List<TaskModel>(tasks)
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            using var stream = File.Create(path);
            await JsonSerializer.SerializeAsync(stream, container, options)
                                .ConfigureAwait(false);
        }

        private class StorageFile
        {
            public int schemaVersion { get; set; }
            public List<TaskModel> tasks { get; set; } = new();
        }

        private List<TaskModel> MigrateV1toV2(IEnumerable<TaskModel> oldList)
        {
            //Optionally, fill in new fields or logic here
            return new List<TaskModel>(oldList);
        }

        private bool AreEqual(List<TaskModel> legacy, List<TaskModel> migrated)
        {
            if (legacy.Count != migrated.Count)
                return false;

            var a = legacy.OrderBy(t => t.Id).ToList();
            var b = migrated.OrderBy(t => t.Id).ToList();

            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].Id != b[i].Id) return false;
                if (a[i].Title != b[i].Title) return false;
                if (a[i].Description != b[i].Description) return false;
                if (a[i].DueDate != b[i].DueDate) return false;
                if (a[i].State != b[i].State) return false;
                if (a[i].Priority != b[i].Priority) return false;
            }
            return true;
        }
    }
}