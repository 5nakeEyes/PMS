using PMS.Models;

namespace PMS.Views.Services
{
    public interface IProjectRepository
    {
        IEnumerable<string> GetAllProjectNames();
        ProjectModel Load(string name);
        void Save(ProjectModel model);
        void Delete(string name);
    }

}
