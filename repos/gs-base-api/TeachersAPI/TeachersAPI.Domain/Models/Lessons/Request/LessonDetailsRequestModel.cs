namespace TeachersAPI.Domain.Models.Lessons.Request
{
    /// <summary>
    /// Handles the lesson details requests.
    /// </summary>
    public class LessonDetailsRequestModel
    {
        /// <summary>
        /// Gets or sets whether if its for outside source. If it is outside, for images, or other sources, the url from Athenas services is going to be included.
        /// </summary>
        public bool IsForOutsideSource { get; set; }

        /// <summary>
        /// Gets or sets the lesson id.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets whether to add the lesson no or not.
        /// </summary>
        public bool AddLessonNo { get; set; }

        /// <summary>
        /// Gets or sets whether add quiz details or not.
        /// </summary>
        public bool AddQuizDetails { get; set; }

        /// <summary>
        /// Gets or sets whether show the URL for definitions and examples. 
        /// </summary>
        public bool ShowUrlInstead { get; set; }

        /// <summary>
        /// Gets or sets whether show only the published quizzes or not.
        /// </summary>
        public bool OnlyPublishedQuizzes { get; set; }

        /// <summary>
        /// Gets or sets whether if the lesson if for game or not.
        /// </summary>
        public bool IsForGame { get; set; }

        /// <summary>
        /// ExamType for quizzes. ExamType = 0 for practices, ExamType = 1 for exams, ExamType = 2 for Pre-tests, ExamType = 3 for Post-tests.
        /// </summary>
        public string ExamType { get; set; }

    }
}