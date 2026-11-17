using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapDiagnosticYearScoreModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string AcademicYearId { get; set; }
        public string StudentId { get; set; }
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }
        public string InitialDiagnosticResultId { get; set; }
        public string InitialDiagnosticDateLimit { get; set; }
        public string FinalDiagnosticResultId { get; set; }
        public string FinalDiagnosticDateLimit { get; set; }
        public string InitialDiagnosticScore { get; set; }
        public string FinalDiagnosticScore { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        /// <summary>
        /// The subject name. Extended property.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Extended property to handle the students' ids request.
        /// </summary>
        public List<string> StudentIds { get; set; }

        /// <summary>
        /// Extended property to handle the diagnostic title.
        /// </summary>
        public string DiagnosticTitle { get; set; }

        /// <summary>
        /// Gets or sets the teacher group ids. (Extended propery) 
        /// </summary>
        public List<string> TeacherGroupIds { get; set; }

        /// <summary>
        /// To check if the initial diagnostic date was updated.
        /// </summary>
        public bool InitialDiagnosticDateUpdated { get; set; }

        /// <summary>
        /// To check if the final diagnostic date was updated.
        /// </summary>
        public bool FinalDiagnosticDateUpdated { get; set; }

        /// <summary>
        /// Gets or sets if the score can be edited by the teacher. Extended property.
        /// </summary>
        public bool EditableByTeacher { get; set; }

    }
}