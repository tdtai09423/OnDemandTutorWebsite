using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Services
{
    public class AutomaticNotifyService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly NotificationData _notificationData;
        private readonly SectionData _sectionData;
        private readonly IServiceScopeFactory _scopeFactory;

        public AutomaticNotifyService(SectionData sectionData, IServiceScopeFactory scopeFactory, NotificationData notificationData)
        {
            _sectionData = sectionData;
            _scopeFactory = scopeFactory;
            _notificationData = notificationData;
        }

        private async void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OnDemandTutorContext>();

                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var sectionsOnClock = await context.Sections.Where(s => s.SectionStart <= DateTime.Now.AddMinutes(5) && s.SectionStatus == "Not Started").ToListAsync();

                foreach (var section in sectionsOnClock)
                {
                    var stbConditions = await context.STBConditions.Where(c => c.StartTime == section.SectionStart).ToListAsync();
                    LearnerOrder? order = null;

                    foreach (var stbCondition in stbConditions)
                    {
                        order = await context.LearnerOrders
                            .Include(o => o.Learner)
                            .Include(o => o.Curriculum!)
                            .ThenInclude(c => c.Tutor)
                            .FirstOrDefaultAsync(o => o.OrderId == stbCondition.OrderId);
                        if (order!.CurriculumId == section!.CurriculumId)
                        {
                            break;
                        }
                    }

                    await emailService.SendMailAsync(order!.Learner!.LearnerEmail, "It's Time To Learn", "It's 5 minutes for you to learn the section you booked. Go to the website to learn on time. Let's go.");
                    var learnerNotification = new UserNotification
                    {
                        AccountId = (int) order!.LearnerId!,
                        Content = "It's 5 minutes for you to learn the section you booked",
                        NotificateDay = DateTime.Now,
                        NotiStatus = "NEW",
                    };
                    context.UserNotifications.Add(learnerNotification);
                    _notificationData.NotificationsData.Add($"The notification and mail were sent to learner {order!.Learner!.LearnerId} at {DateTime.Now.ToShortTimeString()}");

                    await emailService.SendMailAsync(order!.Curriculum!.Tutor!.TutorEmail, "It's Time To Connect With Your Learner", "It's 5 minutes for you to start a new section with the learner. Go to the website to have a good teach day. Let's go.");
                    var tutorNotification = new UserNotification
                    {
                        AccountId = order!.Curriculum!.Tutor!.TutorId,
                        Content = "It's 5 minutes for you to start a new section with the learner. Go to the website to have a good teach day. Let's go.",
                        NotificateDay = DateTime.Now,
                        NotiStatus = "NEW",
                    };
                    context.UserNotifications.Add(tutorNotification);
                    _notificationData.NotificationsData.Add($"The notification and mail were sent to tutor {order!.Curriculum!.Tutor!.TutorId} at {DateTime.Now.ToShortTimeString()}");

                    section.SectionStatus = "Present";
                    context.Sections.Update(section);
                    _sectionData.SectionsData.Add($@"The section {section.SectionId} has been toggled into ""Present"" at {DateTime.Now.ToShortTimeString()}.");
                    await context.SaveChangesAsync();
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
