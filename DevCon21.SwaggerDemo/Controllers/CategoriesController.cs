using DevCon21.SwaggerDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevCon21.SwaggerDemo.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CategoriesController : ControllerBase
    {
        private readonly TodoListContext _context;

        public CategoriesController(TodoListContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(long id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return category;
        }

        // GET: api/Categories/5/workitems
        [HttpGet("{id}/workitems")]
        public async Task<ActionResult<IEnumerable<WorkItem>>> GetCategoryWorkItems(long id)
        {
            var category = await _context.Categories
                .Include(c => c.WorkItems)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return Ok(category.WorkItems);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(long id, Category category)
        {
            if (id != category.Id)
                return BadRequest();

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CategoryExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(long id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
