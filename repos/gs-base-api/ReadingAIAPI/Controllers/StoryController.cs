using Microsoft.AspNetCore.Mvc;
using ReadingAPI.ReadingAPI.Domain.DTOs.Story;
using ReadingAPI.ReadingAPI.Services.FeignAPIs;
using ReadingAPI.ReadingAPI.Services.Story;

namespace ReadingAPI.Controllers
{
    [Route("readingapi/stories")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly StoryModifierService _storyModifierService;
        private readonly StoryReaderService _storyReaderService;

        public StoryController(StoryModifierService storyModifierService, StoryReaderService storyReaderService)
        {
            _storyModifierService = storyModifierService;
            _storyReaderService = storyReaderService;
        }


        /// <summary>
        /// Get a list with all the stories
        /// </summary>
        /// <param name="fromPage">The page index</param>
        /// <param name="limit">Stories per page</param>
        /// <param name="levelId">Optional level id filter</param>
        /// <param name="categoryId">Optional category id filter</param>
        /// <param name="search">Optional search filter to retrieve a specific story</param>
        /// <returns>A list with all the stories from stories table</returns>
        [HttpGet]
        [Route("read/{fromPage}/{limit}")]
        public async Task<IActionResult> GetAllStories(int fromPage, int limit, [FromQuery] int? levelId, [FromQuery] int? categoryId, [FromQuery] string? search)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var stories = await _storyReaderService.GetAllStories(fromPage, limit, levelId, categoryId, search);

                return Ok(stories);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Get a single story by its id, including its level, category, and ordered slides
        /// </summary>
        /// <param name="id">The story id</param>
        /// <returns>The story with the given id</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStoryById(int id)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var story = await _storyReaderService.GetStoryById(id);

                if (story == null)
                {
                    return NotFound();
                }

                return Ok(story);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Create a new story along with its slides.
        /// </summary>
        /// <param name="request">The user request</param>
        /// <returns>The id of the newly created story</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateStory(StoryRequestDto request)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var newId = await _storyModifierService.CreateStory(request);
                return Created(string.Empty, new { IsValid = true, Id = newId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Update an existing story
        /// </summary>
        /// <param name="id">The story id</param>
        /// <param name="request">The user request</param>
        /// <returns>A success flag</returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStory(int id, StoryRequestDto request)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var updated = await _storyModifierService.UpdateStory(id, request);
                return updated ? Ok(new { IsValid = true, Message = $"Story succesfully updated." }) : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Delete a story
        /// </summary>
        /// <param name="id">The story id</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStory(int id)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var deleted = await _storyModifierService.DeleteStory(id);
                return deleted ? Ok(new { IsValid = true, Message = $"Story succesfully deleted." }) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Get all reading levels available for stories
        /// </summary>
        /// <returns>A list with all the levels</returns>
        [HttpGet]
        [Route("levels")]
        public async Task<IActionResult> GetLevels()
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var levels = await _storyReaderService.GetStoryLevels();
                return Ok(levels);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Get all categories available for stories
        /// </summary>
        /// <returns>A list with all the categories</returns>
        [HttpGet]
        [Route("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var categories = await _storyReaderService.GetStoryCategories();
                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Get all stories together with the student's progress status for each one
        /// </summary>
        /// <param name="fromPage">The page index</param>
        /// <param name="limit">Stories per page</param>
        /// <param name="levelId">Optional level id filter</param>
        /// <param name="categoryId">Optional category id filter</param>
        /// <param name="language">The language code to get story progress (en, es)</param>
        /// <returns>
        /// A list of stories, each one with the user's status id
        /// </returns>
        [HttpGet]
        [Route("by-student/{fromPage}/{limit}")]
        public async Task<IActionResult> GetStoriesByStudent(int fromPage, int limit, [FromQuery] int? levelId, [FromQuery] int? categoryId, [FromQuery] string language)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Lecturia");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var stories = await _storyReaderService.GetStudentStoryProgress(int.Parse(responseSession.UserId), fromPage, limit, levelId, categoryId, language);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
