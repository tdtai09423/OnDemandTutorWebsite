using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public SectionController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("tutor-min-price/{tutorId}")]
        public async Task<ActionResult<decimal>> GetMinPrice(int tutorId)
        {
            try
            {
                var minPrice = await _context.Sections.Where(s => s.Curriculum.TutorId == tutorId).MinAsync(s => (decimal?)s.Price) ?? 0;
                return Ok(minPrice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tutor-max-price/{tutorId}")]
        public async Task<ActionResult<decimal>> GetMaxPrice(int tutorId)
        {
            try
            {
                var maxPrice = await _context.Sections.Where(s => s.Curriculum.TutorId == tutorId).MaxAsync(s => (decimal?)s.Price) ?? 0;
                return Ok(maxPrice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
