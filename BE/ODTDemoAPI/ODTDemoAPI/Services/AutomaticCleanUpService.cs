
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Services
{
    public class AutomaticCleanUpService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly AccountData _data;
        private readonly BookingData _bookingData;
        private readonly IServiceScopeFactory _scopeFactory;

        public AutomaticCleanUpService(AccountData data, IServiceScopeFactory scopeFactory, BookingData bookingData)
        {
            _data = data;
            _scopeFactory = scopeFactory;
            _bookingData = bookingData;
        }

        private void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OnDemandTutorContext>();

                var unverifiedAccounts = context.Accounts
                    .Include(a => a.Wallet)
                    .Include(a => a.Tutor)
                    .Include(a => a.Learner)
                    .Where(a => !a.IsEmailVerified && a.Status == true && a.CreatedDate <= DateTime.Now.AddHours(-48)).ToList();

                var expiredBookings = context.LearnerOrders
                    .Where(o => o.OrderStatus == "Pending" && o.OrderDate <= DateTime.Now.AddHours(-48)).ToList();

                if (unverifiedAccounts.Any())
                {
                    foreach (var account in unverifiedAccounts)
                    {
                        account.Status = false;
                        context.Accounts.Update(account);
                        _data.AccountsData.Add($"The account was deleted at {DateTime.Now.ToShortTimeString()}");
                        context.SaveChanges();
                    }
                }

                if (expiredBookings.Any())
                {
                    foreach (var booking  in expiredBookings)
                    {
                        booking.OrderStatus = "Rejected";
                        context.LearnerOrders.Update(booking);
                        _bookingData.BookingsData.Add($"The booking {booking.OrderId} has been automatically rejected.");
                        context.SaveChanges();
                    }
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
