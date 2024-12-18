using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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

        // Mock de usuário com role
        private readonly string _mockUserRole = "Manager";  // Simulando que o usuário tem a role "Manager"

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

        [HttpGet("details/{taskId}")]
        public async Task<IActionResult> GetTaskDetails(string taskId)
        {
            var task = await _taskService.GetTaskAsync(taskId);
            if (task == null) return NotFound("Tarefa não encontrada.");
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskDetails), new { taskId = task.Id }, task);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(string taskId, [FromBody] TaskItem task, [FromQuery] string updatedBy)
        {
            await _taskService.UpdateTaskAsync(taskId, task, updatedBy);
            return NoContent();
        }


        [HttpGet("performance")]
        public async Task<IActionResult> GetPerformanceReport(DateTime? startDate = null, DateTime? endDate = null)
        {

            if (_mockUserRole != "Manager")
            {
                return BadRequest("O usuário não tem permissão para acessar esse relatório.");
            }

            startDate ??= DateTime.UtcNow.AddDays(-30);
            endDate ??= DateTime.UtcNow;

            if (startDate > endDate)
            {
                return BadRequest("A data de início não pode ser posterior à data de término.");
            }

            var report = await _taskService.GetAverageCompletedTasksReportAsync(startDate.Value, endDate.Value);
            return Ok(report);
        }

        [HttpPost("{taskId}/comments")]
        public async Task<IActionResult> AddComment(string taskId, [FromBody] Comment comment)
        {
            await _taskService.AddCommentAsync(taskId, comment);
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
