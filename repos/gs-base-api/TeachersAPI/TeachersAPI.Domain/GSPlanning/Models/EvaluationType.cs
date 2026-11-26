using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the evaluations availables
    /// </summary>
    public class EvaluationType
    {
        public const int DiagnosticType = 1;
        public const int FormativeType = 2;
        public const int SummativeType = 3;
        public const int IsCustom = 1;
        public const int IsDefault = 0;
        public int Id { get; set; }

        /// <summary>
        /// Description of the evaluation,
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Identify what kind of type for the evaluation. 1 = Diagnostic, 2 = Formative , 3 = Summative
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Identify when a evaluation is custom. 0 = default, 1 = Custom
        /// </summary>
        public int Custom { get; set; }

        /// <summary>
        /// The user who modified the material type.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the material type.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the material type was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<TeacherPlanEvaluationType> TeacherPlanEvaluationTypes { get; set; } = new List<TeacherPlanEvaluationType>();
    }
}
