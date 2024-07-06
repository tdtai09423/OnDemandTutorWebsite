using Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.OperationModel;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "TUTOR")]
public class AnalystController : ControllerBase
{
    private readonly OnDemandTutorContext _context;

    public AnalystController(OnDemandTutorContext context)
    {
        _context = context;
    }

    [HttpGet("get-bookings-summary/{tutorId}")]
    public async Task<IActionResult> GetBookingsSummaryForTutor([FromRoute] int tutorId, [FromQuery] DateTime date)
    {
        try
        {
            var query = _context.LearnerOrders
                                .Include(o => o.Curriculum)
                                .Where(o => o.Curriculum!.TutorId == tutorId && o.OrderDate.Date == date.Date);

            var totalBookings = await query.CountAsync();
            var totalAmount = await query.SumAsync(o => o.Total);

            var response = new BookingSummary
            {
                TotalBookings = totalBookings,
                TotalAmount = totalAmount
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("get-monthly-bookings-summary/{tutorId}")]
    public async Task<IActionResult> GetMonthlyBookingsSummaryForTutor([FromRoute] int tutorId, [FromQuery] int month, [FromQuery] int year)
    {
        try
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var query = _context.LearnerOrders
                                .Include(o => o.Curriculum)
                                .Where(o => o.Curriculum!.TutorId == tutorId && o.OrderDate >= startDate && o.OrderDate < endDate);

            var totalBookings = await query.CountAsync();
            var totalAmount = await query.SumAsync(o => o.Total);    
            var orders = await query.ToListAsync();

            var response = new BookingSummary
            {
                TotalBookings = totalBookings,
                TotalAmount = totalAmount,
                Orders = orders
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
   