using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.ChatHubs;
using ODTDemoAPI.Entities;
using ODTDemoAPI.Services;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;
        private readonly IEmailService _emailService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserStatusService _userStatusService;

        public ChatController(OnDemandTutorContext context, IEmailService emailService, IHubContext<ChatHub> hubContext, UserStatusService userStatusService)
        {
            _context = context;
            _emailService = emailService;
            _hubContext = hubContext;
            _userStatusService = userStatusService;
        }

        [HttpPost("new-chatbox-{learnerId}-{tutorId}")]
        [Authorize]
        public async Task<IActionResult> CreateNewChatBox([FromRoute] int tutorId, [FromRoute] int learnerId)
        {
            try
            {
                var chatBox = await _context.ChatBoxes.FirstOrDefaultAsync(cb => cb.LearnerId == learnerId && cb.TutorId == tutorId);

                if (chatBox == null)
                {
                    chatBox = new ChatBox
                    {
                        LearnerId = learnerId,
                        TutorId = tutorId,
                        SendDate = DateTime.Now,
                    };
                    _context.ChatBoxes.Add(chatBox);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { box = chatBox });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpGet("chatboxes/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetChatBoxes([FromRoute] int userId)
        {
            var chatBoxes = await _context.ChatBoxes.Where(cb => cb.TutorId == userId || cb.LearnerId == userId).ToListAsync();
            return Ok(chatBoxes);
        }

        [HttpGet("chatMessages/{chatBoxId}")]
        [Authorize]
        public async Task<IActionResult> GetMessages([FromRoute] int chatBoxId)
        {
            var chatMessages = await _context.ChatMessages.Where(cm => cm.ChatBoxId == chatBoxId).OrderBy(cm => cm.Id).ToListAsync();
            return Ok(chatMessages);
        }

        [HttpGet("chatbox/{learnerId}/{tutorId}")]
        [Authorize]
        public async Task<IActionResult> GetChatBox([FromRoute] int learnerId, [FromRoute] int tutorId)
        {
            var chatBox = await _context.ChatBoxes
                                    .Include(c => c.Learner)
                                    .Include(c => c.Tutor)
                                    .Include(c => c.ChatMessages)
                                    .Where(c => c.LearnerId == learnerId && c.TutorId == tutorId).ToListAsync();
            return Ok(chatBox);
        }
    }
}
