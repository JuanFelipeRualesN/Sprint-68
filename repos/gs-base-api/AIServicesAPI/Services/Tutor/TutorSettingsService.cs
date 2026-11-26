using GSAIServicesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// Reads and writes the AI tutor routing configuration (which grades use the
    /// RAG engine). The grade list is cached in memory to avoid hitting the
    /// ai_tutor_settings table on every tutor question. 
    /// the cache is invalidated when the list is changed.
    /// </summary>
    public class TutorSettingsService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;

        private volatile HashSet<string>? _cachedRagGrades;
        private readonly object _lock = new();

        public TutorSettingsService(IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _config = config;
        }

        /// <summary>
        /// Returns true when the given lesson grade should be routed to the RAG engine.
        /// </summary>
        public async Task<bool> IsRagGradeAsync(string? grade)
        {
            var normalized = Normalize(grade);
            if (normalized.Length == 0) return true; // unknown grade fall back to safest (RAG)

            var set = await GetRagGradesAsync();
            return set.Contains(normalized);
        }

        /// <summary>
        /// Returns the current RAG grades as the raw csv string.
        /// </summary>
        public async Task<string> GetRagGradesRawAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var row = await db.AiTutorSettings.FirstOrDefaultAsync(s => s.Id == AiTutorSetting.SingletonId);

            return row?.RagGrades ?? DefaultRagGrades();
        }

        /// <summary>
        /// Persists a new RAG grades configuration and invalidates the in-memory cache so the next request reflects the change.
        /// </summary>
        public async Task SaveRagGradesAsync(string ragGrades)
        {
            // Re-normalize the incoming list so storage stays clean and consistent.
            var cleaned = string.Join(",", SplitGrades(ragGrades));

            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var row = await db.AiTutorSettings
                    .FirstOrDefaultAsync(s => s.Id == AiTutorSetting.SingletonId);

                var now = DateTime.UtcNow;

                if (row is null)
                {
                    row = new AiTutorSetting
                    {
                        Id = AiTutorSetting.SingletonId,
                        CreatedAt = now
                    };
                    db.AiTutorSettings.Add(row);
                }

                row.RagGrades = cleaned;
                row.UpdatedAt = now;
                await db.SaveChangesAsync();
            }

            // Drop the cache, next read rebuilds it from the stored value.
            lock (_lock)
            {
                _cachedRagGrades = null;
            }
        }

        /// <summary>
        /// Loads and caches the parsed set of RAG grades.
        /// </summary>
        private async Task<HashSet<string>> GetRagGradesAsync()
        {
            var cached = _cachedRagGrades;
            if (cached is not null) return cached;

            var raw = await GetRagGradesRawAsync();
            var set = new HashSet<string>(SplitGrades(raw), StringComparer.OrdinalIgnoreCase);

            lock (_lock)
            {
                _cachedRagGrades = set;
            }

            return set;
        }

        /// <summary>
        /// Splits and normalizes a csv grade list, dropping blanks.
        /// </summary>
        private static IEnumerable<string> SplitGrades(string? csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) yield break;

            foreach (var part in csv.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var normalized = Normalize(part);
                if (normalized.Length > 0) yield return normalized;
            }
        }

        /// <summary>
        /// Returns the default RAG grades.
        /// </summary>
        private string DefaultRagGrades() => _config["AppSettings:default_rag_grades"] ?? "k,1,2,3,4,5,6,7";

        private static string Normalize(string? grade) =>
            (grade ?? string.Empty).Trim().ToLowerInvariant();
    }
}
