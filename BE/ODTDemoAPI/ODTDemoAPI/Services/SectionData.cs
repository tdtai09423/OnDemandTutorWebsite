using System.Collections.Concurrent;

namespace ODTDemoAPI.Services
{
    public class SectionData
    {
        public ConcurrentBag<string> SectionsData { get; set; } = new();
    }
}
