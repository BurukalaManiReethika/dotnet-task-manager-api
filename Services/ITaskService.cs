using dotnet_task_manager_api.DTOs;

namespace dotnet_task_manager_api.Services;

public interface ITaskService
{
    Task<PaginatedResponse<TaskDto>> GetTasksAsync(TaskQueryParameters query, CancellationToken cancellationToken);
    Task<TaskDto> GetTaskAsync(int id, CancellationToken cancellationToken);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto request, CancellationToken cancellationToken);
    Task UpdateTaskAsync(int id, UpdateTaskDto request, CancellationToken cancellationToken);
    Task DeleteTaskAsync(int id, CancellationToken cancellationToken);
}
