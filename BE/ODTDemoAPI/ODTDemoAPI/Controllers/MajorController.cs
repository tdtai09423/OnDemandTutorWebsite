using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public MajorController(OnDemandTutorContext context)
        {
            _context = context;
        }

        // GET: api/Major
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Major>>> GetAllMajors()
        {
            var majors = await _context.Majors.ToListAsync();
            return Ok(majors);
        }

        // GET: api/Major/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Major>> GetMajor(string id)
        {
            try
            {
                var major = await _context.Majors.FindAsync(id);

                if (major == null)
                {
                    return NotFound("No majors can be found with this ID. Please try again.");
                }

                return Ok(major);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Major/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMajor(string id, Major major)
        {
            try
            {
                if (id != major.MajorId)
                {
                    return BadRequest();
                }

                _context.Entry(major).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MajorExists(id))
                    {
                        return NotFound("No majors can be found with this ID. Please try again.");
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Major
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Major>> PostMajor(Major major)
        {
            try
            {
                _context.Majors.Add(major);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (MajorExists(major.MajorId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetMajor", new { id = major.MajorId }, major);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool MajorExists(string id)
        {
            return _context.Majors.Any(e => e.MajorId == id);
        }
    }
}
