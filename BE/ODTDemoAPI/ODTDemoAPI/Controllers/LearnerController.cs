using Microsoft.AspNetCore.Http;
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
                var learner = findLearnerById(learnerId);
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

        [HttpPost]
        public ActionResult<IEnumerable<Learner>> CreateNewLearner(Learner learner)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (findLearnerByEmail(learner.LearnerEmail) == null)
                {
                    _context.Learners.Add(learner);
                    _context.SaveChanges();
                    return CreatedAtAction(nameof(GetLearnerById), new { learnerId = learner.LearnerId }, learner);
                }
                else
                {
                    return BadRequest("Dupplicated email. Please try again.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private Learner? findLearnerById(int learnerId)
        {
            var learner = _context.Learners
                                .Include(l => l.LearnerNavigation) //include account
                                .FirstOrDefault(l => l.LearnerId == learnerId 
                                                    && l.LearnerNavigation.AccountStatus == true);
            return learner;
        }

        private Learner? findLearnerByEmail(string email)
        {
            var learner = _context.Learners
                                .Include(l => l.LearnerNavigation) //include account
                                .FirstOrDefault(l => l.LearnerEmail == email
                                                    && l.LearnerNavigation.AccountStatus == true);
            return learner;
        }
    }
}
