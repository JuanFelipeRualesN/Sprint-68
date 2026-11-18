namespace GSAIServicesAPI.FeignAPIs.Models.Lessons.Requests
{
    /// <summary>
    /// To request a group of lessons according to the attributes of this class / object
    /// </summary>
    public class LessonAdvanceRequestModel
    {
        /// <summary>
        /// Ids of lessons if VR, AR, Genial Web, Edulab
        /// </summary>
        public List<string> AddressIds { get; set; }
        /// <summary>
        /// Lesson Ids 
        /// </summary>
        public string LessonId { get; set; }
        /// <summary>
        /// Subject codes (if MAT-SP, MAT-EN, SCI-EN, SCI-SP, SPA, ENG) 
        /// </summary>
        public List<string> SubjectCodes { get; set; }
        /// <summary>
        /// Level codes (grades): k, 1, 2, 3....
        /// </summary>
        public List<string> LevelCodes { get; set; }
        /// <summary>
        /// Status IDs
        /// </summary>
        public List<string> StatusIds { get; set; }

        /// <summary>
        /// To get or set the code ids (standards, expectations, etc (the ids that represent them)).
        /// </summary>
        public List<string> CodeIds { get; set; } = null;

        /// <summary>
        /// Search by lesson title
        /// </summary>
        public string TeacherInputText { get; set; } = null;

        /// <summary>
        /// get or set the standard code
        /// </summary>
        public List<StandardCodeModel> StandardCodes { get; set; } = null;

        /// <summary>
        /// get or set the expectation code
        /// </summary>
        public List<ExpectationCodeModel> ExpectationCodes { get; set; } = null;

        /// <summary>
        /// filter lessons by text input
        /// </summary>
        public string ContentFilter { get; set; } = null;

        /// <summary>
        /// filter lessons by creation date
        /// </summary>
        public string OrderByCreationDate { get; set; } = null;

        /// <summary>
        /// filter lessons alphabetically by title
        /// </summary>
        public string OrderByTitle { get; set; } = null;

        /// <summary>
        /// From row
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Limit (How many) 
        /// </summary>
        public int Limit { get; set; }

    }

    /// <summary>
    /// The standard code model represents a specific curriculum standard associated with a lesson. It contains the code itself and its unique identifier.
    /// </summary>
    public class StandardCodeModel
    {
        /// <summary>
        /// The standard code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The standard code id
        /// </summary>
        public string CodeId { get; set; }
    }

    /// <summary>
    /// The expectation code model represents a specific expectation or learning standard associated with a lesson. It contains the code itself and its unique identifier.
    /// </summary>
    public class ExpectationCodeModel
    {
        /// <summary>
        /// The expectation code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The expectation description
        /// </summary>
        public string CodeId { get; set; }
    }

}
