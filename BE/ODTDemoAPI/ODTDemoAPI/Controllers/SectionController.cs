using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;

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
                var minPrice = await _context.Curricula.Where(c => c.TutorId == tutorId).MinAsync(c => (decimal?)c.PricePerSection) ?? 0;
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
                var maxPrice = await _context.Curricula.Where(c => c.TutorId == tutorId).MaxAsync(c => (decimal?)c.PricePerSection) ?? 0;
                return Ok(maxPrice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("weeks")]
        public IActionResult GetWeeks()
        {
            var weeks = new List<WeekViewModel>();
            var startDate = new DateTime(DateTime.Now.Year, 1, 1);

            while (startDate.DayOfWeek != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(1);
            }

            var endDate = new DateTime(DateTime.Now.Year, 12, 31);

            while (startDate <= endDate)
            {
                var week = new WeekViewModel
                {
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                };
                weeks.Add(week);
                startDate = startDate.AddDays(7);
            }

            return Ok(weeks);
        }

        [HttpGet("get-order-by-section/{sectionId}")]
        public async Task<IActionResult> GetOrderBySection([FromRoute] int sectionId)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(s => s.SectionId == sectionId);

            var stbConditions = await _context.STBConditions.Where(c => c.StartTime == section!.SectionStart).ToListAsync();

            if (!stbConditions.Any())
            {
                return NotFound(new { Message = "No STBCondition found" });
            }

            LearnerOrder? order = null;

            foreach ( var stbCondition in stbConditions)
            {
                order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == stbCondition.OrderId);
                if (order!.CurriculumId == section!.CurriculumId)
                {
                    break;
                }
            }

            return Ok(order);
        }

        [HttpGet("weekly-schedule-tutor")]
        public async Task<IActionResult> GetWeeklyScheduleTutor(int tutorId, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            var sections = await _context.Sections
                                         .Where(s => s.Curriculum!.TutorId == tutorId && s.SectionStart >= startTime && s.SectionEnd <= endTime)
                                         .OrderBy(s => s.SectionStart)
                                         .ToListAsync();

            var schedule = sections.GroupBy(s => s.SectionStart.Date).Select(s => new ScheduleViewModel
            {
                Date = s.Key,
                Sections = s.Select(s => new SectionViewModel
                {
                    Id = s.SectionId,
                    SectionStart = s.SectionStart,
                    SectionEnd = s.SectionEnd,
                    SectionStatus = s.SectionStatus,
                    MeetUrl = s.MeetUrl,
                }).ToList()
            }).ToList();

            return Ok(schedule);
        }

        [HttpGet("weekly-schedule-learner")]
        [Authorize(Roles = "LEARNER")]
        public async Task<IActionResult> GetWeeklyScheduleLearner(int learnerId, [FromQuery] DateTime startTime, DateTime endTime)
        {
            var orders = await _context.LearnerOrders
                                .Where(o => o.LearnerId == learnerId)
                                .ToListAsync();
            STBCondition? stbCondition = null;

            List<Section> sections = new List<Section>();
            foreach (var order in orders)
            {
                var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.CurriculumId == order.CurriculumId);
                if (curriculum == null)
                {
                    continue;
                }
                if(curriculum.CurriculumType == "ShortTerm")
                {
                    stbCondition = await _context.STBConditions.FirstOrDefaultAsync(c => c.OrderId == order.OrderId);
                    if (stbCondition == null)
                    {
                        continue;
                    }
                    var sectionByOrder = await _context.Sections
                                                        .Where(s => s.CurriculumId == order.CurriculumId
                                                                    && s.SectionStart.Date >= startTime.Date
                                                                    && s.SectionEnd.Date <= endTime.Date)
                                                        .ToListAsync();
                    sectionByOrder = sectionByOrder.Where(s => s.SectionStart == stbCondition.StartTime).ToList();
                    sections.AddRange(sectionByOrder);
                }
                else
                {
                    continue;
                }
            }

            sections = sections.OrderBy(s => s.SectionStart).ToList();

            var schedule = sections.GroupBy(s => s.SectionStart.Date).Select(g => new ScheduleViewModel
            {
                Date = g.Key,
                Sections = g.Select(s => new SectionViewModel
                {
                    Id = s.SectionId,
                    SectionStart = s.SectionStart,
                    SectionEnd = s.SectionEnd,
                    SectionStatus = s.SectionStatus,
                    MeetUrl = s.MeetUrl
                }).ToList()
            }).ToList();

            return Ok(new { Schedule = schedule, Condition = stbCondition });
        }
    }
}
