using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet_task_manager_api.Data;
using dotnet_task_manager_api.Models;

namespace dotnet_task_manager_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all tasks, optionally sorted by creation date.
        /// </summary>
        /// <param name="sort">Use <c>newest</c> to return newest tasks first or <c>oldest</c> to return oldest tasks first.</param>
        /// <returns>The list of tasks.</returns>
        /// <response code="200">Returns all tasks.</response>
        /// <response code="400">Returned when the sort value is not <c>newest</c> or <c>oldest</c>.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks([FromQuery] string? sort)
        {
            var query = _context.Tasks.AsQueryable();

            switch (sort?.Trim().ToLowerInvariant())
            {
                case null:
                case "":
                    break;
                case "newest":
                    query = query.OrderByDescending(task => task.CreatedAt);
                    break;
                case "oldest":
                    query = query.OrderBy(task => task.CreatedAt);
                    break;
                default:
                    return BadRequest("Sort must be either 'newest' or 'oldest'.");
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Searches tasks by keyword in the title or description.
        /// </summary>
        /// <param name="keyword">The case-insensitive keyword to search for.</param>
        /// <returns>Tasks whose title or description contains the keyword.</returns>
        /// <response code="200">Returns the matching tasks.</response>
        /// <response code="400">Returned when the keyword is missing or blank.</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<TaskItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TaskItem>>> SearchTasks([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("A non-empty keyword query parameter is required.");

            var normalizedKeyword = keyword.Trim().ToLower();

            return await _context.Tasks
                .Where(task => task.Title.ToLower().Contains(normalizedKeyword)
                    || task.Description.ToLower().Contains(normalizedKeyword))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            if (id != task.Id)
                return BadRequest();

            var existingTask = await _context.Tasks.FindAsync(id);

            if (existingTask == null)
                return NotFound();

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.IsCompleted = task.IsCompleted;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
