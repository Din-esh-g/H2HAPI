using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewProjectAPI.Data;
using NewProjectAPI.Models;

namespace NewProjectAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class MyJobsController : ControllerBase
    {
        private readonly NewProjectAPIContext _context;

        public MyJobsController(NewProjectAPIContext context)
        {
            _context = context;
        }

        // GET: api/MyJobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyJob>>> GetMyJob()
        {
            return await _context.MyJob.ToListAsync();
        }

        // GET: api/MyJobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MyJob>> GetMyJob(int id)
        {
            var myJob = await _context.MyJob.FindAsync(id);

            if (myJob == null)
            {
                return NotFound();
            }

            return myJob;
        }

        // PUT: api/MyJobs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMyJob(int id, MyJob myJob)
        {
            if (id != myJob.Id)
            {
                return BadRequest();
            }

            _context.Entry(myJob).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyJobExists(id))
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

    [HttpPost("postmyjob")]
    public async Task<IActionResult> PostmyJob(MyJob myjob)
    {
      
      await _context.MyJob.AddAsync(myjob);
      await _context.SaveChangesAsync();
      
      return StatusCode(201);
    }

    // DELETE: api/MyJobs/5
    [HttpDelete("{id}")]
        public async Task<ActionResult<MyJob>> DeleteMyJob(int id)
        {
            var myJob = await _context.MyJob.FindAsync(id);
            if (myJob == null)
            {
                return NotFound();
            }

            _context.MyJob.Remove(myJob);
            await _context.SaveChangesAsync();

            return myJob;
        }

        private bool MyJobExists(int id)
        {
            return _context.MyJob.Any(e => e.Id == id);
        }
    }
}
