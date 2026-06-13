using dotnet_task_manager_api.DTOs;
using dotnet_task_manager_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_task_manager_api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    /// <summary>
    /// Gets a paginated list of tasks, optionally filtered by a search term.
    /// </summary>
    /// <param name="query">Pagination and search parameters. Page size is limited to 100.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A paginated list of tasks.</returns>
    /// <response code="200">Returns the requested page of tasks.</response>
    /// <response code="400">Returned when query parameters fail validation.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResponse<TaskDto>>> GetTasks([FromQuery] TaskQueryParameters query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Task list requested with page {PageNumber}, size {PageSize}, search {Search}", query.PageNumber, query.PageSize, query.Search);
        return Ok(await _taskService.GetTasksAsync(query, cancellationToken));
    }

    /// <summary>
    /// Gets a task by id.
    /// </summary>
    /// <param name="id">The task id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The requested task.</returns>
    /// <response code="200">Returns the task.</response>
    /// <response code="404">Returned when the task does not exist.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetTask(int id, CancellationToken cancellationToken)
    {
        return Ok(await _taskService.GetTaskAsync(id, cancellationToken));
    }

    /// <summary>
    /// Creates a task.
    /// </summary>
    /// <param name="request">The task creation payload.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The created task.</returns>
    /// <response code="201">Returns the created task.</response>
    /// <response code="400">Returned when the request body fails validation.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto request, CancellationToken cancellationToken)
    {
        var task = await _taskService.CreateTaskAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    /// <summary>
    /// Updates a task.
    /// </summary>
    /// <param name="id">The task id.</param>
    /// <param name="request">The replacement task values.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="204">The task was updated.</response>
    /// <response code="400">Returned when the request body fails validation.</response>
    /// <response code="404">Returned when the task does not exist.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateTaskAsync(id, request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes a task.
    /// </summary>
    /// <param name="id">The task id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="204">The task was deleted.</response>
    /// <response code="404">Returned when the task does not exist.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int id, CancellationToken cancellationToken)
    {
        await _taskService.DeleteTaskAsync(id, cancellationToken);
        return NoContent();
    }
}
