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
        private readonly ITaskService _taskService;

        public ProjectsController(IProjectService projectService, ITaskService taskService)
        {
            _projectService = projectService;
            _taskService = taskService;
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
            // Verificar se o projeto existe
            var project = await _projectService.GetProjectAsync(id);
            if (project == null)
            {
                return NotFound("Projeto não encontrado.");
            }

            // Verificar se há tarefas pendentes associadas ao projeto
            var tasks = await _taskService.GetTasksAsync(id);
            if (tasks.Exists(t => t.Status != EnumStatus.DONE))
            {
                return BadRequest("O projeto não pode ser removido, pois há tarefas pendentes ou em andamento. Conclua ou remova as tarefas antes de excluir o projeto.");
            }

            // Remover o projeto
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}
