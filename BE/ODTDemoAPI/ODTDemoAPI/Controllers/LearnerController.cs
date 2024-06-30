using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;
        public LearnerController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("{learnerId}")]
        public ActionResult<IEnumerable<Learner>> GetLearnerById(int learnerId)
        {
            try
            {
                var learner = FindLearnerById(learnerId);
                if (learner == null)
                {
                    return NotFound("No learners can be found with this ID. Please try again.");
                }
                else
                {
                    return Ok(learner);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private Learner? FindLearnerById(int learnerId)
        {
            var learner = _context.Learners
                                .Include(l => l.LearnerNavigation) //include account
                                .FirstOrDefault(l => l.LearnerId == learnerId 
                                                    && l.LearnerNavigation.Status == true);
            return learner;
        }

        //private Learner? FindLearnerByEmail(string email)
        //{
        //    var learner = _context.Learners
        //                        .Include(l => l.LearnerNavigation) //include account
        //                        .FirstOrDefault(l => l.LearnerEmail == email
        //                                            && l.LearnerNavigation.Status == true);
        //    return learner;
        //}
    }
}
