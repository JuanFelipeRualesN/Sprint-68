using ReadingAPI.ReadingAPI.Domain.DTOs.Story;

namespace ReadingAPI.ReadingAPI.Services.Story
{
    public static class StoryValidator
    {
        /// <summary>
        /// Validates the story request
        /// </summary>
        public static void Validate(StoryRequestDto dto)
        {
            if (dto.LevelId <= 0)
            {
                throw new ArgumentException("LevelId is required.");
            }

            if (dto.CategoryId <= 0)
            {
                throw new ArgumentException("CategoryId is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.TitleEn))
            {
                throw new ArgumentException("TitleEn is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.TitleEs))
            {
                throw new ArgumentException("TitleEs is required.");
            }

            if (dto.FluencyGoal <= 0)
            {
                throw new ArgumentException("FluencyGoal is required and must be greater than 0.");
            }

            if (dto.AccuracyGoal <= 0)
            {
                throw new ArgumentException("AccuracyGoal is required and must be greater than 0.");
            }

            if (dto.CompletenessGoal <= 0)
            {
                throw new ArgumentException("CompletenessGoal is required and must be greater than 0.");
            }

            if (dto.PronunciationGoal <= 0)
            {
                throw new ArgumentException("PronunciationGoal is required and must be greater than 0.");
            }

            if (dto.Slides.Count == 0)
            {
                throw new ArgumentException("At least one slide is required.");
            }

            var orderedSlides = dto.Slides.OrderBy(slide => slide.SlideOrder).ToList();

            for (int i = 0; i < orderedSlides.Count; i++)
            {
                var expected = i + 1;
                if (orderedSlides[i].SlideOrder != expected)
                {
                    throw new ArgumentException($"SlideOrder must be contiguous starting at 1.");
                }
            }
        }
    }
}
