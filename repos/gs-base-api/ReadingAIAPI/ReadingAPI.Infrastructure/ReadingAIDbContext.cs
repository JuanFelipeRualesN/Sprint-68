using Microsoft.EntityFrameworkCore;
using ReadingAPI.ReadingAPI.Domain.Entities.Reading;
using ReadingAPI.ReadingAPI.Domain.Entities.Story;
using ReadingAPI.ReadingAPI.Domain.Entities.UsageAndEngagement;
using System;
using System.Collections.Generic;

namespace ReadingAPI.ReadingAPI.Infrastructure;

/// <summary>
/// EF Core DbContext for the ReadingAI DB.
/// This context handles the reading module data and was generated using scaffolding.
/// </summary>
public partial class ReadingAIDbContext : DbContext
{
    public ReadingAIDbContext(DbContextOptions<ReadingAIDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserStoryResultsEntity> UserStoryResults { get; set; }
    public virtual DbSet<UserUnmasteredWordEntity> UserUnmasteredWords { get; set; }
    public virtual DbSet<UserStoryProgressEntity> UserStoryProgress { get; set; }
    public virtual DbSet<UserSessionEntity> UserSessions { get; set; }
    public virtual DbSet<UserSessionRawEntity> UserSessionRaw { get; set; }
    public virtual DbSet<UserSessionEventEntity> UserSessionEvents { get; set; }
    public virtual DbSet<StoryLevelEntity> StoryLevels { get; set; }
    public virtual DbSet<StoryCategoryEntity> StoryCategories { get; set; }
    public virtual DbSet<StoryEntity> Stories { get; set; }
    public virtual DbSet<StorySlideEntity> StorySlides { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<UserSessionEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("user_session");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.TotalSessions).HasColumnName("total_sessions");
            entity.Property(e => e.TotalTimeSeconds).HasColumnName("total_time_seconds");
            entity.Property(e => e.OffSchoolPercent).HasColumnName("off_school_percent");
            entity.Property(e => e.OnSchoolPercent).HasColumnName("on_school_percent");
            entity.Property(e => e.Streak).HasColumnName("streak");
            entity.Property(e => e.DesktopPercent).HasColumnName("desktop_percent");
            entity.Property(e => e.MobilePercent).HasColumnName("mobile_percent");
            entity.Property(e => e.TabletPercent).HasColumnName("tablet_percent");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UserSessionRawEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_session_raw");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DeviceType)
                .HasMaxLength(50)
                .HasColumnName("device_type");
            entity.Property(e => e.IsSchoolHours).HasColumnName("is_school_hours");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<UserSessionEventEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_session_events");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.EventType).HasMaxLength(10).HasColumnName("event_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UserStoryResultsEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("user_story_results");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.Language).HasMaxLength(2).HasColumnName("language");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.SlideId).HasColumnName("slide_id");
            entity.Property(e => e.EvaluatedText).HasColumnName("evaluated_text").HasColumnType("text");
            entity.Property(e => e.AccuracyScore).HasColumnName("accuracy_score");
            entity.Property(e => e.FluencyScore).HasColumnName("fluency_score");
            entity.Property(e => e.CompletenessScore).HasColumnName("completeness_score");
            entity.Property(e => e.PronunciationScore).HasColumnName("pronunciation_score");
            entity.Property(e => e.WordsPerMinute).HasColumnName("words_per_minute");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasMany(e => e.UnmasteredWords)
                  .WithOne()
                  .HasForeignKey(w => w.PracticeId);
        });

        modelBuilder.Entity<UserStoryProgressEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("user_story_progress");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Language).HasMaxLength(2).HasColumnName("language");
            entity.Property(e => e.IsLocked).HasColumnName("is_locked");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UserUnmasteredWordEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("user_unmastered_words");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.PracticeId).HasColumnName("practice_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.Word).HasMaxLength(100).HasColumnName("word");
            entity.Property(e => e.AccuracyScore).HasColumnName("accuracy_score");
            entity.Property(e => e.ErrorType).HasMaxLength(50).HasColumnName("error_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StoryLevelEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("story_levels");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
        });

        modelBuilder.Entity<StoryCategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("story_categories");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
        });

        modelBuilder.Entity<StoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("stories");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.LevelId).HasColumnName("level_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.TitleEn).HasMaxLength(255).HasColumnName("title_en");
            entity.Property(e => e.TitleEs).HasMaxLength(255).HasColumnName("title_es");
            entity.Property(e => e.ImageUrl).HasMaxLength(500).HasColumnName("image_url");
            entity.Property(e => e.FluencyGoal).HasColumnName("fluency_goal");
            entity.Property(e => e.AccuracyGoal).HasColumnName("accuracy_goal");
            entity.Property(e => e.CompletenessGoal).HasColumnName("completeness_goal");
            entity.Property(e => e.PronunciationGoal).HasColumnName("pronunciation_goal");
            entity.Property(e => e.WordCountEn).HasColumnName("word_count_en");
            entity.Property(e => e.WordCountEs).HasColumnName("word_count_es");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne<StoryLevelEntity>()
                  .WithMany()
                  .HasForeignKey(e => e.LevelId);

            entity.HasOne<StoryCategoryEntity>()
                  .WithMany()
                  .HasForeignKey(e => e.CategoryId);

            entity.HasMany(e => e.Slides)
                  .WithOne()
                  .HasForeignKey(s => s.StoryId);
        });

        modelBuilder.Entity<StorySlideEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("story_slides");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.SlideOrder).HasColumnName("slide_order");
            entity.Property(e => e.TextEn).HasColumnType("text").HasColumnName("text_en");
            entity.Property(e => e.TextEs).HasColumnType("text").HasColumnName("text_es");
            entity.Property(e => e.ImageUrl).HasMaxLength(500).HasColumnName("image_url");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });
    }
}
