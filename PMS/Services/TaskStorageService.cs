using PMS.Core.Models;
using System.IO;
using System.Text.Json;

namespace PMS.Services
{
    public static class TaskStorageService
    {
        private static readonly string _path =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PMS",
                "tasks.json");

        public static List<TaskModel> Load()
        {
            try
            {
                if (!File.Exists(_path))
                    return new List<TaskModel>();

                var json = File.ReadAllText(_path);
                return JsonSerializer.Deserialize<List<TaskModel>>(json)
                       ?? new List<TaskModel>();
            }
            catch
            {
                return new List<TaskModel>();
            }
        }

        public static void Save(IEnumerable<TaskModel> list)
        {
            var dir = Path.GetDirectoryName(_path)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(list, options);
            File.WriteAllText(_path, json);
        }
    }
}
