using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachersAPI.Domain.Models.Codes;

namespace TeachersAPI.Domain.Models.Lessons
{       
    public class LessonFilterRequestModel
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
        /// filter lessons (definitions, examples, title, concept, details) by text input
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
}