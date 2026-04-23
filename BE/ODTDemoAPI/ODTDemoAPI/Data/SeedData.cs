using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OnDemandTutorContext>();

            // Drop and recreate database
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await SeedMemberships(context);
            await SeedMajors(context);
            await SeedAccounts(context);
            await SeedLearnersAndTutors(context);
            await SeedWallets(context);
            await SeedCurricula(context);
            await SeedSections(context);
            await SeedLearnerOrders(context);
            await SeedSTBCondition(context);
        }

        private static async Task SeedMemberships(OnDemandTutorContext context)
        {
            context.Memberships.AddRange(
                new Membership { MembershipId = "FREE", MembershipLevel = "Free", MembershipDescription = "Free membership with basic features", DurationInDays = 365 },
                new Membership { MembershipId = "SILVER", MembershipLevel = "Silver", MembershipDescription = "Silver membership with extra benefits", DurationInDays = 30 },
                new Membership { MembershipId = "GOLD", MembershipLevel = "Gold", MembershipDescription = "Gold membership with premium features", DurationInDays = 30 }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedMajors(OnDemandTutorContext context)
        {
            context.Majors.AddRange(
                new Major { MajorId = "MATH", MajorName = "Mathematics" },
                new Major { MajorId = "ENG", MajorName = "English" },
                new Major { MajorId = "PHY", MajorName = "Physics" },
                new Major { MajorId = "CHEM", MajorName = "Chemistry" },
                new Major { MajorId = "BIO", MajorName = "Biology" },
                new Major { MajorId = "CS", MajorName = "Computer Science" },
                new Major { MajorId = "HIS", MajorName = "History" },
                new Major { MajorId = "GEO", MajorName = "Geography" }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedAccounts(OnDemandTutorContext context)
        {
            // Password: "123456" hashed with BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("123456");

            context.Accounts.AddRange(
                // Admin - Id 1
                new Account
                {
                    FirstName = "Admin",
                    LastName = "System",
                    Email = "admin@odt.com",
                    Password = hashedPassword,
                    RoleId = "ADMIN",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now
                },
                // Tutors - Id 2, 3, 4
                new Account
                {
                    FirstName = "Nguyen",
                    LastName = "Van Tuan",
                    Email = "tutor1@odt.com",
                    Password = hashedPassword,
                    RoleId = "TUTOR",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now
                },
                new Account
                {
                    FirstName = "Tran",
                    LastName = "Thi Mai",
                    Email = "tutor2@odt.com",
                    Password = hashedPassword,
                    RoleId = "TUTOR",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now
                },
                new Account
                {
                    FirstName = "Le",
                    LastName = "Minh Duc",
                    Email = "tutor3@odt.com",
                    Password = hashedPassword,
                    RoleId = "TUTOR",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now
                },
                // Learners - Id 5, 6, 7
                new Account
                {
                    FirstName = "Pham",
                    LastName = "Hoang An",
                    Email = "learner1@odt.com",
                    Password = hashedPassword,
                    RoleId = "LEARNER",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now
                },
                new Account
                {
                    FirstName = "Vo",
                    LastName = "Thanh Tam",
                    Email = "learner2@odt.com",
                    Password = hashedPassword,
                    RoleId = "LEARNER",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now
                },
                new Account
                {
                    FirstName = "Hoang",
                    LastName = "Minh Thu",
                    Email = "learner3@odt.com",
                    Password = hashedPassword,
                    RoleId = "LEARNER",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now
                }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedLearnersAndTutors(OnDemandTutorContext context)
        {
            // Tutors
            context.Tutors.AddRange(
                new Tutor
                {
                    TutorId = 2,
                    TutorAge = 30,
                    TutorEmail = "tutor1@odt.com",
                    Nationality = "Vietnamese",
                    TutorDescription = "Experienced Mathematics tutor with 5 years of teaching.",
                    TutorPicture = "https://via.placeholder.com/150",
                    CertiStatus = CertiStatus.Approved,
                    MajorId = "MATH"
                },
                new Tutor
                {
                    TutorId = 3,
                    TutorAge = 28,
                    TutorEmail = "tutor2@odt.com",
                    Nationality = "Vietnamese",
                    TutorDescription = "English language specialist, IELTS 8.0.",
                    TutorPicture = "https://via.placeholder.com/150",
                    CertiStatus = CertiStatus.Approved,
                    MajorId = "ENG"
                },
                new Tutor
                {
                    TutorId = 4,
                    TutorAge = 35,
                    TutorEmail = "tutor3@odt.com",
                    Nationality = "Vietnamese",
                    TutorDescription = "Computer Science tutor specializing in algorithms.",
                    TutorPicture = "https://via.placeholder.com/150",
                    CertiStatus = CertiStatus.Pending,
                    MajorId = "CS"
                }
            );

            // Learners
            context.Learners.AddRange(
                new Learner
                {
                    LearnerId = 5,
                    LearnerAge = 20,
                    LearnerEmail = "learner1@odt.com",
                    LearnerPicture = "https://via.placeholder.com/150",
                    MembershipId = "FREE",
                    MembershipCreatedDate = DateTime.Now
                },
                new Learner
                {
                    LearnerId = 6,
                    LearnerAge = 22,
                    LearnerEmail = "learner2@odt.com",
                    LearnerPicture = "https://via.placeholder.com/150",
                    MembershipId = "SILVER",
                    MembershipCreatedDate = DateTime.Now
                },
                new Learner
                {
                    LearnerId = 7,
                    LearnerAge = 19,
                    LearnerEmail = "learner3@odt.com",
                    LearnerPicture = "https://via.placeholder.com/150",
                    MembershipId = null,
                    MembershipCreatedDate = null
                }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedWallets(OnDemandTutorContext context)
        {
            // Wallets for all accounts (admin + tutors + learners)
            context.Wallets.AddRange(
                new Wallet { WalletId = 1, Balance = 0m },
                new Wallet { WalletId = 2, Balance = 500000m },
                new Wallet { WalletId = 3, Balance = 300000m },
                new Wallet { WalletId = 4, Balance = 100000m },
                new Wallet { WalletId = 5, Balance = 200000m },
                new Wallet { WalletId = 6, Balance = 150000m },
                new Wallet { WalletId = 7, Balance = 50000m }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedCurricula(OnDemandTutorContext context)
        {
            context.Curricula.AddRange(
                new Curriculum
                {
                    CurriculumType = "SHORT",
                    TotalSlot = 10,
                    CurriculumStatus = "ACTIVE",
                    CurriculumDescription = "Basic Mathematics - Algebra & Geometry",
                    PricePerSection = 150000m,
                    TutorId = 2
                },
                new Curriculum
                {
                    CurriculumType = "LONG",
                    TotalSlot = 30,
                    CurriculumStatus = "ACTIVE",
                    CurriculumDescription = "Advanced Mathematics - Calculus",
                    PricePerSection = 200000m,
                    TutorId = 2
                },
                new Curriculum
                {
                    CurriculumType = "SHORT",
                    TotalSlot = 15,
                    CurriculumStatus = "ACTIVE",
                    CurriculumDescription = "IELTS Preparation Course",
                    PricePerSection = 250000m,
                    TutorId = 3
                },
                new Curriculum
                {
                    CurriculumType = "SHORT",
                    TotalSlot = 10,
                    CurriculumStatus = "ACTIVE",
                    CurriculumDescription = "Data Structures & Algorithms",
                    PricePerSection = 300000m,
                    TutorId = 4
                }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedSections(OnDemandTutorContext context)
        {
            var now = DateTime.Now;
            context.Sections.AddRange(
                // Sections for curriculum 1
                new Section
                {
                    SectionStart = now.AddDays(1).Date.AddHours(8),
                    SectionEnd = now.AddDays(1).Date.AddHours(10),
                    SectionStatus = "SCHEDULED",
                    CurriculumId = 1,
                    MeetUrl = "none"
                },
                new Section
                {
                    SectionStart = now.AddDays(3).Date.AddHours(8),
                    SectionEnd = now.AddDays(3).Date.AddHours(10),
                    SectionStatus = "SCHEDULED",
                    CurriculumId = 1,
                    MeetUrl = "none"
                },
                // Sections for curriculum 3
                new Section
                {
                    SectionStart = now.AddDays(2).Date.AddHours(14),
                    SectionEnd = now.AddDays(2).Date.AddHours(16),
                    SectionStatus = "SCHEDULED",
                    CurriculumId = 3,
                    MeetUrl = "none"
                }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedLearnerOrders(OnDemandTutorContext context)
        {
            context.LearnerOrders.AddRange(
                new LearnerOrder
                {
                    OrderDate = DateTime.Now.AddDays(-5),
                    OrderStatus = "COMPLETED",
                    Total = 1500000m,
                    CurriculumId = 1,
                    IsCompleted = true,
                    LearnerId = 5
                },
                new LearnerOrder
                {
                    OrderDate = DateTime.Now.AddDays(-2),
                    OrderStatus = "PENDING",
                    Total = 3750000m,
                    CurriculumId = 3,
                    IsCompleted = false,
                    LearnerId = 6
                }
            );
            await context.SaveChangesAsync();
        }

        private static async Task SeedSTBCondition(OnDemandTutorContext context)
        {
            context.STBConditions.Add(new STBCondition
            {
                OrderId = 1,
                StartTime = DateTime.Now.AddDays(-5),
                Duration = 60
            });
            await context.SaveChangesAsync();
        }
    }
}
