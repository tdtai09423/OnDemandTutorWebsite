using System.Collections.Concurrent;

namespace ODTDemoAPI.Services
{
    public class AccountData
    {
        public ConcurrentBag<string> AccountsData { get; set; } = new();
    }
}
