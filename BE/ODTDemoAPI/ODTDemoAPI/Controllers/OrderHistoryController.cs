using Microsoft.AspNetCore.Mvc;
using ODTDemoAPI.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Google;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHistoryController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public OrderHistoryController(OnDemandTutorContext context)
        {
            _context = context;
        }

        // GET: api/OrderHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LearnerOrder>>> GetOrderHistory()
        {
            return await _context.LearnerOrders.ToListAsync();
        }

        // GET: api/OrderHistory/Learner/5
        [HttpGet("Learner/{learnerId}")]
        public async Task<ActionResult<IEnumerable<LearnerOrder>>> GetOrdersByLearnerId(int learnerId)
        {
            var orders = await _context.LearnerOrders
                                       .Where(order => order.LearnerId == learnerId)
                                       .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return orders;
        }
    }
}
