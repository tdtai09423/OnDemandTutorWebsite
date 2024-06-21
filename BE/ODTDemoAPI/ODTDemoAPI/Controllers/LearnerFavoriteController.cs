using Microsoft.AspNetCore.Mvc;
using ODTDemoAPI.Services;
using ODTDemoAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerFavouritesController : ControllerBase
    {
        private readonly ILearnerFavouriteService _learnerFavouriteService;

        public LearnerFavouritesController(ILearnerFavouriteService learnerFavouriteService)
        {
            _learnerFavouriteService = learnerFavouriteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LearnerFavourite>>> GetLearnerFavourites()
        {
            return await _learnerFavouriteService.GetAllLearnerFavouritesAsync();
        }

        [HttpGet("learner/{learnerId}")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetLearnerFavouriteTutors(int learnerId)
        {
            var tutors = await _learnerFavouriteService.GetLearnerFavouriteTutorsAsync(learnerId);
            if (tutors == null || tutors.Count == 0)
            {
                return NotFound();
            }

            return tutors;
        }

        [HttpPost]
        public async Task<ActionResult<LearnerFavourite>> AddLearnerFavourite(LearnerFavourite learnerFavourite)
        {
            await _learnerFavouriteService.AddLearnerFavouriteAsync(learnerFavourite);
            return CreatedAtAction("GetLearnerFavouriteTutors", new { learnerId = learnerFavourite.LearnerId }, learnerFavourite);
        }

        [HttpDelete("{learnerId}/{tutorId}")]
        public async Task<IActionResult> DeleteLearnerFavourite(int learnerId, int tutorId)
        {
            await _learnerFavouriteService.RemoveLearnerFavouriteAsync(learnerId, tutorId);
            return NoContent();
        }
    }
}
