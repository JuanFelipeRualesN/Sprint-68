using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Catalogs;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data
{
    /// <summary>
    /// EF Core context for gs_web_api_db (ConnectionStrings:GSWebDBConnection). It runs side by side with the
    /// existing DAOs. Migrations only own the Custom Plans and Community tables; existing tables are mapped with
    /// ExcludeFromMigrations().
    /// </summary>
    public class GSWebDbContext : DbContext
    {
        /// <summary>
        /// Creates the context. Cascade deletes and orphan deletes are delayed until SaveChanges, so
        /// <see cref="Interceptors.AuditAndSoftDeleteInterceptor"/> can turn soft-deletable removals into updates first.
        /// </summary>
        /// <param name="options">Provider options, built with <see cref="GSWebDbContextOptions.Configure"/>.</param>
        public GSWebDbContext(DbContextOptions<GSWebDbContext> options)
            : base(options)
        {
            // Let the soft-delete interceptor see Remove() before EF cascades it to tracked children.
            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
        }

        // Catalogs
        /// <summary>Table <c>custom_plan_types</c>.</summary>
        public DbSet<CustomPlanType> CustomPlanTypes => Set<CustomPlanType>();

        /// <summary>Table <c>custom_plan_type_translations</c>.</summary>
        public DbSet<CustomPlanTypeTranslation> CustomPlanTypeTranslations => Set<CustomPlanTypeTranslation>();

        /// <summary>Table <c>curriculum_frameworks</c>.</summary>
        public DbSet<CurriculumFramework> CurriculumFrameworks => Set<CurriculumFramework>();

        /// <summary>Table <c>meeting_formats</c>.</summary>
        public DbSet<MeetingFormat> MeetingFormats => Set<MeetingFormat>();

        // Custom Plans
        /// <summary>Table <c>custom_plan_client</c>.</summary>
        public DbSet<CustomPlanClient> CustomPlanClients => Set<CustomPlanClient>();

        /// <summary>Table <c>custom_plan_client_subjects</c>.</summary>
        public DbSet<CustomPlanClientSubject> CustomPlanClientSubjects => Set<CustomPlanClientSubject>();

        /// <summary>Table <c>custom_plan_client_days</c>.</summary>
        public DbSet<CustomPlanClientDay> CustomPlanClientDays => Set<CustomPlanClientDay>();

        /// <summary>Table <c>custom_plan_client_day_times</c>.</summary>
        public DbSet<CustomPlanClientDayTime> CustomPlanClientDayTimes => Set<CustomPlanClientDayTime>();

        /// <summary>Table <c>custom_plan_client_lessons</c>.</summary>
        public DbSet<CustomPlanClientLesson> CustomPlanClientLessons => Set<CustomPlanClientLesson>();

        /// <summary>Table <c>custom_plan_client_lesson_dates</c>.</summary>
        public DbSet<CustomPlanClientLessonDate> CustomPlanClientLessonDates => Set<CustomPlanClientLessonDate>();

        /// <summary>Table <c>custom_plan_client_lesson_exam_dates</c>.</summary>
        public DbSet<CustomPlanClientLessonExamDate> CustomPlanClientLessonExamDates => Set<CustomPlanClientLessonExamDate>();

        /// <summary>Table <c>custom_plan_client_lesson_practices</c>.</summary>
        public DbSet<CustomPlanClientLessonPractice> CustomPlanClientLessonPractices => Set<CustomPlanClientLessonPractice>();

        /// <summary>Table <c>custom_plan_client_lesson_tests</c>.</summary>
        public DbSet<CustomPlanClientLessonTest> CustomPlanClientLessonTests => Set<CustomPlanClientLessonTest>();

        /// <summary>Table <c>custom_plan_evidences</c>.</summary>
        public DbSet<CustomPlanEvidence> CustomPlanEvidences => Set<CustomPlanEvidence>();

        // Community
        /// <summary>Table <c>community_groups</c>.</summary>
        public DbSet<CommunityGroup> CommunityGroups => Set<CommunityGroup>();

        /// <summary>Table <c>community_members</c>.</summary>
        public DbSet<CommunityMember> CommunityMembers => Set<CommunityMember>();

        /// <summary>Table <c>community_forum_categories</c>.</summary>
        public DbSet<CommunityForumCategory> CommunityForumCategories => Set<CommunityForumCategory>();

        /// <summary>Table <c>community_forum_category_translations</c>.</summary>
        public DbSet<CommunityForumCategoryTranslation> CommunityForumCategoryTranslations => Set<CommunityForumCategoryTranslation>();

        /// <summary>Table <c>community_forums</c>.</summary>
        public DbSet<CommunityForum> CommunityForums => Set<CommunityForum>();

        /// <summary>Table <c>community_forum_posts</c>.</summary>
        public DbSet<CommunityForumPost> CommunityForumPosts => Set<CommunityForumPost>();

        /// <summary>Table <c>community_events</c>.</summary>
        public DbSet<CommunityEvent> CommunityEvents => Set<CommunityEvent>();

        /// <summary>Table <c>community_events_interests</c>.</summary>
        public DbSet<CommunityEventInterest> CommunityEventInterests => Set<CommunityEventInterest>();

        /// <summary>Table <c>community_resources</c>.</summary>
        public DbSet<CommunityResource> CommunityResources => Set<CommunityResource>();

        // Existing tables (read-only, excluded from migrations)
        /// <summary>Existing table <c>usqui_user_quiz_results</c> (read-only, excluded from migrations).</summary>
        public DbSet<UserQuizResultRef> UserQuizResults => Set<UserQuizResultRef>();

        /// <summary>
        /// Sets utf8mb4 / utf8mb4_0900_ai_ci at table level, applies every configuration in the
        /// <c>Data.Configurations</c> namespaces and then gives all columns snake_case names.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table-level only: never emit ALTER DATABASE for the existing schema.
            modelBuilder.HasCharSet("utf8mb4", DelegationModes.ApplyToTables);
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci", DelegationModes.ApplyToTables);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(GSWebDbContext).Assembly,
                type => type.Namespace != null && type.Namespace.StartsWith(typeof(GSWebDbContext).Namespace + ".Configurations", StringComparison.Ordinal));

            SnakeCaseNames.Apply(modelBuilder);
        }

        /// <summary>Stores every <see cref="DateTime"/> and nullable <see cref="DateTime"/> property as UTC through <see cref="UtcDateTimeConverter"/>.</summary>
        /// <param name="configurationBuilder">The convention builder.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
            configurationBuilder.Properties<DateTime?>().HaveConversion<UtcDateTimeConverter>();
        }
    }
}
