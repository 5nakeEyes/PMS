using PMS.Domain.Models;
using PMS.Presentation.Interfaces;
using System.Text.Json;

namespace PMS.Infrastructure.Services
{
    public class JsonProjectStorage : IProjectStorage
    {
        private readonly string _folder;

        public JsonProjectStorage()
        {
            // %APPDATA%\PMS\Projects
            var appData = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);
            _folder = Path.Combine(appData, "PMS", "Projects");
            Directory.CreateDirectory(_folder);
        }

        public async Task SaveAsync(ProjectModel project)
        {
            string fileName = $"{SanitizeName(project.Name)}.json";
            string tempPath = Path.Combine(_folder, fileName + ".tmp");
            string finalPath = Path.Combine(_folder, fileName);

            var options = new JsonSerializerOptions { WriteIndented = true };

            // Zapis do pliku tymczasowego
            using (var stream = File.Create(tempPath))
                await JsonSerializer.SerializeAsync(stream, project, options);

            // Atomowa zamiana
            File.Delete(finalPath);
            File.Move(tempPath, finalPath);
        }

        public async Task<ProjectModel> LoadAsync(string projectName)
        {
            string fileName = $"{SanitizeName(projectName)}.json";
            string path = Path.Combine(_folder, fileName);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Nie znaleziono projektu {projectName}", path);

            using var stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<ProjectModel>(stream);
        }

        public async Task<IEnumerable<ProjectModel>> LoadAllAsync()
        {
            var files = Directory.EnumerateFiles(_folder, "*.json", SearchOption.TopDirectoryOnly);
            var list = new List<ProjectModel>();

            foreach (var file in files)
            {
                using var stream = File.OpenRead(file);
                var project = await JsonSerializer.DeserializeAsync<ProjectModel>(stream);
                if (project != null)
                    list.Add(project);
            }

            return list;
        }

        public Task DeleteAsync(string projectName)
        {
            string fileName = $"{SanitizeName(projectName)}.json";
            string path = Path.Combine(_folder, fileName);

            if (File.Exists(path))
                File.Delete(path);

            return Task.CompletedTask;
        }

        private static string SanitizeName(string name)
        {
            var invalids = Path.GetInvalidFileNameChars();
            var clean = string.Concat(name.Select(ch => invalids.Contains(ch) ? '_' : ch));
            return clean.Trim();
        }
    }
}