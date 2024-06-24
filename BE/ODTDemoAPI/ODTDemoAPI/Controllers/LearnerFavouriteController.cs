using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LearnerFavouriteController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public LearnerFavouriteController(OnDemandTutorContext context)
        {
            _context = context;
        }

        // Add a tutor to learner's favorites
        [HttpPost("{learnerId}/favorites/{tutorId}")]
        public async Task<IActionResult> AddFavourite(int learnerId, int tutorId)
        {
            var favourite = new LearnerFavourite { LearnerId = learnerId, TutorId = tutorId };
            _context.LearnerFavourites.Add(favourite);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFavouriteById), new { learnerId = learnerId, tutorId = tutorId }, favourite);
        }

        // Get all tutors in learner's favorites
        [HttpGet("{learnerId}/favorites")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAllFavourites(int learnerId)
        {
            var favourites = await _context.LearnerFavourites
                .Where(f => f.LearnerId == learnerId)
                .Include(f => f.Tutor)
                .Select(f => f.Tutor)
                .ToListAsync();

            return Ok(favourites);
        }

        // Get a specific tutor in learner's favorites by ID
        [HttpGet("{learnerId}/favorites/{tutorId}")]
        public async Task<ActionResult<Tutor>> GetFavouriteById(int learnerId, int tutorId)
        {
            var favourite = await _context.LearnerFavourites
                .Where(f => f.LearnerId == learnerId && f.TutorId == tutorId)
                .Include(f => f.Tutor)
                .FirstOrDefaultAsync();

            if (favourite == null)
            {
                return NotFound();
            }

            return Ok(favourite.Tutor);
        }

        // Remove a tutor from learner's favorites
        [HttpDelete("{learnerId}/favorites/{tutorId}")]
        public async Task<IActionResult> DeleteFavourite(int learnerId, int tutorId)
        {
            var favourite = await _context.LearnerFavourites
                .Where(f => f.LearnerId == learnerId && f.TutorId == tutorId)
                .FirstOrDefaultAsync();

            if (favourite == null)
            {
                return NotFound();
            }

            _context.LearnerFavourites.Remove(favourite);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
