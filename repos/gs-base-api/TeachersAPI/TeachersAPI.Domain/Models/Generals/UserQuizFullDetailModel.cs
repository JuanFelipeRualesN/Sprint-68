using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.UserQuizzes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals
{
    public class UserQuizFullDetailModel : Domain.Models.Generals.Responses.ResponseModel
    {

        public string QuizId { get; set; }
        public string UserQuizId { get; set; }
        public string QuizTitle { get; set; }
        public string TotalPercent { get; set; }
        public string DoK { get; set; }
        public string TotalCorrect { get; set; }
        public string TryNo { get; set; }
        public string Instructions { get; set; }
        public string ExtraInfo { get; set; }
        public List<QuestionQuiz> QuestionQuizList { get; set; }
        public List<QuizAttachmentModel> QuizAttachmentModelList { get; set; }
        public List<QuizMatchInstructionsModel> QuizMatchInstructionsModelList { get; set; }
        public string Status { get; set; }
        public bool AutoCorrect { get; set; }
        public string CustomValue { get; set; }
        public string DateSubmitted { get; set; }
        public List<UserQuizFileModel> UserQuizFileModelList { get; set; }
        public string Feedback { get; set; }
        public string Rubric { get; set; }
        public bool ManuallyCorrectedByTeacher { get; set; }
        public string Note { get; set; }
        public double MaxScore { get; set; }
        public string Score { get; set; }

    }


    /// <summary>
    /// To handle the questions quizzes
    /// </summary>
    public class QuestionQuiz
    {
        public string QuestionId { get; set; }
        public string Question { get; set; }
        public string CustomValue { get; set; }
        public string QuestionTypeId { get; set; }
        public string QuizMatchInstructionId { get; set; } 
        public List<QuestionResponse> QuestionResponses { get; set; }
    }

    /// <summary>
    /// To handle the questions responses 
    /// </summary>
    public class QuestionResponse
    {
        public string Id { get; set; }
        public string ResponseId { get; set; }
        public string Response { get; set; }
        public bool SelectedByUser { get; set; }
        public string ResponseUserId { get; set; }
        public string ResponseExtra { get; set; }
        public int Correct { get; set; }
        public bool CorrectAnswer { get; set; }
        public string AssignedValue { get; set; }
        public bool UserSelectCorrectAnswer { get; set; }
        public string UserSelectedResponseText { get; set; }
    }
}