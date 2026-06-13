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
        /// Gets tasks using pagination.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of tasks per page. Defaults to 10.</param>
        /// <returns>A paginated list of tasks with paging metadata.</returns>
        /// <remarks>
        /// Example request:
        ///
        ///     GET /api/tasks?pageNumber=2&amp;pageSize=5
        ///
        /// Example response:
        ///
        ///     {
        ///       "pageNumber": 2,
        ///       "pageSize": 5,
        ///       "totalCount": 23,
        ///       "totalPages": 5,
        ///       "items": [
        ///         {
        ///           "id": 6,
        ///           "title": "Review pull request",
        ///           "description": "Review the task pagination changes",
        ///           "isCompleted": false
        ///         }
        ///       ]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the requested page of tasks.</response>
        /// <response code="400">Returned when pageNumber or pageSize is less than 1.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedTasksResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedTasksResponse>> GetTasks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1)
                return BadRequest("pageNumber must be greater than or equal to 1.");

            if (pageSize < 1)
                return BadRequest("pageSize must be greater than or equal to 1.");

            var totalCount = await _context.Tasks.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var offset = ((long)pageNumber - 1) * pageSize;
            var items = offset > int.MaxValue
                ? []
                : await _context.Tasks
                    .OrderBy(task => task.Id)
                    .Skip((int)offset)
                    .Take(pageSize)
                    .ToListAsync();

            return new PaginatedTasksResponse
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = items
            };
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

            _context.Entry(task).State = EntityState.Modified;

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
