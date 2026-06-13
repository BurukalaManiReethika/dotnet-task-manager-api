namespace dotnet_task_manager_api.Models
{
    public class PaginatedTasksResponse
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<TaskItem> Items { get; set; } = [];
    }
}
