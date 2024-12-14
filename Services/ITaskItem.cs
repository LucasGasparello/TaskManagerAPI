using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetTasksAsync(string projectId);
        Task<TaskItem> GetTaskAsync(string taskId);
        Task CreateTaskAsync(TaskItem task);
        Task UpdateTaskAsync(TaskItem task);
        Task DeleteTaskAsync(string taskId);
    }
}