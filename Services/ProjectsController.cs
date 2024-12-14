using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects(string userId)
        {
            var projects = await _projectService.GetProjectsAsync(userId);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            await _projectService.CreateProjectAsync(project);
            return CreatedAtAction(nameof(GetProjects), new { userId = project.UserId }, project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}