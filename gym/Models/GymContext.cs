using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace gym.Models;

public partial class GymContext : DbContext
{
    public GymContext()
    {
    }

    public GymContext(DbContextOptions<GymContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Gallery> Galleries { get; set; }

    public virtual DbSet<Healthinfo> Healthinfos { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Pt> Pts { get; set; }

    public virtual DbSet<Rankuser> Rankusers { get; set; }

    public virtual DbSet<Schdule> Schdules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserClass> UserClasses { get; set; }
    public virtual DbSet<UserSchedule> UserSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;database=gym;user=root;password=000123duy;treattinyasboolean=true", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.ToTable("accounts");

            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.AcceptTerms)
                .HasDefaultValueSql("'0'")
                .HasColumnName("acceptTerms");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.EmailConfirmationToken)
                .HasMaxLength(255)
                .HasColumnName("emailConfirmationToken")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.IsEmailConfirmed)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IsEmailConfirmed");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("passwordHash")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phoneNumber")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ResetToken)
                .HasMaxLength(255)
                .HasColumnName("resetToken")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Roles)
                .HasMaxLength(10)
                .HasColumnName("Roles")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ResetTokenExpires)
                .HasColumnType("datetime")
                .HasColumnName("resetTokenExpires");
            entity.Property(e => e.RoleAdmin)
                .HasDefaultValueSql("'0'")
                .HasColumnName("roleAdmin");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PRIMARY");

            entity.ToTable("classes");

            entity.Property(e => e.ClassId).HasColumnName("classID");
            entity.Property(e => e.CountUser)
                .HasDefaultValueSql("'0'")
                .HasColumnName("countUser");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Descriptions)
                .HasColumnType("text")
                .HasColumnName("descriptions");
            entity.Property(e => e.Detail).HasColumnName("detail");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasColumnName("img")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NameClass)
                .HasMaxLength(255)
                .HasColumnName("nameClass")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Price)
                .HasPrecision(15, 2)
                .HasColumnName("price");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PRIMARY");

            entity.ToTable("contact");

            entity.HasIndex(e => e.UserId, "userID");

            entity.Property(e => e.ContactId).HasColumnName("contactID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Details)
                .HasColumnType("text")
                .HasColumnName("details");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phoneNumber")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("contact_ibfk_1");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PRIMARY");

            entity.ToTable("course");

            entity.HasIndex(e => e.UserId, "userID");

            entity.Property(e => e.CourseId).HasColumnName("courseID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Details)
                .HasColumnType("text")
                .HasColumnName("details");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NameCourse)
                .HasMaxLength(255)
                .HasColumnName("nameCourse")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Tiltle)
                .HasMaxLength(255)
                .HasColumnName("tiltle")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Courses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("course_ibfk_1");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PRIMARY");

            entity.ToTable("feedback");

            entity.HasIndex(e => e.UserId, "userID");
            entity.HasIndex(e => e.PtId, "ptID");
            entity.HasIndex(e => e.NewsId, "newsID");
            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.PtId).HasColumnName("ptID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Details)
                .HasColumnType("text")
                .HasColumnName("details");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.NewsId).HasColumnName("newsID");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("feedback_ibfk_1");

            entity.HasOne(d => d.PT).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.PtId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("feedback_ibfk_2");

            entity.HasOne(d => d.News).WithMany(p => p.Feedbacks)
               .HasForeignKey(d => d.NewsId)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("feedback_ibfk_3");


        });

        modelBuilder.Entity<Gallery>(entity =>
        {
            entity.HasKey(e => e.GalleryId).HasName("PRIMARY");

            entity.ToTable("gallery");

            entity.HasIndex(e => e.UserId, "userID");

            entity.Property(e => e.GalleryId).HasColumnName("galleryID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LinkImg)
                .HasMaxLength(255)
                .HasColumnName("linkImg")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Galleries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("gallery_ibfk_1");
        });

        modelBuilder.Entity<Healthinfo>(entity =>
        {
            entity.HasKey(e => e.HealthId).HasName("PRIMARY");

            entity.ToTable("healthinfo");

            entity.HasIndex(e => e.UserId, "userID");

            entity.Property(e => e.HealthId).HasColumnName("healthID");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Bmi)
                .HasPrecision(5, 2)
                .HasColumnName("BMI");
            entity.Property(e => e.BodyFatPercentage).HasPrecision(5, 2);
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Height).HasPrecision(5, 2);
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Weight).HasPrecision(5, 2);

            entity.HasOne(d => d.User).WithMany(p => p.Healthinfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("healthinfo_ibfk_1");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PRIMARY");

            entity.ToTable("menu");

            entity.Property(e => e.MenuId).HasColumnName("menuID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MenuName)
                .HasMaxLength(255)
                .HasColumnName("menuName")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PRIMARY");

            entity.ToTable("news");

            entity.Property(e => e.NewsId).HasColumnName("newsID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Descriptions)
                .HasColumnType("text")
                .HasColumnName("descriptions");
            entity.Property(e => e.Script)
                .HasColumnType("text")
                .HasColumnName("script");
            entity.Property(e => e.Detail).HasColumnName("detail");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasColumnName("img")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.subMeta)
                .HasMaxLength(255)
                .HasColumnName("subMeta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NewsName)
                .HasMaxLength(255)
                .HasColumnName("newsName")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.IsLike)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isLike");
            entity.Property(e => e.Slogan)
                .HasMaxLength(255)
                .HasColumnName("slogan")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title")
                .UseCollation("utf8mb3_general_ci");
        });

        modelBuilder.Entity<Pt>(entity =>
        {
            entity.HasKey(e => e.PtId).HasName("PRIMARY");

            entity.ToTable("pt");

            entity.HasIndex(e => e.ClassId, "classID");

            entity.Property(e => e.PtId).HasColumnName("ptID");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.ClassId).HasColumnName("classID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Descriptions)
                .HasColumnType("text")
                .HasColumnName("descriptions");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ExperienceYears).HasColumnName("experienceYears");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Height)
                .HasMaxLength(50)
                .HasColumnName("height")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasColumnName("img")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NamePt)
                .HasMaxLength(255)
                .HasColumnName("namePT")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phoneNumber")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Rating)
                .HasDefaultValueSql("'5'")
                .HasColumnName("rating");
            entity.Property(e => e.Skills)
                .HasMaxLength(100)
                .HasColumnName("skills")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Slogan)
                .HasMaxLength(255)
                .HasColumnName("slogan")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Specialty)
                .HasMaxLength(255)
                .HasColumnName("specialty")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Weight)
                .HasMaxLength(50)
                .HasColumnName("weight")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.Class).WithMany(p => p.Pts)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("pt_ibfk_1");

        });

        modelBuilder.Entity<Rankuser>(entity =>
        {
            entity.HasKey(e => e.RankId).HasName("PRIMARY");

            entity.ToTable("rankuser");

            entity.Property(e => e.RankId).HasColumnName("rankID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MembershipType)
                .HasMaxLength(50)
                .HasColumnName("membershipType");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Price)
                .HasPrecision(15, 2)
                .HasColumnName("price");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
        });

        modelBuilder.Entity<Schdule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PRIMARY");

            entity.ToTable("schdule");

            entity.HasIndex(e => e.ClassId, "classID");

            entity.HasIndex(e => e.PtId, "ptID");

            entity.Property(e => e.ScheduleId).HasColumnName("scheduleID");
            entity.Property(e => e.ClassId).HasColumnName("classID");
            entity.Property(e => e.DateCreate).HasColumnName("dateCreate");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.DayOfWeeks)
                .HasColumnType("enum('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday')")
                .HasColumnName("dayOfWeeks");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NameClass)
                .HasMaxLength(255)
                .HasColumnName("nameClass")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NamePt)
                .HasMaxLength(255)
                .HasColumnName("namePT")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.PtId).HasColumnName("ptID");
            entity.Property(e => e.TimeSlot)
                .HasColumnType("enum('06:00 - 08:00','10:00 - 12:00','1:00 - 3:00','5:00 - 7:00','7:00-9:00')")
                .HasColumnName("timeSlot");

            entity.HasOne(d => d.Class).WithMany(p => p.Schdules)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("schdule_ibfk_1");

            entity.HasOne(d => d.Pt).WithMany(p => p.Schdules)
                .HasForeignKey(d => d.PtId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("schdule_ibfk_2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.AccountId, "accountID");

            entity.HasIndex(e => e.MenuId, "menuID");

            entity.HasIndex(e => e.RankId, "rankID");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.Addess)
                .HasMaxLength(1000)
                .HasColumnName("addess")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CountClass)
                .HasDefaultValueSql("'0'")
                .HasColumnName("countClass");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("fullName")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasColumnName("img")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MenuId).HasColumnName("menuID");
            entity.Property(e => e.RankId).HasColumnName("rankID");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phoneNumber")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Price)
                .HasPrecision(15, 2)
                .HasColumnName("price");
            entity.Property(e => e.RankPrice)
                .HasPrecision(15, 2)
                .HasColumnName("rankPrice");
            entity.Property(e => e.Roles)
                .HasMaxLength(10)
                .HasColumnName("Roles")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.HasOne(d => d.Account).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("users_ibfk_1");

            entity.HasOne(d => d.Menu).WithMany(p => p.Users)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("users_ibfk_2");

            entity.HasOne(d => d.Rankuser).WithMany(p => p.Users)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("users_ibfk_3");
            
        });

        modelBuilder.Entity<UserClass>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ClassId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("user_class");

            entity.HasIndex(e => e.ClassId, "classID");
            entity.HasIndex(e => e.UserId, "userID");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.ClassId).HasColumnName("classID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");

            entity.HasOne(d => d.Class).WithMany(p => p.UserClasses)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("user_class_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.UserClasses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_class_ibfk_1");
        });

        modelBuilder.Entity<UserSchedule>(entity =>
        {
            entity.HasKey(e => e.UserScheduleId).HasName("PRIMARY");

            entity.ToTable("userschedule");

            entity.HasIndex(e => e.UserId, "userID");
            entity.HasIndex(e => e.ScheduleId, "scheduleID");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleID");
            entity.Property(e => e.Datebegin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datebegin");
            entity.Property(e => e.Hide)
                .HasDefaultValueSql("'1'")
                .HasColumnName("hide");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .HasColumnName("link")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Meta)
                .HasMaxLength(255)
                .HasColumnName("meta")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Order).HasColumnName("order");

            entity.HasOne(d => d.User).WithMany(p => p.UserSchedules)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserSchedule_ibfk_1");

            entity.HasOne(d => d.Schdule).WithMany(p => p.UserSchedules)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("UserSchedule_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
