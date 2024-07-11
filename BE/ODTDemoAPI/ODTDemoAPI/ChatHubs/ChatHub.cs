using Microsoft.AspNetCore.SignalR;
using ODTDemoAPI.Entities;
using ODTDemoAPI.Services;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ODTDemoAPI.ChatHubs
{
    public class ChatHub : Hub
    {
        private readonly UserStatusService _userStatusService;

        public ChatHub(UserStatusService userStatusService)
        {
            _userStatusService = userStatusService;
        }
        public async Task SendMessage(string sender, string message, int chatBoxId)
        {
            await Clients.Group(chatBoxId.ToString()).SendAsync("ReceiveMessage", sender, message);
        }

        public async Task JoinChatBox(int chatBoxId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId.ToString());
        }

        public async Task LeaveChatBox(int chatBoxId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatBoxId.ToString());
        }

        public override Task OnConnectedAsync()
        {
            var userId = GetUserIdFromContext();
            _userStatusService.SetUserOnline(userId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetUserIdFromContext();
            _userStatusService.SetUserOffline(userId);
            return base.OnDisconnectedAsync(exception);
        }

        private int GetUserIdFromContext()
        {
            var userId = int.Parse(Context.User!.FindFirst("userId")!.Value);
            return userId;
        }
    }
}
