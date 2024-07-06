using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;
using System.Drawing.Printing;

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

        [HttpGet("get-all-learners")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllLearners([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Learner> query = _context.Learners.OrderBy(l => l.LearnerId);
            var totalCount = await query.CountAsync();
            var learners = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (learners == null || learners.Count == 0)
            {
                return NotFound("Not found learners");
            }

            var response = new PaginatedResponse<Learner>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = learners,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }
    }
}
