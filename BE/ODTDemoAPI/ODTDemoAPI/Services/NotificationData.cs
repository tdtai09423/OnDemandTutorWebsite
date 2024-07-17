using System.Collections.Concurrent;

namespace ODTDemoAPI.Services
{
    public class NotificationData
    {
        public ConcurrentBag<string> NotificationsData { get; set; } = new();
    }
}
