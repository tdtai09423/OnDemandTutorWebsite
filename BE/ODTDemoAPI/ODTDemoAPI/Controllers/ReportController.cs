using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;
using ODTDemoAPI.OperationModel;
using System.Drawing.Printing;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public ReportController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-reports/{accountId}")]
        [Authorize]
        public async Task<IActionResult> ViewAllReportsById([FromRoute] int accountId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (account == null)
            {
                return NotFound("Not found account");
            }
            IQueryable<Report> query = _context.Reports
                                               .Where(r => r.UserId == accountId).OrderBy(r => r.Id);
            var totalCount = await query.CountAsync();
            var reports = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (reports == null || reports.Count == 0)
            {
                return NotFound("Not found report.");
            }

            var response = new PaginatedResponse<Report>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = reports,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("get-all-reports")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ViewAllReports([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Report> query = _context.Reports.OrderBy(r => r.Id);
            var totalCount = await query.CountAsync();
            var reports = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (reports == null || reports.Count == 0)
            {
                return NotFound("Not found report.");
            }

            var response = new PaginatedResponse<Report>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = reports,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("get-all-pending-reports")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ViewAllPendingReports([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Report> query = _context.Reports.Where(r => r.Status == "Pending").OrderBy(r => r.Id);
            var totalCount = await query.CountAsync();
            var reports = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (reports == null || reports.Count == 0)
            {
                return NotFound("Not found report.");
            }

            var response = new PaginatedResponse<Report>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = reports,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("get-all-rejected-reports")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ViewAllRejectedReports([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Report> query = _context.Reports.Where(r => r.Status == "Rejected").OrderBy(r => r.Id);
            var totalCount = await query.CountAsync();
            var reports = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (reports == null || reports.Count == 0)
            {
                return NotFound("Not found report.");
            }

            var response = new PaginatedResponse<Report>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = reports,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpGet("get-all-accepted-reports")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ViewAllAcceptedReports([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Report> query = _context.Reports.Where(r => r.Status == "Accepted").OrderBy(r => r.Id);
            var totalCount = await query.CountAsync();
            var reports = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (reports == null || reports.Count == 0)
            {
                return NotFound("Not found report.");
            }

            var response = new PaginatedResponse<Report>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = reports,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        [HttpPost("add-new-report")]
        [Authorize]
        public async Task<IActionResult> AddNewReport([FromBody] AddReportModel model)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == model.UserId);
            if (account == null)
            {
                return NotFound("Not found account");
            }

            var report = new Report
            {
                UserId = model.UserId,
                Content = model.Content,
                Status = "Pending",
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Add report successfully!", Report = report});
        }

        [HttpPut("update-report-content")]
        [Authorize]
        public async Task<IActionResult> UpdateReport([FromForm] string? content, [FromBody] int id)
        {
            var report = await _context.Reports.FirstOrDefaultAsync(b => b.Id == id);
            if (report == null)
            {
                return NotFound("Not found report");
            }

            if(report.Status != "Pending")
            {
                return BadRequest("Your report has been processed by admin. Cannot update.");
            }

            if(!string.IsNullOrEmpty(content))
            {
                report.Content = content;
                _context.Reports.Update(report);
                await _context.SaveChangesAsync();

                return Ok(new {message = "Update Successfully!", Report = report});
            }

            return NoContent();
        }

        [HttpPut("accept-report")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AcceptReport([FromBody] int id)
        {
            try
            {
                var report = await _context.Reports.FirstOrDefaultAsync(b => b.Id == id);
                if (report == null)
                {
                    return NotFound("Not found report");
                }

                if (report.Status != "Pending")
                {
                    return BadRequest("This report is not at Pending status.");
                }

                report.Status = "Accepted";
                _context.Reports.Update(report);
                await _context.SaveChangesAsync();

                return Ok("Accept successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reject-report")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RejectReport([FromBody] int id)
        {
            try
            {
                var report = await _context.Reports.FirstOrDefaultAsync(b => b.Id == id);
                if (report == null)
                {
                    return NotFound("Not found report");
                }

                if (report.Status != "Pending")
                {
                    return BadRequest("This report is not at Pending status.");
                }

                report.Status = "Rejected";
                _context.Reports.Update(report);
                await _context.SaveChangesAsync();

                return Ok("Reject successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reset-report-status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ResetReportStatus([FromBody] int id)
        {
            try
            {
                var report = await _context.Reports.FirstOrDefaultAsync(b => b.Id == id);
                if (report == null)
                {
                    return NotFound("Not found report");
                }

                report.Status = "Pending";
                _context.Reports.Update(report);
                await _context.SaveChangesAsync();

                return Ok("Reset successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
