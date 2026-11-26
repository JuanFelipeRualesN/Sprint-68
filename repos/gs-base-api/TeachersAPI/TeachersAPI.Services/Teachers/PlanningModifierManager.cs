using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TeachersAPI;
using TeachersAPI.CustomClasses;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Services.Generals;
using TeachersAPI.TeachersAPI.Services.Lessons;
using TeachersAPI.Utils;


namespace TeachersAPI.TeachersAPI.Services.Teachers
{
    /// <summary>
    /// To handle all the things related to the instructional plans created by teachers.
    /// </summary>
    public class PlanningModifierManager
    {
        private readonly string _connectionString, _connectionStringRead, _langCode;
        private const int MaxNameLength = 150;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code of the user.</param>
        public PlanningModifierManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To archive the planning.
        /// </summary>
        /// <param name="planningId">The planning id to archive.</param>
        /// <param name="responseSession">The user in session.</param>
        /// <returns>Return the plan details of the plan archive.</returns>
        public async Task<TeacherPlanModel> ArchivePlanning(string planningId, ValidationResponseModel responseSession)
        {
            // First, delete the groups that have the planning. 
            // Call the current groups of the plan.
            var currentGroups = new TeacherPlanGroupDAO().Read(_connectionStringRead,
                new TeacherPlanGroupModel() { TeacherPlanId = planningId }, false, false);
            if (currentGroups.Count > 0)
            {

                var responseDB = new TeacherPlanGroupDAO().Delete(_connectionString, new TeacherPlanGroupModel()
                {
                    TeacherPlanId = planningId
                });
                // Selects the first element from each group
                var groupsDistinct = currentGroups.GroupBy(g => g.GroupId).Select(x => x.First()).ToList();

                if (groupsDistinct.Count > 0)
                {
                    foreach (var group in groupsDistinct)
                    {
                        try
                        {
                            // Remove event in Microsoft Teams for all users.
                            await GraphAPI.DeleteEventAsync(responseSession.Email, planningId, group.GroupId);
                        }
                        catch (Exception)
                        {
                            // Ignore. This is non-critical.
                        }
                    }
                }
            }

            // Then, archive the plan.
            var teacherPlanToReturn =
              new TeacherPlanDAO().Update(_connectionString, new TeacherPlanModel()
              {
                  Available = "0",
                  ModifiedBy = responseSession.UserId,
              }, new TeacherPlanModel()
              {
                  Id = planningId
              });

            if(teacherPlanToReturn.Count > 0)
            {
                teacherPlanToReturn[0].Valid = true;
                return teacherPlanToReturn[0];
            }
            else
            {
                return new TeacherPlanModel();
            }
        }

        /// <summary>
        /// To modify a planning. 
        /// </summary>
        /// <param name="teacherPlan">Teacher plan to modify.</param>
        /// <param name="responseSession">Session info.</param>
        /// <param name="request">Http Request.</param>
        /// <returns>Retruns the plan modified with transaction details.</returns>
        public async Task<TeacherPlanModel> ModifyPlanning(TeacherPlanModel teacherPlan, ValidationResponseModel responseSession, HttpContext context)
        {
            var teacherPlanToValidateIfChangesAllowed = ValidateIfTeacherPlanAllowChanges(teacherPlan.Id);

            if (!teacherPlanToValidateIfChangesAllowed.Valid)
            {
                return teacherPlanToValidateIfChangesAllowed;
            }

            teacherPlan = CleanTeacherPlanModel(teacherPlan);

            // Find plan instructional detalis.
            var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
            var planningDetail = planingReaderManager.GetTeacherPlanFullDetails(teacherPlan.Id, responseSession.UserId, RequestHandler.GetToken(context));

            if(teacherPlan.OpenDate != null)
            {
                teacherPlan.OpenDate = DatesFormatter.ConvertFromCSharpDateTimeToMySql(teacherPlan.OpenDate);
            }
            if(teacherPlan.CloseDate != null)
            {
                teacherPlan.CloseDate = DatesFormatter.ConvertFromCSharpDateTimeToMySql(teacherPlan.CloseDate);
            }
            if(teacherPlanToValidateIfChangesAllowed.OpenDate != null)
            {
                teacherPlanToValidateIfChangesAllowed.OpenDate = DatesFormatter.ConvertFromCSharpDateTimeToMySql(teacherPlanToValidateIfChangesAllowed.OpenDate);
            }
            if (teacherPlanToValidateIfChangesAllowed.CloseDate != null)
            {
                teacherPlanToValidateIfChangesAllowed.CloseDate = DatesFormatter.ConvertFromCSharpDateTimeToMySql(teacherPlanToValidateIfChangesAllowed.CloseDate);
            }

            var teacherPlanToReturn =
            new TeacherPlanDAO().Update(_connectionString, new TeacherPlanModel()
            {
                Available = teacherPlan.Available ?? teacherPlanToValidateIfChangesAllowed.Available,
                SubjectCode = teacherPlan.SubjectCode ?? teacherPlanToValidateIfChangesAllowed.SubjectCode,
                ModifiedBy = responseSession.UserId,
                LevelCode = teacherPlan.LevelCode ?? teacherPlanToValidateIfChangesAllowed.LevelCode,
                Name = teacherPlan.Name ?? teacherPlanToValidateIfChangesAllowed.Name,
                Opened = teacherPlan.Opened ?? teacherPlanToValidateIfChangesAllowed.Opened,
                PeriodId = teacherPlan.PeriodId ?? teacherPlanToValidateIfChangesAllowed.PeriodId,
                OpenDate = teacherPlan.OpenDate ?? teacherPlanToValidateIfChangesAllowed.OpenDate,
                CloseDate = teacherPlan.CloseDate ?? teacherPlanToValidateIfChangesAllowed.CloseDate,
                WeekNumber = teacherPlan.WeekNumber,
            },
            new TeacherPlanModel()
            {
                Id = teacherPlan.Id,
            })[0];

            if (!teacherPlanToReturn.Valid)
            {
                return teacherPlanToReturn;
            }

            // If the group property exists, check the groups of the plan. 
            if (teacherPlan.Groups != null)
            {
                // Call the current groups of the plan.
                var currentGroups = new TeacherPlanGroupDAO().Read(_connectionStringRead, new TeacherPlanGroupModel() { TeacherPlanId = teacherPlan.Id }, false, false);

                // Set the groups to add if they are not in the current groups.
                var groupsToAdd = teacherPlan.Groups.Where(x => !currentGroups.Any(y => y.GroupId == x.GroupId)).ToList();

                // Set the groups to remove if they are not in the new groups.
                var groupsToRemove = currentGroups.Where(x => !teacherPlan.Groups.Any(y => y.GroupId == x.GroupId)).ToList();

                // Add the new groups if any.
                if (groupsToAdd.Count > 0)
                {
                    groupsToAdd.ForEach(group =>
                    {
                        group.ModifiedBy = responseSession.UserId;
                        group.TeacherPlanId = teacherPlan.Id;
                        group.GroupId = group.GroupId;
                        group.Valid = true;
                    });

                    new TeacherPlanGroupDAO().Create(_connectionString, groupsToAdd);
                }

                // Remove the groups if any.
                if (groupsToRemove.Count > 0)
                {
                    var groupsToRemoveIds = groupsToRemove.Select(x => x.GroupId).ToList();
                    var teacherGroupsToDelete = new List<TeacherPlanGroupModel>();
                    foreach (var groupToRemove in groupsToRemove)
                    {
                        var teacherGroup = new TeacherPlanGroupModel()
                        {
                            GroupId = groupToRemove.GroupId,
                            TeacherPlanId = teacherPlan.Id,
                        };
                        teacherGroupsToDelete.Add(teacherGroup);
                    }

                    new TeacherPlanGroupDAO().Delete(_connectionString, teacherGroupsToDelete);
                }
            }

            teacherPlanToReturn.Valid = true;
            teacherPlanToReturn.Messages = new List<string>()
            {
                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                    AppConfiguration.Configuration["AppSettings:tm_message_code_success"])
            };

            try
            {
                if (teacherPlan.Available == "0")
                {
                    // Selects the first element from each group
                    var groupsDistinct = planningDetail.GroupsAttachedList.GroupBy(g => g.GroupId).Select(x => x.First()).ToList();

                    if (groupsDistinct.Count > 0)
                    {
                        foreach (var group in groupsDistinct)
                        {
                            try
                            {
                                // Remove event in Microsoft Teams for all users.
                                await GraphAPI.DeleteEventAsync(responseSession.Email, planningDetail.TeacherPlanModel.Id, group.GroupId);
                            }
                            catch (Exception)
                            {
                                // Ignore. This is non-critical.
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                teacherPlanToReturn = new TeacherPlanModel()
                {
                    Messages = new List<string>()
                    {
                        ex.ToString()
                    }
                };
                Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
            }

            return teacherPlanToReturn;
        }

        /// <summary>
        /// To modify the plan name.
        /// </summary>
        /// <param name="teacherPlanId">The planning id.</param>
        /// <param name="teacherPlanName">The planning name.</param>
        /// <param name="modifiedBy">The user that is modifying the row.</param>
        /// <returns>Returns Teacher plan model with transaction details.</returns>
        public TeacherPlanModel ModifyPlanName(int teacherPlanId, string teacherPlanName, string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(teacherPlanName))
            {
                // Name require
                return new TeacherPlanModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:tm_message_code_error_name_empty"]),
                    }
                };
            }

            // Truncate name. 
            teacherPlanName = TruncateString(teacherPlanName, MaxNameLength);

            // Modify the plan name
            try
            {
                TeacherPlanModel teacherPlanModel = new TeacherPlanDAO().Update(_connectionString,
                    new TeacherPlanModel()
                    {
                        Name = teacherPlanName,
                        ModifiedBy = modifiedBy,
                    },
                    new TeacherPlanModel()
                    {
                        Id = teacherPlanId.ToString(),
                    })[0];
                teacherPlanModel.Valid = true;
                return teacherPlanModel;
            }
            catch (Exception ex)
            {
                return new TeacherPlanModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        ex.ToString(),
                    }
                };
            }
        }

        /// <summary>
        /// To create a teacher planning.
        /// </summary>
        /// <param name="teacherPlan">Teacher plan to create</param>
        /// <param name="userInSession">The user id of the user that is making the request.</param>
        /// <returns>Returns the transaction details with the model created. </returns>
        public TeacherPlanModel CreatePlanning(TeacherPlanModel teacherPlan, string userInSession)
        {
            TeacherPlanModel teacherPlanningDetailModel = new TeacherPlanModel()
            {
                Valid = true,
                Messages = new List<string>(),
            };

            // Check if name is valid 
            var response =
                CustomClasses.Validator.ValidateName(teacherPlan.Name);

            teacherPlan = CleanTeacherPlanModel(teacherPlan);

            MessageManager messageManager = new MessageManager(_connectionStringRead,
                _langCode);

            if (!response.Valid)
            {
                teacherPlanningDetailModel.Valid = false;
                teacherPlanningDetailModel.Messages = messageManager.GetMessageDetails(response.MessageCodes);
                return teacherPlanningDetailModel;
            }
            else
            {
                teacherPlan.ModifiedBy = userInSession;
                teacherPlanningDetailModel = new TeacherPlanDAO().Create(_connectionString, teacherPlan)[0];

                // Create student groups associated
                if (teacherPlanningDetailModel.Groups != null && teacherPlanningDetailModel.Groups.Count > 0)
                {
                    teacherPlanningDetailModel.Groups.ForEach(group =>
                    {
                        group.ModifiedBy = userInSession;
                        group.TeacherPlanId = teacherPlanningDetailModel.Id;
                        group.Valid = true;
                    });

                    new TeacherPlanGroupDAO().Create(
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                        teacherPlan.Groups);
                }

                teacherPlanningDetailModel.Valid = true;
                teacherPlanningDetailModel.Messages = new List<string>()
                {
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                };
                return teacherPlanningDetailModel;
            }
        }

        /// <summary>
        /// To attach lesson to the plan.
        /// </summary>
        /// <param name="teacherPlanLessonModel">Teacher plan lesson model to add</param>
        /// <param name="teacherPlanLessonQuizList">The teacher plan quizzess to insert.</param>
        /// <param name="teacherPlanLessonDateModelList">The teacher's plan lesson week available dates.</param>
        /// <param name="userInSession">The user id of the user that is making the request.</param>
        /// <param name="skipValidations">true to skip validations.</param>
        /// <returns>Retuns the response with transaction details.</returns>
        public TeacherPlanLessonModel AttachLessonToPlanning(TeacherPlanLessonModel teacherPlanLessonModel,
            List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizList,
            List<TeacherPlanLessonDateModel> teacherPlanLessonDateModelList,
            string userInSession, bool skipValidations = false)
        {
            var responseModel = new TeacherPlanLessonModel()
            {
                Valid = true,
                Messages = new List<string>(),
            };

            var messageManager = new MessageManager(_connectionStringRead,
                _langCode);

            if(teacherPlanLessonModel != null && !string.IsNullOrWhiteSpace(teacherPlanLessonModel.DueDate))
            {
                teacherPlanLessonModel.DueDate = ConvertDateToMidnight(
                    DateTime.Parse(teacherPlanLessonModel.DueDate)).ToString();
            }

            var teacherPlanLessonDAO = new TeacherPlanLessonDAO();
            if (!skipValidations)
            {
                var teacherPlanToValidateIfChangesAllowed =
                    ValidateIfTeacherPlanAllowChanges(teacherPlanLessonModel.PlanId);

                if (!teacherPlanToValidateIfChangesAllowed.Valid)
                {
                    return new TeacherPlanLessonModel()
                    {
                        Valid = false,
                        Messages = teacherPlanToValidateIfChangesAllowed.Messages,
                    };
                }

                if (CustomClasses.DateTimeZoneManager.ConvertToUniversalDateTime(DateTime.Parse(teacherPlanLessonModel.DueDate))
               < CustomClasses.DateTimeZoneManager.ConvertToUniversalDateTime(DateTime.Now.AddDays(-1)))
                {
                    responseModel.Valid = false;
                    responseModel.Messages = new List<string>()
                    {
                        messageManager.GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"])
                    };
                    return responseModel;
                }

                // Check for duplicated 
                if (teacherPlanLessonDAO.Read(_connectionStringRead,
                        new TeacherPlanLessonModel()
                        {
                            PlanId = teacherPlanLessonModel.PlanId,
                            LessonId = teacherPlanLessonModel.LessonId,
                            Available = "1"
                        }, true, true).Count > 0)
                {
                    responseModel.Valid = false;
                    responseModel.Messages = new List<string>()
                    {
                        messageManager.GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:tm_message_code_error_duplicated_lesson"])
                    };
                    return responseModel;
                }

                // Check the date of the quizzes. Due date of the lesson should be minor than 
                // the due date of the quizzes.
                if (teacherPlanLessonQuizList != null && teacherPlanLessonQuizList.Count > 0)
                {
                    // Validate details of the teacher plan lesson quizzess
                    var validateLessonQuizzessResponse =
                    ValidateTeacherPlanLessonQuizzess(teacherPlanLessonQuizList,
                        DateTime.Parse(teacherPlanLessonModel.DueDate), false);
                    if (!validateLessonQuizzessResponse.Valid)
                    {
                        responseModel.Valid = false;
                        responseModel.Messages = validateLessonQuizzessResponse.Messages;
                        return responseModel;
                    }
                    /*
                    foreach (TeacherPlanLessonQuizModel teacherPlanLessonQuizModel in teacherPlanLessonQuizList)
                    {
                        if (DateTime.Parse(teacherPlanLessonModel.DueDate) >
                                                       DateTime.Parse(teacherPlanLessonQuizModel.DueDate))
                        {
                            responseModel.Valid = false;
                            responseModel.Messages = new List<string>()
                            {
                                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"])
                            };
                            return responseModel;
                        }
                    } */
                }
            }

            
            if (string.IsNullOrWhiteSpace(teacherPlanLessonModel.LessonId) ||
                string.IsNullOrWhiteSpace(teacherPlanLessonModel.PlanId))
            {
                responseModel.Valid = false;
                responseModel.Messages = new List<string>()
                {
                    messageManager.GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_lesson_required"])
                };
                return responseModel;
            }

            // Evaluate the date 
            DateTime temp;
            if (!DateTime.TryParse(teacherPlanLessonModel.DueDate, out temp))
            {
                responseModel.Valid = false;
                responseModel.Messages = new List<string>()
                {
                    messageManager.GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"])
                };
                return responseModel;
            } 
            teacherPlanLessonModel.DueDate =
                (DateTime.Parse(teacherPlanLessonModel.DueDate).Date + TimeSpan.Zero).ToString();
            teacherPlanLessonModel.DueDate =
                CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(
                    DateTime.Parse(teacherPlanLessonModel.DueDate));
            teacherPlanLessonModel.ModifiedBy = userInSession;
            responseModel = teacherPlanLessonDAO.Create(_connectionString, teacherPlanLessonModel)[0];
            responseModel.Valid = true;
            responseModel.Messages = new List<string>()
            {
                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
            };

            if (teacherPlanLessonQuizList != null && teacherPlanLessonQuizList.Count > 0)
            {
                // Fix the data if any.
                teacherPlanLessonQuizList = SetDefaultTeacherPlanLessonQuizDetails(
                    teacherPlanLessonQuizList,
                teacherPlanLessonModel.DueDate,
                AppConfiguration.Configuration["AppSettings:default_time_zone_for_exams"],
                responseModel.Id);

                // Fix the dates and set the plan lesson ID.  
                foreach (TeacherPlanLessonQuizModel teacherPlanLessonQuizModel in teacherPlanLessonQuizList)
                {
                    teacherPlanLessonQuizModel.DueDate =
                        CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(
                            DateTime.Parse(teacherPlanLessonQuizModel.DueDate));
                    teacherPlanLessonQuizModel.PlanLessonId = responseModel.Id;
                    teacherPlanLessonQuizModel.ModifiedBy = userInSession;
                }

                // And create them 
                new TeacherPlanLessonQuizDAO().Create(_connectionString, teacherPlanLessonQuizList);
            }

            if (teacherPlanLessonDateModelList != null && teacherPlanLessonDateModelList.Count > 0)
            {
                teacherPlanLessonDateModelList.ForEach(teacherPlanLessonDateModel =>
                {
                    teacherPlanLessonDateModel.ImplementationDate = CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(DateTime.Parse(teacherPlanLessonDateModel.ImplementationDate));
                    teacherPlanLessonDateModel.ModifiedBy = userInSession;
                    teacherPlanLessonDateModel.TeacherPlanCustomLessonId = Convert.ToInt32(responseModel.Id);
                });

                // Create dates
                new TeacherPlanLessonDateDAO().Create(_connectionString, teacherPlanLessonDateModelList);
            }

            return responseModel;
        }

        /// <summary>
		/// To modify the teacher plan lesson.
		/// </summary>
		/// <param name="teacherPlanLessonModel">Plan lesson model to modify.</param>
		/// <param name="userInSession">The user id of the user that is making the request.</param>
        /// <param name="teacherPlanLessonDateModelList">The lessons dates for the planning document. </param>
        /// <param name="teacherPlanLessonQuizModels">The quiz plan lessons (the exams). </param>
		/// <returns>Return a response model with transaction details.</returns>
		public TeacherPlanLessonModel ModifyTeacherPlanLessonModel(TeacherPlanLessonModel teacherPlanLessonModel,
            List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizModels,
            List<TeacherPlanLessonDateModel> teacherPlanLessonDateModelList, string userInSession)
        {
            try
            {
                // Do not allow an empty Id. This is required.
                if (teacherPlanLessonModel == null || string.IsNullOrWhiteSpace(teacherPlanLessonModel.Id))
                {
                    return new TeacherPlanLessonModel()
                    {
                        Valid = false,
                    };
                }

                if (teacherPlanLessonModel != null && !string.IsNullOrWhiteSpace(teacherPlanLessonModel.DueDate))
                {
                      teacherPlanLessonModel.DueDate = ConvertDateToMidnight(
                                                 DateTime.Parse(teacherPlanLessonModel.DueDate)).ToString();
                }

                
                if (teacherPlanLessonDateModelList.Count == 0)
                {
                    // Evaluate if plan can be modified 
                    var teacherPlanToValidateIfChangesAllowed =
                        ValidateIfTeacherPlanAllowChanges(teacherPlanLessonModel.PlanId);
                    if (!teacherPlanToValidateIfChangesAllowed.Valid)
                    {
                        return new TeacherPlanLessonModel()
                        {
                            Valid = false,
                            Messages = teacherPlanToValidateIfChangesAllowed.Messages
                        };
                    }
                    // End of evaluation
                }

                var teacherPlanLessonModelToModify = new TeacherPlanLessonModel()
                {
                    ModifiedBy = userInSession,
                    ForGrading = teacherPlanLessonModel.ForGrading,
                    Available = teacherPlanLessonModel.Available,
                    TriesQuantity = teacherPlanLessonModel.TriesQuantity,
                    PrePostRequired = teacherPlanLessonModel.PrePostRequired,
                };

                var teacherPlanLessonToModifyWhere = new TeacherPlanLessonModel()
                {
                    Id = teacherPlanLessonModel.Id,
                };

                var responseModel = new TeacherPlanLessonModel()
                {
                    Valid = true,
                    Messages = new List<string>(),
                };

                var messageManager = new MessageManager(_connectionStringRead,
                    _langCode);
                // Evaluate the date 
                DateTime temp;
                if (!DateTime.TryParse(teacherPlanLessonModel.DueDate, out temp))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(
                        messageManager.GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"]));
                    return responseModel;
                }

                if (DateTime.TryParseExact(teacherPlanLessonModel.DueDate, "yyyy-MM-dd", null,
                        System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    // Set the time component to midnight
                    DateTime midnight = parsedDate.Date;

                    // You can also set the time directly to midnight using TimeSpan
                    DateTime midnightUsingTimeSpan = parsedDate.Date + TimeSpan.Zero;
                    teacherPlanLessonModel.DueDate = midnightUsingTimeSpan.ToString("MM/dd/yyyy HH:mm:ss");
                }
                // exclude teacher pla lesson quizzess validation when there is a range of dates because never comes together.
                if (teacherPlanLessonQuizModels.Count > 0)
                {
                    // Set the defaults of the teacher plan lesson quizzess
                    teacherPlanLessonQuizModels = SetDefaultTeacherPlanLessonQuizDetails(
                        teacherPlanLessonQuizModels,
                        teacherPlanLessonModel.DueDate,
                        AppConfiguration.Configuration["AppSettings:default_time_zone_for_exams"],
                        teacherPlanLessonModel.Id);

                    // Validate details of the teacher plan lesson quizzess
                    var validateLessonQuizzessResponse =
                        ValidateTeacherPlanLessonQuizzess(teacherPlanLessonQuizModels,
                            DateTime.Parse(teacherPlanLessonModel.DueDate), true);
                    if (!validateLessonQuizzessResponse.Valid)
                    {
                        responseModel.Valid = false;
                        responseModel.Messages = validateLessonQuizzessResponse.Messages;
                        return responseModel;
                    }
                }

                teacherPlanLessonModelToModify.DueDate =
                    CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(
                        DateTime.Parse(teacherPlanLessonModel.DueDate));

                // Check and modify the lessons completed or not completed when modifying the PrePostRequired field.
                UpdateLessonCompletionStatus(teacherPlanLessonModel, userInSession, teacherPlanLessonQuizModels.Select(q => q.QuizId).ToList());

                responseModel = new TeacherPlanLessonDAO().Update(_connectionString, teacherPlanLessonModelToModify,
                    teacherPlanLessonToModifyWhere)[0];
                responseModel.Valid = true;
                responseModel.Messages = new List<string>()
                {
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"])
                };

                // If quizzes were sent...
                if (teacherPlanLessonQuizModels.Count > 0)
                {
                    // Update the teacher plan lesson quizzess 
                    var teacherPlanLessonQuizzesModelsToInsertList = new List<TeacherPlanLessonQuizModel>();
                    var teacherPlanlessonQuizzesModelsToUpdateList = new List<TeacherPlanLessonQuizModel>();
                    var teacherPlanlessonQuizzesModelsToUpdatWhereList = new List<TeacherPlanLessonQuizModel>();

                    foreach (TeacherPlanLessonQuizModel teacherPlanLessonQuizModel in teacherPlanLessonQuizModels)
                    {
                        if (string.IsNullOrWhiteSpace(teacherPlanLessonQuizModel.Id))
                        {
                            // Set the new         
                            teacherPlanLessonQuizzesModelsToInsertList.Add(new TeacherPlanLessonQuizModel()
                            {
                                DueDate = CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(
                                    DateTime.Parse(teacherPlanLessonQuizModel.DueDate)),
                                DueDateTimezone = teacherPlanLessonQuizModel.DueDateTimezone,
                                Duration = teacherPlanLessonQuizModel.Duration,
                                PlanLessonId = teacherPlanLessonModel.Id,
                                ModifiedBy = userInSession,
                                QuizId = teacherPlanLessonQuizModel.QuizId,
                                Required = teacherPlanLessonQuizModel.Required,
                                ForGrading = teacherPlanLessonQuizModel.ForGrading,
                            });
                        }
                        else
                        {
                            // If it is null, then it is an update 
                            teacherPlanlessonQuizzesModelsToUpdateList.Add(new TeacherPlanLessonQuizModel()
                            {
                                DueDate = CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(
                                    DateTime.Parse(teacherPlanLessonQuizModel.DueDate)),
                                DueDateTimezone = teacherPlanLessonQuizModel.DueDateTimezone,
                                Duration = teacherPlanLessonQuizModel.Duration,
                                ModifiedBy = userInSession,
                                Required = teacherPlanLessonQuizModel.Required,
                                ForGrading = teacherPlanLessonQuizModel.ForGrading,
                            });
                            teacherPlanlessonQuizzesModelsToUpdatWhereList.Add(new TeacherPlanLessonQuizModel()
                            {
                                Id = teacherPlanLessonQuizModel.Id
                            });
                        }
                    }

                    if (teacherPlanLessonQuizzesModelsToInsertList.Count > 0)
                    {
                        // Insert them
                        new TeacherPlanLessonQuizDAO().Create(_connectionString,
                            teacherPlanLessonQuizzesModelsToInsertList);
                    }

                    if (teacherPlanlessonQuizzesModelsToUpdateList.Count > 0)
                    {
                        // Update
                        new TeacherPlanLessonQuizDAO().Update(_connectionString,
                            teacherPlanlessonQuizzesModelsToUpdateList,
                            teacherPlanlessonQuizzesModelsToUpdatWhereList);
                    }
                }

                // If dates were sent...
                if (teacherPlanLessonDateModelList.Count > 0)
                {
                    var teacherPlanLessonDateDao = new TeacherPlanLessonDateDAO();
                    teacherPlanLessonDateDao.Delete(_connectionString, Convert.ToInt32(teacherPlanLessonModel.Id));

                    teacherPlanLessonDateModelList.ForEach(teacherPlanLessonDateModel =>
                    {
                        teacherPlanLessonDateModel.ModifiedBy = userInSession;
                        teacherPlanLessonDateModel.TeacherPlanCustomLessonId = Convert.ToInt32(teacherPlanLessonModel.Id);
                    });
                    teacherPlanLessonDateDao.Create(_connectionString, teacherPlanLessonDateModelList);
                }
                else
                {
                    // Delete
                    var teacherPlanLessonDateDao = new TeacherPlanLessonDateDAO();
                    teacherPlanLessonDateDao.Delete(_connectionString, Convert.ToInt32(teacherPlanLessonModel.Id));
                }

                return responseModel;
            }
            catch (Exception ex)
            {
                return new TeacherPlanLessonModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        ex.ToString()
                    }
                };
            }
        }

        /// <summary>
        /// To create a student plan lesson if it does not exist.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="planLessonId">The plan lesson id.</param>
        /// <param name="userInSession">The user making the request.</param>
        public void CreateStudentPlanLessonIfNotExists(string studentId, string planLessonId, string userInSession)
        {
            // Check if the student plan lesson exists
            var studentPlanLesson = new PlanLessonUserDAO().Read(_connectionStringRead, new PlanLessonUserModel()
            {
                UserId = studentId,
                PlanLessonId = planLessonId
            }, false, false).FirstOrDefault();

            // If it does not exist, create it
            if (studentPlanLesson == null)
            {
                // Get the lesson id by plan lesson id
                var planLesson = new TeacherPlanLessonDAO().Read(_connectionStringRead, new TeacherPlanLessonModel()
                {
                    Id = planLessonId
                }, true, true).FirstOrDefault();

                var newStudentPlanLesson = new PlanLessonUserModel()
                {
                    UserId = studentId,
                    PlanId = planLesson.PlanId,
                    PlanLessonId = planLessonId,
                    LessonId = planLesson.LessonId,
                    Completed = "0",
                    ModifiedBy = userInSession,
                };
                new PlanLessonUserDAO().Create(_connectionString, new List<PlanLessonUserModel>() { newStudentPlanLesson });
            }
        }

        /// <summary>
        /// To check and modify the lessons completed or not completed when modifying the PrePostRequired field.
        /// </summary>
        /// <param name="teacherPlanLessonModel">The teacher plan lesson model to check.</param>
        /// <param name="userInSession">The user making the request.</param>
        /// <param name="lessonQuizzes">The list of lesson quizzes associated with the lesson.</param>
        /// <param name="studentId">Optional: The student id if sent.</param>
        public void UpdateLessonCompletionStatus(TeacherPlanLessonModel teacherPlanLessonModel, string userInSession, List<string> lessonQuizzes, string studentId = null)
        {
            // Read the teacher plan lesson model to check if PrePostRequired is changed
            TeacherPlanLessonModel existingLesson = new TeacherPlanLessonDAO().Read(_connectionStringRead,
                               new TeacherPlanLessonModel() { Id = teacherPlanLessonModel.Id }, true, true)[0];

            // Call the pre/post form user results. 
            var planPrePostUserResultsToFind = new List<PlanPrePostUserResultModel>()
            {
                new PlanPrePostUserResultModel()
                {
                    PlanLessonId = teacherPlanLessonModel.Id,
                }
            };

            var planPrePostUserResults = new PlanPrePostUserResultDAO().Read(_connectionStringRead, new PlanPrePostUserResultModel()
            {
                PlanLessonId = teacherPlanLessonModel.Id,
                UserId = studentId
            }, false, false);
            var planLessonQuizUserResults = new PlanLessonQuizUserResultDAO().Read(_connectionStringRead, new PlanLessonQuizUserResultModel()
            {
                PlanLessonId = teacherPlanLessonModel.Id,
                UserId = studentId
            }, false, false);

            var planLessonUserDao = new PlanLessonUserDAO();

            var planLessonUsers = planLessonUserDao.Read(_connectionStringRead, new PlanLessonUserModel()
            {
                PlanLessonId = teacherPlanLessonModel.Id,
                UserId = studentId
            }, false, false);

            var planLessonUsersToUpdate = new List<PlanLessonUserModel>();
            var planLessonUserModelsToUpdateWhere = new List<PlanLessonUserModel>();

            // Now for each user, check if they have pre/post results
            foreach (var planLessonUser in planLessonUsers)
            {
                // First, check if all the quizzes are completed by the user

                // Get the user pre/post results for the current user
                var userPrePostResults = planPrePostUserResults.Where(x => x.UserId == planLessonUser.UserId && planLessonUser.PlanLessonId == x.PlanLessonId).ToList();

                // Same for the quizzes, get the user lesson quiz results for the current user
                // Filter all results for the user
                var userQuizResults = planLessonQuizUserResults
                    .Where(x => x.UserId == planLessonUser.UserId)
                    .ToList();

                // Further filter to match the current lesson
                var userQuizResultsForLesson = userQuizResults
                    .Where(x => x.PlanLessonId == planLessonUser.PlanLessonId)
                    .ToList();

                // Finally, check if quiz ID is in the provided list
                var userLessonQuizResults = userQuizResultsForLesson
                    .Where(x => lessonQuizzes.Contains(x.QuizId))
                    .ToList();

                // If the user has no quizzes results, then set the lesson as not completed
                if (userLessonQuizResults.Count == 0)
                {
                    planLessonUsersToUpdate.Add(new PlanLessonUserModel()
                    {
                        Completed = "0",
                        ModifiedBy = userInSession,
                    });
                    planLessonUserModelsToUpdateWhere.Add(new PlanLessonUserModel() { Id = planLessonUser.Id });
                    continue; // Skip to the next user
                }

                // Calculate average score for completed quizzes
                var userScoredQuizzes = userLessonQuizResults
                    .Where(x => lessonQuizzes.Contains(x.QuizId) && x.Score != null)
                    .ToList();

                double averageScore = 0;

                if (userScoredQuizzes.Count > 0)
                {
                    double rawAverage = userScoredQuizzes.Average(x => double.Parse(x.Score));
                    averageScore = Math.Truncate(rawAverage * 100) / 100;
                }

                // The user lesson quiz results (all the quizzes ids, should be in the lesson quizzes) to be considered as completed
                // If the user has quizzes results, then set the lesson as completed
                bool examsCompleted = true;
                foreach (var lessonQuiz in lessonQuizzes) {
                    if(userLessonQuizResults.Find(x => x.QuizId == lessonQuiz) == null)
                    {
                        examsCompleted = false;
                    } 
                }

                // Now check if the pre/post results are completed
                bool preCompleted = userPrePostResults.Any(x => x.PrePost == "0");
                bool postCompleted = userPrePostResults.Any(x => x.PrePost == "1");

                // If pre/post required is true, then check if both pre and post are completed
                if (teacherPlanLessonModel.PrePostRequired == "1")
                {
                    // If pre/post required is true, then check if both pre and post are completed
                    if (preCompleted && postCompleted && examsCompleted)
                    {
                        planLessonUsersToUpdate.Add(new PlanLessonUserModel()
                        {
                            Completed = "1",
                            ModifiedBy = userInSession,
                            OverallScore = averageScore.ToString("F2"),
                        });
                        planLessonUserModelsToUpdateWhere.Add(new PlanLessonUserModel() { Id = planLessonUser.Id });
                    }
                    else
                    {
                        planLessonUsersToUpdate.Add(new PlanLessonUserModel()
                        {
                            Completed = "0",
                            ModifiedBy = userInSession,
                            OverallScore = averageScore.ToString("F2"),
                        });
                        planLessonUserModelsToUpdateWhere.Add(new PlanLessonUserModel() { Id = planLessonUser.Id });
                    }
                }
                else
                {
                    // If pre/post required is false, then set the lesson as completed if the quizzes are completed
                    if (examsCompleted)
                    {
                        planLessonUsersToUpdate.Add(new PlanLessonUserModel()
                        {
                            Completed = "1",
                            ModifiedBy = userInSession,
                            OverallScore = averageScore.ToString("F2"),
                        });
                        planLessonUserModelsToUpdateWhere.Add(new PlanLessonUserModel() { Id = planLessonUser.Id });
                    }
                    else
                    {
                        planLessonUsersToUpdate.Add(new PlanLessonUserModel()
                        {
                            Completed = "0",
                            ModifiedBy = userInSession,
                            OverallScore = averageScore.ToString("F2"),
                        });
                        planLessonUserModelsToUpdateWhere.Add(new PlanLessonUserModel() { Id = planLessonUser.Id });
                    }
                }
            }

            // If there are users to update, then update them
            if (planLessonUsersToUpdate.Count > 0)
            {
                planLessonUserDao.Update(_connectionString, planLessonUsersToUpdate, planLessonUserModelsToUpdateWhere);
            }
        }

        /// <summary>
        /// To attach quiz to the plan.
        /// </summary>
        /// <param name="teacherPlanQuizModel">Teacher plan quiz model to add</param>
        /// <param name="skipValidations">To skip advanced validations. </param>
        /// <returns>Retuns the response with transaction details.</returns>
        public TeacherPlanQuizModel AttachQuizToPlanning(TeacherPlanQuizModel teacherPlanQuizModel, bool skipValidations = false)
        {
            var responseModel = new TeacherPlanQuizModel()
            {
                Valid = true,
                Messages = new List<string>(),
            };

            var messageManager = new MessageManager(_connectionStringRead,
            _langCode);
            if (string.IsNullOrWhiteSpace(teacherPlanQuizModel.QuizId) || string.IsNullOrWhiteSpace(teacherPlanQuizModel.PlanId))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_homework_required"]));
                return responseModel;
            }

            // Evaluate if plan can be modified 
            if (!skipValidations)
            {
                var teacherPlanToValidateIfChangesAllowed = ValidateIfTeacherPlanAllowChanges(teacherPlanQuizModel.PlanId);
                if (!teacherPlanToValidateIfChangesAllowed.Valid)
                {
                    return new TeacherPlanQuizModel()
                    {
                        Valid = false,
                        Messages = teacherPlanToValidateIfChangesAllowed.Messages
                    };
                }
            }
            // End of evaluation

            // Check if assignments exists for plan
            if (new TeacherPlanQuizDAO().Read(_connectionStringRead, new TeacherPlanQuizModel()
            {
                PlanId = teacherPlanQuizModel.PlanId,
                QuizId = teacherPlanQuizModel.QuizId,
                Available = "1"
            }, true, true).Count() > 0)
            {
                // Sorry, duplicated. 
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_homework_assigned_before_error"]));
                return responseModel;
            }

            // Evaluate the date 
            DateTime temp;
            if (!DateTime.TryParse(teacherPlanQuizModel.DueDate, out temp))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"]));
                return responseModel;
            }

            if (!skipValidations)
            {
                DateTime dateTimeToInsert = DateTime.Parse(teacherPlanQuizModel.DueDate);
                dateTimeToInsert = dateTimeToInsert.AddHours(23).AddMinutes(59).AddSeconds(59);
                teacherPlanQuizModel.DueDate = dateTimeToInsert.ToString("yyyy-MM-dd HH:mm:ss");

                if (dateTimeToInsert < DateTime.Now)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"]));
                    return responseModel;
                }
            }

            responseModel = new TeacherPlanQuizDAO().Create(_connectionString, teacherPlanQuizModel)[0];
            responseModel.Valid = true;
            responseModel.Messages.Add(
                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]));
            return responseModel;
        }

        /// <summary>
        /// Add or update event in Microsoft Teams calendar using Graph API Utils.
        /// </summary>
        /// <param name="planningDetail">Plan details response model.</param>
        /// <param name="designationId">Lesson Id or quiz Id.</param>
        /// <param name="responseSession">Session info.</param>
        /// <param name="isAttach">Indicates the type of operation to know in which property to look for the value of the designation.</param>
        /// <param name="isQuiz">Indicate if it is a quiz or a lesson.</param>
        /// <returns>Return a async task.</returns>
        public async Task AddOrUpdateDesignationToParticipantsTeamCalendar(Domain.Models.Generals.Responses.TeacherPlanningDetailResponseModel planningDetail, string designationId, ValidationResponseModel responseSession, bool isAttach, bool isQuiz = false)
        {
            return; // This is not implemented for San Juan yet and requires a lot of testing. 

            Domain.Models.Generals.Responses.PlanningAttachedDetailsModel quizEvent = null;
            Domain.Models.Generals.Responses.PlanningAttachedDetailsModel lessonEvent = null;

            if (isQuiz)
            {
                // Find quiz containing meeting content.
                quizEvent = planningDetail.QuizzesAttachedModelList.FirstOrDefault(quiz => (isAttach ? quiz.ItemId == designationId : quiz.Id == designationId));
            }
            else
            {
                // Find lesson containing meeting content.
                lessonEvent = planningDetail.LessonAttachedModelList.FirstOrDefault(lesson => (isAttach ? lesson.ItemId == designationId : lesson.Id == designationId));
            }

            // Selects the first element from each group
            List<Domain.Models.Generals.Responses.TeacherPlanGroupResponseModel> groupsDistinct = planningDetail.GroupsAttachedList.GroupBy(g => g.GroupId).Select(x => x.First()).ToList();

            // Validate that the instructional plan has been added to a group.
            if (groupsDistinct.Count > 0)
            {
                foreach (Domain.Models.Generals.Responses.TeacherPlanGroupResponseModel group in groupsDistinct)
                {
                    // Get the teacher groups details with Dapper to obtain users ids.
                    List<Domain.Models.Generals.Dapper.TeacherGroupStudentModel> teacherGroupStudentModelList = TeacherGroups.GetTeacherGroups(responseSession.UserId, group.GroupId);
                    UserResponseModel students = GlobalManager.GetUsersIdsFromTeacherGroup(teacherGroupStudentModelList, responseSession);

                    if (isQuiz)
                    {
                        // Create event in Microsoft Teams for all users.
                        await GraphAPI.CreateEventAsync(students.UserInfo, responseSession, quizEvent, planningDetail.TeacherPlanModel.Id, group.GroupId);
                    }
                    else
                    {
                        // Read the plan lesson details to obtain exams. 
                        Domain.Models.Generals.Responses.TeacherPlanLessonFullDetailModel teacherPlanFullDetails = TeacherPlans.GetTeacherPlanLessonFullDetails(lessonEvent.Id);

                        // Select required and sort the exams
                        List<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel> sortedExams = teacherPlanFullDetails.TeacherPlanLessonQuizModels.Where(obj => obj.Required == "1").OrderBy(obj => obj.DueDate).ToList();

                        // Create event in Microsoft Teams for all users in each exams.
                        for (int i = 0; i < sortedExams.Count; i++)
                        {
                            await GraphAPI.CreateEventAsync(students.UserInfo, responseSession, lessonEvent, planningDetail.TeacherPlanModel.Id, group.GroupId, sortedExams[i], i + 1);
                        }
                    }
                }
            }
            else
            {
                // Create event in Microsoft Teams only for teacher.
                await GraphAPI.CreateEventAsync(null, responseSession, isQuiz ? quizEvent : lessonEvent, planningDetail.TeacherPlanModel.Id, string.Empty);
            }
        }



        /// <summary>
        /// Remove Events From Participants Team Calendar.
        /// </summary>
        /// <param name="planningDetail">Plan details response model.</param>
        /// <param name="designationId">Lesson Id or quiz Id.</param>
        /// <param name="responseSession">Session info.</param>
        /// <param name="isQuiz">Indicate if it is a quiz or a lesson.</param>
        /// <returns>Return a async task.</returns>
        public async Task RemoveEventsFromParticipantsTeamCalendar(Domain.Models.Generals.Responses.TeacherPlanningDetailResponseModel planningDetail, string designationId, ValidationResponseModel responseSession, bool isQuiz = false)
        {
            Domain.Models.Generals.Responses.PlanningAttachedDetailsModel quizEvent = null;
            Domain.Models.Generals.Responses.PlanningAttachedDetailsModel lessonEvent = null;
            List<Domain.Models.Generals.Dapper.TeacherGroupStudentModel> teacherGroupStudentModelList = null;
            Domain.Models.Generals.Responses.TeacherPlanLessonFullDetailModel teacherPlanFullDetails = null;
            List<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel> sortedExams = null;

            if (isQuiz)
            {
                // Find quiz containing meeting content.
                quizEvent = planningDetail.QuizzesAttachedModelList.FirstOrDefault(quiz => quiz.Id == designationId);
            }
            else
            {
                // Find lesson containing meeting content.
                lessonEvent = planningDetail.LessonAttachedModelList.FirstOrDefault(lesson => lesson.Id == designationId);
                // Read the plan lesson details to obtain exams. 
                teacherPlanFullDetails = TeacherPlans.GetTeacherPlanLessonFullDetails(lessonEvent.Id);
                // Select required and sort the exams
                sortedExams = teacherPlanFullDetails.TeacherPlanLessonQuizModels.Where(obj => obj.Required == "1").OrderBy(obj => obj.DueDate).ToList();
            }

            // Selects the first element from each group
            List<Domain.Models.Generals.Responses.TeacherPlanGroupResponseModel> groupsDistinct = planningDetail.GroupsAttachedList.GroupBy(g => g.GroupId).Select(x => x.First()).ToList();

            if (groupsDistinct.Count > 0)
            {
                foreach (Domain.Models.Generals.Responses.TeacherPlanGroupResponseModel group in groupsDistinct)
                {
                    // Get the teacher groups details with Dapper to obtain users     ids.
                    teacherGroupStudentModelList = TeacherGroups.GetTeacherGroups(responseSession.UserId, group.GroupId);

                    if (isQuiz)
                    {
                        // Remove event in Microsoft Teams for all users.
                        await GraphAPI.DeleteEventAsync(responseSession.Email, planningDetail.TeacherPlanModel.Id, group.GroupId, quizEvent.Title);
                    }
                    else
                    {
                        // Remove event in Microsoft Teams for all users in each exams.
                        for (int i = 0; i < sortedExams.Count; i++)
                        {
                            await GraphAPI.DeleteEventAsync(responseSession.Email, planningDetail.TeacherPlanModel.Id, group.GroupId, $"{lessonEvent.Title} - Exam {i + 1}");
                        }
                    }
                }
            }
            else
            {
                if (isQuiz)
                {
                    // Delete event in Microsoft Teams only for teacher.
                    await GraphAPI.DeleteEventAsync(responseSession.Email, planningDetail.TeacherPlanModel.Id, string.Empty, quizEvent.Title);
                }
                else
                {
                    // Remove event in Microsoft Teams for all users in each exams.
                    for (int i = 0; i < sortedExams.Count; i++)
                    {
                        await GraphAPI.DeleteEventAsync(responseSession.Email, planningDetail.TeacherPlanModel.Id, string.Empty, $"{lessonEvent.Title} - Exam {i + 1}");
                    }
                }
            }
        }

        /// <summary>
        /// Create or remove participants' calendar events when groups are added or removed from the instructional plan.
        /// </summary>
        /// <param name="planId">The Plan Id.</param>
        /// <param name="groupId">The group Id.</param>
        /// <param name="responseSession">Session info.</param>
        /// <param name="planingReaderManager">Manager object to read from database.</param>
        /// <param name="request">HTTP Request for access token.</param>
        /// <param name="delete">Indicates whether the action to be performed is to add or delete.</param>
        /// <returns>Return a async task.</returns>
        public async Task ManageCalendarDesignationsInGroupsOfParticipants(string planId, string groupId, ValidationResponseModel responseSession, PlanningReaderManager planingReaderManager, HttpContext context, bool delete = false)
        {
            // Find plan instructional detalis.
            Domain.Models.Generals.Responses.TeacherPlanningDetailResponseModel planingDetail = planingReaderManager.GetTeacherPlanFullDetails(planId, responseSession.UserId, RequestHandler.GetToken(context));
            // Get the teacher groups details with Dapper to obtain users ids.
            List<Domain.Models.Generals.Dapper.TeacherGroupStudentModel> teacherGroupStudentModelList = TeacherGroups.GetTeacherGroups(responseSession.UserId, groupId);
            UserResponseModel students = GlobalManager.GetUsersIdsFromTeacherGroup(teacherGroupStudentModelList, responseSession);

            foreach (Domain.Models.Generals.Responses.PlanningAttachedDetailsModel quizEvent in planingDetail.QuizzesAttachedModelList)
            {
                if (delete)
                {
                    // Remove event in Microsoft Teams for all users.
                    await GraphAPI.DeleteEventAsync(responseSession.Email, planingDetail.TeacherPlanModel.Id, groupId, quizEvent.Title);
                }
                else
                {
                    // Create event in Microsoft Teams for all users.
                    await GraphAPI.CreateEventAsync(students.UserInfo, responseSession, quizEvent, planingDetail.TeacherPlanModel.Id, groupId);
                }
            }

            foreach (Domain.Models.Generals.Responses.PlanningAttachedDetailsModel lessonEvent in planingDetail.LessonAttachedModelList)
            {
                // Read the plan lesson details to obtain exams. 
                Domain.Models.Generals.Responses.TeacherPlanLessonFullDetailModel teacherPlanFullDetails = TeacherPlans.GetTeacherPlanLessonFullDetails(lessonEvent.Id);
                // Select required and sort the exams
                List<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel> sortedExams = teacherPlanFullDetails.TeacherPlanLessonQuizModels.Where(obj => obj.Required == "1").OrderBy(obj => obj.DueDate).ToList();

                // Create event in Microsoft Teams for all users in each exams.
                for (int i = 0; i < sortedExams.Count; i++)
                {
                    if (delete)
                    {
                        await GraphAPI.DeleteEventAsync(responseSession.Email, planingDetail.TeacherPlanModel.Id, groupId, $"{lessonEvent.Title} - Exam {i + 1}");
                    }
                    else
                    {
                        await GraphAPI.CreateEventAsync(students.UserInfo, responseSession, lessonEvent, planingDetail.TeacherPlanModel.Id, groupId, sortedExams[i], i + 1);
                    }
                }
            }
        }


        /// <summary>
        /// To modify the teacher plan quiz.
        /// </summary>
        /// <param name="teacherPlanQuizModel">Plan quiz model to modify.</param>
        /// <param name="userInSession">The user in session.</param>
        /// <returns>Return a response model with transaction details.</returns>
        public TeacherPlanQuizModel ModifyTeacherPlaQuizModel(TeacherPlanQuizModel teacherPlanQuizModel, string userInSession)
        {
            // Do not allow an empty Id. This is required.
            if (string.IsNullOrWhiteSpace(teacherPlanQuizModel.Id))
            {
                return new TeacherPlanQuizModel()
                {
                    Valid = false,
                };
            }

            // Evaluate if plan can be modified 
            TeacherPlanModel teacherPlanToValidateIfChangesAllowed = ValidateIfTeacherPlanAllowChanges(teacherPlanQuizModel.PlanId);
            if (!teacherPlanToValidateIfChangesAllowed.Valid)
            {
                return new TeacherPlanQuizModel()
                {
                    Valid = false,
                    Messages = teacherPlanToValidateIfChangesAllowed.Messages
                };
            }
            // End of evaluation

            TeacherPlanQuizModel teacherPlanModelToModify = new TeacherPlanQuizModel()
            {
                ModifiedBy = userInSession,
                ForGrading = teacherPlanQuizModel.ForGrading,
                Available = teacherPlanQuizModel.Available,
                TriesQuantity = teacherPlanQuizModel.TriesQuantity,
            };

            TeacherPlanQuizModel teacherPlanToModifyWhere = new TeacherPlanQuizModel()
            {
                Id = teacherPlanQuizModel.Id,
            };

            TeacherPlanQuizModel responseModel = new TeacherPlanQuizModel()
            {
                Valid = true,
                Messages = new List<string>(),
            };
            MessageManager messageManager = new MessageManager(_connectionStringRead,
           _langCode);
            // Evaluate the date 
            DateTime temp;
            if (!DateTime.TryParse(teacherPlanQuizModel.DueDate, out temp))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"]));
                return responseModel;
            }

            if (!string.IsNullOrWhiteSpace(teacherPlanQuizModel.DueDate))
            {

                DateTime dateTimeToInsert = DateTime.Parse(teacherPlanQuizModel.DueDate);
                dateTimeToInsert = dateTimeToInsert.AddHours(23).AddMinutes(59).AddSeconds(59);

                if (dateTimeToInsert < DateTime.Now)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"]));
                    return responseModel;
                }

                teacherPlanModelToModify.DueDate = dateTimeToInsert.ToString("yyyy-MM-dd HH:mm:ss");
            }

            responseModel = new TeacherPlanQuizDAO().Update(_connectionString, teacherPlanModelToModify, teacherPlanToModifyWhere)[0];
            responseModel.Valid = true;
            responseModel.Messages = new List<string>()
            {
                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"])
            };
            return responseModel;
        }

        /// <summary>
        /// Deletes a quiz from a planning.
        /// </summary>
        /// <param name="teacherPlanQuizModel">Teacher Plan Quiz Model to delete (What it is important is the id)</param>
        /// <returns>Returns a HTTP Response message with transaction details.</returns>
        public ResponseModel DeleteQuizFromPlanning(TeacherPlanQuizModel teacherPlanQuizModel)
        {
            var responseModel = new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>(),
            };

            // Evaluate if plan can be modified 
            var teacherPlanToValidateIfChangesAllowed = ValidateIfTeacherPlanAllowChanges(teacherPlanQuizModel.PlanId);
            if (!teacherPlanToValidateIfChangesAllowed.Valid)
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = teacherPlanToValidateIfChangesAllowed.Messages
                };
            }
            // End of evaluation

            MessageManager messageManager = new MessageManager(_connectionStringRead,
            _langCode);
            if (string.IsNullOrWhiteSpace(teacherPlanQuizModel.Id))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_homework_required"]));
                return responseModel;
            }

            responseModel.Valid = true;
            new TeacherPlanQuizDAO().Update(_connectionString, new TeacherPlanQuizModel()
            {
                Available = "0"
            },
               new TeacherPlanQuizModel()
               {
                   Id = teacherPlanQuizModel.Id,
               });

            responseModel.Messages = new List<string>()
            {
                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"])
            };

            return responseModel;
        }

        /// <summary>
        /// Deletes a lesson from a planning.
        /// </summary>
        /// <param name="teacherPlanLessonModel">Teacher Plan Lesson Model to delete (What it is important is the id)</param>
        /// <returns>Returns a HTTP Response message with transaction details.</returns>
        public ResponseModel DeleteLessonFromPlanning(TeacherPlanLessonModel teacherPlanLessonModel)
        {
            var responseModel = new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>(),
            };

            // Evaluate if plan can be modified 
            TeacherPlanModel teacherPlanToValidateIfChangesAllowed = ValidateIfTeacherPlanAllowChanges(teacherPlanLessonModel.PlanId);
            if (!teacherPlanToValidateIfChangesAllowed.Valid)
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = teacherPlanToValidateIfChangesAllowed.Messages
                };
            }
            // End of evaluation

            var messageManager = new MessageManager(_connectionStringRead,
            _langCode);
            if (string.IsNullOrWhiteSpace(teacherPlanLessonModel.Id))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_lesson_required"]));
                return responseModel;
            }

            responseModel.Valid = true;
            new TeacherPlanLessonDAO().Update(_connectionString, new TeacherPlanLessonModel()
            {
                Available = "0"
            },
            new TeacherPlanLessonModel()
            {
                Id = teacherPlanLessonModel.Id,
            });

            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]));
            return responseModel;
        }

        /// <summary>
        /// To attach an instructional plan to a group. Once attached, the students of the group will have access to the lessons and assignments,
        /// related to the intructional plan.
        /// </summary>
        /// <param name="teacherPlanGroupModels">The teacher plan group model with the group and plan id to add.</param>
        /// <param name="userSessionId">The user in session.</param>
        /// <returns>Returns the teacher plan group model with transaction details.</returns>
        public TeacherPlanGroupModel AttachGroupToPlanning(List<TeacherPlanGroupModel> teacherPlanGroupModels, string userSessionId)
        {
            var messageManager = new MessageManager(_connectionStringRead, _langCode);

            // If group id or teacher plan id is null or empty, send error.
            var teacherPlanGroupModelCheckIfExists = new List<TeacherPlanGroupModel>();
            foreach (TeacherPlanGroupModel teacherPlanGroupModel in teacherPlanGroupModels)
            {
                teacherPlanGroupModel.ModifiedBy = userSessionId;
                teacherPlanGroupModelCheckIfExists.Add(new TeacherPlanGroupModel()
                {
                    TeacherPlanId = teacherPlanGroupModel.TeacherPlanId,
                    GroupId = teacherPlanGroupModel.GroupId,
                });

                if (string.IsNullOrWhiteSpace(teacherPlanGroupModel.GroupId) || string.IsNullOrWhiteSpace(teacherPlanGroupModel.TeacherPlanId))
                {
                    return new TeacherPlanGroupModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                    {
                        messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_messages_codes_required_fields"])
                    }
                    };
                }
            }

            // Check the ones that exists
            var teacherPlanGroupDAO = new TeacherPlanGroupDAO();
            if (teacherPlanGroupModelCheckIfExists.Count > 0)
            {
                var teacherPlanGroupModelCheckIfExistsCheck = teacherPlanGroupDAO.Read(_connectionStringRead, teacherPlanGroupModelCheckIfExists);
                if (teacherPlanGroupModelCheckIfExistsCheck != null && teacherPlanGroupModelCheckIfExistsCheck.Count > 0)
                {
                    foreach (TeacherPlanGroupModel teacherPlanGroupExist in teacherPlanGroupModelCheckIfExistsCheck)
                    {
                        teacherPlanGroupModels.RemoveAll(x => x.GroupId == teacherPlanGroupExist.GroupId && x.TeacherPlanId == teacherPlanGroupExist.TeacherPlanId);
                    }
                }
            }

            // Create teacher plan
            if (teacherPlanGroupModels != null && teacherPlanGroupModels.Count > 0)
            {
                // Double check up for duplicates
                teacherPlanGroupModels = teacherPlanGroupModels.GroupBy(x => new { x.TeacherPlanId, x.GroupId })
                .Select(p => p.FirstOrDefault())
                .Select(p => new TeacherPlanGroupModel
                {
                    TeacherPlanId = p.TeacherPlanId,
                    GroupId = p.GroupId,
                })
                .ToList();
                teacherPlanGroupDAO.Create(_connectionString, teacherPlanGroupModels);
            }

            return new TeacherPlanGroupModel()
            {
                Valid = true,
                Messages = new List<string>()
                {
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"])
                }
            };
        }

        /// <summary>
        /// To delete a instructional plan from a group. Once the group is deleted from the planning, the student will not have 
        /// access to the lessons and assignments related to this.
        /// </summary>
        /// <param name="teacherPlanGroupModel">The teacher's plan group to delete.</param>
        /// <returns>Returns the teacher plan group model with transaction details.</returns>
        public TeacherPlanGroupModel DeleteGroupFromPlanning(TeacherPlanGroupModel teacherPlanGroupModel)
        {
            var messageManager = new MessageManager(_connectionStringRead, _langCode);
            // Create teacher plan
            var teacherPlanGroup = new TeacherPlanGroupDAO().Delete(_connectionString, teacherPlanGroupModel)[0];
            teacherPlanGroup.Valid = true;
            teacherPlanGroup.Messages = new List<string>()
            {
                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"])
            };

            return teacherPlanGroup;
        }

        /// <summary>
        /// To validate if teacher's instructional plan stills valid to make changes.
        /// </summary>
        /// <param name="teacherPlanId">The teacher instrunctional plan.</param>
        /// <returns>Returns the teacher plan with details if can be modified or not.</returns>
        public TeacherPlanModel ValidateIfTeacherPlanAllowChanges(string teacherPlanId)
        {
            // Read if deleted or closed. If deleted or closed, it cannot be changed. 
            TeacherPlanDAO teacherPlanDAO = new TeacherPlanDAO();

            // If not opened or deleted, they cannot be modified
            List<TeacherPlanModel> teacherPlans = teacherPlanDAO.Read(_connectionStringRead, new TeacherPlanModel() { Id = teacherPlanId }, true, true);
            if (teacherPlans.Count == 0)
            {
                return new TeacherPlanModel()
                {
                    Valid = false,
                };
            }

            if (teacherPlans[0].Opened == "0" || teacherPlans[0].Available == "0")
            {
                return new TeacherPlanModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_plan_not_available"]),
                    }
                };
            }
            else
            {

                TeacherPlanModel teacherPlanModel = teacherPlans[0];
                teacherPlanModel.Valid = true;
                return teacherPlanModel;
            }
        }

        /// <summary>
        /// To create the relationship between the user quiz result and the teacher assignment result.
        /// </summary>
        /// <param name="planAssignmentUserResultModels">The plan assignment results to create. </param>
        /// <param name="userInSession">The user id of the user that is making the request.</param>
        /// <returns>Returns the created plan assignment.</returns>
        public List<PlanAssignmentUserResultModel> AddPlanAssignmentUserResult(List<PlanAssignmentUserResultModel> planAssignmentUserResultModels, string userInSession)
        {

            List<PlanAssignmentUserResultModel> planAssignmentExistsToCheck = new List<PlanAssignmentUserResultModel>();
            foreach (PlanAssignmentUserResultModel planAssignmentUserResultModel in planAssignmentUserResultModels)
            {
                planAssignmentExistsToCheck.Add(new PlanAssignmentUserResultModel()
                {
                    UserId = planAssignmentUserResultModel.UserId,
                    UsquiUserResultId = planAssignmentUserResultModel.UsquiUserResultId,
                    PlanQuizId = planAssignmentUserResultModel.PlanQuizId,
                });
            }

            PlanAssignmentUserResultDAO planAssignmentUserResultDAO = new PlanAssignmentUserResultDAO();
            // Check fi exists 
            List<PlanAssignmentUserResultModel> planAssignmentUserResultModelToCheckList = planAssignmentUserResultDAO.Read(_connectionStringRead, planAssignmentExistsToCheck);

            // If not exists, insert.
            if (planAssignmentUserResultModelToCheckList.Count == 0)
            {
                foreach (PlanAssignmentUserResultModel planAssignmentUserResultModel in planAssignmentUserResultModels)
                {
                    planAssignmentUserResultModel.ModifiedBy = userInSession;
                }

                List<PlanAssignmentUserResultModel> planAssignmentUserResult = planAssignmentUserResultDAO.Create(_connectionString, planAssignmentUserResultModels);
                return planAssignmentUserResult;
            }
            else
            {
                var planAssignmentUserResultModelUpdateList = new List<PlanAssignmentUserResultModel>();
                var planAssignmentUseResultToUpdateWhereList = new List<PlanAssignmentUserResultModel>();

                // Update the ones that exists
                foreach (var planAssignmentUserResultModel in planAssignmentUserResultModelToCheckList)
                {
                    var planAssignmentUserResultModelToCheck =
                        planAssignmentUserResultModels.FirstOrDefault(x =>
                            x.PlanQuizId == planAssignmentUserResultModel.PlanQuizId &&
                            x.UserId == planAssignmentUserResultModel.UserId);
                    planAssignmentUserResultModelUpdateList.Add(new PlanAssignmentUserResultModel()
                    {
                        Score = planAssignmentUserResultModelToCheck.Score,
                        TotalPercent = planAssignmentUserResultModelToCheck.TotalPercent,
                        ModifiedBy = userInSession,
                    });

                    planAssignmentUseResultToUpdateWhereList.Add(new PlanAssignmentUserResultModel()
                    {
                        Id = planAssignmentUserResultModel.Id,
                    });
                }

                if (planAssignmentUserResultModelUpdateList.Count > 0)
                {
                    planAssignmentUserResultDAO.Update(_connectionString, planAssignmentUserResultModelUpdateList,
                        planAssignmentUseResultToUpdateWhereList);
                }

                return planAssignmentUserResultModelToCheckList;
            }
        }

        /// <summary>
        /// Creates the plan lesson quiz results models. 
        /// </summary>
        /// <param name="planLessonQuizUserResults">The plan lesson quiz results to insert.</param>
        /// <param name="userInSession">The user id of the user that is making the request.</param>
        /// <returns>Returns the plan lesson quiz user results.</returns>
        public List<PlanLessonQuizUserResultModel> AddPlanLessonQuizResultModels(List<PlanLessonQuizUserResultModel> planLessonQuizUserResults, string userInSession)
        {
            foreach (PlanLessonQuizUserResultModel userQuizResult in planLessonQuizUserResults)
            {
                userQuizResult.ModifiedBy = userInSession;
            }

            return new PlanLessonQuizUserResultDAO().Create(_connectionString, planLessonQuizUserResults);
        }

        /// <summary>
        /// To add the plan lesson pre post result.
        /// </summary>
        /// <param name="planLessonPrePostUserResults">The plan lesson pre-post user results.</param>
        /// <param name="userInSession">The user id of the user that is making the request.</param>
        /// <returns>Returns the created plan pre user results.</returns>
        public List<PlanPrePostUserResultModel> AddPlanLessonPrePostResultModels(List<PlanPrePostUserResultModel> planLessonPrePostUserResults, string userInSession)
        {
            foreach (PlanPrePostUserResultModel userQuizResult in planLessonPrePostUserResults)
            {
                userQuizResult.ModifiedBy = userInSession;
            }

            return new PlanPrePostUserResultDAO().Create(_connectionString, planLessonPrePostUserResults);
        }

        /// <summary>
        /// To manually correct plan lesson quiz results.
        /// </summary>
        /// <param name="planLessonQuizUserResultModel">The plan lesson quiz model to correct.</param>
        /// <param name="userInSession">The user that is entering the result.</param>
        /// <returns>Returns the transaction details.</returns>
        public PlanLessonQuizUserResultModel ManuallyCorrectPlanLessonQuizStudent(
            PlanLessonQuizUserResultModel planLessonQuizUserResultModel, string userInSession)
        {
            // Find the plan id by plan lesson id
            var teacherPlanLessonModel = new TeacherPlanLessonDAO().Read(_connectionStringRead,
                new TeacherPlanLessonModel() { Id = planLessonQuizUserResultModel.PlanLessonId }, true, true)[0];

            // Check if plan is available and opened
            if (!ValidateIfTeacherPlanAllowChanges(teacherPlanLessonModel.PlanId).Valid)
            {
                planLessonQuizUserResultModel.Valid = false;
                planLessonQuizUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_plan_not_available"]),
                };
                return planLessonQuizUserResultModel;
            }

            // Validate the model
            if (string.IsNullOrWhiteSpace(planLessonQuizUserResultModel.PlanLessonId))
            {
                planLessonQuizUserResultModel.Valid = false;
                planLessonQuizUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_plan_lesson_id_required"]),
                };
                return planLessonQuizUserResultModel;
            }

            if (string.IsNullOrWhiteSpace(planLessonQuizUserResultModel.QuizId))
            {
                planLessonQuizUserResultModel.Valid = false;
                planLessonQuizUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_quiz_id_required"]),
                };
            }

            if (string.IsNullOrWhiteSpace(planLessonQuizUserResultModel.UserId))
            {

                planLessonQuizUserResultModel.Valid = false;
                planLessonQuizUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_user_id_required"]),
                };
                return planLessonQuizUserResultModel;
            }

            if (string.IsNullOrWhiteSpace(planLessonQuizUserResultModel.Score))
            {
                planLessonQuizUserResultModel.Valid = false;
                planLessonQuizUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_score_required"]),
                };
                return planLessonQuizUserResultModel;
            }

            double score = 0;
            try
            {
                score = double.Parse(planLessonQuizUserResultModel.Score);
            }
            catch (Exception e)
            {
                planLessonQuizUserResultModel.Valid = false;
                planLessonQuizUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_score_invalid"]),
                };
                return planLessonQuizUserResultModel;
            }

            // Check that the score is no more than the maximum score (100), because it is an exam.
            if (score > 100 || score < 0)
            {
                planLessonQuizUserResultModel.Valid = false;
                planLessonQuizUserResultModel.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_score_invalid"]),
                };
                return planLessonQuizUserResultModel;
            }

            // Check if the user has already taken the quiz
            var planLessonQuizUserResultModelToCheck = new PlanLessonQuizUserResultDAO().Read(_connectionStringRead,
                new PlanLessonQuizUserResultModel()
                {
                    PlanLessonId = planLessonQuizUserResultModel.PlanLessonId,
                    QuizId = planLessonQuizUserResultModel.QuizId,
                    UserId = planLessonQuizUserResultModel.UserId,
                }, true, true);

            if (planLessonQuizUserResultModelToCheck.Count > 0)
            {
                // Update it
                var responseModelUpdate = new PlanLessonQuizUserResultDAO().Update(_connectionString,
                    new PlanLessonQuizUserResultModel()
                    {
                        Score = planLessonQuizUserResultModel.Score,
                        ModifiedBy = userInSession,
                        ManuallyCorrectedByTeacher = "1",
                    },
                    new PlanLessonQuizUserResultModel()
                    {
                        Id = planLessonQuizUserResultModelToCheck[0].Id
                    })[0];

                responseModelUpdate.Valid = true;
                responseModelUpdate.Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_success_update_manual_quiz_result"]),
                };

                return responseModelUpdate;
            }

            planLessonQuizUserResultModel.ModifiedBy = userInSession;
            planLessonQuizUserResultModel.Score = score.ToString();
            planLessonQuizUserResultModel.ManuallyCorrectedByTeacher = "1";

            // Insert it 
            var responseModel = new PlanLessonQuizUserResultDAO().Create(_connectionString,
                planLessonQuizUserResultModel)[0];

            responseModel.Valid = true;
            responseModel.Messages = new List<string>()
            {
                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                    AppConfiguration.Configuration["AppSettings:tm_message_code_success_insert_manual_quiz_result"]),
            };


            var teacherPlanLessonQuizzes = new TeacherPlanLessonQuizDAO().Read(_connectionStringRead,
                               new TeacherPlanLessonQuizModel() { PlanLessonId = planLessonQuizUserResultModel.PlanLessonId }, false, false);

            // Update the plan lesson user completed if applicable. 
            // Retrieve the quizzes ids of teacher plan lesson quizzes. 
            CreateStudentPlanLessonIfNotExists(planLessonQuizUserResultModel.UserId, teacherPlanLessonModel.Id, userInSession);
            UpdateLessonCompletionStatus(teacherPlanLessonModel, userInSession, teacherPlanLessonQuizzes.Select(q => q.QuizId).ToList(), planLessonQuizUserResultModel.UserId);

            return responseModel;
        }

        /// <summary>
        /// To add the plan lesson users.
        /// </summary>
        /// <param name="planLessonUserModels">Plan lesson users to insert.</param>
        /// <param name="userInSession">The user id of the user that is making the request.</param>
        /// <returns>Returns the list of plan lesson users to insert.</returns>
        public List<PlanLessonUserModel> AddPlanLessonUserModels(List<PlanLessonUserModel> planLessonUserModels, string userInSession)
        {
            foreach (PlanLessonUserModel planLessonUser in planLessonUserModels)
            {
                planLessonUser.ModifiedBy = userInSession;
            }

            return new PlanLessonUserDAO().Create(_connectionString, planLessonUserModels);
        }

        public ResponseModel DeletePlanLessonQuiz(string planningId, string quizId)
        {
            ResponseModel responseModel = new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>(),
            };

            // Evaluate if plan can be modified 
            TeacherPlanModel teacherPlanToValidateIfChangesAllowed =
                ValidateIfTeacherPlanAllowChanges(planningId);
            if (!teacherPlanToValidateIfChangesAllowed.Valid)
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = teacherPlanToValidateIfChangesAllowed.Messages
                };
            }
            // End of evaluation

            MessageManager messageManager = new MessageManager(_connectionStringRead,
                _langCode);
            if (string.IsNullOrWhiteSpace(quizId))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(
                    messageManager.GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_code_homework_required"]));
                return responseModel;
            }

            responseModel.Valid = true;
            new TeacherPlanLessonQuizDAO().Delete(_connectionString, new TeacherPlanLessonQuizModel()
            {
                Id = quizId
            });

            responseModel.Messages.Add(
                messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]));
            return responseModel;
        }

        /// <summary>
        /// To update plan user lesson model list.  (Usually use to update the overall score).
        /// </summary>
        /// <param name="planLessonUserModelToUpdateList">The plan user lesson model to update.</param>
        /// <param name="planLessonUserModelsWhere">The plan user lesson model list where.</param>
        /// <param name="userInSession">The user that is making the transaction.</param>
        /// <returns>Returns the list of updated plan user lesson.</returns>
        public List<PlanLessonUserModel> UpdatePlanLessonModels(List<PlanLessonUserModel> planLessonUserModelToUpdateList, List<PlanLessonUserModel> planLessonUserModelsWhere, string userInSession)
        {
            foreach (PlanLessonUserModel planLessonUserModel in planLessonUserModelToUpdateList)
            {
                planLessonUserModel.ModifiedBy = userInSession;
            }

            return new PlanLessonUserDAO().Update(_connectionString, planLessonUserModelToUpdateList, planLessonUserModelsWhere);
        }

        /// <summary>
        /// To validate the teacher plan lesson quizzess.
        /// </summary>
        /// <param name="teacherPlanLessonQuizzess">Teacher plan lessons to evaluate.</param>
        /// <param name="isEdit">True if edit, else, false.</param>
        /// <returns>Returns if the plan lesson quizzess are valid or not.</returns>
        private ResponseModel ValidateTeacherPlanLessonQuizzess(List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizzess, bool isEdit)
        {
            ResponseModel responseModel = new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
            };

            if (teacherPlanLessonQuizzess == null)
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quizzes_required"]));
                return responseModel;
            }

            List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizExistingList = new List<TeacherPlanLessonQuizModel>();

            if (isEdit)
            {
                // Call the existings, so we can compare the dates 
                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuizzess[0].PlanLessonId))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_id_null"]));
                    return responseModel;
                }

                // Read the teacher plan lessons
                teacherPlanLessonQuizExistingList = new TeacherPlanLessonQuizDAO().Read(_connectionStringRead, new TeacherPlanLessonQuizModel() { PlanLessonId = teacherPlanLessonQuizzess[0].PlanLessonId }, false, false);
            }


            // Now evaluate 
            foreach (TeacherPlanLessonQuizModel teacherPlanLessonQuiz in teacherPlanLessonQuizzess)
            {
                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuiz.QuizId))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quiz_id_null"]));
                    return responseModel;
                }

                // Validate the dates 
                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuiz.DueDate))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_due_date_null"]));
                    return responseModel;
                }

                DateTime temp;
                if (!DateTime.TryParse(teacherPlanLessonQuiz.DueDate, out temp))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_due_date_invalid"]));
                    return responseModel;
                }

                if (isEdit)
                {
                    if (string.IsNullOrWhiteSpace(teacherPlanLessonQuiz.PlanLessonId))
                    {
                        responseModel.Valid = false;
                        responseModel.Messages.Add(new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_id_null"]));
                        return responseModel;
                    }

                    // Find the original date 
                    TeacherPlanLessonQuizModel teacherPlanLessonQuizModelToCompareDate = teacherPlanLessonQuizExistingList.Find(x => x.Id == teacherPlanLessonQuiz.Id);

                    if (teacherPlanLessonQuizModelToCompareDate != null)
                    {
                        // If the dates are not the same, evaluate if the new date is more than today
                        if (DateTime.Parse(teacherPlanLessonQuiz.DueDate) != DateTime.Parse(teacherPlanLessonQuizModelToCompareDate.DueDate))
                        {

                            if (CustomClasses.DateTimeZoneManager.ConvertTimeZoneAndDateToUniversalDateTime(DateTime.Parse(teacherPlanLessonQuiz.DueDate), teacherPlanLessonQuiz.DueDateTimezone).AddMinutes(1) <= CustomClasses.DateTimeZoneManager.ConvertToUniversalDateTime(DateTime.Now))
                            {
                                responseModel.Valid = false;
                                responseModel.Messages = new List<string>() {
                                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quiz_due_date_past"])
                                };
                                return responseModel;
                            }
                        }
                    }
                }
                else
                {

                    if (CustomClasses.DateTimeZoneManager.ConvertTimeZoneAndDateToUniversalDateTime(DateTime.Parse(teacherPlanLessonQuiz.DueDate), teacherPlanLessonQuiz.DueDateTimezone).AddMinutes(1) <= CustomClasses.DateTimeZoneManager.ConvertToUniversalDateTime(DateTime.Now))
                    {
                        responseModel.Valid = false;
                        responseModel.Messages = new List<string>() {
                                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quiz_due_date_past"])
                                };
                        return responseModel;
                    }
                }
            }

            return responseModel;
        }

        /// <summary>
		/// To validate the teacher plan lesson quizzes.
		/// </summary>
		/// <param name="teacherPlanLessonQuizzess">Teacher plan lessons to evaluate.</param>
		/// <param name="isEdit">True if edit, else, false.</param>
		/// <returns>Returns if the plan lesson quizzes are valid or not.</returns>
		private ResponseModel ValidateTeacherPlanLessonQuizzess(
            List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizzess, DateTime teacherPlanLessonAvailableDate,
            bool isEdit)
        {
            var responseModel = new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
            };

            if (teacherPlanLessonQuizzess == null)
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quizzes_required"]));
                return responseModel;
            }

            List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizExistingList = new List<TeacherPlanLessonQuizModel>();

            if (isEdit)
            {
                // Call the existings, so we can compare the dates 
                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuizzess[0].PlanLessonId))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_id_null"]));
                    return responseModel;
                }

                // Read the teacher plan lessons
                teacherPlanLessonQuizExistingList = new TeacherPlanLessonQuizDAO().Read(_connectionStringRead,
                    new TeacherPlanLessonQuizModel() { PlanLessonId = teacherPlanLessonQuizzess[0].PlanLessonId },
                    false, false);
            }


            // Now evaluate 
            foreach (TeacherPlanLessonQuizModel teacherPlanLessonQuiz in teacherPlanLessonQuizzess)
            {
                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuiz.QuizId))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quiz_id_null"]));
                    return responseModel;
                }

                // Validate the dates 
                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuiz.DueDate))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_due_date_null"]));
                    return responseModel;
                }

                DateTime temp;
                if (!DateTime.TryParse(teacherPlanLessonQuiz.DueDate, out temp))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_due_date_invalid"]));
                    return responseModel;
                }

                try
                {
                    DateTime quizDueDate = DateTime.Parse(teacherPlanLessonQuiz.DueDate).Date;

                    TimeZoneInfo sourceTimeZone =
                        TimeZoneInfo.FindSystemTimeZoneById(teacherPlanLessonQuiz.DueDateTimezone);

                    DateTime quizDueDateNormalized = TimeZoneInfo.ConvertTimeToUtc(quizDueDate, sourceTimeZone);
                    DateTime planLessonAvailableDateNormalized = teacherPlanLessonAvailableDate.Date;

                    if (planLessonAvailableDateNormalized.Date > quizDueDateNormalized.Date)
                    {
                        responseModel.Valid = false;
                        responseModel.Messages.Add(
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                                AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quiz_less_than_availability_lesson_date"]));
                        return responseModel;
                    }
                }
                catch (Exception ex)
                {
                    // Ignore
                }

                if (isEdit)
                {
                    if (string.IsNullOrWhiteSpace(teacherPlanLessonQuiz.PlanLessonId))
                    {
                        responseModel.Valid = false;
                        responseModel.Messages.Add(
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                                AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_id_null"]));
                        return responseModel;
                    }

                    // Find the original date 
                    TeacherPlanLessonQuizModel teacherPlanLessonQuizModelToCompareDate =
                        teacherPlanLessonQuizExistingList.Find(x => x.Id == teacherPlanLessonQuiz.Id);

                    if (teacherPlanLessonQuizModelToCompareDate != null)
                    {
                        // If the dates are not the same, evaluate if the new date is more than today
                        if (DateTime.Parse(teacherPlanLessonQuiz.DueDate) !=
                            DateTime.Parse(teacherPlanLessonQuizModelToCompareDate.DueDate))
                        {
                            if (CustomClasses.DateTimeZoneManager.ConvertTimeZoneAndDateToUniversalDateTime(
                                    DateTime.Parse(teacherPlanLessonQuiz.DueDate),
                                    teacherPlanLessonQuiz.DueDateTimezone).AddMinutes(1) <=
                                CustomClasses.DateTimeZoneManager.ConvertToUniversalDateTime(DateTime.Now))
                            {
                                responseModel.Valid = false;
                                responseModel.Messages = new List<string>()
                                {
                                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                                        AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quiz_due_date_past"])
                                };
                                return responseModel;
                            }
                        }
                    }
                }
                else
                {
                    if (CustomClasses.DateTimeZoneManager.ConvertTimeZoneAndDateToUniversalDateTime(
                            DateTime.Parse(teacherPlanLessonQuiz.DueDate), teacherPlanLessonQuiz.DueDateTimezone).AddMinutes(1) <=
                        CustomClasses.DateTimeZoneManager.ConvertToUniversalDateTime(DateTime.Now))
                    {
                        responseModel.Valid = false;
                        responseModel.Messages = new List<string>()
                        {
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                                AppConfiguration.Configuration["AppSettings:tm_message_plan_lesson_quiz_due_date_past"])
                        };
                        return responseModel;
                    }
                }
            }

            return responseModel;
        }

        /// <summary>
        /// To clean the teacher plan model.
        /// </summary>
        /// <param name="teacherPlan">Teacher plan to clean.</param>
        /// <returns>Returns the teacher plan model. </returns>
        private TeacherPlanModel CleanTeacherPlanModel(TeacherPlanModel teacherPlan)
        {
            if (!string.IsNullOrWhiteSpace(teacherPlan.Name))
            {
                teacherPlan.Name = teacherPlan.Name.Trim();
                // Truncate name. 
                teacherPlan.Name = TruncateString(teacherPlan.Name, MaxNameLength);
            }

            if (!string.IsNullOrWhiteSpace(teacherPlan.SubjectCode))
            {
                teacherPlan.SubjectCode = teacherPlan.SubjectCode.Trim();
                teacherPlan.SubjectCode = teacherPlan.SubjectCode.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(teacherPlan.LevelCode))
            {
                teacherPlan.LevelCode = teacherPlan.LevelCode.Trim();
                teacherPlan.LevelCode = teacherPlan.LevelCode.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(teacherPlan.CloseDate))
            {
                teacherPlan.CloseDate = teacherPlan.CloseDate.Trim();
                // Check if the date is valid
                DateTime temp;
                if (!DateTime.TryParse(teacherPlan.CloseDate, out temp))
                {
                    teacherPlan.CloseDate = null;
                }
            }

            // Same for Start date
            if (!string.IsNullOrWhiteSpace(teacherPlan.OpenDate))
            {
                teacherPlan.OpenDate = teacherPlan.OpenDate.Trim();
                // Check if the date is valid
                DateTime temp;
                if (!DateTime.TryParse(teacherPlan.OpenDate, out temp))
                {
                    teacherPlan.OpenDate = null;
                }
            }

            if(teacherPlan.Groups != null && teacherPlan.Groups.Count > 0)
            {
                // Delete the duplication
                teacherPlan.Groups = teacherPlan.Groups
               .DistinctBy(g => g.GroupId)
               .ToList();
            }

            return teacherPlan;
        }

        /// <summary>
        /// To truncate a string.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="maxLength">The length.</param>
        /// <returns>Returns the string truncated.</returns>
        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }

            return value;
        }

        /// <summary>
        /// To set some default values to the teacher plan lesson quiz details.
        /// </summary>
        /// <param name="teacherPlanLessonQuizModels">Teacher plan lesson quiz models to set.</param>
        /// <param name="teacherPlanDate">Teacher plan date when empty. It will set one day for the exam if the exam is not required, just to handle the date details.</param>
        /// <param name="defaultDateTimeZoneWhenEmpty">Default time zone when empty.</param>
        /// <param name="planLessonId">The plan lesson id.</param>
        /// <returns>Returns the teacher plan lesson quiz models.</returns>
        private List<TeacherPlanLessonQuizModel> SetDefaultTeacherPlanLessonQuizDetails(
            List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizModels, string teacherPlanDate,
            string defaultDateTimeZoneWhenEmpty, string planLessonId)
        {
            foreach (TeacherPlanLessonQuizModel teacherPlanLessonQuizModel in teacherPlanLessonQuizModels)
            {
                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuizModel.Required))
                {
                    teacherPlanLessonQuizModel.Required = "1";
                }

                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuizModel.ForGrading))
                {
                    teacherPlanLessonQuizModel.ForGrading = "1";
                }

                if (teacherPlanLessonQuizModel.ForGrading == "1")
                {
                    teacherPlanLessonQuizModel.Required = "1";
                }

                if (string.IsNullOrWhiteSpace(teacherPlanLessonQuizModel.Duration))
                {
                    teacherPlanLessonQuizModel.Duration = "60";
                }

                if (teacherPlanLessonQuizModel.Required == "0" &&
                    string.IsNullOrEmpty(teacherPlanLessonQuizModel.DueDate))
                {
                    teacherPlanLessonQuizModel.DueDate =
                        CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(DateTime.Parse(teacherPlanDate));
                    teacherPlanLessonQuizModel.DueDateTimezone = defaultDateTimeZoneWhenEmpty;
                }

                teacherPlanLessonQuizModel.PlanLessonId = planLessonId;
            }

            return teacherPlanLessonQuizModels;
        }

        /// <summary>
        /// Check if the lesson to be attached to a planning is in the available courses
        /// </summary>
        /// <param name="lessonId">The lesson ID</param>
        /// <param name="availableCourses">The list with the available courses for the teacher</param>
        /// <returns>A valid response if lesson is in an available course</returns>
        public async Task<ResponseModel> IsLessonInTeacherCourses(string lessonId, SubscriptionResponseModel availableCourses)
        {
            var messageManager = new MessageManager(_connectionStringRead, _langCode);

            var responseModel = new ResponseModel
            {
                Valid = false,
                Messages = new List<string>()
            };

            try
            {
                // Get lesson details from Athenas API
                var lessonReaderServices = new LessonsReaderServices();
                LessonModel lessonDetails = await lessonReaderServices.LessonDetailsServiceAsync(lessonId);

                string lessonSubject = lessonDetails.SubjectCode;
                string lessonLevel = lessonDetails.LevelCode;

                bool isLessonAvailable = false;

                if (availableCourses == null || availableCourses.UserSubscribedLevelsSubjectsModelList == null)
                {
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_no_courses_available"]));
                    return responseModel;
                }

                foreach (var course in availableCourses.UserSubscribedLevelsSubjectsModelList)
                {
                    string courseSubject = course.Subject;
                    string courseLevel = course.Level;

                    // If found a matching lesson, break the loop
                    if (lessonSubject.ToLower() == courseSubject.ToLower() && lessonLevel.ToLower() == courseLevel.ToLower())
                    {
                        isLessonAvailable = true;
                        break;
                    }
                }

                if (!isLessonAvailable)
                {
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_no_courses_available"]));
                    return responseModel;
                }

                responseModel.Valid = true;
                responseModel.LessonId = lessonId;
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_no_courses_available"]));
                return responseModel;
            }
        }

        /// <summary>
        /// To remote the time if any.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns the new date without any time. </returns>
        private DateTime ConvertDateToMidnight(DateTime date)
        {
            return date.Date;
        }
    }
}
