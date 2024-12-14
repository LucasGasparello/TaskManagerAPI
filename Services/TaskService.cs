using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMongoCollection<TaskItem> _tasks;

        public TaskService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _tasks = database.GetCollection<TaskItem>("Tasks");
        }

        public async Task<List<TaskItem>> GetTasksAsync(string projectId)
        {
            return await _tasks.Find(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskItem> GetTaskAsync(string taskId)
        {
            return await _tasks.Find(t => t.Id == taskId).FirstOrDefaultAsync();
        }

        public async Task CreateTaskAsync(TaskItem task)
        {
            await _tasks.InsertOneAsync(task);
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            await _tasks.ReplaceOneAsync(t => t.Id == task.Id, task);
        }

        public async Task DeleteTaskAsync(string taskId)
        {
            await _tasks.DeleteOneAsync(t => t.Id == taskId);
        }
    }
}
