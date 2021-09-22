using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevCon21.SwaggerDemo.Models;

namespace DevCon21.SwaggerDemo.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly TodoListContext _context;

        public PeopleController(TodoListContext context)
        {
            _context = context;
        }

        // GET: api/People
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.People.ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _context.People.SingleOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return NotFound();

            return person;
        }

        // GET: api/People/5/workitems
        [HttpGet("{id}/workitems")]
        public async Task<ActionResult<IEnumerable<WorkItem>>> GetPersonWorkitems(long id)
        {
            var person = await _context.People
                .Include(p => p.WorkItems)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return NotFound();

            return Ok(person.WorkItems);
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.Id)
                return BadRequest();

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PersonExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/People
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
                return NotFound();

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(long id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
