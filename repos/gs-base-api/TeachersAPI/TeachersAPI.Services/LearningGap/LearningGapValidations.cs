using TeachersAPI.Domain.Models.LearningGap;
using System;
using System.Collections.Generic;
using TeachersAPI.TeachersAPI.Services.Generals;
using System.Configuration;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.LearningGap
{
    /// <summary>
    /// To validate the learning gap annotation.
    /// </summary>
    public class LearningGapValidations
    {
        private readonly GenericValidation _genericValidation;
        private readonly MessageManager _messageManager;

        /// <summary>
        /// To construct the LearningGapValidations.
        /// </summary>
        /// <param name="connectionStringRead">The connection string read.</param>
        /// <param name="langCode">The language code.</param>
        public LearningGapValidations(string connectionStringRead, string langCode)
        {
            _genericValidation = new GenericValidation(connectionStringRead, langCode);
            _messageManager = new MessageManager(connectionStringRead, langCode);
        }

        /// <summary>
        ///  To validate the learning gap annotation.
        /// </summary>
        /// <param name="model">Validate model.</param>
        /// <returns>Returns the validated learning gap annotation.</returns>
        public LearningGapAnnotationModel ValidateAnnotation(LearningGapAnnotationModel model)
        {
            var requiredFields = new Dictionary<string, string>
            {
                { nameof(model.TeacherId), model.TeacherId },
                { nameof(model.StudentId), model.StudentId },
                { nameof(model.AcademicYearId), model.AcademicYearId },
            };

            return _genericValidation.ValidateModel(
                model,
                requiredFields,
                () => string.IsNullOrWhiteSpace(model.Text) ?
                false : model.Text.Length <= 10000,

                    "lgap_message_annotation_text_required"
                );
        }

        /// <summary>
        /// Validate the learning gap academic year score.
        /// </summary>
        /// <param name="model">The gap academic year score model to validate.</param>
        /// <returns>Returns the learning gap academic year score.</returns>
        public LearningGapAcademicYearScoreModel ValidateGapAcademicYearScore(LearningGapAcademicYearScoreModel model)
        {
            var requiredFields = new Dictionary<string, string>
            {
                { nameof(model.AcademicYearId), model.AcademicYearId },
                { nameof(model.StudentId), model.StudentId },
                { nameof(model.SubjectCode), model.SubjectCode }
            };

            return _genericValidation.ValidateModel(
                model,
            requiredFields,
            () => !string.IsNullOrWhiteSpace(model.InitialGradeScore) ?
            _genericValidation.IsValidDecimal(model.InitialGradeScore) : true

            && !string.IsNullOrWhiteSpace(model.FinalGradeScore) ?
            _genericValidation.IsValidDecimal(model.FinalGradeScore) : true,
            null
            );
        }

        /// <summary>
        /// To validate the learning gap diagnostic test result model.
        /// </summary>
        /// <param name="model">The learning gap diagnostic test result model to validate.</param>
        /// <returns>Returns the validated assigned lesson.</returns>
        public LearningGapDiagnosticTestResultModel ValidateLearningGapDiagnosticTestResultModel(LearningGapDiagnosticTestResultModel model)
        {
            var requiredFields = new Dictionary<string, string>
            {
                { nameof(model.QuizId), model.QuizId },
                { nameof(model.StudentId), model.StudentId },
                { nameof(model.SubjectCode), model.SubjectCode },
                { nameof(model.LevelCode), model.LevelCode },
                { nameof(model.TotalCorrect), model.TotalCorrect },
                { nameof(model.TotalQuestions), model.TotalQuestions },
                { nameof(model.TotalPercent), model.TotalPercent },
                { nameof(model.TotalScore), model.TotalScore }
            };

            return _genericValidation.ValidateModel(
                model,
                requiredFields,
                () => _genericValidation.IsValidDecimal(model.TotalPercent) && _genericValidation.IsValidDecimal(model.TotalScore) &&
                      _genericValidation.IsValidInt(model.TotalQuestions) && _genericValidation.IsValidInt(model.TotalCorrect),
                "lgap_message_score_invalid"
            );
        }

        /// <summary>
        /// To validate the learning gap assigned lesson models.
        /// </summary>
        /// <param name="model">The learning gap assigned lesson.</param>
        /// <returns>Returns the validated assigned lesson.</returns>
        public LearningGapAssignedLessonModel ValidateLearningGapAssignedLessonModel(LearningGapAssignedLessonModel model)
        {
            var requiredFields = new Dictionary<string, string>
            {
                { nameof(model.AcademicYearId), model.AcademicYearId },
                { nameof(model.StudentId), model.StudentId },
                { nameof(model.TeacherId), model.TeacherId },
                { nameof(model.LimitDate), model.LimitDate }
            };

            return _genericValidation.ValidateModel(
                model,
                requiredFields,
                () => DateTime.TryParse(model.LimitDate, out _),
                "lgap_message_date_invalid"
            );
        }

        /// <summary>
        /// To validate learning gap assigned lesson model just date and id.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns the validation details</returns>
        public LearningGapAssignedLessonModel ValidateLearningGapAssignedLessonModelJustDateAndId(LearningGapAssignedLessonModel model)
        {
            var requiredFields = new Dictionary<string, string>
            {
                { nameof(model.Id), model.Id },
            };

            return _genericValidation.ValidateModel(
                model,
                requiredFields,
                () => DateTime.TryParse(model.LimitDate, out _),
                "lgap_message_date_invalid"
            );
        }

        /// <summary>
        /// To validate the learning gap diagnostic test result response model.
        /// </summary>
        /// <param name="model">The learning gap diagnostic test result responde model.</param>
        /// <returns>Returns the learning gap diagnostic test result response model.</returns>
        public LearningGapDiagnosticTestResultResponseModel ValidateLearningGapDiagnosticTestResultModel(LearningGapDiagnosticTestResultResponseModel model)
        {

            var requiredFields = new Dictionary<string, string>
            {
                { nameof(model.QuestionId), model.QuestionId },
                { nameof(model.ResponseId), model.ResponseId },
                { nameof(model.Correct), model.Correct }
            };

            return _genericValidation.ValidateModel(
                model,
                requiredFields,
                () => _genericValidation.IsValidInt(model.Correct),
                "lgap_message_score_invalid"
                                                                                       );
        }
        
        /// <summary>
        /// To validate the learning gap lesson quiz result model.
        /// </summary>
        /// <param name="model">The learning gap lesson quiz result model.</param>
        /// <returns>Returns the learning gap lesson quiz model result validated.</returns>
        public LearningGapLessonQuizResultModel ValidateLearningGapLessonQuizResultModel(LearningGapLessonQuizResultModel model)
        {

            var requiredFields = new Dictionary<string, string>
            {
                { nameof(model.QuizId), model.QuizId },
                { nameof(model.Score), model.Score },
            };

            return _genericValidation.ValidateModel(
                model,
                requiredFields,
                () => _genericValidation.IsValidDecimal(model.Score),
                "lgap_message_score_invalid");
        }

        /// <summary>
        /// To validate the assigned dates diagnostic dates.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns the learning gap academic dates validated.</returns>
        public LearningGapDiagnosticYearScoreModel ValidateAssignDiagnosticDates(LearningGapDiagnosticYearScoreModel model)
        {
            // Check the dates
            if (model.InitialDiagnosticDateUpdated && !DateTime.TryParse(model.InitialDiagnosticDateLimit, out _))
            {
                model.Valid = false;
                model.Messages = new List<string> {
                    _messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_date_invalid"])
                };
                return model;
            }

            if (model.FinalDiagnosticDateUpdated && !DateTime.TryParse(model.FinalDiagnosticDateLimit, out _))
            {
                model.Valid = false;
                model.Messages = new List<string> {
                    _messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_date_invalid"])
                };
                return model;
            }

            // If valid dates, check if more than today. 
            if (model.InitialDiagnosticDateUpdated && DateTime.Parse(model.InitialDiagnosticDateLimit) < DateTime.Now)
            {
                model.Valid = false;
                model.Messages = new List<string>
                {
                    _messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_date_invalid"])
                };
                return model;
            }

            if (model.FinalDiagnosticDateUpdated && DateTime.Parse(model.FinalDiagnosticDateLimit) < DateTime.Now)
            {
                model.Valid = false;
                model.Messages = new List<string>
                {
                    _messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_date_invalid"])
                };
                return model;
            }

            model.Valid = true;
            return model;
        }

        /// <summary>
        /// To validate the manual diagnostic score assign.
        /// </summary>
        /// <param name="model">The model to validate.</param>
        /// <returns>Returns the m</returns>
        public LearningGapDiagnosticYearScoreModel ValidateManualDiagnosticScore(LearningGapDiagnosticYearScoreModel model)
        {
            // If scores were sent, check if they are a valid number, including integers, decimals, and so on. 
            if(model.InitialDiagnosticScore != null && !string.IsNullOrWhiteSpace(model.InitialDiagnosticScore))
            {
                if (!_genericValidation.IsValidDecimal(model.InitialDiagnosticScore))
                {
                    model.Valid = false;
                    model.Messages = new List<string>
                    {
                        _messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_score_invalid"])
                    };
                    return model;
                }
            }

            if (model.FinalDiagnosticScore != null && !string.IsNullOrWhiteSpace(model.FinalDiagnosticScore))
            {
                if (!_genericValidation.IsValidDecimal(model.FinalDiagnosticScore))
                {
                    model.Valid = false;
                    model.Messages = new List<string>
                    {
                        _messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_score_invalid"])
                    };
                    return model;
                }
            }

            model.Valid = true;
            return model;
        }
    }
}