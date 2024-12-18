using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMongoCollection<Project> _projects;

        public ProjectService(IMongoCollection<Project> projects)
        {
            _projects = projects;
        }


        public async Task<List<Project>> GetProjectsAsync(string userId)
        {
            return await _projects.Find(p => p.UserId == userId).ToListAsync();
        }

        public async Task<Project> GetProjectAsync(string projectId)
        {
            return await _projects.Find(p => p.Id == projectId).FirstOrDefaultAsync();
        }

        public async Task CreateProjectAsync(Project project)
        {
            await _projects.InsertOneAsync(project);
        }

        public async Task DeleteProjectAsync(string projectId)
        {
            await _projects.DeleteOneAsync(p => p.Id == projectId);
        }
    }
}