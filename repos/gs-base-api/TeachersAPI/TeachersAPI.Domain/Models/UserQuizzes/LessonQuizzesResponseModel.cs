using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.UserQuizzes
{
    public class LessonQuizzesResponseModel
    {
        private double _quizAverage;
        private bool _quizCompleted;
        private bool _passed;

        public double QuizAverage { get => _quizAverage; set => _quizAverage = value; }
        public bool QuizCompleted { get => _quizCompleted; set => _quizCompleted = value; }
        public bool Passed { get => _passed; set => _passed = value; }
    }
}
