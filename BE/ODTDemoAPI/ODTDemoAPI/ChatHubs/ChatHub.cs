using Google;
using Microsoft.AspNetCore.SignalR;
using ODTDemoAPI.Entities;
using ODTDemoAPI.Services;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ODTDemoAPI.ChatHubs
{
    public class ChatHub : Hub
    {
        private readonly OnDemandTutorContext _context;

        public ChatHub(OnDemandTutorContext context) {
            _context = context;
        }

        public async Task SendMessage(int chatBoxId, string sender, string message) {
            var chatMessage = new ChatMessage {
                ChatBoxId = chatBoxId,
                Sender = sender,
                Content = message,
                SendDate = DateTime.UtcNow
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            var chatBox = await _context.ChatBoxes.FindAsync(chatBoxId);
            if (chatBox != null) {
                chatBox.LastMessageId = chatMessage.Id;
                chatBox.SendDate = chatMessage.SendDate;
                _context.ChatBoxes.Update(chatBox);
                await _context.SaveChangesAsync();
            }

            await Clients.Group(chatBoxId.ToString()).SendAsync("ReceiveMessage", sender, message);
        }

        public override async Task OnConnectedAsync() {
            var chatBoxId = Context.GetHttpContext().Request.Query["chatBoxId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            var chatBoxId = Context.GetHttpContext().Request.Query["chatBoxId"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatBoxId);
            await base.OnDisconnectedAsync(exception);
        }
        //public async Task JoinChatBox(int chatBoxId)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId.ToString());
        //}

        //public async Task LeaveChatBox(int chatBoxId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatBoxId.ToString());
        //}

        //public override Task OnConnectedAsync()
        //{
        //    var userId = GetUserIdFromContext();
        //    _userStatusService.SetUserOnline(userId);
        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var userId = GetUserIdFromContext();
        //    _userStatusService.SetUserOffline(userId);
        //    return base.OnDisconnectedAsync(exception);
        //}

        //private int GetUserIdFromContext()
        //{
        //    var userId = int.Parse(Context.User!.FindFirst("userId")!.Value);
        //    return userId;
        //}
    }
}
