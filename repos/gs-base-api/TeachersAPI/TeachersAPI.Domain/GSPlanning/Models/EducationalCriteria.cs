using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the education criteria availables for the custom lessons
    /// </summary>
    public class EducationalCriteria
    {
        public const int Transversal = 0;
        public int Id { get; set; }

        /// <summary>
        /// Description of the educational criteria,
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Identify what kind of type for the educational criteria. 0 = Transversal, 1 = Standard, 2 = Expectative , 3 = Academy Strategy, 4 = Objective, 5 = Unity , 6 = Initial, 7 = Development, 8 = Close
        /// </summary>
        public int Type { get; set; }

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
    }
}
