using DigitalHub_Task.Data;
using DigitalHub_Task.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalHub_Task.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;

        public TasksController(TaskContext context)
        {
            _context = context;
        }

        // POST: /tasks
        [HttpPost]
        public async Task<ActionResult<DigitalHub_Task.Models.Entity.Task>> CreateTask(DigitalHub_Task.Models.Entity.Dto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map DTO to Task entity
            var task = new DigitalHub_Task.Models.Entity.Task
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Status = Enum.TryParse<DigitalHub_Task.Models.Entity.TaskStatus>(dto.Status, true, out var status) ? status : DigitalHub_Task.Models.Entity.TaskStatus.NotStarted, // Default status if parsing fails
                DueDate = dto.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            // Validate DueDate
            if (task.DueDate.HasValue && task.DueDate.Value < DateTime.UtcNow)
            {
                return BadRequest("DueDate cannot be set in the past.");
            }


            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // GET: /tasks

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DigitalHub_Task.Models.Entity.Task>>> GetTasks(
    [FromQuery] bool? isCompleted,
    [FromQuery] DateTime? dueDate,
    [FromQuery] string? title,
    [FromQuery] string sortBy = "DueDate",
    [FromQuery] bool descending = false,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var query = _context.Tasks.AsQueryable();

            // Filtering Bonus Requirement #1
            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.Status == (isCompleted.Value ? DigitalHub_Task.Models.Entity.TaskStatus.Completed : DigitalHub_Task.Models.Entity.TaskStatus.NotStarted));
            }

            if (dueDate.HasValue)
            {
                query = query.Where(t => t.DueDate == dueDate.Value);
            }

            if (!string.IsNullOrEmpty(title))
            {
              
                query = query.Where(t => EF.Functions.Like(t.Title.ToLower(), $"%{title.ToLower()}%"));
            }

            // Sorting Bonus Requirement #2
            if (sortBy.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase))
            {
                query = descending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt);
            }
            else if (sortBy.Equals("DueDate", StringComparison.OrdinalIgnoreCase))
            {
                query = descending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate);
            }

            // Pagination Bonus Requirement #3
            var totalItems = await query.CountAsync();
            var tasks = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Tasks = tasks
            };

            return Ok(result);
        }




        // GET: /tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DigitalHub_Task.Models.Entity.Task>> GetTaskById(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        public class TaskUpdateDto
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public DateTime? DueDate { get; set; }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] Dto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            // Find the existing task
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            // Validate DueDate
            if (updateDto.DueDate.HasValue && updateDto.DueDate.Value < DateTime.UtcNow)
            {
                return BadRequest("DueDate cannot be set in the past.");
            }

       
            task.Title = updateDto.Title ?? task.Title;  // Only update if new value is provided
            task.Description = updateDto.Description ?? task.Description;
            task.Status = Enum.TryParse<DigitalHub_Task.Models.Entity.TaskStatus>(updateDto.Status, true, out var status) ? status : DigitalHub_Task.Models.Entity.TaskStatus.NotStarted;
            task.DueDate = updateDto.DueDate ?? task.DueDate;


            _context.Entry(task).State = EntityState.Modified;


            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: /tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
