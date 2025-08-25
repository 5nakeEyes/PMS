using PMS.Domain.Models;

namespace PMS.Presentation.Interfaces
{
    public interface IProjectStorage
    {
        Task<IEnumerable<ProjectModel>> LoadAllAsync();
        Task SaveAsync(ProjectModel project);
    }
}