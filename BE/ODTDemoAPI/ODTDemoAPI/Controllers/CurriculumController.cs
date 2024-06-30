using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public CurriculumController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-curricula/{tutorId}")]
        public async Task<ActionResult<IEnumerable<Curriculum>>> GetAllCurricula([FromRoute] int tutorId)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);

                if (tutor == null)
                {
                    return NotFound("Not found tutor.");
                }

                var curricula = await _context.Curricula.Where(c => c.TutorId == tutorId).ToListAsync();

                if(curricula == null || curricula.Count == 0)
                {
                    return NotFound("No curriculum is found with this tutor ID.");
                }
                return Ok(curricula);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
