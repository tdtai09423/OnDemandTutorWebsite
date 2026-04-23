using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Data
{
    public static class SeedData
    {
        private const string DefaultPicture = "https://via.placeholder.com/150";

        // Account IDs layout:
        //  1       = Admin
        //  2 - 21  = 20 Tutors
        //  22 - 25 = 4 Learners

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OnDemandTutorContext>();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await SeedMemberships(context);
            await SeedMajors(context);
            await SeedAccounts(context);
            await SeedTutors(context);
            await SeedLearners(context);
            await SeedWallets(context);
            await SeedCurricula(context);
            await SeedSections(context);
            await SeedLearnerOrders(context);
            await SeedReviewRatings(context);
            await SeedSTBConditions(context);
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
            var hp = BCrypt.Net.BCrypt.HashPassword("123456");
            var now = DateTime.Now;

            var accounts = new List<Account>
            {
                // 1 - Admin
                new() { FirstName = "Admin", LastName = "System", Email = "admin@odt.com", Password = hp, RoleId = "ADMIN", Status = true, IsEmailVerified = true, CreatedDate = now },

                // 2-21 - 20 Tutors
                new() { FirstName = "Nguyen", LastName = "Van Tuan", Email = "tutor1@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-60) },
                new() { FirstName = "Tran", LastName = "Thi Mai", Email = "tutor2@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-55) },
                new() { FirstName = "Le", LastName = "Minh Duc", Email = "tutor3@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-50) },
                new() { FirstName = "Pham", LastName = "Quoc Bao", Email = "tutor4@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-48) },
                new() { FirstName = "Vo", LastName = "Hoang Nam", Email = "tutor5@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-45) },
                new() { FirstName = "Hoang", LastName = "Thi Lan", Email = "tutor6@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-42) },
                new() { FirstName = "Dang", LastName = "Van Hieu", Email = "tutor7@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-40) },
                new() { FirstName = "Bui", LastName = "Minh Khoa", Email = "tutor8@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-38) },
                new() { FirstName = "Do", LastName = "Thanh Son", Email = "tutor9@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-35) },
                new() { FirstName = "Ngo", LastName = "Phuong Thao", Email = "tutor10@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-33) },
                new() { FirstName = "Dinh", LastName = "Quang Huy", Email = "tutor11@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-30) },
                new() { FirstName = "Ly", LastName = "Thi Hong", Email = "tutor12@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-28) },
                new() { FirstName = "Truong", LastName = "Van Phuc", Email = "tutor13@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-25) },
                new() { FirstName = "Duong", LastName = "Thi Ngoc", Email = "tutor14@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-22) },
                new() { FirstName = "Mai", LastName = "Xuan Truong", Email = "tutor15@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-20) },
                new() { FirstName = "Vuong", LastName = "Duc Anh", Email = "tutor16@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-18) },
                new() { FirstName = "Tang", LastName = "Bich Ngoc", Email = "tutor17@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-15) },
                new() { FirstName = "Luu", LastName = "Van Tai", Email = "tutor18@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-12) },
                new() { FirstName = "Ha", LastName = "Thi My", Email = "tutor19@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-10) },
                new() { FirstName = "Cao", LastName = "Minh Tri", Email = "tutor20@odt.com", Password = hp, RoleId = "TUTOR", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-7) },

                // 22-25 - 4 Learners
                new() { FirstName = "Pham", LastName = "Hoang An", Email = "learner1@odt.com", Password = hp, RoleId = "LEARNER", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-30) },
                new() { FirstName = "Vo", LastName = "Thanh Tam", Email = "learner2@odt.com", Password = hp, RoleId = "LEARNER", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-25) },
                new() { FirstName = "Hoang", LastName = "Minh Thu", Email = "learner3@odt.com", Password = hp, RoleId = "LEARNER", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-20) },
                new() { FirstName = "Tran", LastName = "Khanh Linh", Email = "learner4@odt.com", Password = hp, RoleId = "LEARNER", Status = true, IsEmailVerified = true, CreatedDate = now.AddDays(-15) },
            };
            context.Accounts.AddRange(accounts);
            await context.SaveChangesAsync();
        }

        private static async Task SeedTutors(OnDemandTutorContext context)
        {
            var majors = new[] { "MATH", "ENG", "PHY", "CHEM", "BIO", "CS", "HIS", "GEO" };
            var nationalities = new[] { "Vietnamese", "Vietnamese", "Vietnamese", "Japanese", "Korean", "Vietnamese", "American", "Vietnamese" };
            var descriptions = new[]
            {
                "Experienced Mathematics tutor with 5 years of teaching. Specializes in Algebra and Calculus.",
                "English language specialist with IELTS 8.0. Helps students master all four skills.",
                "Computer Science tutor specializing in algorithms and competitive programming.",
                "Physics expert with PhD. Makes complex concepts simple and fun.",
                "Chemistry tutor with 8 years of experience in organic and inorganic chemistry.",
                "Biology tutor passionate about genetics and molecular biology.",
                "History enthusiast with deep knowledge of Vietnamese and world history.",
                "Geography tutor specializing in physical geography and GIS.",
                "Advanced Mathematics tutor focusing on statistics and probability.",
                "TOEFL and IELTS preparation expert with 10+ years of experience.",
                "Physics tutor specializing in mechanics and thermodynamics.",
                "Chemistry tutor focusing on analytical chemistry and lab techniques.",
                "Full-stack developer turned CS tutor. Expert in web technologies.",
                "Biology tutor with research background in ecology and environment.",
                "English literature and creative writing specialist.",
                "Mathematics competition coach with national award-winning students.",
                "Physics tutor with aerospace engineering background.",
                "Chemistry olympiad coach with multiple gold medalists.",
                "Computer Science tutor specializing in AI and machine learning.",
                "Geography and environmental science tutor with field experience.",
            };

            var tutors = new List<Tutor>();
            for (int i = 0; i < 20; i++)
            {
                int accountId = i + 2; // IDs 2-21
                var status = i < 17 ? CertiStatus.Approved : CertiStatus.Pending; // last 3 pending
                tutors.Add(new Tutor
                {
                    TutorId = accountId,
                    TutorAge = 25 + (i % 15),
                    TutorEmail = $"tutor{i + 1}@odt.com",
                    Nationality = nationalities[i % nationalities.Length],
                    TutorDescription = descriptions[i],
                    TutorPicture = DefaultPicture,
                    CertiStatus = status,
                    MajorId = majors[i % majors.Length],
                });
            }
            context.Tutors.AddRange(tutors);
            await context.SaveChangesAsync();
        }

        private static async Task SeedLearners(OnDemandTutorContext context)
        {
            var memberships = new[] { "FREE", "SILVER", "GOLD", null as string };
            var ages = new[] { 20, 22, 19, 24 };
            var now = DateTime.Now;

            var learners = new List<Learner>();
            for (int i = 0; i < 4; i++)
            {
                int accountId = 22 + i; // IDs 22-25
                learners.Add(new Learner
                {
                    LearnerId = accountId,
                    LearnerAge = ages[i],
                    LearnerEmail = $"learner{i + 1}@odt.com",
                    LearnerPicture = DefaultPicture,
                    MembershipId = memberships[i],
                    MembershipCreatedDate = memberships[i] != null ? now : null,
                });
            }
            context.Learners.AddRange(learners);
            await context.SaveChangesAsync();
        }

        private static async Task SeedWallets(OnDemandTutorContext context)
        {
            var wallets = new List<Wallet>();
            // Admin wallet
            wallets.Add(new Wallet { WalletId = 1, Balance = 0m });
            // 20 tutor wallets (IDs 2-21)
            var rng = new Random(42);
            for (int i = 2; i <= 21; i++)
                wallets.Add(new Wallet { WalletId = i, Balance = rng.Next(100, 2000) * 1000m });
            // 4 learner wallets (IDs 22-25)
            for (int i = 22; i <= 25; i++)
                wallets.Add(new Wallet { WalletId = i, Balance = rng.Next(50, 500) * 1000m });

            context.Wallets.AddRange(wallets);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCurricula(OnDemandTutorContext context)
        {
            // 2 curricula per approved tutor (IDs 2-18, 17 tutors) = 34 curricula
            var currDescriptions = new (string type, string desc, int slots, decimal price)[]
            {
                ("SHORT", "Algebra Fundamentals", 10, 150000m),
                ("LONG",  "Calculus Mastery", 30, 200000m),
                ("SHORT", "IELTS Speaking & Writing", 15, 250000m),
                ("LONG",  "Academic English Advanced", 25, 220000m),
                ("SHORT", "Python Programming Basics", 12, 180000m),
                ("LONG",  "Data Structures & Algorithms", 30, 300000m),
                ("SHORT", "Classical Mechanics", 10, 170000m),
                ("LONG",  "Electromagnetism Deep Dive", 20, 230000m),
                ("SHORT", "Organic Chemistry Essentials", 10, 160000m),
                ("LONG",  "Analytical Chemistry Lab", 24, 280000m),
                ("SHORT", "Genetics & Molecular Bio", 12, 190000m),
                ("LONG",  "Ecology & Environment", 20, 210000m),
                ("SHORT", "Vietnamese History Overview", 8, 120000m),
                ("LONG",  "World War History", 18, 180000m),
                ("SHORT", "Physical Geography", 10, 140000m),
                ("LONG",  "GIS & Remote Sensing", 22, 260000m),
                ("SHORT", "Statistics & Probability", 10, 170000m),
                ("LONG",  "Linear Algebra Complete", 28, 240000m),
                ("SHORT", "TOEFL Preparation", 15, 270000m),
                ("LONG",  "Business English", 20, 230000m),
                ("SHORT", "Thermodynamics Basics", 10, 160000m),
                ("LONG",  "Quantum Physics Intro", 25, 290000m),
                ("SHORT", "Lab Safety & Techniques", 8, 130000m),
                ("LONG",  "Biochemistry Fundamentals", 22, 250000m),
                ("SHORT", "Web Development Full Stack", 15, 280000m),
                ("LONG",  "Machine Learning Foundations", 30, 350000m),
                ("SHORT", "Marine Biology", 10, 180000m),
                ("LONG",  "Biodiversity Conservation", 18, 200000m),
                ("SHORT", "Creative Writing in English", 10, 160000m),
                ("LONG",  "English Literature Analysis", 20, 210000m),
                ("SHORT", "Math Olympiad Training", 12, 300000m),
                ("LONG",  "Advanced Problem Solving", 25, 350000m),
                ("SHORT", "Aerospace Physics", 10, 220000m),
                ("LONG",  "Rocket Science Fundamentals", 20, 280000m),
            };

            var curricula = new List<Curriculum>();
            for (int t = 0; t < 17; t++) // 17 approved tutors
            {
                int tutorId = t + 2;
                int descIdx = (t * 2) % currDescriptions.Length;
                int descIdx2 = (t * 2 + 1) % currDescriptions.Length;

                curricula.Add(new Curriculum
                {
                    CurriculumType = currDescriptions[descIdx].type,
                    TotalSlot = currDescriptions[descIdx].slots,
                    CurriculumStatus = "ACTIVE",
                    CurriculumDescription = currDescriptions[descIdx].desc,
                    PricePerSection = currDescriptions[descIdx].price,
                    TutorId = tutorId,
                });
                curricula.Add(new Curriculum
                {
                    CurriculumType = currDescriptions[descIdx2].type,
                    TotalSlot = currDescriptions[descIdx2].slots,
                    CurriculumStatus = "ACTIVE",
                    CurriculumDescription = currDescriptions[descIdx2].desc,
                    PricePerSection = currDescriptions[descIdx2].price,
                    TutorId = tutorId,
                });
            }
            context.Curricula.AddRange(curricula);
            await context.SaveChangesAsync();
        }

        private static async Task SeedSections(OnDemandTutorContext context)
        {
            var now = DateTime.Now;
            var sections = new List<Section>();
            int curriculumCount = 34; // 17 tutors * 2

            var hours = new[] { 8, 10, 13, 15, 17, 19 };

            for (int cId = 1; cId <= curriculumCount; cId++)
            {
                // 3 sections per curriculum: past, today-ish, future
                int hourIdx = cId % hours.Length;

                // Past completed section
                sections.Add(new Section
                {
                    SectionStart = now.AddDays(-7).Date.AddHours(hours[hourIdx]),
                    SectionEnd = now.AddDays(-7).Date.AddHours(hours[hourIdx] + 2),
                    SectionStatus = "COMPLETED",
                    CurriculumId = cId,
                    MeetUrl = "https://meet.google.com/abc-defg-hij",
                });
                // Upcoming section
                sections.Add(new Section
                {
                    SectionStart = now.AddDays(1 + cId % 5).Date.AddHours(hours[(hourIdx + 1) % hours.Length]),
                    SectionEnd = now.AddDays(1 + cId % 5).Date.AddHours(hours[(hourIdx + 1) % hours.Length] + 2),
                    SectionStatus = "SCHEDULED",
                    CurriculumId = cId,
                    MeetUrl = "none",
                });
                // Another upcoming section
                sections.Add(new Section
                {
                    SectionStart = now.AddDays(5 + cId % 7).Date.AddHours(hours[(hourIdx + 2) % hours.Length]),
                    SectionEnd = now.AddDays(5 + cId % 7).Date.AddHours(hours[(hourIdx + 2) % hours.Length] + 2),
                    SectionStatus = "SCHEDULED",
                    CurriculumId = cId,
                    MeetUrl = "none",
                });
            }
            context.Sections.AddRange(sections);
            await context.SaveChangesAsync();
        }

        private static async Task SeedLearnerOrders(OnDemandTutorContext context)
        {
            var now = DateTime.Now;
            var orders = new List<LearnerOrder>();

            // Each learner books from multiple tutors' curricula
            // Learner 22 (An)   → curricula 1, 5, 9, 17, 25       (Math, CS, Chem, Stats, Web)
            // Learner 23 (Tam)  → curricula 3, 7, 11, 19, 27      (IELTS, Mechanics, Genetics, TOEFL, Marine)
            // Learner 24 (Thu)  → curricula 2, 6, 13, 21, 31      (Calculus, DSA, VN History, Thermo, Olympiad)
            // Learner 25 (Linh) → curricula 4, 10, 15, 23, 29     (Academic Eng, Analytical Chem, Geography, Lab, Creative Writing)

            var learnerCurricula = new Dictionary<int, int[]>
            {
                { 22, new[] { 1, 5, 9, 17, 25 } },
                { 23, new[] { 3, 7, 11, 19, 27 } },
                { 24, new[] { 2, 6, 13, 21, 31 } },
                { 25, new[] { 4, 10, 15, 23, 29 } },
            };

            var prices = new Dictionary<int, decimal>
            {
                {1, 150000m}, {2, 200000m}, {3, 250000m}, {4, 220000m}, {5, 180000m},
                {6, 300000m}, {7, 170000m}, {9, 160000m}, {10, 280000m}, {11, 190000m},
                {13, 120000m}, {15, 140000m}, {17, 170000m}, {19, 270000m}, {21, 160000m},
                {23, 130000m}, {25, 280000m}, {27, 180000m}, {29, 160000m}, {31, 300000m},
            };

            int dayOffset = -20;
            foreach (var (learnerId, curricula) in learnerCurricula)
            {
                for (int i = 0; i < curricula.Length; i++)
                {
                    int cId = curricula[i];
                    bool isCompleted = i < 3; // first 3 completed, last 2 pending
                    decimal price = prices.GetValueOrDefault(cId, 200000m);

                    orders.Add(new LearnerOrder
                    {
                        OrderDate = now.AddDays(dayOffset + i * 3),
                        OrderStatus = isCompleted ? "COMPLETED" : "PENDING",
                        Total = price * (isCompleted ? 10 : 5),
                        CurriculumId = cId,
                        IsCompleted = isCompleted,
                        LearnerId = learnerId,
                    });
                }
            }
            context.LearnerOrders.AddRange(orders);
            await context.SaveChangesAsync();
        }

        private static async Task SeedReviewRatings(OnDemandTutorContext context)
        {
            var now = DateTime.Now;
            var reviews = new List<ReviewRating>();

            // Only completed orders get reviews. Orders 1-3 (learner22), 6-8 (learner23), 11-13 (learner24), 16-18 (learner25)
            // We need tutorId for each curriculum. Curriculum cId → tutorId = ((cId-1)/2) + 2
            var currToTutor = new Dictionary<int, int>
            {
                {1,2},{2,2},{3,3},{4,3},{5,4},{6,4},{7,5},{9,6},{10,6},{11,7},
                {13,8},{15,9},{17,10},{19,11},{21,12},{23,13},{25,14},{27,15},{29,16},{31,17},
            };

            var completedOrders = new (int orderId, int learnerId, int curriculumId)[]
            {
                (1, 22, 1), (2, 22, 5), (3, 22, 9),
                (6, 23, 3), (7, 23, 7), (8, 23, 11),
                (11, 24, 2), (12, 24, 6), (13, 24, 13),
                (16, 25, 4), (17, 25, 10), (18, 25, 15),
            };

            var reviewTexts = new[]
            {
                "Excellent tutor! Very clear explanations and patient.",
                "Great teaching method. I improved a lot.",
                "Very knowledgeable and well-prepared for each session.",
                "Good tutor but sometimes goes too fast.",
                "Amazing experience! Highly recommended.",
                "The tutor made complex topics easy to understand.",
                "Very helpful and always available for questions.",
                "Professional and dedicated. Will book again!",
                "Good content but could improve time management.",
                "One of the best tutors I've ever had!",
                "Clear structure and great practice exercises.",
                "Friendly, patient, and very supportive.",
            };

            var rng = new Random(123);
            for (int i = 0; i < completedOrders.Length; i++)
            {
                var o = completedOrders[i];
                int tutorId = currToTutor.GetValueOrDefault(o.curriculumId, 2);

                reviews.Add(new ReviewRating
                {
                    TutorId = tutorId,
                    LearnerId = o.learnerId,
                    Review = reviewTexts[i],
                    Rating = rng.Next(3, 6), // 3-5 stars
                    ReviewDate = now.AddDays(-5 + i),
                    OrderId = o.orderId,
                });
            }
            context.ReviewRatings.AddRange(reviews);
            await context.SaveChangesAsync();
        }

        private static async Task SeedSTBConditions(OnDemandTutorContext context)
        {
            var now = DateTime.Now;
            // STB conditions for completed orders
            var completedOrderIds = new[] { 1, 2, 3, 6, 7, 8, 11, 12, 13, 16, 17, 18 };
            var conditions = new List<STBCondition>();
            foreach (var oid in completedOrderIds)
            {
                conditions.Add(new STBCondition
                {
                    OrderId = oid,
                    StartTime = now.AddDays(-15 + oid),
                    Duration = 60,
                });
            }
            context.STBConditions.AddRange(conditions);
            await context.SaveChangesAsync();
        }
    }
}
