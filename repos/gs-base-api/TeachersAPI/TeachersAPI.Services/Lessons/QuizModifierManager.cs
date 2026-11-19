using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TeachersAPI.TeachersAPI.Services.Lessons.CustomClasses.HTMLSanitizer;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI.Domain.Models.Lessons.Request;
using TeachersAPI.TeachersAPI.Services.Generals;
using HtmlAgilityPack;
using TeachersAPI.Domain.Models.Generals.Dapper;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.UserQuizzes;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.Utils;
using ResponseModel = TeachersAPI.Domain.Models.Lessons.Response.ResponseModel;
using TeacherPlanQuizModel = TeachersAPI.Domain.Models.Generals.Dapper.TeacherPlanQuizModel;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.Lessons
{
    /// <summary>
    /// To handle the quizzes (custom assignments created by the teacher.
    /// </summary>
    public class QuizModifierManager
    {
        private readonly string _connectionString, _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language code.</param>
        public QuizModifierManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

		/// <summary>
		/// Creates a custom quiz (homework) by the teacher.
		/// </summary>
		/// <param name="quizRequestModel">Quiz request model to create that include the title, instructions and so on...</param>
		/// <param name="userCreatedBy">The user that is making the request.</param>
		/// <returns>Returns a HTTP Response with transaction details.</returns>
		public QuizResponseModel CreateSimpleQuiz(QuizRequestModel quizRequestModel, string userCreatedBy)
        {
            QuizResponseModel responseModel = new QuizResponseModel
            {
                Valid = true,
                Messages = new List<string>()
            };

            quizRequestModel.QuizModel.Instruction = UnsignUrlFromHtmlSourceCode(quizRequestModel.QuizModel.Instruction);

            QuizDAO quizDAO = new QuizDAO();
            QuizQuestionResponseDAO quizQuestionResponseDAO = new QuizQuestionResponseDAO();
            quizRequestModel.QuizModel.ModifiedBy = userCreatedBy;
            quizRequestModel.QuizModel.Id = quizDAO.Create(_connectionString, quizRequestModel.QuizModel)[0].Id;
			// Set the images if any 
			if (!string.IsNullOrWhiteSpace(quizRequestModel.QuizModel.Instruction))
			{
				quizRequestModel.QuizModel.Instruction = Replacer.ReplaceSrc(quizRequestModel.QuizModel.Instruction);
			}

			if (quizRequestModel.QuizAttachmentModelList != null
               && quizRequestModel.QuizAttachmentModelList.Count > 0)
            {
                foreach (QuizAttachmentModel attachment in quizRequestModel.QuizAttachmentModelList)
                {
                    QuizAttachmentModel attachmentModel = CreateAttachmentURL(attachment, quizRequestModel.QuizModel.Id, userCreatedBy);
                    attachment.Id = attachmentModel.Id;
                    attachment.QuizId = attachmentModel.QuizId;
                }
            }

            if (quizRequestModel.QuizRubricModel != null)
            {
                responseModel.QuizRubricModel = QuizSimpleRubricCreate(quizRequestModel.QuizRubricModel, quizRequestModel.QuizModel.Id, userCreatedBy);
            }

            // Create QuizMatchInstructions
            QuizMatchInstructionsDAO quizMatchInstructionsDAO = new QuizMatchInstructionsDAO();
            if (quizRequestModel.QuizMatchInstructionsModel != null)
            {
                foreach (var matchInstructions in quizRequestModel.QuizMatchInstructionsModel)
                {
                    // Check if match instruction is new or existing
                    List<QuizMatchInstructionsModel> existingMatchInstruction =
                        new List<QuizMatchInstructionsModel>();

                    existingMatchInstruction = quizMatchInstructionsDAO.Read(_connectionStringRead,
                        new QuizMatchInstructionsModel() { Id = matchInstructions.Id }, false, false);

                    int parsedId;
                    if (!int.TryParse(matchInstructions.Id, out parsedId) || existingMatchInstruction == null ||
                        existingMatchInstruction.Count == 0)
                    {
                        // save id to change on questions
                        var matchInstructionId = matchInstructions.Id;
                        // remove actual id
                        matchInstructions.Id = null;
                        // create new match instruction
                        matchInstructions.QuizId = quizRequestModel.QuizModel.Id;
                        matchInstructions.ModifiedBy = userCreatedBy;
                        matchInstructions.MatchGroupNumber = "1";
                        matchInstructions.Id =
                            quizMatchInstructionsDAO.Create(_connectionString, matchInstructions)[0].Id;

                        // update quizMatchInstructionsId in questions that have the same id
                        foreach (var questionRequest in quizRequestModel.QuestionRequestModelList)
                        {
                            if (questionRequest.QuizQuestionModel.QuizMatchInstructionId == matchInstructionId)
                            {
                                questionRequest.QuizQuestionModel.QuizMatchInstructionId = matchInstructions.Id;
                            }
                        }
                    }
                    else
                    {
                        // update existing math instruction
                        quizMatchInstructionsDAO.Update(_connectionString, matchInstructions,
                            new QuizMatchInstructionsModel() { Id = matchInstructions.Id });
                    }
                }
            }

            if (quizRequestModel.QuestionRequestModelList != null &&
                quizRequestModel.QuestionRequestModelList.Count > 0)
            {
                foreach (QuestionRequestModel questionRequest in quizRequestModel.QuestionRequestModelList)
                {

                    // Insert question now. You need the ID for the responses. 
                    QuizQuestionModel quizQuestionAdded = QuestionCreate(
                                    questionRequest.QuizQuestionModel,
                                    quizRequestModel.QuizModel.Id,
                                    userCreatedBy);

                    // Set the ID to return it
                    questionRequest.QuizQuestionModel.Id = quizQuestionAdded.Id;
					if (questionRequest.QuizQuestionModel != null && !string.IsNullOrWhiteSpace(questionRequest.QuizQuestionModel.Desc))
					{
						questionRequest.QuizQuestionModel.Desc = Replacer.ReplaceSrc(questionRequest.QuizQuestionModel.Desc);
					}

					// Now insert the responses, which are new
					if (questionRequest.QuizQuestionResponseModelList != null && questionRequest.QuizQuestionResponseModelList.Count > 0)
                    {

                        foreach (QuizQuestionResponseModel questionResponse in questionRequest.QuizQuestionResponseModelList)
                        {
							questionResponse.Response = UnsignUrlFromHtmlSourceCode(questionResponse.Response);
							questionResponse.ModifiedBy = userCreatedBy;
                            questionResponse.QuizQuestionId = questionRequest.QuizQuestionModel.Id;
                            // Return the IDs. They are needed for the FE. 
                            questionResponse.Id = quizQuestionResponseDAO.Create(_connectionString, questionResponse)[0].Id;

							if (!string.IsNullOrWhiteSpace(questionResponse.Response))
							{
								questionResponse.Response = Replacer.ReplaceSrc(questionResponse.Response);
							}
						}
                    }
                }
            }

            MessageManager messageManager = new MessageManager(_connectionStringRead,
              _langCode);
            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ls_message_code_success"]));
            responseModel.QuestionRequestModelList = quizRequestModel.QuestionRequestModelList;
            responseModel.QuizAttachmentModelList = quizRequestModel.QuizAttachmentModelList;
            responseModel.QuizMatchInstructionsModel = quizRequestModel.QuizMatchInstructionsModel;
            responseModel.QuizModel = quizRequestModel.QuizModel;
            return responseModel;

        }

        /// <summary>
        /// To duplicate a simple quiz.
        /// </summary>
        /// <param name="quizId">The id of the quiz.</param>
        /// <param name="modifiedBy">The user that is modifying the row.</param>
        /// <returns>Returns the quiz response model details.</returns>
        public Domain.Models.Generals.Responses.QuizResponseModel DuplicateSimpleQuiz(string quizId, string modifiedBy)
        {
            var quizResponseModel = new Domain.Models.Generals.Responses.QuizResponseModel();

            // First, we check if the quiz id is not null
            if (string.IsNullOrWhiteSpace(quizId))
            {
                return new Domain.Models.Generals.Responses.QuizResponseModel()
                {
                    TransactionValid = false,
                    SessionValid = true,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:ls_message_error_quiz_id_required"])
                    }
                };
            }

            // Then, we retrieve the quiz details.
            var quizManager = new QuizReaderManager(_connectionStringRead, _langCode);
            var quizDetails = quizManager.GetQuiz(quizId, false, modifiedBy, false);

            // If the quiz details are null, then we return an error.
            if (quizDetails == null)
            {
                return new Domain.Models.Generals.Responses.QuizResponseModel()
                {
                    TransactionValid = false,
                    SessionValid = true,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:ls_message_error_quiz_id_required"])
                    }
                };
            }

            // Delete all the ids because we are going to create a new one based on the old one.
            quizDetails.QuizModel.Id = null;
            quizDetails.QuizModel.CreatedAt = null;
            quizDetails.QuizModel.UpdatedAt = null;
            quizDetails.QuizModel.ModifiedBy = modifiedBy;
            quizDetails.QuizModel.Available = "1";
            quizDetails.QuizModel.ReadyToPublish = "0";
            quizDetails.QuizModel.CanBeEdited = true;

            if (quizDetails.QuizRubricModel != null)
            {
                quizDetails.QuizRubricModel.QuizId = null;
                quizDetails.QuizRubricModel.Id = null;
                quizDetails.QuizRubricModel.CreatedAt = null;
                quizDetails.QuizRubricModel.UpdatedAt = null;
                quizDetails.QuizRubricModel.ModifiedBy = modifiedBy;
            }

            if (quizDetails.QuizAttachmentModelList != null && quizDetails.QuizAttachmentModelList.Count > 0)
            {
                foreach (var attachment in quizDetails.QuizAttachmentModelList)
                {
                    attachment.Id = null;
                    attachment.CreatedAt = null;
                    attachment.UpdatedAt = null;
                    attachment.ModifiedBy = modifiedBy;
                    attachment.QuizId = null;
                }
            }

            Dictionary<string, string> matchInstructionIdMap = new Dictionary<string, string>();

            if (quizDetails.QuestionRequestModelList != null && quizDetails.QuestionRequestModelList.Count > 0)
            {
                foreach (var question in quizDetails.QuestionRequestModelList)
                {
                    question.QuizQuestionModel.Id = null;
                    question.QuizQuestionModel.CreatedAt = null;
                    question.QuizQuestionModel.UpdatedAt = null;
                    question.QuizQuestionModel.ModifiedBy = modifiedBy;
                    question.QuizQuestionModel.QuizId = null;

                    if (question.QuizQuestionResponseModelList != null &&
                        question.QuizQuestionResponseModelList.Count > 0)
                    {
                        foreach (var response in question.QuizQuestionResponseModelList)
                        {
                            response.Id = null;
                            response.CreatedAt = null;
                            response.UpdatedAt = null;
                            response.ModifiedBy = modifiedBy;
                            response.QuizQuestionId = null;
                        }
                    }


                    // validate if question has match instructions and
                    // if it is first question with match instructions on quizDetail, create it and assign the new match id to the question
                    // if it is not the first question with match instructions 
                    // assign the new match id to the question
                    if (question.QuizQuestionModel.QuizMatchInstructionId != null)
                    {
                        // if it is the first question with this match instructions create it
                        if (matchInstructionIdMap.ContainsKey(question.QuizQuestionModel.QuizMatchInstructionId))
                        {
                            question.QuizQuestionModel.QuizMatchInstructionId =
                                matchInstructionIdMap[question.QuizQuestionModel.QuizMatchInstructionId];
                        }
                        else
                        {
                            // get the match instruction from the old question
                            QuizMatchInstructionsModel matchInstruction = new QuizMatchInstructionsDAO().Read(
                                _connectionStringRead, new QuizMatchInstructionsModel()
                                {
                                    Id = question.QuizQuestionModel.QuizMatchInstructionId
                                }, false, false)[0];

                            Guid newUuid = Guid.NewGuid();

                            QuizMatchInstructionsModel duplicatedInstruction = new QuizMatchInstructionsModel()
                            {
                                QuizId = null,
                                MatchGroupNumber = matchInstruction.MatchGroupNumber,
                                ModifiedBy = modifiedBy,
                                Instruction = matchInstruction.Instruction,
                            };

                            // create the new match instruction

                            duplicatedInstruction.Id = newUuid.ToString();

                            matchInstructionIdMap.Add(question.QuizQuestionModel.QuizMatchInstructionId,
                                duplicatedInstruction.Id);

                            quizDetails.QuizMatchInstructionsModel.Add(duplicatedInstruction);

                            question.QuizQuestionModel.QuizMatchInstructionId =
                                matchInstructionIdMap[question.QuizQuestionModel.QuizMatchInstructionId];
                        }
                    }

                    if (question.QuizQuestionRubricModelList != null && question.QuizQuestionRubricModelList.Count > 0)
                    {
                        foreach (var rubric in question.QuizQuestionRubricModelList)
                        {
                            rubric.Id = null;
                            rubric.CreatedAt = null;
                            rubric.UpdatedAt = null;
                            rubric.ModifiedBy = modifiedBy;
                            rubric.QuestionId = null;

                            if (rubric.QuestionRubricCriterionModelList != null &&
                                rubric.QuestionRubricCriterionModelList.Count > 0)
                            {
                                foreach (var criterion in rubric.QuestionRubricCriterionModelList)
                                {
                                    criterion.Id = null;
                                    criterion.CreatedAt = null;
                                    criterion.UpdatedAt = null;
                                    criterion.ModifiedBy = modifiedBy;
                                    criterion.QuizQuestionRubricId = null;

                                    if (criterion.QuestionRubricCriterionValueModelList != null &&
                                        criterion.QuestionRubricCriterionValueModelList.Count > 0)
                                    {
                                        foreach (var criterionValue in criterion.QuestionRubricCriterionValueModelList)
                                        {
                                            criterionValue.Id = null;
                                            criterionValue.CreatedAt = null;
                                            criterionValue.UpdatedAt = null;
                                            criterionValue.ModifiedBy = modifiedBy;
                                            criterionValue.QuizQuestionRubricCriterionId = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Now, we create the quiz
            var quizResponse = CreateSimpleQuiz(quizDetails, modifiedBy);

            if (!quizResponse.Valid)
            {
                return new Domain.Models.Generals.Responses.QuizResponseModel()
                {
                    TransactionValid = false,
                    SessionValid = true,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:tm_message_homework_cannot_create_duplicate"])
                    }
                };
            }

            quizResponseModel.QuizRequestModel = new QuizRequestModel()
            {
                QuizAttachmentModelList = quizResponse?.QuizAttachmentModelList,
                QuizModel = quizResponse.QuizModel,
                QuestionRequestModelList = quizResponse.QuestionRequestModelList,
                QuizRubricModel = quizResponse.QuizRubricModel,
            };
            quizResponseModel.Messages = quizResponse.Messages;

            quizResponseModel.TransactionValid = true; // Remove this after update in frontend that evaluate this.
            quizResponseModel.SessionValid = true; // Remove this after update in frontend that evaluate this.
            return quizResponseModel;
        }


		/// <summary>
		/// Replace signed url with store path file.
		/// </summary>
		/// <param name="html">string html property value.</param>
		/// <returns>Modified html as string.</returns>
		private string UnsignUrlFromHtmlSourceCode(string html)
		{
			if (string.IsNullOrWhiteSpace(html))
				return html;

			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			// Find all elements with the "src" attribute
			var elementsWithSrc = doc.DocumentNode.SelectNodes("//*[@src]");

			if (elementsWithSrc != null)
			{
				foreach (var element in elementsWithSrc)
				{
                    string src = element.Attributes["src"].Value;
					if (src.StartsWith("data:image") || src.Contains("base64"))
						continue;

					var uri = new Uri(src);
					string url = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath.TrimStart('/')}";

					// Replace the "src" attribute with the new source
					element.Attributes["src"].Value = url;
				}
			}

			// Get the modified HTML
			string modifiedHtml = doc.DocumentNode.OuterHtml;

            // 🧹 Clean invisible UTF-8 control characters (BOM, zero-width spaces, etc.)
            modifiedHtml = modifiedHtml
                .Replace("\uFEFF", "")  // BOM (Byte Order Mark)
                .Replace("\u200B", "")  // Zero width space
                .Replace("\u200C", "")  // Zero width non-joiner
                .Replace("\u200D", "")  // Zero width joiner
                .Replace("\u2060", ""); // Word joiner

            return modifiedHtml;
		}

		/// <summary>
		/// To archive a quiz. 
		/// </summary>
		/// <param name="quizId">The quiz id.</param>
		/// <returns>Returns a Http Response message if the quiz was archived or not. </returns>
		public ResponseModel ArchiveQuiz(string quizId)
        {
            MessageManager messageManager = new MessageManager(_connectionStringRead,
              _langCode);
            new QuizDAO().Update(_connectionString, new QuizModel()
            {
                Available = "0",
            }, new QuizModel()
            {
                Id = quizId,
            });
            return new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>(){
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ls_message_code_success"]) }
            };
        }

        /// <summary>
        /// To validate if a quiz can be updated after being published.
        /// </summary>
        /// <param name="quizId">The id of the quiz.</param>
        /// <returns>Returns a response model with details if it can be updated or not.</returns>
        public ResponseModel ValidateIfQuizCanBeUpdatedAfterPublish(int quizId)
        {
            // First, check if the quiz was published
            var quizDAO = new QuizDAO();
            var quizModelList = quizDAO.Read(_connectionStringRead, new QuizModel()
            {
                Id = quizId.ToString(),
            }, true, true);

            if (quizModelList.Count == 0)
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }

            var quizModelToCheck = quizModelList[0];

            // Since it is not published, it can be edited.
            if (quizModelToCheck.ReadyToPublish == "0")
            {
                return new ResponseModel()
                {
                    Valid = true
                };
            }

            // Since it was published, we have to check if it was added to a instructional plan. 
            if (new TeacherPlanQuizDAO().Read(_connectionStringRead, new TeacherPlanQuizModel()
            {
                QuizId = quizId.ToString(),
            }, false, false).Count > 0)
            {
                // Since it was not added to an instructional plan, it can be edited.
                return new ResponseModel()
                {
                    Valid = false
                };
            }

            return new ResponseModel()
            {
                Valid = true
            };
        }

        /// <summary>
        /// Update simple quiz, for new teacher module.
        /// </summary>
        /// <param name="quizRequestModelToUpdate">Quiz Request Model to update.</param>
        /// <param name="userCreatedBy">The user that is making the request.</param>
        /// <returns>Returns the quiz with the transaction details.</returns>
        public QuizResponseModel UpdateSimpleQuiz(QuizRequestModel quizRequestModelToUpdate, string userCreatedBy)
        {
            // Check if already published. If it was published before, it cannot be changed. 
            QuizDAO quizDAO = new QuizDAO();
            List<QuizModel> quizModelList = new List<QuizModel>();
            quizModelList = quizDAO.Read(_connectionStringRead, new QuizModel()
            {
                Id = quizRequestModelToUpdate.QuizModel.Id,
                ModifiedBy = userCreatedBy, // the one who created it.
            }, true, true);

            if (quizModelList.Count == 0)
            {
                return new QuizResponseModel()
                {
                    Valid = false,
                };
            }


            QuizModel quizModelToCheck = quizModelList[0];

            // Now check if they were published or not

            QuizResponseModel responseModel = new QuizResponseModel
            {
                Valid = true,
                Messages = new List<string>()
            };

            MessageManager messageManager = new MessageManager(_connectionStringRead,
               _langCode);

            // If was published, it cannot be changed
            if (quizModelToCheck.ReadyToPublish == "1")
            {
                responseModel.Valid = false;
                // Return that changes cannot be made
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ls_message_error_homework_was_published"]));
                responseModel.QuestionRequestModelList = quizRequestModelToUpdate.QuestionRequestModelList;
                responseModel.QuizAttachmentModelList = quizRequestModelToUpdate.QuizAttachmentModelList;
                responseModel.QuizModel = quizRequestModelToUpdate.QuizModel;

                return responseModel;
            }

			if (!string.IsNullOrWhiteSpace(quizRequestModelToUpdate.QuizModel.Instruction))
				quizRequestModelToUpdate.QuizModel.Instruction = UnsignUrlFromHtmlSourceCode(quizRequestModelToUpdate.QuizModel.Instruction);

            // First, update the quiz. It cannot be deleted, but can be edited freely
            quizRequestModelToUpdate.QuizModel.ModifiedBy = userCreatedBy;
            quizRequestModelToUpdate.QuizModel.CreatedAt = null; // This is set on DB 
            quizRequestModelToUpdate.QuizModel.UpdatedAt = null; // This is set on DB 
            quizDAO.Update(_connectionString, quizRequestModelToUpdate.QuizModel,
                new QuizModel() { Id = quizRequestModelToUpdate.QuizModel.Id });

			if (!string.IsNullOrWhiteSpace(quizRequestModelToUpdate.QuizModel.Instruction))
			{
				quizRequestModelToUpdate.QuizModel.Instruction = Replacer.ReplaceSrc(quizRequestModelToUpdate.QuizModel.Instruction);
			}

			if (quizRequestModelToUpdate.QuizAttachmentModelList != null
                && quizRequestModelToUpdate.QuizAttachmentModelList.Count > 0)
            {
                // DO NOT DELETE, since frontend is not prepared for that   new QuizAttachmentDAO().Delete(this._connectionString, new QuizAttachmentModel() { QuizId = quizRequestModelToUpdate.QuizModel.Id });
                CreateAttachmentURL(quizRequestModelToUpdate.QuizAttachmentModelList, quizRequestModelToUpdate.QuizModel.Id, userCreatedBy);
            }

            // Creates or update rubric
            if (quizRequestModelToUpdate.QuizRubricModel != null)
            {
                responseModel.QuizRubricModel = QuizSimpleRubricCreate(quizRequestModelToUpdate.QuizRubricModel, quizRequestModelToUpdate.QuizModel.Id, userCreatedBy);
            }
            else
            {
                // Delete if exits 
                try
                {
                    new QuizRubricDAO().Delete(_connectionString, new QuizRubricModel()
                    {
                        QuizId = quizRequestModelToUpdate.QuizModel.Id
                    });
                }
                catch (Exception)
                {
                    // continue
                }
            }

            QuizQuestionDAO quizQuestionDAO = new QuizQuestionDAO();
            QuizQuestionResponseDAO quizQuestionResponseDAO = new QuizQuestionResponseDAO();

            // Get the questions to edit 
            List<QuizQuestionModel> quizQuestionModelToEditList = new List<QuizQuestionModel>();
            List<QuizQuestionModel> quizQuestionModelToEditWhereList = new List<QuizQuestionModel>();

            List<QuizQuestionModel> quizQuestionsCurrent = new List<QuizQuestionModel>();
            quizQuestionsCurrent = quizQuestionDAO.Read(_connectionStringRead, new QuizQuestionModel() { QuizId = quizRequestModelToUpdate.QuizModel.Id }, false, false);


            List<QuizQuestionResponseModel> quizQuestionResponseModelToUpdateList = new List<QuizQuestionResponseModel>();
            List<QuizQuestionResponseModel> quizQuestionResponseModelToUpdateWhereList = new List<QuizQuestionResponseModel>();
            List<QuizQuestionModel> quizquestionModelToDelete = new List<QuizQuestionModel>();
            List<QuizQuestionResponseModel> quizQUestionsRepsonseModelList = new List<QuizQuestionResponseModel>();
            foreach (QuizQuestionModel quizQuestion in quizQuestionsCurrent)
            {
                if (quizRequestModelToUpdate.QuestionRequestModelList.Find(x => x.QuizQuestionModel.Id == quizQuestion.Id) == null)
                {
                    quizquestionModelToDelete.Add(new QuizQuestionModel()
                    {
                        Id = quizQuestion.Id,
                    });
                    quizQUestionsRepsonseModelList.Add(new QuizQuestionResponseModel()
                    {
                        QuizQuestionId = quizQuestion.Id,
                    });
                }
            }

            if (quizquestionModelToDelete.Count > 0)
            {
                foreach (var questionToDelete in quizquestionModelToDelete)
                {
                    DeleteQuizQuestionPosition(questionToDelete.Id);
                }
                // Delete the responses first
                quizQuestionResponseDAO.Delete(_connectionString, quizQUestionsRepsonseModelList);
                quizQuestionDAO.Delete(_connectionString, quizquestionModelToDelete);
            }

            foreach (QuestionRequestModel questionRequest in quizRequestModelToUpdate.QuestionRequestModelList)
            {


                // If the question is a new question
                if (string.IsNullOrWhiteSpace(questionRequest.QuizQuestionModel.Id))
                {
                    // Insert question now. You need the ID for the responses. 
                    QuizQuestionModel quizQuestionAdded = QuestionCreate(
                                    questionRequest.QuizQuestionModel,
                                    quizRequestModelToUpdate.QuizModel.Id,
                                    userCreatedBy);

                    // Set the ID to return it
                    questionRequest.QuizQuestionModel.Id = quizQuestionAdded.Id;
					if (!string.IsNullOrWhiteSpace(questionRequest.QuizQuestionModel.Desc))
					{
						questionRequest.QuizQuestionModel.Desc = Replacer.ReplaceSrc(questionRequest.QuizQuestionModel.Desc);
					}

                    // Now insert the responses, which are new
                    if (questionRequest.QuizQuestionResponseModelList != null && questionRequest.QuizQuestionResponseModelList.Count > 0)
                    {

                        foreach (QuizQuestionResponseModel questionResponse in questionRequest.QuizQuestionResponseModelList)
                        {
                            questionResponse.Response = UnsignUrlFromHtmlSourceCode(questionResponse.Response);
							questionResponse.ModifiedBy = userCreatedBy;
                            questionResponse.QuizQuestionId = questionRequest.QuizQuestionModel.Id;
                            // Return the IDs. They are needed for the FE. 
                            questionResponse.Id = quizQuestionResponseDAO.Create(_connectionString, questionResponse)[0].Id;

							if (!string.IsNullOrWhiteSpace(questionResponse.Response))
							{
								questionResponse.Response = Replacer.ReplaceSrc(questionResponse.Response);
							}
						}
                    }
                }
                else
                {
                    // If the question exists, then we must edit it, however, for the responses, we have to check which responses are new and which are old. 
                    quizQuestionModelToEditList.Add(new QuizQuestionModel()
                    {
                        QuizId = quizRequestModelToUpdate.QuizModel.Id,
						Desc = !string.IsNullOrWhiteSpace(questionRequest.QuizQuestionModel.Desc) ? 
                            UnsignUrlFromHtmlSourceCode(questionRequest.QuizQuestionModel.Desc) : null,
						ModifiedBy = userCreatedBy,
                    });

                    quizQuestionModelToEditWhereList.Add(new QuizQuestionModel()
                    {
                        Id = questionRequest.QuizQuestionModel.Id,
                    });

                    // Now get the responses
                    foreach (QuizQuestionResponseModel response in questionRequest.QuizQuestionResponseModelList)
                    {
                        // If ID is null, is new, so add it. 
                        if (string.IsNullOrWhiteSpace(response.Id))
                        {
                            response.Response = response.Response?.Trim();
							response.Id = quizQuestionResponseDAO.Create(_connectionString, new QuizQuestionResponseModel()
                            {
                                QuizQuestionId = questionRequest.QuizQuestionModel.Id,
                                Correct = response.Correct,
                                Value = response.Value,
								Response = !string.IsNullOrWhiteSpace(response.Response) ? UnsignUrlFromHtmlSourceCode(response.Response) : null,
							})[0].Id;

							if (!string.IsNullOrWhiteSpace(response.Response))
							{
								response.Response = Replacer.ReplaceSrc(response.Response);
							}
						}
                        else
                        {
                            quizQuestionResponseModelToUpdateList.Add(new QuizQuestionResponseModel()
                            {
                                Correct = response.Correct,
                                Value = response.Value,
                                QuizQuestionId = questionRequest.QuizQuestionModel.Id,
								Response = !string.IsNullOrWhiteSpace(response.Response) ? UnsignUrlFromHtmlSourceCode(response.Response) : null,
								ModifiedBy = userCreatedBy
                            });

                            quizQuestionResponseModelToUpdateWhereList.Add(new QuizQuestionResponseModel()
                            {
                                Id = response.Id
                            });
                        }
                    }

                }
            }

            if (quizQuestionModelToEditList.Count > 0)
            {
                //  Matters most for a mixed quiz, where questions of
                // different kinds are interleaved and id order says nothing about the layout.
                for (int i = 0; i < quizQuestionModelToEditList.Count; i++)
                {
                    SetQuizQuestionPosition(quizQuestionModelToEditList[i].QuizId,
                        quizQuestionModelToEditWhereList[i].Id, i + 1);
                }

                quizQuestionDAO.Update(_connectionString, quizQuestionModelToEditList, quizQuestionModelToEditWhereList);
				// Set the URLs
				foreach (QuizQuestionModel quizQuestionToUpdate in quizQuestionModelToEditList)
				{
					if (!string.IsNullOrWhiteSpace(quizQuestionToUpdate.Desc))
					{
						quizQuestionToUpdate.Desc = Replacer.ReplaceSrc(quizQuestionToUpdate.Desc);
					}
				}
			}

            if (quizQuestionResponseModelToUpdateList.Count > 0)
            {
                quizQuestionResponseDAO.Update(_connectionString, quizQuestionResponseModelToUpdateList, quizQuestionResponseModelToUpdateWhereList);

				// Set the URL
				foreach (QuizQuestionResponseModel response in quizQuestionResponseModelToUpdateList)
				{
					if (!string.IsNullOrWhiteSpace(response.Response))
					{
						response.Response = Replacer.ReplaceSrc(response.Response);

					}
				}
			}

            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ls_message_code_success"]));
            responseModel.QuestionRequestModelList = quizRequestModelToUpdate.QuestionRequestModelList;
            responseModel.QuizAttachmentModelList = quizRequestModelToUpdate.QuizAttachmentModelList;
            responseModel.QuizModel = quizRequestModelToUpdate.QuizModel;

            return responseModel;
        }

        /// <summary>
        /// Update simple quiz, for new teacher module.
        /// </summary>
        /// <param name="quizRequestModelToUpdate">Quiz Request Model to update.</param>
        /// <param name="webServiceURL">Web Service URL to set the full path of the files.</param>
        /// <param name="userCreatedBy">The user that is making the request.</param>
        /// <returns>Returns the quiz with the transaction details.</returns>
        public QuizResponseModel UpdateSimpleQuiz(QuizRequestModel quizRequestModelToUpdate,
            string webServiceURL,
            string userCreatedBy)
        {
            // Check if already published. If it was published before, it cannot be changed. 
            QuizDAO quizDAO = new QuizDAO();
            List<QuizModel> quizModelList = new List<QuizModel>();
            quizModelList = quizDAO.Read(_connectionStringRead, new QuizModel()
            {
                Id = quizRequestModelToUpdate.QuizModel.Id,
                ModifiedBy = userCreatedBy, // the one who created it.
            }, true, true);

            if (quizModelList.Count == 0)
            {
                return new QuizResponseModel()
                {
                    Valid = false,
                };
            }

            QuizModel quizModelToCheck = quizModelList[0];

            // Now check if they were published or not
            QuizResponseModel responseModel = new QuizResponseModel
            {
                Valid = true,
                Messages = new List<string>()
            };

            MessageManager messageManager = new MessageManager(_connectionStringRead,
                _langCode);

            // If was published, it cannot be changed
            var quizToCheck = ValidateIfQuizCanBeUpdatedAfterPublish(int.Parse(quizModelToCheck.Id));
            if (!quizToCheck.Valid)
            {
                responseModel.Valid = false;
                responseModel.Messages = new List<string>();
                responseModel.Messages.AddRange(quizToCheck.Messages);
                responseModel.QuestionRequestModelList = quizRequestModelToUpdate.QuestionRequestModelList;
                responseModel.QuizAttachmentModelList = quizRequestModelToUpdate.QuizAttachmentModelList;
                responseModel.QuizModel = quizRequestModelToUpdate.QuizModel;

                return responseModel;
            }

            if (!string.IsNullOrWhiteSpace(quizRequestModelToUpdate.QuizModel.Instruction))
            {
                quizRequestModelToUpdate.QuizModel.Instruction = quizRequestModelToUpdate.QuizModel.Instruction.Trim();
            }

            // Sum the custom value
            quizRequestModelToUpdate.QuizModel.CustomValue = quizRequestModelToUpdate.QuestionRequestModelList
                .Where(q => q.QuizQuestionModel != null) // Ensure QuizQuestionModel is not null
                .Sum(q => decimal.Parse(q.QuizQuestionModel.CustomValue)).ToString();

            // First, update the quiz. It cannot be deleted, but can be edited freely
            quizRequestModelToUpdate.QuizModel.ModifiedBy = userCreatedBy;
            quizRequestModelToUpdate.QuizModel.CreatedAt = null; // This is set on DB 
            quizRequestModelToUpdate.QuizModel.UpdatedAt = null; // This is set on DB 
            quizDAO.Update(_connectionString, quizRequestModelToUpdate.QuizModel,
                new QuizModel() { Id = quizRequestModelToUpdate.QuizModel.Id });

            if (!string.IsNullOrWhiteSpace(quizRequestModelToUpdate.QuizModel.Instruction))
            {
                quizRequestModelToUpdate.QuizModel.Instruction = quizRequestModelToUpdate.QuizModel.Instruction.Trim();
            }

            // Attachments
            // Get the current attachment from the DB
            QuizAttachmentDAO quizAttachmentDAO = new QuizAttachmentDAO();
            List<QuizAttachmentModel> quizAttachmentModelListCurrent = quizAttachmentDAO.Read(_connectionStringRead,
                new List<QuizAttachmentModel>()
                    { new QuizAttachmentModel() { QuizId = quizRequestModelToUpdate.QuizModel.Id } });

            // Check if there is any attachment to delete


            // Delete the attachments that are not exist
            foreach (var currentAttachment in quizAttachmentModelListCurrent)
            {
                // DO NOT DELETE, since frontend is not prepared for that   new QuizAttachmentDAO().Delete(this._connectionString, new QuizAttachmentModel() { QuizId = quizRequestModelToUpdate.QuizModel.Id });
                if (!quizRequestModelToUpdate.QuizAttachmentModelList.Exists(a => a.Id == currentAttachment.Id))
                {
                    quizAttachmentDAO.Delete(_connectionString,
                        new QuizAttachmentModel() { Id = currentAttachment.Id });
                    // Delete the file from the server
                    // AzureAssets.DeleteFileToAzureStorage(currentAttachment.AttachmentUrl);
                }
            }

            if (quizRequestModelToUpdate.QuizAttachmentModelList != null &&
                quizRequestModelToUpdate.QuizAttachmentModelList.Count > 0)
            {
                CreateAttachmentURL(quizRequestModelToUpdate.QuizAttachmentModelList,
                    quizRequestModelToUpdate.QuizModel.Id, userCreatedBy);
            }


            // Creates or update rubric
            if (quizRequestModelToUpdate.QuizRubricModel != null)
            {
                responseModel.QuizRubricModel = QuizSimpleRubricCreate(quizRequestModelToUpdate.QuizRubricModel,
                    quizRequestModelToUpdate.QuizModel.Id, userCreatedBy);
            }
            else
            {
                // Delete if exits 
                try
                {
                    new QuizRubricDAO().Delete(_connectionString, new QuizRubricModel()
                    {
                        QuizId = quizRequestModelToUpdate.QuizModel.Id
                    });
                }
                catch (Exception)
                {
                    // continue
                }
            }

            // create a new list for the new attachments
            List<QuizAttachmentModel> quizAttachmentModelListNew = new List<QuizAttachmentModel>();

            // append the new attachments to the current list
            quizAttachmentModelListCurrent.AddRange(quizAttachmentModelListNew);

            // add the new list to the response model
            quizRequestModelToUpdate.QuizAttachmentModelList = quizAttachmentModelListCurrent;


            // if (quizRequestModelToUpdate.QuizAttachmentModelList != null
            //     && quizRequestModelToUpdate.QuizAttachmentModelList.Count > 0)
            // {
            //     // DO NOT DELETE, since frontend is not prepared for that   new QuizAttachmentDAO().Delete(this._connectionString, new QuizAttachmentModel() { QuizId = quizRequestModelToUpdate.QuizModel.Id });
            //     quizRequestModelToUpdate.QuizAttachmentModelList = CreateAttachmentURL(
            //         quizRequestModelToUpdate.QuizAttachmentModelList, quizRequestModelToUpdate.QuizModel.Id,
            //         webServiceURL, userCreatedBy);
            // }
            // else
            // {
            //     /*
            //     // Check previous attachments 
            //     List<QuizAttachmentModel> quizAttachmentModels = new QuizAttachmentDAO().Read(this._connectionStringRead, new QuizAttachmentModel() { QuizId = quizRequestModelToUpdate.QuizModel.Id }, false, false);
            //     if (quizAttachmentModels != null && quizAttachmentModels.Count > 0)
            //     {
            //         // Delete them 
            //         new QuizAttachmentDAO().Delete(this._connectionString, quizAttachmentModels);
            //     } */
            // }

            // Creates or update rubric
            QuizQuestionDAO quizQuestionDAO = new QuizQuestionDAO();
            QuizQuestionResponseDAO quizQuestionResponseDAO = new QuizQuestionResponseDAO();

            // Create or update QuizMatchInstructions
            QuizMatchInstructionsDAO quizMatchInstructionsDAO = new QuizMatchInstructionsDAO();
            if (quizRequestModelToUpdate.QuizMatchInstructionsModel != null)
            {
                foreach (var matchInstruction in quizRequestModelToUpdate.QuizMatchInstructionsModel)
                {
                    // Check if match instruction is new or existing
                    List<QuizMatchInstructionsModel> existingMatchInstruction = new List<QuizMatchInstructionsModel>();
                    int parsedId;

                    existingMatchInstruction = quizMatchInstructionsDAO.Read(_connectionStringRead,
                        new QuizMatchInstructionsModel() { Id = matchInstruction.Id }, false, false);
                    if (!int.TryParse(matchInstruction.Id, out parsedId) || existingMatchInstruction == null ||
                        existingMatchInstruction.Count == 0)
                    {
                        // save id to change on questions
                        var matchInstructionId = matchInstruction.Id;
                        // remove actual id
                        matchInstruction.Id = null;
                        // create new match instruction
                        matchInstruction.QuizId = quizRequestModelToUpdate.QuizModel.Id;
                        matchInstruction.ModifiedBy = userCreatedBy;
                        matchInstruction.MatchGroupNumber = "1";
                        matchInstruction.Id =
                            quizMatchInstructionsDAO.Create(_connectionString, matchInstruction)[0].Id;

                        // update quizMatchInstructionsId in questions that have the same id
                        foreach (var questionRequest in quizRequestModelToUpdate.QuestionRequestModelList)
                        {
                            if (questionRequest.QuizQuestionModel.QuizMatchInstructionId == matchInstructionId)
                            {
                                questionRequest.QuizQuestionModel.QuizMatchInstructionId = matchInstruction.Id;
                            }
                        }
                    }
                    else
                    {
                        // update existing math instruction
                        matchInstruction.UpdatedAt = null; // This is set on DB
                        matchInstruction.CreatedAt = null; // This is set on DB
                        quizMatchInstructionsDAO.Update(_connectionString, matchInstruction,
                            new QuizMatchInstructionsModel() { Id = matchInstruction.Id });
                    }
                }
            }

            // Get the questions to edit 
            List<QuizQuestionModel> quizQuestionModelToEditList = new List<QuizQuestionModel>();
            List<QuizQuestionModel> quizQuestionModelToEditWhereList = new List<QuizQuestionModel>();

            List<QuizQuestionModel> quizQuestionsCurrent = new List<QuizQuestionModel>();
            quizQuestionsCurrent = quizQuestionDAO.Read(_connectionStringRead,
                new QuizQuestionModel() { QuizId = quizRequestModelToUpdate.QuizModel.Id }, false, false);


            List<QuizQuestionResponseModel> quizQuestionResponseModelToUpdateList =
                new List<QuizQuestionResponseModel>();
            List<QuizQuestionResponseModel> quizQuestionResponseModelToUpdateWhereList =
                new List<QuizQuestionResponseModel>();
            List<QuizQuestionModel> quizquestionModelToDelete = new List<QuizQuestionModel>();
            List<QuizQuestionResponseModel> quizQUestionsRepsonseModelList = new List<QuizQuestionResponseModel>();
            foreach (QuizQuestionModel quizQuestion in quizQuestionsCurrent)
            {
                if (quizRequestModelToUpdate.QuestionRequestModelList.Find(x =>
                        x.QuizQuestionModel.Id == quizQuestion.Id) == null)
                {
                    quizquestionModelToDelete.Add(new QuizQuestionModel()
                    {
                        Id = quizQuestion.Id,
                    });
                    quizQUestionsRepsonseModelList.Add(new QuizQuestionResponseModel()
                    {
                        QuizQuestionId = quizQuestion.Id,
                    });
                }
            }

            if (quizquestionModelToDelete.Count > 0)
            {
                // Delete the responses first 
                quizQuestionResponseDAO.Delete(_connectionString, quizQUestionsRepsonseModelList);
                quizQuestionDAO.Delete(_connectionString, quizquestionModelToDelete);
            }

            foreach (QuestionRequestModel questionRequest in quizRequestModelToUpdate.QuestionRequestModelList)
            {
                if (questionRequest.QuizQuestionModel.QuestionTypeId ==
                    AppConfiguration.Configuration["AppSettings:question_type_opened"])
                {
                    questionRequest.QuizQuestionResponseModelList = new List<QuizQuestionResponseModel>();
                }


                if (string.IsNullOrWhiteSpace(questionRequest.QuizQuestionModel.Id))
                {
                    // Insert question now. You need the ID for the responses. 
                    QuizQuestionModel quizQuestionAdded = QuestionCreate(
                        questionRequest.QuizQuestionModel,
                        quizRequestModelToUpdate.QuizModel.Id,
                        userCreatedBy);

                    // Set the ID to return it
                    questionRequest.QuizQuestionModel.Id = quizQuestionAdded.Id;
                    if (!string.IsNullOrWhiteSpace(questionRequest.QuizQuestionModel.Desc))
                    {
                        questionRequest.QuizQuestionModel.Desc = questionRequest.QuizQuestionModel.Desc.Trim();
                    }


                    // Now insert the responses, which are new
                    if (questionRequest.QuizQuestionResponseModelList != null &&
                        questionRequest.QuizQuestionResponseModelList.Count > 0)
                    {
                        foreach (QuizQuestionResponseModel questionResponse in questionRequest
                                     .QuizQuestionResponseModelList)
                        {
                            // If match, then the response is corect.
                            if (questionRequest.QuizQuestionModel.QuestionTypeId ==
                                AppConfiguration.Configuration["AppSettings:question_type_match"])
                            {
                                questionResponse.Correct = "1";
                                questionResponse.Value = "1";
                            }

                            // validate if value is '1,00' and change for '1' because an error found in duplicate quiz test
                            if (questionResponse.Value == "1,00")
                            {
                                questionResponse.Value = "1";
                            }

                            questionResponse.Response = questionResponse.Response.Trim();
                            questionResponse.ModifiedBy = userCreatedBy;
                            questionResponse.QuizQuestionId = questionRequest.QuizQuestionModel.Id;

                            // Return the IDs. They are needed for the FE. 
                            questionResponse.Id = quizQuestionResponseDAO.Create(_connectionString, questionResponse)[0]
                                .Id;
                        }
                    }
                }
                else
                {
                    // If the question exists, then we must edit it, however, for the responses, we have to check which responses are new and which are old. 
                    quizQuestionModelToEditList.Add(new QuizQuestionModel()
                    {
                        QuizId = quizRequestModelToUpdate.QuizModel.Id,
                        Desc = questionRequest.QuizQuestionModel.Desc.Trim(),
                        QuestionTypeId = questionRequest.QuizQuestionModel.QuestionTypeId,
                        CustomValue = questionRequest.QuizQuestionModel.CustomValue,
                        QuizMatchInstructionId = questionRequest.QuizQuestionModel.QuizMatchInstructionId,
                        ModifiedBy = userCreatedBy,
                    });

                    quizQuestionModelToEditWhereList.Add(new QuizQuestionModel()
                    {
                        Id = questionRequest.QuizQuestionModel.Id,
                    });

                    // Now get the responses
                    foreach (QuizQuestionResponseModel response in questionRequest.QuizQuestionResponseModelList)
                    {
                        // If match, then the response is corect.
                        if (questionRequest.QuizQuestionModel.QuestionTypeId ==
                            AppConfiguration.Configuration["AppSettings:question_type_match"])
                        {
                            response.Correct = "1";
                            response.Value = "1";
                        }

                        // If ID is null, is new, so add it. 
                        if (string.IsNullOrWhiteSpace(response.Id))
                        {
                            response.Id = quizQuestionResponseDAO.Create(_connectionString,
                                new QuizQuestionResponseModel()
                                {
                                    QuizQuestionId = questionRequest.QuizQuestionModel.Id,
                                    Correct = response.Correct,
                                    Value = response.Value,
                                    Response = response.Response.Trim(),
                                })[0].Id;

                            if (!string.IsNullOrWhiteSpace(response.Response))
                            {
                                /* this does not apply anymore because of QuillJs
                response.Response = Replacer.ReplaceSrc(response.Response);
                */
                                response.Response = response.Response.Trim();
                            }
                        }
                        else
                        {
                            quizQuestionResponseModelToUpdateList.Add(new QuizQuestionResponseModel()
                            {
                                Correct = response.Correct,
                                Value = response.Value,
                                QuizQuestionId = questionRequest.QuizQuestionModel.Id,
                                Response = response.Response.Trim(),
                                ModifiedBy = userCreatedBy
                            });

                            quizQuestionResponseModelToUpdateWhereList.Add(new QuizQuestionResponseModel()
                            {
                                Id = response.Id
                            });
                        }
                    }
                }
            }

            if (quizQuestionModelToEditList.Count > 0)
            {
                foreach (QuizQuestionModel quizQuestionToUpdate in quizQuestionModelToEditList)
                {
                    if (!string.IsNullOrWhiteSpace(quizQuestionToUpdate.Desc))
                    {
                        quizQuestionToUpdate.Desc = quizQuestionToUpdate.Desc.Trim();
                    }
                }

                quizQuestionDAO.Update(_connectionString, quizQuestionModelToEditList,
                    quizQuestionModelToEditWhereList);
            }

            if (quizQuestionResponseModelToUpdateList.Count > 0)
            {
                foreach (QuizQuestionResponseModel response in quizQuestionResponseModelToUpdateList)
                {
                    if (!string.IsNullOrWhiteSpace(response.Response))
                    {
                        response.Response = response.Response.Trim();
                    }
                }

                quizQuestionResponseDAO.Update(_connectionString, quizQuestionResponseModelToUpdateList,
                    quizQuestionResponseModelToUpdateWhereList);
            }

            responseModel.Messages.Add(
                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:ls_message_code_success"]));
            responseModel.QuestionRequestModelList = quizRequestModelToUpdate.QuestionRequestModelList;
            responseModel.QuizAttachmentModelList = quizRequestModelToUpdate.QuizAttachmentModelList;
            responseModel.QuizMatchInstructionsModel = quizRequestModelToUpdate.QuizMatchInstructionsModel;
            responseModel.QuizModel = quizRequestModelToUpdate.QuizModel;

            // Read the quiz again to get the updated details.
            // This is added, because the attachments has a process to get the full path.
            // In the future, this should be changed to read just the attachments.
            responseModel.QuizAttachmentModelList =
                new QuizReaderManager(_connectionStringRead, _langCode).GetQuizAttachments(quizRequestModelToUpdate
                    .QuizModel.Id);

            responseModel.QuizModel.CanBeEdited =
                new QuizReaderManager(this._connectionStringRead, this._langCode).CheckIfQuizCanBeEdited(responseModel
                    .QuizModel.Id);

            return responseModel;
        }

        /// <summary>
        /// To manually correct plan quiz student without student submit.
        /// </summary>
        /// <param name="planAssignmentUserResultModel">The plan quiz assignment to correct.</param>
        /// <param name="userInSession">The user that is modifying the row.</param>
        /// <returns>Returns the plan assignment user result model with transaction details.</returns>
        public PlanAssignmentUserResultModel ManuallyCorrectPlanQuizStudentWithoutStudentSubmit(
            PlanAssignmentUserResultModel planAssignmentUserResultModel, string userInSession)
        {
            var planningModifierManager = new PlanningModifierManager(_connectionString,
                _connectionStringRead, _langCode);

            // Find the plan id by plan quiz id
            var teacherQuizModel = new TeacherPlanQuizDAO().Read(_connectionStringRead,
                new TeacherPlanQuizModel() { Id = planAssignmentUserResultModel.PlanQuizId },
                true, true)[0];

            // Check if plan is available and opened
            if (!planningModifierManager.ValidateIfTeacherPlanAllowChanges(teacherQuizModel.PlanId).Valid)
            {
                planAssignmentUserResultModel.Valid = false;
                planAssignmentUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_plan_not_available"]),
                };
                return planAssignmentUserResultModel;
            }

            // Validate the model
            if (string.IsNullOrWhiteSpace(planAssignmentUserResultModel.PlanQuizId))
            {
                planAssignmentUserResultModel.Valid = false;
                planAssignmentUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_plan_quiz_id_required"]),
                };
                return planAssignmentUserResultModel;
            }

            if (string.IsNullOrWhiteSpace(planAssignmentUserResultModel.UserId))
            {
                planAssignmentUserResultModel.Valid = false;
                planAssignmentUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_user_id_required"]),
                };
                return planAssignmentUserResultModel;
            }

            if (string.IsNullOrWhiteSpace(planAssignmentUserResultModel.Score))
            {
                planAssignmentUserResultModel.Valid = false;
                planAssignmentUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_score_required"]),
                };
                return planAssignmentUserResultModel;
            }

            double score = 0;
            try
            {
                score = double.Parse(planAssignmentUserResultModel.Score);
            }
            catch (Exception e)
            {
                planAssignmentUserResultModel.Valid = false;
                planAssignmentUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_score_invalid"]),
                };
                return planAssignmentUserResultModel;
            }

            // Get the quiz details to sum the total score. 
            var quizQuestions = new QuizQuestionDAO().Read(this._connectionStringRead,
                new QuizQuestionModel()
                {
                    QuizId = teacherQuizModel.QuizId,
                }, false, false);

            // If the quiz questions are empty, then we return an error.
            if (quizQuestions.Count == 0)
            {
                planAssignmentUserResultModel.Valid = false;
                planAssignmentUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_quiz_questions_empty"]),
                };
                return planAssignmentUserResultModel;
            }

            // Sum the total score
            double totalScore = 0;
            foreach (var question in quizQuestions)
            {
                totalScore += double.Parse(question.CustomValue);
            }

            // If the total score is less than the score, then we return an error.
            if (totalScore < score || score < 0)
            {
                planAssignmentUserResultModel.Valid = false;
                planAssignmentUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_score_invalid"]),
                };
                return planAssignmentUserResultModel;
            }

            // Check if the user has already taken the quiz
            var planAssignmentQuizUserResultModelToCheck = new PlanAssignmentUserResultDAO().Read(_connectionStringRead,
                new PlanAssignmentUserResultModel()
                {
                    PlanQuizId = planAssignmentUserResultModel.PlanQuizId,
                    UserId = planAssignmentUserResultModel.UserId
                }, true, true);

            // Calculate the total percent and set to two decimals without rounding
            planAssignmentUserResultModel.TotalPercent = (score / totalScore * 100).ToString("0.00");
            // Set the score.
            planAssignmentUserResultModel.Score = score.ToString("0.00");

            if (planAssignmentQuizUserResultModelToCheck.Count > 0)
            {
                // Update it
                var responseModel = new PlanAssignmentUserResultDAO().Update(_connectionString,
                    new PlanAssignmentUserResultModel()
                    {
                        Score = planAssignmentUserResultModel.Score,
                        ManuallyCorrectedByTeacher = "1",
                        TotalPercent = planAssignmentUserResultModel.TotalPercent,
                        ModifiedBy = userInSession
                    }, new PlanAssignmentUserResultModel()
                    {
                        Id = planAssignmentQuizUserResultModelToCheck[0].Id,
                    })[0];

                responseModel.Valid = true;
                responseModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_success_insert_manual_quiz_result"]),
                };
                return responseModel;
            }
            else
            {
                planAssignmentUserResultModel.ModifiedBy = userInSession;
                planAssignmentUserResultModel.ManuallyCorrectedByTeacher = "1";

                // Insert it 
                var responseModel = new PlanAssignmentUserResultDAO().Create(_connectionString,
                    planAssignmentUserResultModel)[0];

                responseModel.Valid = true;
                responseModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_success_insert_manual_quiz_result"]),
                };
                return responseModel;
            }
        }

        /// <summary>
        /// To create the question. Returns the question Model.
        /// </summary>
        /// <param name="questionRequestModel">The question to create with the responses.</param>
        /// <param name="quizId">The id of the quiz to attach the questions.</param>
        /// <param name="userCreatedBy">The user id that is making the request.</param>
        /// <returns>Returns the quiz question with all details.</returns>
        private QuizQuestionModel QuestionCreate(QuizQuestionModel questionRequestModel, string quizId, string userCreatedBy)
        {
            // Insert questions
            questionRequestModel.QuizId = quizId;
            questionRequestModel.ModifiedBy = userCreatedBy;
            questionRequestModel.CreatedAt = null; // This is set on DB
            questionRequestModel.UpdatedAt = null; // This is set on DB 
            if (!string.IsNullOrWhiteSpace(questionRequestModel.Desc))
            {
				questionRequestModel.Desc = UnsignUrlFromHtmlSourceCode(questionRequestModel.Desc);
			}
            return new QuizQuestionDAO().Create(
                _connectionString, questionRequestModel)[0];
        }

        /// <summary>
        /// To create a rubric for whole quiz instead of each rubric for each questions. (One rubric per quiz)
        /// </summary>
        /// <param name="quizRubricModel">Quiz rubricl model details.</param>
        /// <param name="quizId">Id of the quiz.</param>
        /// <param name="userCreatedBy">The user who created the quiz.</param>
        /// <param name="webServiceURL">The web service url.</param>
        private QuizRubricModel QuizSimpleRubricCreate(QuizRubricModel quizRubricModel, string quizId,
            string userCreatedBy)
        {
            // Check if exists 
            if (!string.IsNullOrWhiteSpace(quizRubricModel.Desc))
            {
                quizRubricModel.QuizId = quizId;
                quizRubricModel.CreatedAt = null; // This is set on DB 
                quizRubricModel.UpdatedAt = null; // This is set on DB 
                quizRubricModel.ModifiedBy = userCreatedBy;
                quizRubricModel.Desc = quizRubricModel.Desc.Trim();
                QuizRubricDAO quizRubricDAO = new QuizRubricDAO();

                if (quizRubricDAO.Read(_connectionStringRead, new QuizRubricModel()
                {
                    QuizId = quizId,
                }, true, true).Count > 0)
                {
                    return quizRubricDAO.Update(_connectionString, quizRubricModel, new QuizRubricModel()
                    {
                        QuizId = quizId,
                    })[0];
                }
                else
                {
                    return quizRubricDAO.Create(_connectionString, quizRubricModel)[0];
                }
            }

            return new QuizRubricModel();
        }

        /// <summary>
        /// To attach a list of attachments to a quiz.
        /// </summary>
        /// <param name="quizAttachmentList">The quiz attachments list.</param>
        /// <param name="quizId">The quiz id that will have the attachment.</param>
        /// <param name="userCreatedBy">The user making a request.</param>
        private void CreateAttachmentURL(List<QuizAttachmentModel> quizAttachmentList, string quizId, string userCreatedBy)
        {
            if (quizAttachmentList != null && quizAttachmentList.Count > 0)
            {
                foreach (QuizAttachmentModel quizRequest in quizAttachmentList)
                {
                    quizRequest.QuizId = quizId;
                    quizRequest.ModifiedBy = userCreatedBy;
                }

                new QuizAttachmentDAO().Create(_connectionString, quizAttachmentList);
            }
        }
        
        /// <summary>
        /// To create a single attachment and return the Id.
        /// </summary>
        /// <param name="quizAttachment">The attachment model with the url, and other required details.</param>
        /// <param name="quizId">The quiz id that will have the attachment.</param>
        /// <param name="userCreatedBy">The user making a request.</param>
        /// <returns>Returns the quiz attachment details.</returns>
        private QuizAttachmentModel CreateAttachmentURL(QuizAttachmentModel quizAttachment, string quizId,
            string userCreatedBy)
        {
            quizAttachment.QuizId = quizId;
            quizAttachment.ModifiedBy = userCreatedBy;

            return new QuizAttachmentDAO().Create(_connectionString, quizAttachment)[0];
        }

        /// <summary>
        /// To validate quiz type and questions.
        /// </summary>
        /// <param name="quizRequestModel">The questions to validate.</param>
        /// <returns>Returns the validated quiz.</returns>
        private bool ValidateQuizTypeAndQuestions(QuizRequestModel quizRequestModel)
        {
            // Get the quiz type
            QuizTypeDAO quizTypeDAO = new QuizTypeDAO();
            List<QuizTypeModel> quizType = quizTypeDAO.Read(_connectionStringRead,
                new List<QuizTypeModel>() { new QuizTypeModel() { Id = quizRequestModel.QuizModel.QuizTypeId } });

            // Validate if quiz type is "Mixto / Mix"
            if (quizType[0].Name == "Mix / Mixto" && quizRequestModel.QuestionRequestModelList != null)
            {
                // Check every QuestionRequestModelList
                foreach (QuestionRequestModel questionRequest in quizRequestModel.QuestionRequestModelList)
                {
                    // Check if question type is assigned.
                    if (string.IsNullOrWhiteSpace(questionRequest.QuizQuestionModel.QuestionTypeId))
                    {
                        return false;
                    }

                    // Check if the response has any text.
                    if (questionRequest.QuizQuestionResponseModelList != null)
                    {
                        foreach (QuizQuestionResponseModel response in questionRequest.QuizQuestionResponseModelList)
                        {
                            if (response == null)
                            {
                                return false;
                            }

                            if (string.IsNullOrWhiteSpace(response.Response))
                            {
                                return false;
                            }

                            // Also trim the response
                            response.Response = response.Response.Trim();

                            // And check if the response has any text.
                            if (string.IsNullOrWhiteSpace(response.Response))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Sets the position of quiz questions based on the provided models and returns the updated positions.
        /// </summary>
        private void SetQuizQuestionPosition(string quizId, string quizQuestionId, int position)
        {
            var query = @"
                        INSERT INTO quiz_question_positions (
                            quiz_id,
                            quiz_question_id,
                            quiz_question_position
                        ) VALUES (
                            @quizId,
                            @quizQuestionId,
                            @position
                        )
                        ON DUPLICATE KEY UPDATE
                            quiz_question_position = @position;";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var parameters = new
            {
                quizId,
                quizQuestionId,
                position
            };

            try
            {
                connection.Query(query, parameters);
            }
            catch (Exception)
            {
                // continue
            }
            finally
            {
                connection.Close();
            }
        }

        private void DeleteQuizQuestionPosition(string quizQuestionId)
        {
            var query = @"DELETE FROM quiz_question_positions
                          WHERE quiz_question_id = @quizQuestionId;";

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            try
            {
                connection.Execute(query, new { quizQuestionId });
            }
            catch (Exception)
            {
                // continue
            }
            finally
            {
                connection.Close();
            }
        }
}
}
