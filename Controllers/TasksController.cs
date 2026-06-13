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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
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
