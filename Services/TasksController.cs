using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetTasks(string projectId)
        {
            var tasks = await _taskService.GetTasksAsync(projectId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTasks), new { projectId = task.ProjectId }, task);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskItem task)
        {
            await _taskService.UpdateTaskAsync(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
