
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.OperationModel
{
    public class EmailVerificationCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailVerificationCleanupService> _logger;

        public EmailVerificationCleanupService(IServiceProvider serviceProvider, ILogger<EmailVerificationCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmailVerificationCleanUpService is starting.");

            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("EmailVerificationCleanUpService is working.");
                await CleanupUnverifiedAccountAsync();

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            _logger.LogInformation("EmailVerificationCleanUpService is stopping.");
        }

        private async Task CleanupUnverifiedAccountAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OnDemandTutorContext>();

            var cutoffDate = DateTime.UtcNow.AddHours(-24);
            var unverifiedAccounts = context.Accounts.Where(a => !a.IsEmailVerified && a.CreatedDate <= cutoffDate).ToList();

            if(unverifiedAccounts.Any())
            {
                context.Accounts.RemoveRange(unverifiedAccounts);
                await context.SaveChangesAsync();

                _logger.LogInformation($"{unverifiedAccounts.Count} unverified accounts have been removed.");
            }
        }
    }
}
