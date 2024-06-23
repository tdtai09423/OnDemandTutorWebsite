using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public MajorController(OnDemandTutorContext context)
        {
            _context = context;
        }

        // GET: api/Major
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Major>>> GetAllMajors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Major> query = _context.Majors.OrderBy(m => m.MajorId);
            var totalCount = await query.CountAsync();
            var majors = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (majors == null || majors.Count == 0)
            {
                return NotFound("No major was found.");
            }
            var response = new PaginatedResponse<Major>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = majors,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        // GET: api/Major/5
        [HttpGet("{majorId}")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetTutorByMajor([FromRoute] string majorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Tutor> query = _context.Tutors.Where(t => t.MajorId == majorId).OrderBy(t => t.TutorId);
            var totalCount = await query.CountAsync();
            var tutors = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (tutors == null || tutors.Count == 0)
            {
                return NotFound("No major was found.");
            }
            var response = new PaginatedResponse<Tutor>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = tutors,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("get-major/{majorId}")]
        public async Task<ActionResult<IEnumerable<Major>>> GetMajorById([FromRoute] string majorId)
        {
            var major = await _context.Majors.FirstOrDefaultAsync(m => m.MajorId == majorId);
            if(major == null)
            {
                return NotFound("Not found major.");
            }
            return Ok(new {Major = major});
        }

        // PUT: api/Major/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add-new-major")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PutMajor([FromForm] Major major)
        {
            try
            {
                var findMajor1 = await _context.Majors.FindAsync(major.MajorId);
                var findMajor2 = await _context.Majors.FindAsync(major.MajorName);
                if(findMajor1 != null)
                {
                    return BadRequest(new {message = "Existed major Id."});
                }

                if(findMajor2 != null)
                {
                    return BadRequest(new { message = "Existed major name." });
                }

                _context.Majors.Add(major);
                await _context.SaveChangesAsync();

                return Ok(new {Major = major});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Major
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update-major")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Major>> PostMajor([FromForm] string majorId, [FromForm] string newMajorId, [FromForm] string newMajorName)
        {
            try
            {
                var findMajor = await _context.Majors.FirstOrDefaultAsync(m => m.MajorId == majorId);
                if(findMajor == null)
                {
                    return NotFound("Not found major");
                }

                if (!string.IsNullOrEmpty(newMajorName))
                {
                    findMajor.MajorId = newMajorId;
                }

                if (!string.IsNullOrEmpty(newMajorName))
                {
                    findMajor.MajorName = newMajorName;
                }

                _context.Majors.Update(findMajor);
                await _context.SaveChangesAsync();

                return Ok(new {Major = findMajor});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
