using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Newtonsoft.Json;
using ReadingAPI.ReadingAPI.Domain.Entities.Reading;
using ReadingAPI.ReadingAPI.Infrastructure;

namespace ReadingAPI.ReadingAPI.Services.Story
{
    public class UserStoryResultsService
    {
        // placeholder threshold
        private const double UnmasteredWordsThreshold = 80.0;

        private readonly ReadingAIDbContext _dbContext;
        private readonly ILogger<UserStoryResultsService> _logger;

        public UserStoryResultsService(ReadingAIDbContext dbContext, ILogger<UserStoryResultsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Saves a student's story results for a single slide, including scores,
        /// unmastered words, and story progress.
        /// </summary>
        /// <param name="userId">The student's id</param>
        /// <param name="storyId">The story the practice belongs to</param>
        /// <param name="slideId">The slide the student was reading</param>
        /// <param name="langCode">The language code (en, es)</param>
        /// <param name="result">The response returned by the speech service</param>
        public async Task SaveUserStoryResults(int userId, int storyId, int slideId, string langCode, AssessmentResultDto result)
        {
            try
            {
                var (schoolId, regionId) = await GetSchoolAndRegion(userId);
                var wordsPerMinute = CalculateWordsPerMinute(result.RawNBestJson);

                // Get evaluated slide
                var slide = await _dbContext.StorySlides.FirstOrDefaultAsync(slide => slide.Id == slideId && slide.StoryId == storyId);

                if (slide == null)
                {
                    throw new ArgumentException($"Slide {slideId} not found for story {storyId}.");
                }

                // Store a snapshot of the evaluated text in the selected language
                var evaluatedText = string.Equals(langCode, "es", StringComparison.OrdinalIgnoreCase) ? slide.TextEs : slide.TextEn;

                // Extract words whose pronunciation score is below the threshold
                var unmasteredWords = result.Issues
                    .Where(word => word.AccuracyScore < UnmasteredWordsThreshold)
                    .Select(word => new UserUnmasteredWordEntity
                    {
                        UserId = userId,
                        StoryId = storyId,
                        Word = word.Word,
                        AccuracyScore = word.AccuracyScore,
                        ErrorType = word.ErrorType ?? "None"
                    })
                    .ToList();

                var studentStoryResult = new UserStoryResultsEntity
                {
                    UserId = userId,
                    SchoolId = schoolId,
                    RegionId = regionId,
                    Language = langCode,
                    StoryId = storyId,
                    SlideId = slideId,
                    EvaluatedText = evaluatedText,
                    AccuracyScore = result.Overall.Accuracy,
                    FluencyScore = result.Overall.Fluency,
                    CompletenessScore = result.Overall.Completeness,
                    PronunciationScore = result.Overall.Pronunciation,
                    WordsPerMinute = wordsPerMinute,
                    UnmasteredWords = unmasteredWords
                };

                _dbContext.UserStoryResults.Add(studentStoryResult);

                var currentScores = new SlideScores(
                    result.Overall.Accuracy,
                    result.Overall.Fluency,
                    result.Overall.Completeness,
                    result.Overall.Pronunciation);

                // Check if there's existing progress for this story and update or create accordingly
                var existingProgress = await _dbContext.UserStoryProgress
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.StoryId == storyId && p.Language == langCode);

                if (existingProgress == null)
                {
                    await CreateUserStoryProgress(userId, storyId, slideId, langCode, currentScores);
                }
                else
                {
                    await UpdateUserStoryProgress(existingProgress, userId, storyId, slideId, langCode, currentScores);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving story results for user {UserId}, story {StoryId}, slide {SlideId}", userId, storyId, slideId);
                throw;
            }
        }

        /// <summary>
        /// Create a student's story progress.
        /// </summary>
        /// <param name="userId">The student's user id</param>
        /// <param name="storyId">The story being practiced</param>
        /// <param name="currentSlideId">The slide of the assessment</param>
        /// <param name="langCode">The language code (en, es)</param>
        /// <param name="currentScores">The assessment scores for the slide just evaluated</param>
        private async Task CreateUserStoryProgress(int userId, int storyId, int currentSlideId, string langCode, SlideScores currentScores)
        {
            var statusId = await CalculateStoryStatus(userId, storyId, currentSlideId, langCode, currentScores);
            if (statusId == null) return;

            _dbContext.UserStoryProgress.Add(new UserStoryProgressEntity
            {
                UserId = userId,
                StoryId = storyId,
                StatusId = statusId.Value,
                Language = langCode,
                IsLocked = false
            });
        }

        /// <summary>
        /// Updates the status of an existing story progress record.
        /// </summary>
        /// <param name="existingProgress">The progress record to update</param>
        /// <param name="userId">The student's user id</param>
        /// <param name="storyId">The story being practiced</param>
        /// <param name="currentSlideId">The slide of the assessment</param>
        /// <param name="langCode">The language code (en, es)</param>
        /// <param name="currentScores">The assessment scores for the slide just evaluated</param>
        private async Task UpdateUserStoryProgress(UserStoryProgressEntity existingProgress, int userId, int storyId, int currentSlideId, string langCode, SlideScores currentScores)
        {
            var statusId = await CalculateStoryStatus(userId, storyId, currentSlideId, langCode, currentScores);
            if (statusId == null) return;

            existingProgress.StatusId = statusId.Value;
        }

        /// <summary>
        /// Determines whether a story is Completed for a student, considering the latest attempt on every slide.
        /// A slide counts as mastered only when its accuracy, fluency, completeness and pronunciation scores all meet the story's respective goals.
        /// </summary>
        /// <param name="userId">The student's user id</param>
        /// <param name="storyId">The story being practiced</param>
        /// <param name="currentSlideId">The slide of the assessment</param>
        /// <param name="langCode">The language code (en, es)</param>
        /// <param name="currentScores">The assessment scores for the slide just evaluated</param>
        /// <returns>The resulting status, or null if the story has no slides and progress should not change</returns>
        private async Task<StoryProgressStatus?> CalculateStoryStatus(int userId, int storyId, int currentSlideId, string langCode, SlideScores currentScores)
        {
            var story = await _dbContext.Stories
                .Include(story => story.Slides)
                .FirstOrDefaultAsync(story => story.Id == storyId);
            if (story == null || story.Slides.Count == 0) return null;

            var practices = await _dbContext.UserStoryResults
                .Where(practice => practice.UserId == userId && practice.StoryId == storyId && practice.Language == langCode)
                .Select(practice => new
                {
                    practice.Id,
                    practice.SlideId,
                    practice.AccuracyScore,
                    practice.FluencyScore,
                    practice.CompletenessScore,
                    practice.PronunciationScore
                })
                .ToListAsync();

            // Keep the latest attempt per slide
            var latestScoresPerSlide = practices
                .GroupBy(practice => practice.SlideId)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var latest = g.OrderByDescending(practice => practice.Id).First();
                        return new SlideScores(latest.AccuracyScore, latest.FluencyScore, latest.CompletenessScore, latest.PronunciationScore);
                    });

            latestScoresPerSlide[currentSlideId] = currentScores;

            var allSlidesMastered = story.Slides.All(slide =>
                latestScoresPerSlide.TryGetValue(slide.Id, out var scores)
                && scores.Accuracy >= story.AccuracyGoal
                && scores.Fluency >= story.FluencyGoal
                && scores.Completeness >= story.CompletenessGoal
                && scores.Pronunciation >= story.PronunciationGoal);

            return allSlidesMastered ? StoryProgressStatus.Completed : StoryProgressStatus.InProgress;
        }

        /// <summary>
        /// Calculate words per minute based on Azure Speech Service response
        /// </summary>
        /// <param name="rawJSON">The raw JSON from the Speech Service</param>
        /// <returns>The words per minute</returns>
        private static int CalculateWordsPerMinute(string? rawJSON)
        {
            if (string.IsNullOrWhiteSpace(rawJSON)) return 0;

            var parsed = JsonConvert.DeserializeObject<AzureSttEnvelope>(rawJSON);
            var words = parsed?.NBest?.FirstOrDefault()?.Words;

            if (words == null || words.Count == 0) return 0;

            var lastWord = words.Last();
            if (lastWord.Offset == null || lastWord.Duration == null) return 0;

            var totalTicks = lastWord.Offset.Value + lastWord.Duration.Value;
            var totalSeconds = totalTicks / 10_000_000.0; // convert Azure ticks to seconds
            if (totalSeconds <= 0) return 0;

            return (int)Math.Round((words.Count / totalSeconds) * 60);
        }

        /// <summary>
        /// Get the student's school and region and stores them on the result row,
        /// so that metrics queries can filter by school or region. 
        /// Homeschool students are not tied to any school, so they resolve to (0, 0).
        /// </summary>
        /// <param name="userId">The student's user id</param>
        /// <returns>The student's school and region ids, or (0, 0) when not tied to a school</returns>
        private static async Task<(int SchoolId, int RegionId)> GetSchoolAndRegion(int userId)
        {
            var connectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"];
            using var connection = new MySqlConnection(connectionString);
            var result = await connection.QueryFirstOrDefaultAsync(
                @"SELECT us.school_id, COALESCE(sa.region_id, 0) AS region_id
                  FROM gs_users_db_prod.upd_user_schools us
                  LEFT JOIN gs_schools_api_db_prod.sch_addresses sa ON sa.entity_id = us.school_id
                  WHERE us.user_id = @UserId
                  LIMIT 1",
                new { UserId = userId });

            // Homeschool students are not associated with any school
            if (result == null)
            {
                return (0, 0);
            }

            return ((int)result.school_id, (int)result.region_id);
        }

        /// <summary>
        /// The assessment scores for a single slide attempt that determine slide status.
        /// </summary>
        private readonly record struct SlideScores(double Accuracy, double Fluency, double Completeness, double Pronunciation);
    }
}
