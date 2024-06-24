using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ODTDemoAPI.Entities;

public partial class OnDemandTutorContext : DbContext
{
    public OnDemandTutorContext()
    {
    }

    public OnDemandTutorContext(DbContextOptions<OnDemandTutorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Curriculum> Curricula { get; set; }

    public virtual DbSet<Learner> Learners { get; set; }

    public virtual DbSet<LearnerOrder> LearnerOrders { get; set; }

    public virtual DbSet<LearnerFavourite> LearnerFavourites { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<ReviewRating> ReviewRatings { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<STBCondition> STBConditions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorCerti> TutorCertis { get; set; }

    public virtual DbSet<UserNotification> UserNotifications { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("OnDemandTutor"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC0761FF9DFD");

            entity.ToTable("Account");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(60);
            entity.Property(e => e.RoleId).HasMaxLength(10);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.HasKey(e => e.CurriculumId).HasName("PK__Curricul__06C9FA1C9F0F3487");

            entity.ToTable("Curriculum");

            entity.Property(e => e.CurriculumDescription).HasMaxLength(255);
            entity.Property(e => e.CurriculumStatus).HasMaxLength(10);
            entity.Property(e => e.CurriculumType).HasMaxLength(10);
            entity.Property(e => e.PricePerSection).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Tutor).WithMany(p => p.Curricula)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Curriculu__Tutor__5DCAEF64");
        });

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.ToTable("Learner");

            entity.HasIndex(e => e.LearnerId, "UQ__Learner__67ABFCDBC39A9A47").IsUnique();

            entity.Property(e => e.LearnerId).ValueGeneratedNever();
            entity.Property(e => e.LearnerEmail).HasMaxLength(50);
            entity.Property(e => e.LearnerPicture).HasMaxLength(255);
            entity.Property(e => e.MembershipId).HasMaxLength(10);

            entity.HasOne(d => d.LearnerNavigation).WithOne(p => p.Learner)
                .HasForeignKey<Learner>(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Learner_Account");

            entity.HasOne(d => d.Membership).WithMany(p => p.Learners)
                .HasForeignKey(d => d.MembershipId)
                .HasConstraintName("FK__Learner__Members__5EBF139D");
        });

        modelBuilder.Entity<LearnerOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__LearnerO__C3905BCF6D0D7565");

            entity.ToTable("LearnerOrder");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus).HasMaxLength(10);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Curriculum).WithMany(p => p.LearnerOrders)
                .HasForeignKey(d => d.CurriculumId)
                .HasConstraintName("FK__LearnerOr__Curri__628FA481");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearnerOrders)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__LearnerOr__Learn__6383C8BA");
        });

        modelBuilder.Entity<LearnerFavourite>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LearnerFavourite");

            entity.HasIndex(e => new { e.LearnerId, e.TutorId }, "UQ_Favourite").IsUnique();

            entity.HasOne(d => d.Learner).WithMany()
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Learner_Favourite");

            entity.HasOne(d => d.Tutor).WithMany()
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Learner_Favorite");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.MajorId).HasName("PK__Major__D5B8BF91462533E5");

            entity.ToTable("Major");

            entity.HasIndex(e => e.MajorName, "UQ__Major__5FF4A37B705A71D0").IsUnique();

            entity.Property(e => e.MajorId).HasMaxLength(10);
            entity.Property(e => e.MajorName).HasMaxLength(50);
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A786791E02A90D");

            entity.ToTable("Membership");

            entity.HasIndex(e => e.MembershipDescription, "UQ__Membersh__28D65D23C30059DE").IsUnique();

            entity.HasIndex(e => e.MembershipLevel, "UQ__Membersh__D059A621B60DB827").IsUnique();

            entity.Property(e => e.MembershipId).HasMaxLength(10);
            entity.Property(e => e.MembershipDescription).HasMaxLength(255);
            entity.Property(e => e.MembershipLevel).HasMaxLength(50);
        });

        modelBuilder.Entity<ReviewRating>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK_Review_Rating");

            entity.ToTable("ReviewRating");

            entity.HasIndex(e => new { e.TutorId, e.LearnerId }, "UQ_RevewRating").IsUnique();

            entity.Property(e => e.Review).HasMaxLength(255);
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Learner).WithMany()
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__ReviewRat__Learn__6477ECF3");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("KF_Order_Review");

            entity.HasOne(d => d.Tutor).WithMany()
                .HasForeignKey(d => d.TutorId)
                .HasConstraintName("FK__ReviewRat__Tutor__656C112C");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK__Section__80EF0872D14657AC");

            entity.ToTable("Section");

            entity.Property(e => e.MeetUrl)
                .HasMaxLength(300)
                .HasDefaultValue("none")
                .HasColumnName("MeetURL");
            entity.Property(e => e.SectionEnd).HasColumnType("datetime");
            entity.Property(e => e.SectionStart).HasColumnType("datetime");
            entity.Property(e => e.SectionStatus).HasMaxLength(20);

            entity.HasOne(d => d.Curriculum).WithMany(p => p.Sections)
                .HasForeignKey(d => d.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Section__Curricu__66603565");
        });

        modelBuilder.Entity<STBCondition>(entity =>
        {
            entity.HasKey(e => e.CID).HasName("PK_STB_Condition");

            entity.ToTable("STBCondition");

            entity.Property(e => e.CID).HasColumnName("CID");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B0278AFCF");

            entity.ToTable("Transaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Accou__4CA06362");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.ToTable("Tutor");

            entity.HasIndex(e => e.TutorId, "UQ__Tutor__77C70FE35771873E").IsUnique();

            entity.Property(e => e.TutorId).ValueGeneratedNever();
            entity.Property(e => e.CertiStatus)
                .HasMaxLength(20)
                .HasConversion
                (
                    v => v.ToString(),
                    v => (CertiStatus)Enum.Parse(typeof(CertiStatus), v)
                );
            entity.Property(e => e.MajorId).HasMaxLength(10);
            entity.Property(e => e.Nationality).HasMaxLength(50);
            entity.Property(e => e.TutorDescription).HasMaxLength(600);
            entity.Property(e => e.TutorEmail).HasMaxLength(50);
            entity.Property(e => e.TutorPicture).HasMaxLength(255);

            entity.HasOne(d => d.Major).WithMany(p => p.Tutors)
                .HasForeignKey(d => d.MajorId)
                .HasConstraintName("FK_Major");

            entity.HasOne(d => d.TutorNavigation).WithOne(p => p.Tutor)
                .HasForeignKey<Tutor>(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tutor_Account");
        });

        modelBuilder.Entity<TutorCerti>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TutorCerti");

            entity.Property(e => e.TutorCertificate).HasMaxLength(50);

            entity.HasOne(d => d.Tutor).WithMany()
                .HasForeignKey(d => d.TutorId)
                .HasConstraintName("FK__TutorCert__Tutor__693CA210");
        });

        modelBuilder.Entity<UserNotification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__UserNoti__20CF2E12BE235002");

            entity.ToTable("UserNotification");

            entity.Property(e => e.Content).HasMaxLength(300);
            entity.Property(e => e.NotificateDay).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.UserNotifications)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificate_Account");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK_UserWallet");

            entity.ToTable("Wallet");

            entity.HasIndex(e => e.WalletId, "UQ__Wallet__84D4F90F9A3D95CE").IsUnique();

            entity.Property(e => e.WalletId).ValueGeneratedNever();
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)").HasDefaultValue(0);

            entity.HasOne(d => d.WalletNavigation).WithOne(p => p.Wallet)
                .HasForeignKey<Wallet>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wallet_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
