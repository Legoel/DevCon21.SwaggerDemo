using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevCon21.SwaggerDemo.Models;
using System.Net.Mime;

namespace DevCon21.SwaggerDemo.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class PeopleController : ControllerBase
    {
        private readonly TodoListContext _context;

        public PeopleController(TodoListContext context)
        {
            _context = context;
        }

        // GET: api/People
        /// <summary>
        /// Return all People
        /// </summary>
        /// <returns>All the existing resources</returns>
        /// <response code="200">Ok - all the resources</response>
        /// <response code="401">Unauthorized - identification token is missing, invalid or expired</response>
        [AllowAnonymous]
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.People.ToListAsync();
        }

        // GET: api/People/5
        /// <summary>
        /// Return a Person
        /// </summary>
        /// <param name="id" example="42">The id of the resource</param>
        /// <returns>The resource identified by its id</returns>
        /// <response code="200">Ok - the resources identified</response>
        /// <response code="400">Bad Request - the resource representation is invalid</response>
        /// <response code="401">Unauthorized - identification token is missing, invalid or expired</response>
        /// <response code="404">Not Found - no resource found for this identifier</response>
        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _context.People.SingleOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return NotFound();

            return person;
        }

        // GET: api/People/5/workitems
        /// <summary>
        /// Return the workitems of a Person
        /// </summary>
        /// <param name="id" example="42">The id of the resource</param>
        /// <response code="200">Ok - the resources identified</response>
        /// <response code="400">Bad Request - the resource representation is invalid</response>
        /// <response code="401">Unauthorized - identification token is missing, invalid or expired</response>
        /// <response code="404">Not Found - no resource found for this identifier</response>
        [HttpGet("{id}/workitems")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
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
        /// <summary>
        /// Update a Person
        /// </summary>
        /// <param name="id" example="42">The id of the resource</param>
        /// <param name="person">The new represention of the resource to update</param>
        /// <response code="204">Ok - the resource has been updated</response>
        /// <response code="400">Bad Request - the resource representation is invalid</response>
        /// <response code="401">Unauthorized - identification token is missing, invalid or expired</response>
        /// <response code="404">Not Found - no resource found for this identifier</response>
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
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
        /// <summary>
        /// Create a Person
        /// </summary>
        /// <param name="person">The resource represention to create</param>
        /// <response code="201">Ok - the resource has been updated</response>
        /// <response code="400">Bad Request - the resource representation is invalid</response>
        /// <response code="401">Unauthorized - identification token is missing, invalid or expired</response>
        /// <response code="404">Not Found - no resource found for this identifier</response>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        /// <summary>
        /// Delete a Person
        /// </summary>
        /// <param name="id" example="42">The id of the resource</param>
        /// <response code="204">Ok - the resource has been updated</response>
        /// <response code="400">Bad Request - the resource representation is invalid</response>
        /// <response code="401">Unauthorized - identification token is missing, invalid or expired</response>
        /// <response code="404">Not Found - no resource found for this identifier</response>
        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
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
