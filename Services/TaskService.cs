using dotnet_task_manager_api.Data;
using dotnet_task_manager_api.DTOs;
using dotnet_task_manager_api.Exceptions;
using dotnet_task_manager_api.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_task_manager_api.Services;

public sealed class TaskService : ITaskService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TaskService> _logger;

    public TaskService(AppDbContext context, ILogger<TaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PaginatedResponse<TaskDto>> GetTasksAsync(TaskQueryParameters query, CancellationToken cancellationToken)
    {
        var tasks = _context.Tasks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = $"%{query.Search.Trim()}%";
            tasks = tasks.Where(task => EF.Functions.Like(task.Title, term) || EF.Functions.Like(task.Description, term));
        }

        var totalCount = await tasks.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
        var skip = (query.PageNumber - 1) * query.PageSize;

        var items = await tasks
            .OrderBy(task => task.Id)
            .Skip(skip)
            .Take(query.PageSize)
            .Select(task => ToDto(task))
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} tasks from page {PageNumber} with search term {SearchTerm}", items.Count, query.PageNumber, query.Search);

        return new PaginatedResponse<TaskDto>
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = items
        };
    }

    public async Task<TaskDto> GetTaskAsync(int id, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(task => task.Id == id, cancellationToken)
            ?? throw new NotFoundException($"Task with id '{id}' was not found.");

        return ToDto(task);
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto request, CancellationToken cancellationToken)
    {
        var task = new TaskItem
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            IsCompleted = request.IsCompleted
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Created task {TaskId}", task.Id);

        return ToDto(task);
    }

    public async Task UpdateTaskAsync(int id, UpdateTaskDto request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FindAsync([id], cancellationToken)
            ?? throw new NotFoundException($"Task with id '{id}' was not found.");

        task.Title = request.Title.Trim();
        task.Description = request.Description.Trim();
        task.IsCompleted = request.IsCompleted;

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Updated task {TaskId}", id);
    }

    public async Task DeleteTaskAsync(int id, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FindAsync([id], cancellationToken)
            ?? throw new NotFoundException($"Task with id '{id}' was not found.");

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Deleted task {TaskId}", id);
    }

    private static TaskDto ToDto(TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        IsCompleted = task.IsCompleted
    };
}
