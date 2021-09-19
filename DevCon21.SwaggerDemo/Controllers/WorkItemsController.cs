using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevCon21.SwaggerDemo.Models;

namespace DevCon21.SwaggerDemo.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class WorkItemsController : ControllerBase
    {
        private readonly TodoListContext _context;

        public WorkItemsController(TodoListContext context)
        {
            _context = context;
        }

        // GET: api/WorkItems
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkItem>>> GetTasks()
        {
            return await _context.WorkItems
                .Include(e => e.Person)
                .Include(e => e.Category)
                .ToListAsync();
        }

        // GET: api/WorkItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkItem>> GetTask(long id)
        {
            var task = await _context.WorkItems.FindAsync(id);

            if (task == null)
                return NotFound();

            return task;
        }

        // PUT: api/WorkItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, WorkItem task)
        {
            if (id != task.Id)
                return BadRequest();

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!WorkItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/WorkItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkItem>> PostTask(WorkItem task)
        {
            _context.WorkItems.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // DELETE: api/WorkItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var task = await _context.WorkItems.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.WorkItems.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkItemExists(long id)
        {
            return _context.WorkItems.Any(e => e.Id == id);
        }
    }
}
