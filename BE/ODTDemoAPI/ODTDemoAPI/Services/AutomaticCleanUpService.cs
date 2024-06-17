using ODTDemoAPI.Entities;

public class AutomaticCleanUpService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public AutomaticCleanUpService(IServiceProvider serviceProvider, Timer timer)
    {
        _serviceProvider = serviceProvider;
        _timer = timer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1)); // Run every hour
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<OnDemandTutorContext>();

            var unverifiedAccounts = context.Accounts
                .Where(a => !a.IsEmailVerified && a.CreatedDate <= DateTime.Now.AddHours(-24))
                .ToList();

            if (unverifiedAccounts.Any())
            {
                context.Accounts.RemoveRange(unverifiedAccounts);
                context.SaveChanges();
            }
        }
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
