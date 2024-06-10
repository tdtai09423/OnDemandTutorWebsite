using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

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
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAllApprovedTutors()
        {
            var approvedTutors = await _context.Tutors
                                               .Where(t => t.CertiStatus == CertiStatus.Approved)
                                               .Include(t => t.Major)
                                               .Include(t => t.Curricula)
                                               .Include(t => t.TutorNavigation)
                                               .ToListAsync();

            return approvedTutors;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAllPendingTutors()
        {
            var pendingTutors = await _context.Tutors
                                               .Where(t => t.CertiStatus == CertiStatus.Pending)
                                               .Include(t => t.Major)
                                               .Include(t => t.Curricula)
                                               .Include(t => t.TutorNavigation)
                                               .ToListAsync();

            return pendingTutors;
        }

        [HttpGet("rejected")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAllRejectedTutors()
        {
            var approvedTutors = await _context.Tutors
                                               .Where(t => t.CertiStatus == CertiStatus.Rejected)
                                               .Include(t => t.Major)
                                               .Include(t => t.Curricula)
                                               .Include(t => t.TutorNavigation)
                                               .ToListAsync();

            return approvedTutors;
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
