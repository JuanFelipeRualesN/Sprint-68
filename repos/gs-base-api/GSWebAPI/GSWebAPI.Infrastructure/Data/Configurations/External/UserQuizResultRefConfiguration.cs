using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.External
{
    /// <summary>
    /// Table <c>usqui_user_quiz_results</c>: existing table owned by the DAOs, mapped read-only with signed INT columns.
    /// Never created, altered or dropped by migrations (ExcludeFromMigrations).
    /// </summary>
    public class UserQuizResultRefConfiguration : IEntityTypeConfiguration<UserQuizResultRef>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserQuizResultRef> builder)
        {
            builder.ToTable("usqui_user_quiz_results", t => t.ExcludeFromMigrations());
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnType("int");
            builder.Property(e => e.UserId).HasColumnType("int");
            builder.Property(e => e.QuizId).HasColumnType("int");
            builder.Property(e => e.TryNo).HasColumnType("int");
            builder.Property(e => e.IsExam).HasColumnType("int");
            builder.Property(e => e.LessonId).HasColumnType("int");
            builder.Property(e => e.TotalCorrect).HasColumnType("int");
            builder.Property(e => e.TotalQuestions).HasColumnType("int");
            builder.Property(e => e.TotalPercent).HasColumnType("float");
            builder.Property(e => e.CreatedAt).HasColumnType("timestamp");
        }
    }
}
