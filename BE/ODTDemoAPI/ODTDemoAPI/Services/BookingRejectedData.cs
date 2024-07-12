using System.Collections.Concurrent;

namespace ODTDemoAPI.Services
{
    public class BookingRejectedData
    {
        public ConcurrentBag<string> Data { get; set; } = new();
    }
}
