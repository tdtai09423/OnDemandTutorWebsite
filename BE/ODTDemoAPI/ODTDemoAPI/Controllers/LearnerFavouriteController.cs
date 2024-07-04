using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerFavouriteController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public LearnerFavouriteController(OnDemandTutorContext context)
        {
            _context = context;
        }

        // POST: api/LearnerFavourite
        [HttpPost("add-favorite")]
        [Authorize(Roles = "LEARNER")]
        public async Task<IActionResult> AddToFavourites(int learnerId, int tutorId)
        {
            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerId);
            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);

            if (learner == null || tutor == null)
            {
                return NotFound(new { Message = "Learner or Tutor not found." });
            }

            var fav = await _context.LearnerFavourites.FirstOrDefaultAsync(f => f.TutorId == tutorId && f.LearnerId == learnerId);

            if (fav != null)
            {
                return BadRequest("Existed favorite.");
            }

            var learnerFavourite = new LearnerFavourite
            {
                LearnerId = learnerId,
                TutorId = tutorId,
            };

            _context.LearnerFavourites.Add(learnerFavourite);
            await _context.SaveChangesAsync();

            // Load the related entities to include them in the response
            var favor = await _context.LearnerFavourites
                         .Include(f => f.Learner)
                         .Include(f => f.Tutor)
                         .FirstOrDefaultAsync(f => f.LearnerId == learnerFavourite.LearnerId && f.TutorId == learnerFavourite.TutorId);

            return Ok( new { learnerId = favor!.LearnerId, FavourInfo = favor! });
        }

        // GET: api/LearnerFavourite
        [HttpGet("get-all-favorite")]
        public async Task<ActionResult<IEnumerable<LearnerFavourite>>> GetFavourites()
        {
            return await _context.LearnerFavourites.ToListAsync();
        }

        // GET: api/LearnerFavourite/{learnerId}
        [HttpGet("{learnerId}")]
        public async Task<ActionResult<IEnumerable<LearnerFavourite>>> GetFavouritesByLearnerId(int learnerId)
        {
            var favourites = await _context.LearnerFavourites
                .Where(lf => lf.LearnerId == learnerId)
                .ToListAsync();

            return favourites;
        }

        // DELETE: api/LearnerFavourite/{learnerId}/{tutorId}
        [HttpDelete("{learnerId}/{tutorId}")]
        public async Task<IActionResult> RemoveFromFavourites(int learnerId, int tutorId)
        {
            var favourite = await _context.LearnerFavourites
                .FirstOrDefaultAsync(lf => lf.LearnerId == learnerId && lf.TutorId == tutorId);

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
