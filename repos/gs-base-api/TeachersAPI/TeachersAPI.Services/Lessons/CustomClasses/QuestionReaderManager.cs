using System.Collections.Generic;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons;

namespace TeachersAPI.TeachersAPI.Services.Lessons.CustomClasses
{
    /// <summary>
    /// To handle the reads associated with the questions.
    /// </summary>
    
    
    public class QuestionReaderManager
    {
        private readonly string _connectionStringRead, _langCode;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string to read from the database.</param>
        /// <param name="langCode">The language code.</param>
        
        public QuestionReaderManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }
        
        /// <summary>
        /// Return the question models.
        /// </summary>
        /// param name="quizQuestionModel">The quiz question model.</param>
        /// <returns>Return the Question model list</returns>
        public List<QuizQuestionModel> GetQuestions(QuizQuestionModel quizQuestionModel)
        {
            return new QuizQuestionDAO().Read(_connectionStringRead, quizQuestionModel, false, false);
        }

        /// <summary>
        /// To get the question types.
        /// </summary>
        /// <returns>Returns the list of question type models.</returns>
        public List<QuizQuestionTypesModel> GetQuestionType()
        {
            return new QuizQuestionTypesDAO().Read(_connectionStringRead, new QuizQuestionTypesModel(), false, false);
            
        }
        
        /// <summary>
        /// Get question types by filtering.
        /// </summary>
        /// <param name="quizQuestionTypesModel">The quiz question type model.</param>
        /// <returns>Returns the list of question type models.</returns>
        public List<QuizQuestionTypesModel> GetQuestionType(QuizQuestionTypesModel quizQuestionTypesModel)
        {
            return new QuizQuestionTypesDAO().Read(_connectionStringRead, quizQuestionTypesModel, false, false);
        }
    }
}