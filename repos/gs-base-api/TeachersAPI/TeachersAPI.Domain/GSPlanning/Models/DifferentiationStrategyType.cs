using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSPlanning.Models
{
    /// <summary>
    /// Represents a differentiation strategy type used in teacher planning.
    /// Example: Manipulatives, Graphic organizers, Web-Quests, etc.
    /// </summary>
    public class DifferentiationStrategyType
    {

        /// <summary>
        /// Primary key identifier of the differentiation strategy type.
        /// Maps to column: id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Description of the differentiation strategy.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the differentiation strategy.
        /// Maps to column: description
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Category of the differentiation strategy.
        /// Maps to column: type
        /// 1 = Content, 2 = Product, 3 = Learning environment, 4 = Process.
        /// </summary>
        public int DifferentiationStrategyCategoryId { get; set; }

        /// <summary>
        /// Type of the differentiation strategy: default (0), custom (1).
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Identifier of the user who last modified the record.
        /// Maps to column: modified_by
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime when the record was created.
        /// Maps to column: created_at
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the record was last updated.
        /// Maps to column: updated_at
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
