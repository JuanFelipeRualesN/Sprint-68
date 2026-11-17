using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Lessons.Request;
using TeachersAPI.TeachersAPI.Services.Lessons.CustomClasses.HTMLSanitizer;
using TeachersAPI.Utils;
using System.IO;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Services.UserQuizzes;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.Lessons
{
    /// <summary>
    /// To handle the reads associated directly to quizzes (custom assignments created by the teacher).
    /// </summary>
    public class QuizReaderManager
    {
        private readonly string _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead"></param>
        /// <param name="langCode"></param>
        public QuizReaderManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To get the quiz details with the student results.
        /// </summary>
        /// <param name="planQuizId">The plan quiz id.</param>
        /// <param name="usquiUserResultId">The usqui User result id.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Returns the quiz details with student results.</returns>
        public UserQuizFullDetailModel GetQuizDetailsWithStudentResults(string planQuizId, string usquiUserResultId,
            string studentId)
        {
            // First get the quiz id
            var teacherPlanQuizModelList = new TeacherPlanQuizDAO().Read(_connectionStringRead,
                new TeacherPlanQuizModel() { Id = planQuizId }, true, true);

            // If not found, return an error
            if (teacherPlanQuizModelList.Count == 0)
            {
                return new UserQuizFullDetailModel();
            }

            // Now, find the quiz details
            var quizRequestModel = GetQuiz(
                teacherPlanQuizModelList[0].QuizId, true);

            // Check if auto correct 
            var autoCorrect = true;
            var quizTypeModels = GetQuizType(new List<QuizTypeModel>()
            {
                new QuizTypeModel()
                {
                    Id = quizRequestModel.QuizModel.QuizTypeId,
                    AutoCorrect = "1"
                }
            });
            if (quizTypeModels.Count == 0)
            {
                autoCorrect = false;
            }

            // Get the user quiz to find if the results. 
            var userQuizManager = new UserQuizManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);

            // Call full quiz details
            
            // Get plan quiz assignment result details. 
            var planQuizAssignmentResultModel = new PlanAssignmentUserResultModel()
            {
                PlanQuizId = planQuizId,
                UserId = studentId
            };
            var planQuizAssignmentResultModels = new PlanAssignmentUserResultDAO().Read(_connectionStringRead,
                planQuizAssignmentResultModel, true, true);
            
            // Get User responses
            UserQuizResponseModel userQuizResponseModel = null;
            if (!string.IsNullOrWhiteSpace(usquiUserResultId))
            {
                userQuizResponseModel = userQuizManager.GetUserQuizResponse(usquiUserResultId, studentId);
            };

            // Set the general details related to the quiz. 
            var userQuizFullDetailModel = new UserQuizFullDetailModel()
            {
                QuizId = quizRequestModel.QuizModel.Id,
                QuizTitle = quizRequestModel.QuizModel.Title,
                Instructions = quizRequestModel.QuizModel.Instruction,
                AutoCorrect = autoCorrect,
                CustomValue = quizRequestModel.QuizModel.CustomValue,
                QuestionQuizList = new List<QuestionQuiz>(),
                Rubric = quizRequestModel?.QuizRubricModel?.Desc,
                QuizMatchInstructionsModelList = quizRequestModel.QuizMatchInstructionsModel
            };

            if (planQuizAssignmentResultModels != null && planQuizAssignmentResultModels.Count > 0)
            {
                userQuizFullDetailModel.ManuallyCorrectedByTeacher =
                    planQuizAssignmentResultModels[0].ManuallyCorrectedByTeacher == "1";

                if (userQuizFullDetailModel.ManuallyCorrectedByTeacher)
                {
                    userQuizFullDetailModel.Note = AppConfiguration.Configuration["AppSettings:text_message_manually_corrected_by_teacher_" + _langCode];
                }
                
                userQuizFullDetailModel.TotalPercent = planQuizAssignmentResultModels[0].TotalPercent;
                userQuizFullDetailModel.Score = planQuizAssignmentResultModels[0].Score;
            }

            // If there are user responses, then attach everything
            if (userQuizResponseModel?.UserQuizResult != null)
            {
                userQuizFullDetailModel.Feedback = userQuizResponseModel.UserQuizResult.Feedback;
                userQuizFullDetailModel.Status = userQuizResponseModel.UserQuizResult.Status;
                userQuizFullDetailModel.UserQuizId = userQuizResponseModel.UserQuizResult.Id;
                userQuizFullDetailModel.DoK = userQuizResponseModel.UserQuizResult.Dok;
                userQuizFullDetailModel.TotalCorrect = userQuizResponseModel.UserQuizResult.TotalCorrect;
                userQuizFullDetailModel.TryNo = userQuizResponseModel.UserQuizResult.TryNo;
                userQuizFullDetailModel.DateSubmitted = userQuizResponseModel.UserQuizResult.CreatedAt;
                userQuizFullDetailModel.UserQuizFileModelList = userQuizResponseModel.UserQuizFileModelList; 
            }
            
            var maxScore = 0.0;
            foreach (var question in quizRequestModel.QuestionRequestModelList)
            {
                var questionQuizToAdd = new QuestionQuiz()
                {
                    QuestionId = question.QuizQuestionModel.Id,
                    Question = question.QuizQuestionModel.Desc,
                    QuestionResponses = new List<QuestionResponse>(),
                    CustomValue = question.QuizQuestionModel.CustomValue,
                    QuestionTypeId = question.QuizQuestionModel.QuestionTypeId,
                    QuizMatchInstructionId = question.QuizQuestionModel.QuizMatchInstructionId
                };
                
                maxScore += question.QuizQuestionModel.CustomValue != null ? double.Parse(question.QuizQuestionModel.CustomValue) : 0;

                // Attach the quizzes responses
                foreach (var questionResponse in question.QuizQuestionResponseModelList)
                {

                    var responseToAdd = new QuestionResponse()
                    {
                        ResponseId = questionResponse.Id,
                        Response = questionResponse.Response,
                        CorrectAnswer = questionResponse.Correct == "1",
                    };
                    
                    if (userQuizResponseModel != null)
                    {
                        var userResponse = userQuizResponseModel.UserResponsesList.Find(x => x.QuestionId == questionResponse.QuizQuestionId);
                        if (userResponse != null)
                        {
                            responseToAdd.SelectedByUser = userResponse.ResponseId == questionResponse.Id;
                            responseToAdd.ResponseUserId = userResponse.ResponseId;
                            responseToAdd.AssignedValue = userResponse?.ResponseId == questionResponse.Id
                                ? userResponse.AssignedValue
                                : "";
                            responseToAdd.ResponseExtra = userResponse.ResponseExtra;
                            responseToAdd.Id = userResponse.Id;
                            responseToAdd.UserSelectedResponseText = "";
                            responseToAdd.UserSelectCorrectAnswer = autoCorrect && userResponse.ResponseId == questionResponse.Id && questionResponse.Correct == "1";
                            
                            if (!string.IsNullOrWhiteSpace(userResponse.ResponseId))
                            {
                                foreach (var quizQuestionResponse in from questionResponseToFindUserResponse in quizRequestModel.QuestionRequestModelList where questionResponseToFindUserResponse.QuizQuestionResponseModelList != null &&
                                             questionResponseToFindUserResponse.QuizQuestionResponseModelList.Count > 0 from quizQuestionResponse in questionResponseToFindUserResponse
                                             .QuizQuestionResponseModelList where userResponse.ResponseId == quizQuestionResponse.Id select quizQuestionResponse)
                                {
                                    responseToAdd.UserSelectedResponseText = quizQuestionResponse.Response;
                                }
                            }
                        }
                    }

                    questionQuizToAdd.QuestionResponses.Add(responseToAdd);
                }

                // If there are no responses, the question could be opened and we should return the open answer to that question. 
                if (question.QuizQuestionResponseModelList.Count == 0)
                {
                    var userResponse =
                        userQuizResponseModel?.UserResponsesList.Find(x =>
                            x.QuestionId == question.QuizQuestionModel.Id);
                    if (userResponse != null)
                    {
                        var responseToAdd = new QuestionResponse
                        {
                            AssignedValue = userResponse.AssignedValue,
                            ResponseExtra = userResponse?.ResponseExtra,
                            Id = userResponse?.Id,
                        };

                        questionQuizToAdd.QuestionResponses.Add(responseToAdd);
                    }
                }

                userQuizFullDetailModel.MaxScore = maxScore;
                userQuizFullDetailModel.QuestionQuizList.Add(questionQuizToAdd);
            }
            
            return userQuizFullDetailModel;
        }

		/// <summary>
		/// Returns the quiz with all details.
		/// </summary>
		/// <param name="quizId">The id of the quiz.</param>
		/// <param name="isForOutsideSource">Verifies if it is for outside source. If true, then the image src would be replace by full URL. To make this work, replaceImgSrcByFullURL must be true. </param>
		/// <param name="userInSessionId">The user id of the user making the request.</param>
		/// <param name="callAzure">To call azure for quizzes attachments.</param>/// 
		/// <returns>Returns the quiz details.</returns>
		public QuizRequestModel GetQuiz(string quizId, bool isForOutsideSource, string userInSessionId = null, bool callAzure = true)
        {
            QuizRequestModel quizRequestModel = new QuizRequestModel();
            quizRequestModel.QuizAttachmentModelList = new List<QuizAttachmentModel>();

            QuizModel quizModelToFind = new QuizModel
            {
                Id = quizId,
                Available = "1",
                ModifiedBy = userInSessionId
            };

            QuizDAO quizDAO = new QuizDAO();
            QuizModel quizModelResult = new QuizModel();
            quizModelResult = quizDAO.Read(_connectionStringRead, quizModelToFind, true, true)[0];

            // Check if quiz is auto correct or not 
            List<QuizTypeModel> quizTypeModels = new QuizTypeDAO().Read(_connectionStringRead, new QuizTypeModel()
            {
                Id = quizModelResult.QuizTypeId,
                AutoCorrect = "1",
            }, true, true);

            if (quizTypeModels.Count == 0)
            {
                quizRequestModel.AutoCorrect = false;
            }
            else
            {
                quizRequestModel.AutoCorrect = true;
            }


            // Prepare quiz exta info model list, the related terms, and the questions, 
            // if needed 
            List<QuizExtraInfoModel> quizExtraInfoModelListToFind = new List<QuizExtraInfoModel>();
            List<QuizQuestionModel> quizQuestionModelListToFind = new List<QuizQuestionModel>();
            List<QuizAttachmentModel> quizAttachmentModelsToFind = new List<QuizAttachmentModel>();
            List<QuizRubricModel> quizRubricModelsToFind = new List<QuizRubricModel>();

            quizRequestModel.QuizModel = new QuizEditModel(quizModelResult);
            if (!string.IsNullOrWhiteSpace(quizRequestModel.QuizModel.Title))
                quizRequestModel.QuizModel.Title = Replacer.ReplaceDoubleQuotesBySingleQuote
                (quizRequestModel.QuizModel.Title);

			if (!string.IsNullOrWhiteSpace(quizRequestModel.QuizModel.Instruction))
			{
				quizRequestModel.QuizModel.Instruction = Replacer.ReplaceDoubleQuotesBySingleQuote(quizRequestModel.QuizModel.Instruction);
				if (isForOutsideSource)
					quizRequestModel.QuizModel.Instruction = Replacer.ReplaceSrc(quizRequestModel.QuizModel.Instruction);
			}
			quizExtraInfoModelListToFind.Add(new QuizExtraInfoModel() { QuizId = quizId });
            quizQuestionModelListToFind.Add(new QuizQuestionModel() { QuizId = quizId });
            quizAttachmentModelsToFind.Add(new QuizAttachmentModel() { QuizId = quizId });
            quizRubricModelsToFind.Add(new QuizRubricModel() { QuizId = quizId });

            // Now call all the details 
            List<QuizQuestionModel> quizQuestionModelResult = new QuizQuestionDAO().Read(_connectionStringRead,
                quizQuestionModelListToFind);


            // Now we have to find the criterion, related terms, responses 
            List<QuizQuestionRubricModel> quizQuestionRubricModelListToFind = new List<QuizQuestionRubricModel>();
            List<QuizQuestionRubricCriterionModel> quizQuestionCriterionModelListToFind = new List<QuizQuestionRubricCriterionModel>();
            List<QuizQuestionRubricCriterionValueModel> quiezQuestionRubricCriterionValueModelListToFind = new List<QuizQuestionRubricCriterionValueModel>();
            List<QuizQuestionResponseModel> quizQuestionReponseModelListToFind = new List<QuizQuestionResponseModel>();

            foreach (QuizQuestionModel quizQuestion in quizQuestionModelResult)
            {
                quizQuestionRubricModelListToFind.Add(new QuizQuestionRubricModel()
                {
                    QuestionId = quizQuestion.Id
                });

                quizQuestionReponseModelListToFind.Add(new QuizQuestionResponseModel()
                {
                    QuizQuestionId = quizQuestion.Id
                });

				if (isForOutsideSource && !string.IsNullOrWhiteSpace(quizQuestion.Desc))
				{
					quizQuestion.Desc = Replacer.ReplaceDoubleQuotesBySingleQuote(quizQuestion.Desc);
					quizQuestion.Desc = Replacer.ReplaceSrc(quizQuestion.Desc);

				}
				if (!string.IsNullOrWhiteSpace(quizQuestion.Desc))
                    quizQuestion.Desc = Replacer.MultilineToSingleLine(quizQuestion.Desc);
            }

            List<QuizQuestionRubricModel> quizQuestionRubricModelListResult = new List<QuizQuestionRubricModel>();
            List<QuizQuestionResponseModel> quizQuestionReponseModelListResult = new List<QuizQuestionResponseModel>();
            List<QuizQuestionRubricCriterionModel> quizQuestionCriterionModelListResult = new List<QuizQuestionRubricCriterionModel>();
            List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValueModelListResult = new List<QuizQuestionRubricCriterionValueModel>();

            quizQuestionRubricModelListResult = new QuizQuestionRubricDAO().
                Read(_connectionStringRead, quizQuestionRubricModelListToFind);
            quizQuestionReponseModelListResult = new QuizQuestionResponseDAO().
                Read(_connectionStringRead, quizQuestionReponseModelListToFind);

            if (quizQuestionRubricModelListResult.Count > 0)
            {
                // Find the rubric details
                foreach (QuizQuestionRubricModel rubric in quizQuestionRubricModelListResult)
                {
                    quizQuestionCriterionModelListToFind.Add(new QuizQuestionRubricCriterionModel()
                    {
                        QuizQuestionRubricId = rubric.Id
                    });
                }

                // Find the  criterions 
                if (quizQuestionCriterionModelListToFind.Count > 0)
                {
                    quizQuestionCriterionModelListResult = new QuizQuestionRubricCriterionDAO().Read(_connectionStringRead,
                        quizQuestionCriterionModelListToFind);

                    // Find the values if any 
                    if (quizQuestionCriterionModelListResult.Count > 0)
                    {
                        foreach (QuizQuestionRubricCriterionModel criterion in quizQuestionCriterionModelListResult)
                        {
                            quiezQuestionRubricCriterionValueModelListToFind.Add(
                                new QuizQuestionRubricCriterionValueModel()
                                {
                                    QuizQuestionRubricCriterionId = criterion.Id
                                });
                        }

                        if (quiezQuestionRubricCriterionValueModelListToFind.Count > 0)
                        {
                            quizQuestionRubricCriterionValueModelListResult = new QuizQuestionRubricCriterionValueDAO().
                                Read(_connectionStringRead, quiezQuestionRubricCriterionValueModelListToFind);
                        }
                    }
                }
            }

            // Prepare the question question result 
            List<QuestionRequestModel> questionRequestModelList = new List<QuestionRequestModel>();
            foreach (QuizQuestionModel quizQuestion in quizQuestionModelResult)
            {
                // Prepare the question (with rubric pending) 
                QuestionRequestModel questionToAttach = new QuestionRequestModel()
                {
                    QuizQuestionModel = quizQuestion,
                    QuizQuestionResponseModelList =
                    quizQuestionReponseModelListResult.FindAll(x => x.QuizQuestionId == quizQuestion.Id),
                    QuizQuestionRubricModelList = new List<QuestionRubricRequestModel>()
                };

                if (isForOutsideSource)
                {
                    if (questionToAttach.QuizQuestionResponseModelList != null && questionToAttach.QuizQuestionResponseModelList.Count > 0)
                    {
						// Replace
						foreach (QuizQuestionResponseModel response in questionToAttach.QuizQuestionResponseModelList)
						{
							if (!string.IsNullOrWhiteSpace(response.Response))
							{
								response.Response = Replacer.ReplaceDoubleQuotesBySingleQuote(response.Response);
								response.Response = Replacer.ReplaceSrc(response.Response);
							}
							if (!string.IsNullOrWhiteSpace(response.Response))
								response.Response = Replacer.MultilineToSingleLine(response.Response);

						}
					}
                }

                // Check if the question has rubrics 
                List<QuizQuestionRubricModel> quizQuestionRubricModels =
                    quizQuestionRubricModelListResult.FindAll(x => x.QuestionId == quizQuestion.Id);

                if (quizQuestionRubricModels != null && quizQuestionRubricModels.Count > 0)
                {
                    List<QuestionRubricRequestModel> questionRubricRequestModelsToAttach = new List<QuestionRubricRequestModel>();
                    foreach (QuizQuestionRubricModel rubric in quizQuestionRubricModels)
                    {
                        QuestionRubricRequestModel questionRubricRequestModel = new QuestionRubricRequestModel()
                        {
                            Rubric = !string.IsNullOrWhiteSpace(rubric.Rubric) ? Replacer.ReplaceHTMLEntities(rubric?.Rubric) : "",
                            Id = rubric.Id,
                            QuestionId = rubric.QuestionId,
                            ModifiedBy = rubric.ModifiedBy,
                            CreatedAt = rubric.CreatedAt,
                            UpdatedAt = rubric.UpdatedAt,
                            QuestionRubricCriterionModelList = new List<QuestionRubricCriterionRequestModel>()
                        };

						if (isForOutsideSource && !string.IsNullOrWhiteSpace(questionRubricRequestModel.Rubric))
						{
							questionRubricRequestModel.Rubric = Replacer.ReplaceDoubleQuotesBySingleQuote(questionRubricRequestModel?.Rubric);
							questionRubricRequestModel.Rubric = Replacer.ReplaceSrc(questionRubricRequestModel?.Rubric);

						}
						if (!string.IsNullOrWhiteSpace(questionRubricRequestModel.Rubric))
                            questionRubricRequestModel.Rubric = Replacer.MultilineToSingleLine(questionRubricRequestModel.Rubric);

                        // Find the criterions 
                        List<QuizQuestionRubricCriterionModel> criterionRequestModelList = new List<QuizQuestionRubricCriterionModel>();
                        criterionRequestModelList = quizQuestionCriterionModelListResult.FindAll(x => x.QuizQuestionRubricId == rubric.Id);
                        if (criterionRequestModelList != null && criterionRequestModelList.Count > 0)
                        {
                            foreach (QuizQuestionRubricCriterionModel criterionRequest in criterionRequestModelList)
                            {
                                QuestionRubricCriterionRequestModel questionRubricCriterionRequestModel = new QuestionRubricCriterionRequestModel()
                                {
                                    Id = criterionRequest.Id,
                                    CreatedAt = criterionRequest.CreatedAt,
                                    UpdatedAt = criterionRequest.UpdatedAt,
                                    ModifiedBy = criterionRequest.ModifiedBy,
                                    QuizQuestionRubricId = criterionRequest.QuizQuestionRubricId,
                                    Criterion = criterionRequest.Criterion,
                                    QuestionRubricCriterionValueModelList = new List<QuizQuestionRubricCriterionValueModel>()
                                };

                                // And finally attach the children 
                                List<QuizQuestionRubricCriterionValueModel> criterionValuesList = new List<QuizQuestionRubricCriterionValueModel>();
                                criterionValuesList = quizQuestionRubricCriterionValueModelListResult.FindAll(x => x.QuizQuestionRubricCriterionId == criterionRequest.Id);
                                if (criterionValuesList != null && criterionValuesList.Count > 0)
                                {
                                    questionRubricCriterionRequestModel.QuestionRubricCriterionValueModelList = new List<QuizQuestionRubricCriterionValueModel>();
                                    foreach (QuizQuestionRubricCriterionValueModel criterionValue in criterionValuesList)
                                    {
                                        QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValueModelToAdd =
                                            new QuizQuestionRubricCriterionValueModel()
                                            {
                                                CreatedAt = criterionValue.CreatedAt,
                                                Id = criterionValue.Id,
                                                ModifiedBy = criterionValue.ModifiedBy,
                                                UpdatedAt = criterionValue.UpdatedAt,
                                                Value = criterionValue.Value,
                                                QuizQuestionRubricCriterionId = criterionValue?.QuizQuestionRubricCriterionId,
                                                Desc = criterionValue.Desc
                                            };

                                        if (!string.IsNullOrWhiteSpace(criterionValue.Desc) && isForOutsideSource)
                                        {
                                            quizQuestionRubricCriterionValueModelToAdd.Desc =
                                            Replacer.ReplaceDoubleQuotesBySingleQuote(quizQuestionRubricCriterionValueModelToAdd.Desc);

                                            quizQuestionRubricCriterionValueModelToAdd.Desc =
                                            Replacer.ReplaceHTMLEntities(quizQuestionRubricCriterionValueModelToAdd.Desc);

											quizQuestionRubricCriterionValueModelToAdd.Desc =
											Replacer.ReplaceSrc(quizQuestionRubricCriterionValueModelToAdd.Desc);
										}
                                        questionRubricCriterionRequestModel.QuestionRubricCriterionValueModelList.Add(
                                            quizQuestionRubricCriterionValueModelToAdd);
                                    }
                                }

                                // And finally attach que questionRubric 
                                questionRubricRequestModel.QuestionRubricCriterionModelList.Add(questionRubricCriterionRequestModel);
                            }
                        }
                        questionToAttach.QuizQuestionRubricModelList.Add(questionRubricRequestModel);
                    }
                }

                questionRequestModelList.Add(questionToAttach);
            }

            // Call the other details left 
            List<QuizExtraInfoModel> quizExtraInfoModelsResult = new List<QuizExtraInfoModel>();
            List<QuizAttachmentModel> quizAttachmentModelsResult = new List<QuizAttachmentModel>();
            List<QuizRubricModel> quizRubricModelsResult = new List<QuizRubricModel>();
            quizAttachmentModelsResult = new QuizAttachmentDAO().Read(_connectionStringRead,
                quizAttachmentModelsToFind);
            quizRubricModelsResult = new QuizRubricDAO().Read(_connectionStringRead, quizRubricModelsToFind);

            // And finally attach the question to quiz                 

            quizRequestModel.QuizExtraInfoModel =
                    quizExtraInfoModelsResult.Find(x => x.QuizId == quizRequestModel.QuizModel.Id);
            quizRequestModel.QuestionRequestModelList = questionRequestModelList.FindAll(
                    x => x.QuizQuestionModel.QuizId == quizRequestModel.QuizModel.Id);
            quizRequestModel.QuizRubricModel = quizRubricModelsResult.Find(x => x.QuizId == quizRequestModel.QuizModel.Id);

			if (quizAttachmentModelsResult.Count > 0)
			{
				quizRequestModel.QuizAttachmentModelList = quizAttachmentModelsResult;
				foreach (QuizAttachmentModel attachment in quizRequestModel.QuizAttachmentModelList)
				{
					string fileName = Path.GetFileName(attachment.AttachmentUrl);
					string filePath = $"{AwsAssets.FolderPath}{fileName}";
					string signedUrl = AwsAssets.GetSignedUrlFronCloudFront(filePath);
					attachment.AttachmentUrl = signedUrl;
				}
			}

			if (quizRequestModel.QuizExtraInfoModel != null)
            {
				if (!string.IsNullOrWhiteSpace(quizRequestModel.QuizExtraInfoModel.ExtraInfo) && isForOutsideSource)
				{
					quizRequestModel.QuizExtraInfoModel.ExtraInfo =
						Replacer.ReplaceDoubleQuotesBySingleQuote(quizRequestModel.QuizExtraInfoModel.ExtraInfo);
					quizRequestModel.QuizExtraInfoModel.ExtraInfo =
						Replacer.ReplaceHTMLEntities(
							Replacer.ReplaceSrc(quizRequestModel.QuizExtraInfoModel.ExtraInfo));

				}
				if (!string.IsNullOrWhiteSpace(quizRequestModel.QuizExtraInfoModel.ExtraInfo))
                    quizRequestModel.QuizExtraInfoModel.ExtraInfo = Replacer.MultilineToSingleLine(quizRequestModel.QuizExtraInfoModel.ExtraInfo);
            }

            if (quizRequestModel.QuizRubricModel != null)
            {
				if (!string.IsNullOrWhiteSpace(quizRequestModel.QuizRubricModel.Desc) && isForOutsideSource)
				{
					quizRequestModel.QuizRubricModel.Desc =
						Replacer.ReplaceDoubleQuotesBySingleQuote(quizRequestModel.QuizRubricModel.Desc);
					quizRequestModel.QuizRubricModel.Desc =
						Replacer.ReplaceHTMLEntities(
							Replacer.ReplaceSrc(quizRequestModel.QuizRubricModel.Desc));

				}
				if (!string.IsNullOrWhiteSpace(quizRequestModel.QuizRubricModel.Desc))
                    quizRequestModel.QuizRubricModel.Desc = Replacer.MultilineToSingleLine(quizRequestModel.QuizRubricModel.Desc);
            }
            return quizRequestModel;
        }

        /// <summary>
        /// To get the quiz attachments.
        /// </summary>
        /// <param name="quizId">The id of the quiz.</param>
        /// <param name="callAzure">To call azure for quizzes attachments.</param>
        /// <returns>Returns the quiz attachments.</returns>
        public List<QuizAttachmentModel> GetQuizAttachments(string quizId, bool callAzure = true)
        {
            var quizAttachmentModelsResult = new QuizAttachmentDAO().Read(_connectionStringRead,
                new QuizAttachmentModel()
                {
                    QuizId = quizId
                }, false, false);
            
            if (quizAttachmentModelsResult.Count > 0 && callAzure)
            {
                foreach (var attachment in quizAttachmentModelsResult)
                {
                    var fileName = Path.GetFileName(attachment.AttachmentUrl);
                    var signedUrl = AwsAssets.GetSignedUrlFronCloudFront(fileName);
                    attachment.AttachmentUrl = signedUrl;
                }
            }

            return quizAttachmentModelsResult;
        }

        /// <summary>
        /// To get quiz attachments URLs.
        /// </summary>
        /// <param name="quizId">The quiz id.</param>
        /// <returns>Returns a list of attachments.</returns>
        public List<string> GetQuizAttachmentsURLs(string quizId)
        {
            List<string> attachmentsURL = new List<string>();
            List<QuizAttachmentModel> quizAttachmentModels = new QuizAttachmentDAO().Read(_connectionStringRead,
                new QuizAttachmentModel()
                {
                    QuizId = quizId,
                }, false, false);

            if (quizAttachmentModels.Count > 0)
            {
                foreach (QuizAttachmentModel quizAttachmentModel in quizAttachmentModels)
                {
                    string fileName = Path.GetFileName(quizAttachmentModel.AttachmentUrl);
                    string filePath = $"{AwsAssets.FolderPath}{fileName}";
                    string signedUrl = AwsAssets.GetSignedUrlFronCloudFront(filePath);
                    attachmentsURL.Add(signedUrl);
                }
            }

            return attachmentsURL;
        }

        /// <summary>
        /// Return the quiz models. (Custom assignments).
        /// </summary>
        /// <param name="quizModels">Quiz models list to find.</param>
        /// <returns>Returns the quiz model list.</returns>
        public List<QuizModel> GetQuizModels(List<QuizModel> quizModels)
        {
            var quizzes = new QuizDAO().Read(_connectionStringRead, quizModels);
            //if not empty, order by created at
            if (quizzes.Count > 0)
            {
                quizzes = quizzes.OrderByDescending(x => DateTime.Parse(x.CreatedAt)).ToList();
            }

            return quizzes;
        }

        /// <summary>
        /// To get the quiz model with can be edited evaluation.
        /// </summary>
        /// <param name="quizModels">Quiz models to filter.</param>
        /// <returns>Returns the quiz model with can be edited property.</returns>
        public List<QuizEditModel> GetQuizModelsWithCanBeEditedEvaluation(List<QuizModel> quizModels)
        {
            var quizzesEditModels = new List<QuizEditModel>();
            var quizzes = new QuizDAO().Read(_connectionStringRead, quizModels);
            //if not empty, order by created at
            if (quizzes.Count > 0)
            {
                quizzes = quizzes.OrderByDescending(x => DateTime.Parse(x.CreatedAt)).ToList();

                var planningQuizzesToCheck =
                    quizzes.Select(quiz => new TeacherPlanQuizModel() { QuizId = quiz.Id }).ToList();

                // Check if they are attached to a planning. 
                var planningQuizzes = new TeacherPlanQuizDAO().Read(_connectionStringRead, planningQuizzesToCheck);

                quizzesEditModels.AddRange(quizzes.Select(quiz => new QuizEditModel()
                {
                    Id = quiz.Id,
                    Available = quiz.Available,
                    Instruction = quiz.Instruction,
                    ReadyToPublish = quiz.ReadyToPublish,
                    CustomValue = quiz.CustomValue,
                    IsExam = quiz.IsExam,
                    Title = quiz.Title,
                    CreatedAt = quiz.CreatedAt,
                    UpdatedAt = quiz.UpdatedAt,
                    CanBeEdited = planningQuizzes.Find(x => x.QuizId == quiz.Id) == null,
                }));
            }

            return quizzesEditModels;
        }

        /// <summary>
		/// To get the quizzes' types (match, selection, etc).
		/// </summary>
		/// <returns>Returns a list of quiz types.</returns>
		public List<QuizTypeModel> GetQuizType()
        {
            return new QuizTypeDAO().Read(_connectionStringRead, new QuizTypeModel() { }, false, false);
        }

        /// <summary>
        /// Get quiz type models by filtering.
        /// </summary>
        /// <param name="quizTypeModels">The quiz types models to filter.</param>
        /// <returns>Return a list of quiz types.</returns>
        public List<QuizTypeModel> GetQuizType(List<QuizTypeModel> quizTypeModels)
        {
            return new QuizTypeDAO().Read(_connectionStringRead, quizTypeModels);
        }

        /// <summary>
        /// Checks whether a quiz can be edited or not.
        /// </summary>
        /// <param name="quizId">The id of the quiz.</param>
        /// <returns>Returns true if can be edited, else, false.</returns>
        public bool CheckIfQuizCanBeEdited(string quizId)
        {
            var planningQuizzes = new TeacherPlanQuizDAO().Read(_connectionStringRead,
                new Domain.Models.Teachers.TeacherPlanQuizModel()
                {
                    QuizId = quizId
                }, true, true);

            return planningQuizzes == null || planningQuizzes.Count <= 0;
        }
    }
}
