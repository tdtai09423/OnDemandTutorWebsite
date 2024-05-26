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

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<ReviewRating> ReviewRatings { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorCerti> TutorCertis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=Zincerious;Initial Catalog=OnDemandTutor;User ID=sa;Password=12345;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07C9D95BA3");

            entity.ToTable("Account");

            entity.Property(e => e.AccountStatus).HasDefaultValue(true);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasMaxLength(10);
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.HasKey(e => e.CurriculumId).HasName("PK__Curricul__06C9FA1CE7E54128");

            entity.ToTable("Curriculum");

            entity.Property(e => e.CurriculumDesription).HasMaxLength(255);
            entity.Property(e => e.CurriculumStatus).HasMaxLength(10);
            entity.Property(e => e.CurriculumType).HasMaxLength(10);

            entity.HasOne(d => d.Tutor).WithMany(p => p.Curricula)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Curriculu__Tutor__4E88ABD4");
        });

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.ToTable("Learner");

            entity.HasIndex(e => e.LearnerId, "UQ__Learner__67ABFCDBAB069D68").IsUnique();

            entity.Property(e => e.LearnerId).ValueGeneratedNever();
            entity.Property(e => e.LearnerEmail).HasMaxLength(50);
            entity.Property(e => e.MembershipId).HasMaxLength(10);

            entity.HasOne(d => d.LearnerNavigation).WithOne(p => p.Learner)
                .HasForeignKey<Learner>(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Learner_Account");

            entity.HasOne(d => d.Membership).WithMany(p => p.Learners)
                .HasForeignKey(d => d.MembershipId)
                .HasConstraintName("FK__Learner__Members__47DBAE45");
        });

        modelBuilder.Entity<LearnerOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__LearnerO__C3905BCF119EBD06");

            entity.ToTable("LearnerOrder");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus).HasMaxLength(10);
            entity.Property(e => e.OrderType).HasMaxLength(10);

            entity.HasOne(d => d.Curriculum).WithMany(p => p.LearnerOrders)
                .HasForeignKey(d => d.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__LearnerOr__Curri__5441852A");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearnerOrders)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__LearnerOr__Learn__5535A963");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.MajorId).HasName("PK__Major__D5B8BF9167337AF1");

            entity.ToTable("Major");

            entity.HasIndex(e => e.MajorName, "UQ__Major__5FF4A37B5E925CC9").IsUnique();

            entity.Property(e => e.MajorId).HasMaxLength(10);
            entity.Property(e => e.MajorName).HasMaxLength(50);
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A7867993FA2114");

            entity.ToTable("Membership");

            entity.HasIndex(e => e.MembershipDescription, "UQ__Membersh__28D65D232B4D2E5B").IsUnique();

            entity.HasIndex(e => e.MembershipLevel, "UQ__Membersh__D059A621B912AF82").IsUnique();

            entity.Property(e => e.MembershipId).HasMaxLength(10);
            entity.Property(e => e.MembershipDescription).HasMaxLength(255);
            entity.Property(e => e.MembershipLevel).HasMaxLength(50);
        });

        modelBuilder.Entity<ReviewRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ReviewRating");

            entity.Property(e => e.Review).HasMaxLength(255);

            entity.HasOne(d => d.Learner).WithMany()
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__ReviewRat__Learn__4BAC3F29");

            entity.HasOne(d => d.Tutor).WithMany()
                .HasForeignKey(d => d.TutorId)
                .HasConstraintName("FK__ReviewRat__Tutor__4AB81AF0");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK__Section__80EF0872A932A9DB");

            entity.ToTable("Section");

            entity.Property(e => e.SectionEnd).HasColumnType("datetime");
            entity.Property(e => e.SectionStart).HasColumnType("datetime");
            entity.Property(e => e.SectionStatus).HasMaxLength(10);

            entity.HasOne(d => d.Curriculum).WithMany(p => p.Sections)
                .HasForeignKey(d => d.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Section__Curricu__5165187F");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.ToTable("Tutor");

            entity.HasIndex(e => e.TutorId, "UQ__Tutor__77C70FE3F1B03B6B").IsUnique();

            entity.Property(e => e.TutorId).ValueGeneratedNever();
            entity.Property(e => e.CertiStatus)
                .HasMaxLength(20)
                .HasConversion(
                v => v.ToString(),
                v => (CertiStatus)Enum.Parse(typeof(CertiStatus), v));
            entity.Property(e => e.MajorId).HasMaxLength(10);
            entity.Property(e => e.TutorDescription).HasMaxLength(255);
            entity.Property(e => e.TutorEmail).HasMaxLength(50);

            entity.HasOne(d => d.Major).WithMany(p => p.Tutors)
                .HasForeignKey(d => d.MajorId)
                .HasConstraintName("FK__Tutor__MajorId__3D5E1FD2");

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
                .HasConstraintName("FK__TutorCert__Tutor__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
