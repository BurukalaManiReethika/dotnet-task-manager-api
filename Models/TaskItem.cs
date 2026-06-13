namespace dotnet_task_manager_api.Models
{
    /// <summary>
    /// Represents a task managed by the Task Manager API.
    /// </summary>
    public class TaskItem
    {
        /// <summary>
        /// The unique task identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The task title.
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// The task description.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Indicates whether the task is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// The UTC date and time when the task was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
