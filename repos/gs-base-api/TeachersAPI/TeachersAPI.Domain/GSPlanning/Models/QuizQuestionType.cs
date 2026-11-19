using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    public class QuizQuestionType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool? AutoCorrect { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
    }
}
