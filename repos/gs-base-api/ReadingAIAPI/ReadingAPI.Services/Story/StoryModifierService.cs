using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingAPI.ReadingAPI.Domain.DTOs.Story;
using ReadingAPI.ReadingAPI.Domain.Entities.Story;
using ReadingAPI.ReadingAPI.Infrastructure;

namespace ReadingAPI.ReadingAPI.Services.Story
{
    public class StoryModifierService
    {
        private readonly ReadingAIDbContext _dbContext;
        private readonly ILogger<StoryModifierService> _logger;

        public StoryModifierService(ReadingAIDbContext dbContext, ILogger<StoryModifierService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Create a new story along with its slides
        /// </summary>
        /// <param name="dto">The story request</param>
        /// <returns>The id of the newly created story</returns>
        public async Task<int> CreateStory(StoryRequestDto dto)
        {
            try
            {
                // Validate story fields
                StoryValidator.Validate(dto);

                // Validate LevelId
                if (!await _dbContext.StoryLevels.AnyAsync(level => level.Id == dto.LevelId))
                {
                    throw new ArgumentException($"Level with id {dto.LevelId} not found.");
                }

                // Validate CategoryId
                if (!await _dbContext.StoryCategories.AnyAsync(category => category.Id == dto.CategoryId))
                {
                    throw new ArgumentException($"Category with id {dto.CategoryId} not found.");
                }

                var slides = dto.Slides.Select(slide => new StorySlideEntity
                {
                    SlideOrder = slide.SlideOrder,
                    TextEn = slide.TextEn,
                    TextEs = slide.TextEs,
                    ImageUrl = slide.ImageUrl
                }).ToList();

                var story = new StoryEntity
                {
                    LevelId = dto.LevelId,
                    CategoryId = dto.CategoryId,
                    TitleEn = dto.TitleEn,
                    TitleEs = dto.TitleEs,
                    ImageUrl = dto.ImageUrl,
                    FluencyGoal = dto.FluencyGoal,
                    AccuracyGoal = dto.AccuracyGoal,
                    CompletenessGoal = dto.CompletenessGoal,
                    PronunciationGoal = dto.PronunciationGoal,
                    WordCountEn = CountSlideWords(slides, slide => slide.TextEn),
                    WordCountEs = CountSlideWords(slides, slide => slide.TextEs),
                    Slides = slides
                };

                // Insert the story into the DB
                _dbContext.Stories.Add(story);
                await _dbContext.SaveChangesAsync();

                return story.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating story");
                throw;
            }
        }

        /// <summary>
        /// Update an existing story
        /// matching slides are updated, slides with no id are inserted, and existing slides not
        /// present in the request are deleted
        /// </summary>
        /// <param name="id">The id of the story</param>
        /// <param name="dto">The fields to apply to the story</param>
        /// <returns>true if the story was updated, false if no story matched the given id</returns>
        public async Task<bool> UpdateStory(int id, StoryRequestDto dto)
        {
            try
            {
                // Validate story fields
                StoryValidator.Validate(dto);

                var story = await _dbContext.Stories
                    .Include(story => story.Slides)
                    .FirstOrDefaultAsync(story => story.Id == id);

                if (story == null) return false;

                // Validate LevelId
                if (!await _dbContext.StoryLevels.AnyAsync(level => level.Id == dto.LevelId))
                {
                    throw new ArgumentException($"Level with id {dto.LevelId} not found.");
                }

                // Validate CategoryId
                if (!await _dbContext.StoryCategories.AnyAsync(category => category.Id == dto.CategoryId))
                {
                    throw new ArgumentException($"Category with id {dto.CategoryId} not found.");
                }

                story.LevelId = dto.LevelId;
                story.CategoryId = dto.CategoryId;
                story.TitleEn = dto.TitleEn;
                story.TitleEs = dto.TitleEs;
                story.ImageUrl = dto.ImageUrl;
                story.FluencyGoal = dto.FluencyGoal;
                story.AccuracyGoal = dto.AccuracyGoal;
                story.CompletenessGoal = dto.CompletenessGoal;
                story.PronunciationGoal = dto.PronunciationGoal;

                SyncSlides(story, dto.Slides);

                // Count the words with the new slides
                story.WordCountEn = CountSlideWords(story.Slides.ToList(), slide => slide.TextEn);
                story.WordCountEs = CountSlideWords(story.Slides.ToList(), slide => slide.TextEs);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating story with id {StoryId}", id);
                throw;
            }
        }

        /// <summary>
        /// Delete a story, its slides, and every student's progress row for it.
        /// Practice rows in user_story_results are intentionally kept,
        /// since they carry a snapshot of the evaluated text and preserve the student's
        /// score history even if the story/slide is later deleted.
        /// </summary>
        /// <param name="id">The id of the story</param>
        /// <returns>A boolean</returns>
        public async Task<bool> DeleteStory(int id)
        {
            try
            {
                var story = await _dbContext.Stories
                    .Include(s => s.Slides)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (story == null) return false;

                var userStoryprogress = await _dbContext.UserStoryProgress
                    .Where(p => p.StoryId == id)
                    .ToListAsync();

                 // Remove the row that store the user's progress for the story
                _dbContext.UserStoryProgress.RemoveRange(userStoryprogress);
                _dbContext.Stories.Remove(story);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting story with id {StoryId}", id);
                throw;
            }
        }

        /// <summary>
        /// Syncs a story's slides with the incoming request using SlideOrder as the id.
        /// Existing slides are updated, new slides are added, and missing slides are removed.
        /// </summary>
        private void SyncSlides(StoryEntity story, List<StorySlideRequestDto> requestedSlides)
        {
            // Collect all slide orders sent by the user
            var requestedSlideOrders = requestedSlides.Select(slide => slide.SlideOrder).ToHashSet();

            // Slides that are no longer present in the request should be removed
            var toRemove = story.Slides.Where(slide => !requestedSlideOrders.Contains(slide.SlideOrder)).ToList();

            foreach (var slide in toRemove)
            {
                _dbContext.Remove(slide);
                story.Slides.Remove(slide);
            }

            // Find existing slides
            var existingByOrder = story.Slides.ToDictionary(slide => slide.SlideOrder);

            foreach (var dto in requestedSlides)
            {
                // If a slide with the same SlideOrder already exists, update its content
                if (existingByOrder.TryGetValue(dto.SlideOrder, out var existing))
                {
                    existing.TextEn = dto.TextEn;
                    existing.TextEs = dto.TextEs;
                    existing.ImageUrl = dto.ImageUrl;
                }
                else
                {
                    // Otherwise, create a new slide and attach it to the story
                    story.Slides.Add(new StorySlideEntity
                    {
                        SlideOrder = dto.SlideOrder,
                        TextEn = dto.TextEn,
                        TextEs = dto.TextEs,
                        ImageUrl = dto.ImageUrl
                    });
                }
            }
        }

        /// <summary>
        /// Sums word counts across all slides for a given language
        /// </summary>
        /// <param name="slides">The story's slides</param>
        /// <param name="getText">Selector returning the language specific text for a slide</param>
        /// <returns>The total number of words across all slides for the selected language</returns>
        private static int CountSlideWords(List<StorySlideEntity> slides, Func<StorySlideEntity, string?> getText)
        {
            return slides.Sum(slide => getText(slide)?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length ?? 0);
        }
    }
}
