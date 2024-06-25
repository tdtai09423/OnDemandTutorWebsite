using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;
        public TutorController(OnDemandTutorContext context)
        {
            _context = context;
        } 

        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAllApprovedTutors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Tutor> query = _context.Tutors
                                               .Where(t => t.CertiStatus == CertiStatus.Approved)
                                               .Include(t => t.Major)
                                               .Include(t => t.Curricula)
                                               .Include(t => t.TutorNavigation);
            query = query.OrderBy(t => t.TutorId);
            var totalCount = await query.CountAsync();
            var approvedTutors = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if(approvedTutors == null || approvedTutors.Count == 0)
            {
                return NotFound("Not found approved tutors.");
            }

            var response = new PaginatedResponse<Tutor>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = approvedTutors,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("pending")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAllPendingTutors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Tutor> query = _context.Tutors
                                               .Where(t => t.CertiStatus == CertiStatus.Pending)
                                               .Include(t => t.Major)
                                               .Include(t => t.Curricula)
                                               .Include(t => t.TutorNavigation);
            query = query.OrderBy(t => t.TutorId);
            var totalCount = await query.CountAsync();
            var pendingTutors = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (pendingTutors == null || pendingTutors.Count == 0)
            {
                return NotFound("Not found pending tutors.");
            }

            var response = new PaginatedResponse<Tutor>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = pendingTutors,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("rejected")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAllRejectedTutors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Tutor> query = _context.Tutors
                                   .Where(t => t.CertiStatus == CertiStatus.Rejected)
                                   .Include(t => t.Major)
                                   .Include(t => t.Curricula)
                                   .Include(t => t.TutorNavigation);
            query = query.OrderBy(t => t.TutorId);
            var totalCount = await query.CountAsync();
            var rejectedTutors = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (rejectedTutors == null || rejectedTutors.Count == 0)
            {
                return NotFound("Not found rejected tutors.");
            }

            var response = new PaginatedResponse<Tutor>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = rejectedTutors,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }
        
        [HttpGet("{tutorId}")]
        public ActionResult<IEnumerable<Tutor>> GetTutorById(int tutorId)
        {
            try
            {
                var tutor = FindTutorById(tutorId);
                if (tutor == null)
                {
                    return NotFound("No tutors can be found with this ID. Please try again.");
                }
                else
                {
                    return Ok(tutor);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Tutor? FindTutorById(int tutorId)
        {
            var tutor = _context.Tutors
                                .Include(t => t.Major)
                                .Include(t => t.Curricula)
                                .Include(t => t.TutorNavigation) //include account
                                .FirstOrDefault(t => t.TutorId == tutorId && t.CertiStatus == CertiStatus.Approved 
                                                    && t.TutorNavigation.Status == true);
            return tutor;
        }
    }
}
