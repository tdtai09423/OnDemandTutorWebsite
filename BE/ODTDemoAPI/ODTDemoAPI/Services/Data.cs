using System.Collections.Concurrent;

namespace ODTDemoAPI.Services
{
    public class Data
    {
        public ConcurrentBag<string> SampleData { get; set; } = new();
    }
}
