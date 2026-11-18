namespace ReadingAPI.ReadingAPI.Domain.Entities.Reading
{
    /// <summary>
    ///  Represents the progress status of a student's story practice activity.
    /// </summary>
    public enum StoryProgressStatus
    {
        /// <summary>
        /// The student has not submitted any story slide.
        /// </summary>
        NotStarted = 1,

        /// <summary>
        /// The student has submitted at least one story slide but has not completed the story.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// The student has completed the story.
        /// </summary>
        Completed = 3
    }
}
