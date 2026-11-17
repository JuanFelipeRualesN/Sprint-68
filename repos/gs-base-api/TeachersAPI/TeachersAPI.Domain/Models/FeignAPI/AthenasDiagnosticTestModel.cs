
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.FeignAPI
{
    public class AthenasDiagnosticTestModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public string LevelCode { get; set; }

        public string LevelName { get; set; }

        public List<DiagnosticTest> DiagnosticTests { get; set; }

        // Extended properties
        public bool AvailableForStudent { get; set; } = false;

    }

    public class DiagnosticTest
    {
        /// <summary>
        /// Auto increment primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Foreign key referencing DiagnosticSubjectLevels
        /// </summary>
        public int DiagnosticSubjectLevelId { get; set; }
        /// <summary>
        /// Instructions for the diagnostic test
        /// </summary>
        public string Instructions { get; set; }
        /// <summary>
        /// Additional information related to the diagnostic test
        /// </summary>
        public string ExtraInfo { get; set; }
        
        /// <summary>
        /// The user who modified the row
        /// </summary>
        public int ModifiedBy { get; set; } = 1;
        /// <summary>
        /// The datetime of the creation of the row
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// The datetime when the row was updated
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual List<DiagnosticTestQuestion> Questions { get; set; } = new List<DiagnosticTestQuestion>();
    }

    /// <summary>
    /// Represents a diagnostic test question.
    /// </summary>
    public class DiagnosticTestQuestion
    {
        /// <summary>
        /// Auto-increment primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The diagnostic test ID this question belongs to.
        /// </summary>
        public int DiagnosticTestsId { get; set; }

        /// <summary>
        /// The question's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The user who last modified the row.
        /// </summary>
        public int ModifiedBy { get; set; } = 1;

        /// <summary>
        /// The datetime when the row was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property: The associated diagnostic test.
        /// </summary>
        public virtual DiagnosticTest DiagnosticTest { get; set; }

        /// <summary>
        /// Navigation property: The responses associated with this question.
        /// </summary>
        public virtual List<DiagnosticTestQuestionResponse> Responses { get; set; } = new List<DiagnosticTestQuestionResponse>();
    }

    /// <summary>
    /// Represents a response to a diagnostic test question.
    /// </summary>
    public class DiagnosticTestQuestionResponse
    {
        /// <summary>
        /// Auto-increment primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The diagnostic test question ID this response belongs to.
        /// </summary>
        public int DiagnosticTestQuestionId { get; set; }

        /// <summary>
        /// The response's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether the response is correct (1 if correct, 0 if not).
        /// </summary>
        public int IsCorrect { get; set; } = 0;

        /// <summary>
        /// The user who last modified the row.
        /// </summary>
        public int ModifiedBy { get; set; } = 1;

        /// <summary>
        /// The datetime when the row was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property: The associated diagnostic test question.
        /// </summary>
        public virtual DiagnosticTestQuestion DiagnosticTestQuestion { get; set; }

        // Extended properties
        public bool SelectedByStudent { get; set;} = false;
    }
}