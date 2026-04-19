using System.Collections.Concurrent;

namespace ODTDemoAPI.Services
{
    public class BookingData
    {
        public ConcurrentBag<string> BookingsData { get; set; } = new();
        //test commit 1
    }
}
