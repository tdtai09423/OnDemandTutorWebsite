using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public FilterController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("get-available-tutors-by-date")]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAvailableTutorsByDate([FromQuery] DateTime date, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var tutors = await _context.Tutors.ToListAsync();

            var availableTutors = new List<Tutor>();

            DateTime startOfDay = date.Date.AddHours(7);
            DateTime endOfDay = date.Date.AddHours(23);

            foreach (var tutor in tutors)
            {
                var sections = await _context.Sections
                                    .Where(s => s.Curriculum!.LearnerOrders.Any(lo => lo.Curriculum!.TutorId == tutor.TutorId) && s.SectionStart.Date == date.Date)
                                    .OrderBy(s => s.SectionStart)
                                    .ToListAsync();

                //nếu tutor không có section nào => tutor rảnh cả ngày => add vào
                if (!sections.Any())
                {
                    availableTutors.Add(tutor);
                    continue;
                }

                DateTime previousEnd = startOfDay;
                bool hasAtLeastOneHourFree = false;

                foreach(var section in sections)
                {
                    if((section.SectionStart - previousEnd).TotalSeconds >= 60 * 60) //rảnh ít nhất 1 giờ
                    {
                        hasAtLeastOneHourFree = true;
                        break;
                    }

                    previousEnd = section.SectionEnd;
                }

                if(!hasAtLeastOneHourFree && (endOfDay - previousEnd).TotalSeconds >= 60 * 60)
                {
                    hasAtLeastOneHourFree = true;
                }

                if(hasAtLeastOneHourFree)
                {
                    availableTutors.Add(tutor);
                }
            }

            var totalCount = availableTutors.Count();
            var pagedTutor = availableTutors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = new PaginatedResponse<Tutor>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = pagedTutor
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
