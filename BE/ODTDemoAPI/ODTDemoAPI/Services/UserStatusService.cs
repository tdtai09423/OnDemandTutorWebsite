using System.Collections.Concurrent;

namespace ODTDemoAPI.Services
{
    public class UserStatusService
    {
        private static ConcurrentDictionary<int, bool> _userStatus = new ConcurrentDictionary<int, bool>();

        public void SetUserOnline(int userId)
        {
            _userStatus[userId] = true;
        }

        public void SetUserOffline(int userId)
        {
            _userStatus[userId] = false;
        }

        public bool IsUserOnline(int userId)
        {
            return _userStatus.TryGetValue(userId, out var isOnline) && isOnline;
        }
    }
}
