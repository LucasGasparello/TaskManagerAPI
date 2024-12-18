using Microsoft.VisualBasic;
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
        Task UpdateTaskAsync(string taskId, TaskItem updatedTask, string updatedBy);
        Task DeleteTaskAsync(string taskId);
        Task AddCommentAsync(string taskId, Comment comment);
        Task<PerformanceReport> GetAverageCompletedTasksReportAsync(DateTime start, DateTime end);

    }
}
