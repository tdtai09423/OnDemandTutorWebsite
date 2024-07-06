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

        [HttpPost("send-message")]
        [Authorize]
        public async Task<IActionResult> SendMessage(int learnerId, int tutorId, string message, string sender)
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

            var chatMessage = new ChatMessage
            {
                ChatBoxId = chatBox.Id,
                Sender = sender,
                Content = message,
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            chatBox.LastMessageId = chatMessage.Id;
            _context.ChatBoxes.Update(chatBox);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(chatBox.Id.ToString()).SendAsync("ReceiveMessage", sender, message);

            var recipientId = sender == "learner" ? tutorId : learnerId;
            var recipient = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == recipientId);

            if (!_userStatusService.IsUserOnline(recipientId))
            {
                await _emailService.SendMailAsync(recipient!.Email, "New Inbox", $"You have received a new message from a {sender}");
            }

            return Ok(new { chatBoxId = chatBox.Id, messageId = chatMessage.Id });
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
    }
}
