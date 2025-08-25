using PMS.Models;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace PMS.Services
{
    public static class ProjectJsonStorage
    {
        private static readonly string BaseDir =
            Path.Combine(Environment.CurrentDirectory, "Projects");

        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        static ProjectJsonStorage()
        {
            Directory.CreateDirectory(BaseDir);
        }

        private static string GetPath(string projectName) =>
            Path.Combine(BaseDir, projectName + ".json");

        public static void Save(ProjectModel project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
                throw new ArgumentException("Nazwa projektu nie może być pusta.");

            var json = JsonSerializer.Serialize(project, _options);
            File.WriteAllText(GetPath(project.Name), json);
        }

        public static ProjectModel Load(string projectName)
        {
            var path = GetPath(projectName);
            if (!File.Exists(path))
                throw new FileNotFoundException($"Brak pliku projektu: {path}");

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<ProjectModel>(json, _options)
                   ?? throw new InvalidOperationException("Deserializacja zwróciła null.");
        }

        public static IEnumerable<string> GetAllProjectNames() =>
            Directory
                .EnumerateFiles(BaseDir, "*.json")
                .Select(f => Path.GetFileNameWithoutExtension(f));

        public static void Delete(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentException("Nazwa projektu nie może być pusta.");

            var path = GetPath(projectName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                throw new FileNotFoundException($"Nie znaleziono pliku projektu do usunięcia: {path}");
            }
        }
    }
}