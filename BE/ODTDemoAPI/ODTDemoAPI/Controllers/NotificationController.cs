using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public IActionResult GetNotisById(int id)
        {
            try
            {
                var notis = _context.UserNotifications.Where(s => s.AccountId == id);
                return Ok(notis);
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
