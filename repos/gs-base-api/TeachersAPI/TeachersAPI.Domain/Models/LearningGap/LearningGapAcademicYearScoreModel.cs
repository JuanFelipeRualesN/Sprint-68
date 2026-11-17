namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapAcademicYearScoreModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string AcademicYearId { get; set; }
        public string StudentId { get; set; }
        public string SubjectCode { get; set; }
        public string InitialGradeScore { get; set; }
        public string FinalGradeScore { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the Subject's name. Extended property.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the actual score. Extended property.
        /// </summary>
        public string ActualScore { get; set; }

        /// <summary>
        /// Gets or sets the initial score. Extended property.
        /// </summary>
        public bool EditableByTeacher { get; set; }

    }
}