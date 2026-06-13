using System.ComponentModel.DataAnnotations;

namespace dotnet_task_manager_api.DTOs;

public sealed class TaskDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
}

public sealed class CreateTaskDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; init; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; init; } = string.Empty;

    public bool IsCompleted { get; init; }
}

public sealed class UpdateTaskDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; init; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; init; } = string.Empty;

    public bool IsCompleted { get; init; }
}

public sealed class TaskQueryParameters
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 10;

    [StringLength(100)]
    public string? Search { get; init; }
}

public sealed class PaginatedResponse<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public IReadOnlyCollection<T> Items { get; init; } = [];
}
