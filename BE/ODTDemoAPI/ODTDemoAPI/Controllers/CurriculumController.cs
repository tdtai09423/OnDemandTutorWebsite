using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;
using ODTDemoAPI.OperationModel;

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

                var curricula = await _context.Curricula.Where(c => c.TutorId == tutorId && c.CurriculumStatus == "Accepted").ToListAsync();

                if (curricula == null || curricula.Count == 0)
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

        [HttpPost("add-neww-curriculum")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> AddNewCurriculum([FromBody] int tutorId, [FromForm] AddCurriculumModel curriculum)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
                if (tutor == null)
                {
                    return NotFound("Not found tutor");
                }

                if (curriculum.CurriculumType != "ShortTerm" && curriculum.CurriculumType != "LongTerm")
                {
                    return BadRequest("Not found type");
                }

                var findCurriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.TutorId == tutorId
                                                                    && c.CurriculumType == curriculum.CurriculumType
                                                                    && c.CurriculumDescription == curriculum.CurriculumDescription);
                if (findCurriculum != null)
                {
                    return BadRequest("Existed curriculum");
                }

                var newCurriculum = new Curriculum
                {
                    CurriculumType = curriculum.CurriculumType,
                    CurriculumStatus = "Pending",
                    TotalSlot = curriculum.TotalSlot,
                    CurriculumDescription = curriculum.CurriculumDescription,
                    PricePerSection = curriculum.PricePerSection,
                    TutorId = tutorId
                };

                _context.Curricula.Add(newCurriculum);
                await _context.SaveChangesAsync();

                var find = await _context.Curricula.FirstOrDefaultAsync(c => c.TutorId == tutorId
                                                                    && c.CurriculumType == curriculum.CurriculumType
                                                                    && c.CurriculumDescription == curriculum.CurriculumDescription);

                return Ok(new { message = "Add successfully!", Curriculum = find });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-curriculum-info")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> UpdateCurriculumInfo([FromBody] int tutorId, [FromQuery] int curriculumId, [FromForm] UpdateCurriculumModel model)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
                if (tutor == null)
                {
                    return NotFound("Not found tutor");
                }

                var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.CurriculumId == curriculumId);
                var oldCurriculum = curriculum;

                if (curriculum == null)
                {
                    return NotFound("Not found curriculum");
                }

                if (!string.IsNullOrEmpty(model.CurriculumType))
                {
                    curriculum.CurriculumType = model.CurriculumType;
                }

                if (!string.IsNullOrEmpty(model.CurriculumDescription))
                {
                    curriculum.CurriculumDescription = model.CurriculumDescription;
                }

                if (model.TotalSlot.HasValue)
                {
                    curriculum.TotalSlot = model.TotalSlot.Value;
                }

                if (model.PricePerSection.HasValue)
                {
                    curriculum.PricePerSection = model.PricePerSection.Value;
                }

                if(curriculum != oldCurriculum)
                {
                    curriculum.CurriculumStatus = "Pending";
                }

                _context.Curricula.Update(curriculum);
                await _context.SaveChangesAsync();

                var find = await _context.Curricula.FirstOrDefaultAsync(c => c.TutorId == tutorId
                                                                    && c.CurriculumType == curriculum.CurriculumType
                                                                    && c.CurriculumDescription == curriculum.CurriculumDescription);
                return Ok(new { message = "Update successfully!", Curriculum = find });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("view-all-curriculum")]
        public async Task<IActionResult> ViewAllCurricula([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Curriculum> query = _context.Curricula.OrderBy(c => c.CurriculumId);
            var totalCount = await query.CountAsync();
            var curricula = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (curricula == null || curricula.Count == 0)
            {
                return NotFound("No transaction was found.");
            }
            var response = new PaginatedResponse<Curriculum>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = curricula,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("view-all-pending-curriculum")]
        public async Task<IActionResult> ViewAllPendingCurricula([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Curriculum> query = _context.Curricula.Where(c => c.CurriculumStatus == "Pending").OrderBy(c => c.CurriculumId);
            var totalCount = await query.CountAsync();
            var curricula = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (curricula == null || curricula.Count == 0)
            {
                return NotFound("No transaction was found.");
            }
            var response = new PaginatedResponse<Curriculum>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = curricula,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("view-all-Accepted-curriculum")]
        public async Task<IActionResult> ViewAllAcceptedCurricula([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Curriculum> query = _context.Curricula.Where(c => c.CurriculumStatus == "Accepted").OrderBy(c => c.CurriculumId);
            var totalCount = await query.CountAsync();
            var curricula = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (curricula == null || curricula.Count == 0)
            {
                return NotFound("No transaction was found.");
            }
            var response = new PaginatedResponse<Curriculum>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = curricula,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("view-all-rejected-curriculum")]
        public async Task<IActionResult> ViewAllRejectedCurricula([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Curriculum> query = _context.Curricula.Where(c => c.CurriculumStatus == "Rejected").OrderBy(c => c.CurriculumId);
            var totalCount = await query.CountAsync();
            var curricula = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (curricula == null || curricula.Count == 0)
            {
                return NotFound("No transaction was found.");
            }
            var response = new PaginatedResponse<Curriculum>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = curricula,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpPut("accept-curriculum")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AcceptCurriculum([FromBody] int curriculumId)
        {
            try
            {
                var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.CurriculumId == curriculumId);
                if(curriculum == null)
                {
                    return NotFound("Not found curriculum");
                }

                curriculum.CurriculumStatus = "Accepted";
                _context.Curricula.Update(curriculum);
                await _context.SaveChangesAsync();

                return Ok(new {message = "Update successfully", Curriculum = curriculum });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reject-curriculum")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RejectCurriculum([FromBody] int curriculumId)
        {
            try
            {
                var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.CurriculumId == curriculumId);
                if (curriculum == null)
                {
                    return NotFound("Not found curriculum");
                }

                curriculum.CurriculumStatus = "Rejected";
                _context.Curricula.Update(curriculum);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Update successfully", Curriculum = curriculum });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reset-curriculum-status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ResetCurriculumStatus([FromBody] int curriculumId)
        {
            try
            {
                var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.CurriculumId == curriculumId);
                if (curriculum == null)
                {
                    return NotFound("Not found curriculum");
                }

                curriculum.CurriculumStatus = "Pending";
                _context.Curricula.Update(curriculum);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Update successfully", Curriculum = curriculum });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
