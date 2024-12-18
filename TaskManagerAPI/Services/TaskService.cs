using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMongoCollection<TaskItem> _tasks;

        public TaskService(IMongoCollection<TaskItem> tasks)
        {
            _tasks = tasks;
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
            task.History = new List<ChangeHistory>
            {
                new ChangeHistory
                {
                    Action = "Tarefa criada",
                    Date = DateTime.UtcNow,
                    ChangedBy = task.CreatedBy,
                    Changes = new Dictionary<string, string>
                    {
                        { "Title", task.Title },
                        { "Description", task.Description },
                        { "Priority", task.Priority.ToString() },
                        { "Status", task.Status.ToString() }
                    }
                }
            };
            await _tasks.InsertOneAsync(task);
        }

        public async Task UpdateTaskAsync(string taskId, TaskItem updatedTask, string updatedBy)
        {
            var existingTask = await GetTaskAsync(taskId);
            if (existingTask == null) throw new Exception("Task not found.");

            // Registrar histórico de alterações
            var changes = new Dictionary<string, string>();

            if (existingTask.Title != updatedTask.Title) changes.Add("Title", $"{existingTask.Title} -> {updatedTask.Title}");
            if (existingTask.Description != updatedTask.Description) changes.Add("Description", $"{existingTask.Description} -> {updatedTask.Description}");
            if (existingTask.Status != updatedTask.Status) changes.Add("Status", $"{existingTask.Status} -> {updatedTask.Status}");

            if (existingTask.Status == EnumStatus.DONE)
            {
                existingTask.CompletedAt = DateTime.UtcNow;
            }


            if (changes.Any())
            {
                existingTask.History.Add(new ChangeHistory
                {
                    Action = "Tarefa atualizada",
                    Date = DateTime.UtcNow,
                    ChangedBy = updatedBy,
                    Changes = changes
                });
            }

            // Atualizar os dados
            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.Status = updatedTask.Status;

            await _tasks.ReplaceOneAsync(t => t.Id == taskId, existingTask);
        }

        public async Task DeleteTaskAsync(string taskId)
        {
            await _tasks.DeleteOneAsync(t => t.Id == taskId);
        }

        public async Task AddCommentAsync(string taskId, Comment comment)
        {
            var task = await GetTaskAsync(taskId);
            if (task == null) throw new Exception("Tarefa não encontrada");

            task.Comments.Add(comment);

            task.History.Add(new ChangeHistory
            {
                Action = "Commentário Adicionado",
                Date = comment.CreatedAt,
                ChangedBy = comment.CreatedBy,
                Changes = new Dictionary<string, string>
                {
                    { "Comment", comment.Content }
                }
            });

            await _tasks.ReplaceOneAsync(t => t.Id == taskId, task);
        }

        public async Task<PerformanceReport> GetAverageCompletedTasksReportAsync(DateTime start, DateTime end)
        {

            // Filtrando as tarefas concluídas nos últimos 30 dias
            var completedTasks = await _tasks
                .Find(t => t.Status == EnumStatus.DONE && t.CompletedAt >= start && t.CompletedAt <= end)
                .ToListAsync();

            // Agrupando as tarefas por usuário e contando o número de tarefas concluídas
            var userTaskCount = completedTasks
                .GroupBy(t => t.CreatedBy)
                .Select(group => new
                {
                    UserId = group.Key,
                    TaskCount = group.Count()
                })
                .ToList();

            // Calculando a média de tarefas concluídas por usuário
            var totalUsers = userTaskCount.Count();
            var totalTasks = userTaskCount.Sum(u => u.TaskCount);
            var averageTasksPerUser = totalUsers == 0 ? 0 : (double)totalTasks / totalUsers;

            return new PerformanceReport
            {
                TotalUsers = totalUsers,
                TotalCompletedTasks = totalTasks,
                AverageTasksPerUser = averageTasksPerUser
            };
        }


    }
}
