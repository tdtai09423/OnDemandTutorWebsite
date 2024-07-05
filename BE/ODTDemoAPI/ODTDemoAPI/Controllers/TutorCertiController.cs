using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorCertiController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;
        public TutorCertiController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCertisById([FromRoute] int id)
        {
            try
            {
                var tutorCerti = await _context.TutorCertis.Where(t => t.TutorId == id).ToListAsync();
                if (tutorCerti != null)
                {
                    return Ok(tutorCerti);
                }
                else
                {
                    return NotFound("No certificates can be found with this ID. Please try again.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-new-certi")]
        [Authorize(Roles = "TUTOR")]
        public IActionResult AddNewCerti(TutorCerti tutorCerti)
        {
            try
            {
                var certi = _context.TutorCertis.SingleOrDefault(t => t.TutorId == tutorCerti.TutorId);
                if(certi == null)
                {
                    _context.TutorCertis.Add(tutorCerti);
                    var tutor = _context.Tutors.SingleOrDefault(t => t.TutorId == tutorCerti.TutorId);
                    if(tutor == null)
                    {
                        return NotFound("Not found tutor");
                    }
                    else
                    {
                        tutor.CertiStatus = CertiStatus.Pending;
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest("Dupplicated certificates");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("approve/{tutorId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ApproveTutor([FromRoute]int tutorId)
        {
            try
            {
                var tutor = await _context.Tutors.FindAsync(tutorId);
                if(tutor == null)
                {
                    return NotFound("Not found tutor.");
                }
                else
                {
                    tutor.CertiStatus = CertiStatus.Approved;
                    _context.Tutors.Update(tutor);
                    _context.SaveChanges();
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("reject/{tutorId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RejectTutor([FromRoute] int tutorId)
        {
            try
            {
                var tutor = await _context.Tutors.FindAsync(tutorId);
                if (tutor == null)
                {
                    return NotFound("Not found tutor.");
                }
                else
                {
                    tutor.CertiStatus = CertiStatus.Rejected;
                    _context.Tutors.Update(tutor);
                    _context.SaveChanges();
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("reset/{tutorId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ResetTutorCerti([FromRoute] int tutorId)
        {
            try
            {
                var tutor = await _context.Tutors.FindAsync(tutorId);
                if (tutor == null)
                {
                    return NotFound("Not found tutor.");
                }
                else
                {
                    tutor.CertiStatus = CertiStatus.Pending;
                    _context.Tutors.Update(tutor);
                    _context.SaveChanges();
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
