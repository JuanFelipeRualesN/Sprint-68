using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the evaluations related to the teacher plans.
    /// </summary>
    public class TeacherPlanEvaluation
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher’s plan id.
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Description of the evaluation types associated with the teacher plan, grouped by type and according to the language.
        /// </summary>
        public virtual string Description
        {
            get
            {
                return GetDescriptionByLang();
            }
        }

        /// <summary>
        /// Return the description of the evaluation types associated with the teacher plan, grouped by type and according to the language.
        /// </summary>
        /// <param name="langCode"> Language code (default: "es") </param>
        /// <returns>Group description of evaluation types</returns>
        public string GetDescriptionByLang(string langCode = "es")
        {
            if (TeacherPlanEvaluationTypes.Any())
            {
                StringBuilder builder = new StringBuilder();

                // Etiquetas por idioma
                var labels = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
                {
                    ["es"] = new Dictionary<string, string>
                    {
                        ["Diagnostic"] = "Diagnóstica: ",
                        ["Formative"] = "Formativa: ",
                        ["Summative"] = "Sumativa: "
                    },
                    ["en"] = new Dictionary<string, string>
                    {
                        ["Diagnostic"] = "Diagnostic: ",
                        ["Formative"] = "Formative: ",
                        ["Summative"] = "Summative: "
                    }
                };

                var selectedLabels = labels.ContainsKey(langCode) ? labels[langCode] : labels["es"];

                TeacherPlanEvaluationTypes.GroupBy(x => x.EvaluationType?.Type)
                    .Select(x => new { x.Key, Set = x.Select(y => y.EvaluationType?.Description).ToArray() })
                    .ToList()
                    .ForEach(x =>
                    {
                        switch (x.Key)
                        {
                            case EvaluationType.DiagnosticType:
                                builder.Append(selectedLabels["Diagnostic"]).Append(string.Join(", ", x.Set)).AppendLine();
                                break;
                            case EvaluationType.FormativeType:
                                builder.Append(selectedLabels["Formative"]).Append(string.Join(", ", x.Set)).AppendLine();
                                break;
                            case EvaluationType.SummativeType:
                                builder.Append(selectedLabels["Summative"]).Append(string.Join(", ", x.Set)).AppendLine();
                                break;
                        }
                    });

                return builder.ToString();
            }

            return string.Empty;
        }

        public virtual TeacherPlan TeacherPlan { get; set; }

        public virtual ICollection<TeacherPlanEvaluationType> TeacherPlanEvaluationTypes { get; set; } = new List<TeacherPlanEvaluationType>();

        public virtual ICollection<TeacherPlanEvaluationsDate> TeacherPlanEvaluationsDates { get; set; } = new List<TeacherPlanEvaluationsDate>();
    }
}
