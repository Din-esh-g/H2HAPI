using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewProjectAPI.Data;
using NewProjectAPI.Models;

namespace NewProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly NewProjectAPIContext _context;

        public EventsController(NewProjectAPIContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvent()
        {
            return await _context.Event.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Event.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

    // POST: api/Events
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    [HttpPost("postevent")]
   // public async Task<ActionResult<Event>> PostEvent(Event @event)
     //   {

            
    public async Task<IActionResult> PostEvent(Event @event)
    {
      _context.Event.Add(@event);
            await _context.SaveChangesAsync();
              return StatusCode(201);

           // return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Event>> DeleteEvent(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();

            return @event;
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }
    }
}
