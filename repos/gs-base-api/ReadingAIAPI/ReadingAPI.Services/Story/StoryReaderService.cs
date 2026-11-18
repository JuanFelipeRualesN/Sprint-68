using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingAPI.Domain.DTOs;
using ReadingAPI.ReadingAPI.Domain.DTOs.Story;
using ReadingAPI.ReadingAPI.Domain.Entities.Reading;
using ReadingAPI.ReadingAPI.Infrastructure;

namespace ReadingAPI.ReadingAPI.Services.Story
{
    public class StoryReaderService
    {
        private readonly ReadingAIDbContext _dbContext;
        private readonly ILogger<StoryReaderService> _logger;

        public StoryReaderService(ReadingAIDbContext dbContext, ILogger<StoryReaderService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        ///  Retrieves all the stories
        /// </summary>
        /// <param name="fromPage">The page index</param>
        /// <param name="limit">Stories per page</param>
        /// <param name="levelId">Optional level id filter</param>
        /// <param name="categoryId">Optional category id filter</param>
        /// <param name="search">Optional search filter to retrieve a specific story</param>
        /// <returns>A list with all the stories from stories table</returns>
        public async Task<PaginationModel<StoryResponseDto>> GetAllStories(int fromPage, int limit, int? levelId, int? categoryId, string? search)
        {
            try
            {
                var pageIndex = Math.Max(0, fromPage);
                var pageSize = Math.Max(1, limit);

                var query = _dbContext.Stories.AsQueryable();

                // Filter by level
                if (levelId.HasValue)
                {
                    query = query.Where(story => story.LevelId == levelId.Value);
                }

                // Filter by category
                if (categoryId.HasValue)
                {
                    query = query.Where(story => story.CategoryId == categoryId.Value);
                }

                // Filter by title
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var term = search.Trim();
                    query = query.Where(story =>
                        EF.Functions.Like(story.TitleEn, $"%{term}%") ||
                        EF.Functions.Like(story.TitleEs, $"%{term}%"));
                }

                query = query.OrderBy(story => story.Id);

                var total = await query.CountAsync();

                var stories = await query
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .Select(story => new StoryResponseDto
                    {
                        Id = story.Id,
                        LevelId = story.LevelId,
                        CategoryId = story.CategoryId,
                        TitleEn = story.TitleEn,
                        TitleEs = story.TitleEs,
                        ImageUrl = story.ImageUrl,
                        FluencyGoal = story.FluencyGoal,
                        AccuracyGoal = story.AccuracyGoal,
                        CompletenessGoal = story.CompletenessGoal,
                        PronunciationGoal = story.PronunciationGoal,
                        WordCountEn = story.WordCountEn,
                        WordCountEs = story.WordCountEs,
                        Slides = story.Slides
                            .OrderBy(slide => slide.SlideOrder)
                            .Select(slide => new StorySlideDto
                            {
                                Id = slide.Id,
                                SlideOrder = slide.SlideOrder,
                                TextEn = slide.TextEn,
                                TextEs = slide.TextEs,
                                ImageUrl = slide.ImageUrl
                            }).ToList()
                    })
                    .ToListAsync();

                return new PaginationModel<StoryResponseDto>(total, pageSize, stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all stories");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a single story by its id
        /// </summary>
        /// <param name="id">The story id</param>
        /// <returns>The matching story</returns>
        public async Task<StoryResponseDto?> GetStoryById(int id)
        {
            try
            {
                var story = await _dbContext.Stories
                    .Where(story => story.Id == id)
                    .Select(story => new StoryResponseDto
                    {
                        Id = story.Id,
                        LevelId = story.LevelId,
                        CategoryId = story.CategoryId,
                        TitleEn = story.TitleEn,
                        TitleEs = story.TitleEs,
                        ImageUrl = story.ImageUrl,
                        FluencyGoal = story.FluencyGoal,
                        AccuracyGoal = story.AccuracyGoal,
                        CompletenessGoal = story.CompletenessGoal,
                        PronunciationGoal = story.PronunciationGoal,
                        WordCountEn = story.WordCountEn,
                        WordCountEs = story.WordCountEs,
                        Slides = story.Slides
                            .OrderBy(slide => slide.SlideOrder)
                            .Select(slide => new StorySlideDto
                            {
                                Id = slide.Id,
                                SlideOrder = slide.SlideOrder,
                                TextEn = slide.TextEn,
                                TextEs = slide.TextEs,
                                ImageUrl = slide.ImageUrl
                            }).ToList()
                    })
                    .FirstOrDefaultAsync();

                return story;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching story with id {StoryId}", id);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all reading levels available for stories
        /// </summary>
        /// <returns>A list with all the story levels</returns>
        public async Task<List<StoryLevelDto>> GetStoryLevels()
        {
            try
            {
                var storyLevels = await _dbContext.StoryLevels
                    .OrderBy(level => level.Id)
                    .Select(level => new StoryLevelDto
                    {
                        Id = level.Id,
                        Name = level.Name
                    })
                    .ToListAsync();

                return storyLevels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching story levels");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all story categories available for stories
        /// </summary>
        /// <returns>A list with all the categories</returns>
        public async Task<List<StoryCategoryDto>> GetStoryCategories()
        {
            try
            {
                var storyCategories = await _dbContext.StoryCategories
                    .OrderBy(category => category.Id)
                    .Select(category => new StoryCategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name
                    })
                    .ToListAsync();

                return storyCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching story categories");
                throw;
            }
        }

        /// <summary>
        ///  Retrieves all stories for a specific level, including the student's progress for each story.
        /// </summary>
        /// <param name="userId">The user Id</param>
        /// <param name="fromPage">The page index</param>
        /// <param name="limit">Stories per page</param>
        /// <param name="levelId">The level Id (1, 2, 3, 4, 5)</param>
        /// <param name="categoryId">category id filter</param>
        /// <param name="langCode">The language code to get story progress in english or spanish </param>
        /// <returns>A list of the stories with the progress for the student</returns>
        public async Task<PaginationModel<StudentStoryProgressDto>> GetStudentStoryProgress(int userId, int fromPage, int limit, int? levelId, int? categoryId, string langCode)
        {
            try
            {
                var pageIndex = Math.Max(0, fromPage);
                var pageSize = Math.Max(1, limit);

                var storiesQuery = _dbContext.Stories.AsQueryable();

                // Filter stories by level
                if (levelId.HasValue)
                {
                    storiesQuery = storiesQuery.Where(story => story.LevelId == levelId.Value);
                }

                // Filter stories by category
                if (categoryId.HasValue)
                {
                    storiesQuery = storiesQuery.Where(story => story.CategoryId == categoryId.Value);
                }

                storiesQuery = storiesQuery.OrderBy(story => story.Id);

                var total = await storiesQuery.CountAsync();

                var stories = await storiesQuery
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .Select(story => new StudentStoryProgressDto
                    {
                        Id = story.Id,
                        LevelId = story.LevelId,
                        CategoryId = story.CategoryId,
                        TitleEn = story.TitleEn,
                        TitleEs = story.TitleEs,
                        ImageUrl = story.ImageUrl,
                        FluencyGoal = story.FluencyGoal,
                        AccuracyGoal = story.AccuracyGoal,
                        CompletenessGoal = story.CompletenessGoal,
                        PronunciationGoal = story.PronunciationGoal,
                        WordCountEn = story.WordCountEn,
                        WordCountEs = story.WordCountEs,
                        Language = langCode
                    })
                    .ToListAsync();

                // Get the progress records for the stories on the current page
                var pageStoryIds = stories.Select(story => story.Id).ToList();
                var studentStoryprogress = await _dbContext.UserStoryProgress
                    .Where(storyProgress => storyProgress.UserId == userId
                        && storyProgress.Language == langCode
                        && pageStoryIds.Contains(storyProgress.StoryId))
                    .ToDictionaryAsync(storyProgress => storyProgress.StoryId, storyProgress => storyProgress.StatusId);

                // Stories without a progress record are considered NotStarted.
                foreach (var story in stories)
                {
                    story.StatusId = studentStoryprogress.TryGetValue(story.Id, out var statusId)
                        ? statusId : StoryProgressStatus.NotStarted;
                }

                return new PaginationModel<StudentStoryProgressDto>(total, pageSize, stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student story progress");
                throw;
            }
        }
    }
}
