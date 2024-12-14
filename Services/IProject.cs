using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjectsAsync(string userId);
        Task<Project> GetProjectAsync(string projectId);
        Task CreateProjectAsync(Project project);
        Task DeleteProjectAsync(string projectId);
    }
}