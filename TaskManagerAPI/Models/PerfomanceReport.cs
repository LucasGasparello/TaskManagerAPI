namespace TaskManagerAPI.Models
{
    public class PerformanceReport
    {
        public int TotalUsers { get; set; }
        public int TotalCompletedTasks { get; set; }
        public double AverageTasksPerUser { get; set; }
    }
}
