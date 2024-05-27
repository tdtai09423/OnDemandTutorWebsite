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
                var tutor = findTutorById(tutorId);
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

        //chua test vi luoi input
        [HttpPost]
        public ActionResult<IEnumerable<Tutor>> CreateNewTutor(Tutor newTutor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (findTutorByEmail(newTutor.TutorEmail) == null)
                {
                    newTutor.CertiStatus = CertiStatus.Pending;
                    _context.Tutors.Add(newTutor);
                    _context.SaveChanges();
                    return CreatedAtAction(nameof(GetTutorById), new { tutorId = newTutor.TutorId }, newTutor);
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

        //[HttpPost("uploadFile")]
        //public async Task<IActionResult> PostWithImage([FromForm] TutorImage tutor)
        //{
        //    try
        //    {
        //        if(findTutorById(tutor.TutorId) == null)
        //        {
        //            return NotFound("No tutors can be found with this ID. Please try again.");
        //        }
        //        else
        //        {
        //            if(tutor.Image.Length > 0)
        //            {
        //                var t = new Tutor {--khai báo mọi thứ trừ picture--};
        //                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", tutor.Image.FileName);
        //                using(var stream = System.IO.File.Create(path))
        //                {
        //                    await tutor.Image.CopyToAsync(stream);
        //                }
        //                t.Picture = "images" + tutor.Image.FIleName;
        //            }
        //            else
        //            {
        //                t.Picture = "";
        //            }
        //            _context.Tutors.Add(t);
        //            _context.SaveChanges();
        //            return Ok(t);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //chua test vi li do nhu tren
        [HttpPut("{tutorId}")]
        public ActionResult<IEnumerable<Tutor>> UpdateTutor(int tutorId, Tutor newTutor)
        {
            try
            {
                if(tutorId != newTutor.TutorId)
                {
                    return BadRequest("Tutor ID mismatch. Please try again");
                }
                var tutor = findTutorById(tutorId);
                if(tutor == null)
                {
                    return NotFound("No tutors can be found with this ID. Please try again.");
                }
                else
                {
                    tutor = newTutor;
                    if(tutor.TutorNavigation != null)
                    {
                        tutor.TutorNavigation.Email = newTutor.TutorNavigation.Email;
                        tutor.TutorNavigation.Password = newTutor.TutorNavigation.Password;
                    }
                    _context.Entry(tutor).State = EntityState.Modified;
                    _context.SaveChanges();
                    return NoContent();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if(findTutorById(tutorId) == null)
                {
                    return NotFound("No tutors can be found with this ID. Please try again.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Tutor? findTutorById(int tutorId)
        {
            var tutor = _context.Tutors
                                .Include(t => t.Major)
                                .Include(t => t.Curricula)
                                .Include(t => t.TutorNavigation) //include account
                                .FirstOrDefault(t => t.TutorId == tutorId && t.CertiStatus == CertiStatus.Approved 
                                                    && t.TutorNavigation.Status == true);
            return tutor;
        }

        private Tutor? findTutorByEmail(string email)
        {
            var tutor = _context.Tutors
                                .Include(t => t.Major)
                                .Include(t => t.Curricula)
                                .Include(t => t.TutorNavigation) //include account
                                .FirstOrDefault(t => t.TutorEmail == email 
                                                    && (t.CertiStatus == CertiStatus.Approved || t.CertiStatus == CertiStatus.Pending) 
                                                    && t.TutorNavigation.Status == true);
            return tutor;
        }
    }
}
