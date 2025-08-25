using PMS.Models;
using PMS.Services;

namespace PMS.Views.Services
{
    public sealed class JsonProjectRepository : IProjectRepository
    {
        public IEnumerable<string> GetAllProjectNames() => ProjectJsonStorage.GetAllProjectNames();
        public ProjectModel Load(string name) => ProjectJsonStorage.Load(name);
        public void Save(ProjectModel model) => ProjectJsonStorage.Save(model);
        public void Delete(string name) => ProjectJsonStorage.Delete(name);
    }

}
