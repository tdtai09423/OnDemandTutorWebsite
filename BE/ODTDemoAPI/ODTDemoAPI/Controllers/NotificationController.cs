using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public NotificationController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("get-notifications-by-account/{accountId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserNotification>>> GetNotificationsById([FromRoute] int accountId)
        {
            var notis = await _context.UserNotifications.Where(n => n.AccountId == accountId).OrderByDescending(n => n.NotificateDay).ToListAsync();
            var totalCount = notis.Count;
            var totalCountNew = await _context.UserNotifications.CountAsync(n => n.NotiStatus == "NEW");
            return Ok(new {NotificationList = notis, CountNew = totalCountNew, Count = totalCount});
        }

        [HttpPut("mark-as-read")]
        [Authorize]
        public async Task<IActionResult> MarkAsReadAll([FromBody] int accountId)
        {
            try
            {
                var notis = await _context.UserNotifications.Where(n => n.NotiStatus == "NEW" && n.AccountId == accountId).ToListAsync();
                if(notis == null || notis.Count == 0)
                {
                    return Ok(new {message = "You have no notifications yet."});
                }

                foreach(var noti in notis)
                {
                    noti.NotiStatus = "READ";
                }

                _context.UserNotifications.UpdateRange(notis);
                await _context.SaveChangesAsync();

                return Ok(new { message = "All notifications are marked as read." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //chức năng của con mụ admin
        //[HttpPost("create-new-notification")]
        //public async Task<IActionResult> CreateNewNoti()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
