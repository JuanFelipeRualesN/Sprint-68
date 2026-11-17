using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Generals;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using ForumModel = TeachersAPI.Domain.Models.Teachers.ForumModel;
using ForumResponseModel = TeachersAPI.Domain.Models.Teachers.Responses.ForumResponseModel;

namespace TeachersAPI.TeachersAPI.Services.Teachers
{
    /// <summary>
    /// To handle the forums. The forums are a place of text where the students and teachers can exchange ideas.
    /// </summary>
    public class ForumManager
    {
        private readonly string _connectionString, _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader)</param>
        /// <param name="langCode">The language code of the user.</param>
        public ForumManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Reads the forum list.
        /// </summary>
        /// <param name="forum">Forum to filter.</param>
        /// <returns>Return the list of forums.</returns>
        public List<Domain.Models.Teachers.ForumModel> ReadForums(Domain.Models.Teachers.ForumModel forum)
        {
            return new ForumDAO().Read(_connectionStringRead, forum, false, true);
        }

        /// <summary>
        /// Read forum posts.
        /// </summary>
        /// <param name="forumPost">Post model to filter.</param>
        /// <returns>Returns the forum details.</returns>
        public List<ForumPostModel> ReadForumPostsModel(ForumPostModel forumPost)
        {
            return new ForumPostDAO().Read(_connectionStringRead, forumPost, false, true);
        }

        /// <summary>
        /// To read the teacher's forums.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="createdAt">The date created at to filter.</param>
        /// <param name="title">The forum title to filter.</param>
        /// <param name="teacherPlanId">The teacher plan's id.</param>
        /// <param name="userIdMakingRequest">The id of the user making the request.</param>
        /// <param name="fromPage">From what page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <returns>Returns the forum response.</returns>
        public ForumsResponseModel ReadTeacherForums(string token, string ipAddress,
            DateTime? createdAt = null,
            string title = null,
            string teacherPlanId = null,
            string userIdMakingRequest = null,
            int fromPage = 0,
            int limiter = 10)
        {
            // First, read the forum. 
            var forums = this.ReadForums(new Domain.Models.Generals.Responses.ForumModel()
            {
                CreatedAt = createdAt?.ToString("yyyy-MM-dd"),
                Title = title,
                Active = "1",
                TeacherPlanId = teacherPlanId,
                Author = userIdMakingRequest,
            });

            // If there are no forums, return an empty response.
            if (forums.Count == 0)
            {
                return new ForumsResponseModel()
                {
                    ForumModelList = new List<Domain.Models.Generals.Responses.ForumModel>()
                };
            }
            var forumResponseModel = new ForumsResponseModel()
            {
                TotalItems = forums.Count,
                TotalPages = int.Parse(Math.Ceiling((double)forums.Count / limiter).ToString()),
            };

            var userFullNames = GetUserDetails.GetUsersFullNamesSameSchool(token,
                ipAddress, new List<string>() { userIdMakingRequest });

            forums = forums.Skip(fromPage * limiter).Take(limiter).ToList();

            var implementationDatesToFind = new List<ForumDatesModel>();
            foreach(var forum in forums)
            {
                implementationDatesToFind.Add(new ForumDatesModel()
                {
                    ForumId = forum.Id
                });
            }

            var forumImplementationDateDAO = new ForumDateDAO();
            var forumDates = forumImplementationDateDAO.Read(this._connectionString, implementationDatesToFind);

            foreach (var forum in forums)
            {
                var userFullName = userFullNames.Find(x => x.UserId == userIdMakingRequest);
                forumResponseModel.ForumModelList.Add(new Domain.Models.Generals.Responses.ForumModel()
                {
                    Active = forum.Active,
                    AuthorName = userFullName?.GivenName + " " + userFullName?.Surname,
                    Description = forum.Description,
                    CreatedAt = forum.CreatedAt,
                    Title = forum.Title,
                    Id = forum.Id,
                    PlanName = forum.PlanName,
                    ImplementationDates = forumDates?.FindAll(x => x.ForumId == forum.Id)
                });
            }

            return forumResponseModel;
        }

        /// <summary>
        /// Creates a forum. The defaults is active.
        /// </summary>
        /// <param name="forumModel">Forum to create.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <returns>Return the created forum.</returns>
        public ForumResponseModel CreateForum(ForumModel forumModel, string userSessionId)
        {
            List<string> errorCodes = new List<string>();

            forumModel.Author = userSessionId;
            forumModel.ModifiedBy = userSessionId;
            forumModel.Active = "1";

            if (string.IsNullOrWhiteSpace(forumModel.Title))
            {
                // Title required
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_title_required"]);
            }

            if (string.IsNullOrWhiteSpace(forumModel.Description))
            {
                // Description required.
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_description_required"]);
            }

            if (errorCodes.Count > 0)
            {
                return new ForumResponseModel()
                {
                    Messages = new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(errorCodes),
                    Valid = false,
                };
            }

            // Create Forum 
            var forumResponse =  new ForumResponseModel()
            {
                Valid = true,
                ForumModel = new ForumDAO().Create(_connectionString, forumModel)[0],
            };

            // If there are implementation dates, create the implementation dates. 
            if(forumModel.ImplementationDates != null && forumModel.ImplementationDates.Count > 0)
            {
                var forumImplementationDates = new List<ForumDatesModel>(); 
                foreach(var date in forumModel.ImplementationDates)
                {
                    // Convert the date to mysql. 
                    forumImplementationDates.Add(new ForumDatesModel()
                    {
                        ForumId = forumResponse.ForumModel.Id,
                        ImplementationDate =
                        CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(DateTime.Parse(date.ImplementationDate)),
                        ModifiedBy = userSessionId,
                    });
                }

                // Create the forum dates 
                new ForumDateDAO().Create(this._connectionString, forumImplementationDates);
            }

            return forumResponse;

        }

        /// <summary>
        /// To modify a forum. The user who archive it must be the same as the user who created it.
        /// </summary>
        /// <param name="forumModel">Forum to create.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <returns>Return the modified forum.</returns>
        public ForumResponseModel ModifyForum(ForumModel forumModel, string userSessionId)
        {
            List<string> errorCodes = new List<string>();

            ForumModel forumToModify = new ForumModel()
            {
                Id = forumModel.Id,
                Description = forumModel.Description,
                Active = forumModel.Active,
                Author = userSessionId,
                Title = forumModel.Title,
                ModifiedBy = userSessionId,
                ImplementationDates = forumModel.ImplementationDates
            };
            ForumModel forumModelWhere = new ForumModel()
            {
                Id = forumModel.Id,
                Author = userSessionId,
            };

            if (string.IsNullOrWhiteSpace(forumModel.Id))
            {
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_id_required"]);
            }

            if (string.IsNullOrWhiteSpace(forumModel.Title))
            {
                // Title required
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_title_required"]);
            }

            if (string.IsNullOrWhiteSpace(forumModel.Description))
            {
                // Description required.
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_description_required"]);
            }

            if (errorCodes.Count > 0)
            {
                return new ForumResponseModel()
                {
                    Messages = new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(errorCodes),
                    Valid = false,
                };
            }

            ForumDAO forumDAO = new ForumDAO();
            ForumResponseModel response = new ForumResponseModel()
            {
                Valid = false
            };

            try
            {
                // Delete the previous forum dates. 
                var forumDatesDao = new ForumDateDAO();
                forumDatesDao.Delete(this._connectionString, new ForumDatesModel()
                {
                    ForumId = forumToModify.Id,
                });

                if(forumToModify.ImplementationDates != null && forumToModify.ImplementationDates.Count > 0)
                {
                    var forumImplementationDates = new List<ForumDatesModel>();
                    foreach (var date in forumModel.ImplementationDates)
                    {
                        // Convert the date to mysql. 
                        forumImplementationDates.Add(new ForumDatesModel()
                        {
                            ForumId = forumToModify.Id,
                            ImplementationDate =
                            CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(DateTime.Parse(date.ImplementationDate)),
                            ModifiedBy = userSessionId,
                        });
                    }

                    // Create the forum dates 
                    new ForumDateDAO().Create(this._connectionString, forumImplementationDates);
                }

                response.ForumModel = forumDAO.Update(_connectionString, forumToModify, forumModelWhere)[0];
                response.Valid = true;
            }
            catch (Exception)
            {

            }

            return response;
        }

        /// <summary>
        /// To archive a forum.
        /// </summary>
        /// <param name="forumId">The Id of the forum.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <param name="isStaff">If it is a staff.</param>
        /// <returns>Returns the transaction details.</returns>
        public ForumResponseModel ArchiveForum(string forumId, string userSessionId, bool isStaff)
        {
            List<string> errorCodes = new List<string>();

            ForumModel forumToModify = new ForumModel()
            {
                Active = "0",
            };
            ForumModel forumModelWhere = new ForumModel()
            {
                Id = forumId,
                Author = isStaff ? null : userSessionId,
            };

            if (string.IsNullOrWhiteSpace(forumId))
            {
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_id_required"]);
            }

            if (errorCodes.Count > 0)
            {
                return new ForumResponseModel()
                {
                    Messages = new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(errorCodes),
                    Valid = false,
                };
            }

            // Create Forum 
            return new ForumResponseModel()
            {
                Valid = true,
                ForumModel = new ForumDAO().Update(_connectionString, forumToModify, forumModelWhere)[0],
            };
        }

        /// <summary>
        /// Create forum post.
        /// </summary>
        /// <param name="forumPostModel">Post to create.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <param name="userMakingRequestFullName">The full name of the user making the request.</param>
        /// <returns>Return the created post.</returns>
        public ForumResponseModel CreatePost(ForumPostModel forumPostModel, string userSessionId, string userMakingRequestFullName)
        {

            // Check if forum available
            if (!TeacherPlans.CheckIfForumActive(forumPostModel.ForumId))
            {
                return new ForumResponseModel()
                {
                    Messages = new List<string>(){
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_forum_not_available"]),
                    },
                    Valid = false,
                };
            }

            var errorCodes = new List<string>();

            forumPostModel.Author = userSessionId;
            forumPostModel.ModifiedBy = userSessionId;
            forumPostModel.Available = "1";

            if (string.IsNullOrWhiteSpace(forumPostModel.Body))
            {
                // Description required.
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_description_required"]);
            }

            if (errorCodes.Count > 0)
            {
                return new ForumResponseModel()
                {
                    Messages = new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(errorCodes),
                    Valid = false,
                };
            }

            var forumPostCreated = new ForumPostDAO().Create(_connectionString, forumPostModel)[0];
            var postModel = ReadForumPostsModel(new ForumPostModel { Id = forumPostCreated.Id }).FirstOrDefault();
            forumPostCreated.CreatedAt = postModel?.CreatedAt;
            forumPostCreated.AuthorName = userMakingRequestFullName;

            return new ForumResponseModel()
            {
                Valid = true,
                ForumPostModel = forumPostCreated,
                SessionValid = true,
                TransactionValid = true,
            };
        }

        /// <summary>
        /// Modify a post forum.
        /// </summary>
        /// <param name="forumPostModel">Post to modify.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <param name="userFullNameMakingRequest">The full name of the user making the request.</param>
        /// <returns>Return the forum post with the transaction details.</returns>
        public ForumResponseModel ModifyPost(ForumPostModel forumPostModel, string userSessionId, string userFullNameMakingRequest)
        {
            List<string> errorCodes = new List<string>();

            ForumPostModel forumToModify = new ForumPostModel()
            {
                Body = forumPostModel.Body,
                Available = forumPostModel.Available,
                Author = userSessionId,
                ModifiedBy = userSessionId
            };
            ForumPostModel forumModelWhere = new ForumPostModel()
            {
                Id = forumPostModel.Id,
                Author = userSessionId,
            };

            if (string.IsNullOrWhiteSpace(forumPostModel.Id))
            {
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_id_required"]);
            }

            if (string.IsNullOrWhiteSpace(forumPostModel.Body))
            {
                // Description required.
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_description_required"]);
            }

            if (errorCodes.Count > 0)
            {
                return new ForumResponseModel()
                {
                    Messages = new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(errorCodes),
                    Valid = false,
                };
            }

            // Create Forum 
            var updatedForum = new ForumResponseModel()
            {
                Valid = true,
                ForumPostModel = new ForumPostDAO().Update(_connectionString, forumToModify, forumModelWhere)[0],
            };

            // Read the forum post. 
            var postModel = ReadForumPostsModel(new ForumPostModel { Id = forumPostModel.Id }).FirstOrDefault();
            updatedForum.ForumPostModel.Id = postModel?.Id;
            updatedForum.ForumPostModel.ForumId = postModel?.ForumId;
            updatedForum.ForumPostModel.CreatedAt = postModel?.CreatedAt;
            updatedForum.ForumPostModel.AuthorName = userFullNameMakingRequest;
            updatedForum.ForumPostModel.ParentPostId = postModel?.ParentPostId;

            return updatedForum;
        }

        /// <summary>
        /// To archive a post.
        /// </summary>
        /// <param name="forumPostId">Forum post id to archive.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <returns>Returns a response of the forum post archived.</returns>
        public ForumResponseModel ArchivePost(string forumPostId, string userSessionId)
        {
            var errorCodes = new List<string>();

            // Read the forum to check the creator 
            var forumPostModel = new ForumPostDAO().Read(_connectionStringRead, new ForumPostModel() { Id = forumPostId }, true, true)[0];

            // Now read the forum 
            var forumModel = new ForumDAO().Read(_connectionStringRead, new ForumModel() { Id = forumPostModel.ForumId }, true, true)[0];
            var isForumAuthor = forumModel.Author == userSessionId;

            if (string.IsNullOrWhiteSpace(forumPostId))
            {
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_id_required"]);
            }

            if (errorCodes.Count > 0)
            {
                return new ForumResponseModel()
                {
                    Messages = new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(errorCodes),
                    Valid = false,
                };
            }

            // Archive children. 
            new ForumPostDAO().Update(_connectionString, new ForumPostModel()
            {
                Available = "0",
                ModifiedBy = userSessionId,
            }, new ForumPostModel()
            {
                ParentPostId = forumPostId,
            });

            // Then archive the parent.
            return new ForumResponseModel()
            {
                Valid = true,
                ForumPostModel = new ForumPostDAO().Update(_connectionString, new ForumPostModel()
                {
                    Available = "0",
                    ModifiedBy = userSessionId,
                }, new ForumPostModel()
                {
                    Id = forumPostId,
                    Author = isForumAuthor ? null : userSessionId,
                })[0],
            };
        }
        
        /// <summary>
        /// Reads the forum posts.
        /// </summary>
        /// <param name="token">User token.</param>
        /// <param name="ipAddress">IP Address.</param>
        /// <param name="langCode">Language code.</param>
        /// <param name="forumId">The ID of the forum.</param>
        /// <param name="fromPage">From page</param>
        /// <param name="limiter">How many items per page.</param>
        /// <param name="userInSessionId">The user id of the user that is making the request.</param>
        /// <param name="forumModel">Forum parent.</param>
        /// <param name="returnForumModel">If return forum model.</param>
        public Domain.Models.Generals.Responses.ForumResponseModel ReadForumPosts(
            string token,
            string ipAddress,
            string langCode,
            string forumId,
            int fromPage,
            int limiter,
            string userInSessionId,
            bool returnForumModel,
            string parentPostId = null)
        {

            var forum = new ForumDAO().Read(this._connectionStringRead,
                new ForumModel()
                {
                    Id = forumId,
                    Active = "1",
                }, false, true)[0];

            // If forum is null, then return bad response.
            if (forum == null)
            {
                return new Domain.Models.Generals.Responses.ForumResponseModel()
                {
                    SessionValid = true,
                    TransactionValid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(
                                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
                                    , langCode)
                            .GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_forum_not_available"])
                    }
                };
            }

            // Read the participant groups. 
            // First, read the relation between groups and plan. 
            var teacherGroups = new List<TeacherGroupModel>();
            var teacherGroupPlans = new TeacherPlanGroupDAO().Read(this._connectionStringRead,
                new TeacherPlanGroupModel()
                {
                    TeacherPlanId = forum.TeacherPlanId,
                }, false, true);
            if (teacherGroupPlans.Count > 0)
            {
                // Now read the groups. 
                // Get the ids of the groups.
                var groupIds = teacherGroupPlans.Select(x => x.GroupId).ToList();
                var teacherGroupToFind = new List<TeacherGroupModel>();
                foreach (var groupId in groupIds)
                {
                    teacherGroupToFind.Add(new TeacherGroupModel()
                    {
                        Id = groupId,
                        Available = "1",
                    });
                }

                if (teacherGroupToFind.Count > 0)
                {
                    teacherGroups = new TeacherGroupDAO().Read(this._connectionStringRead, teacherGroupToFind);
                }
            }

            var forumPosts = new ForumPostDAO().Read(this._connectionStringRead,
                new ForumPostModel()
                {
                    Available = "1",
                    ForumId = forum.Id,
                    ParentPostId = parentPostId,
                }, false, true);

            // Get the ids to call the children.
            var forumPostIds = forumPosts.Select(x => x.Id).ToList();
            // Prepare the forumPost children models.
            var forumPostChildModelListToFind = new List<ForumPostModel>();
            foreach (var forumPost in forumPosts)
            {
                forumPostChildModelListToFind.Add(new ForumPostModel()
                {
                    ParentPostId = forumPost.Id,
                    Available = "1",
                });
            }

            var forumPostChildren = new ForumPostDAO().Read(this._connectionStringRead, forumPostChildModelListToFind);
            // Order them by last date, more recent first.
            forumPostChildren = forumPostChildren.OrderByDescending(x => x.CreatedAt).ToList();

            // Get the total posts of the original forum (parent post id should be null,
            // since it is not a response of another post). 
            var originalPosts = string.IsNullOrWhiteSpace(parentPostId) ? forumPosts.FindAll(x => string.IsNullOrWhiteSpace(x.ParentPostId)) : forumPosts;

            var forumResponseModel = new Domain.Models.Generals.Responses.ForumResponseModel()
            {
                ForumModel = forum,
                TotalItems = originalPosts.Count
            };

            // If the author is the user in session, the author can archive or edit the post.
            if (forum.Author == userInSessionId)
            {
                forumResponseModel.ShowArchive = true;
                forumResponseModel.ShowEdit = true;
            }

            // Limit the results 
            forumResponseModel.TotalPages = int.Parse(Math.Ceiling((double)originalPosts.Count / limiter).ToString());
            originalPosts = originalPosts.Skip(fromPage * limiter).Take(limiter).ToList();

            // Call the user details
            var userIds = originalPosts.Select(forumPost => forumPost.Author).ToList();

            // Add the author to get the general details.
            userIds.Add(userInSessionId);
            userIds.Add(forum.Author);
            var userFullNames = GetUserDetails.GetUsersFullNamesSameSchool(token, ipAddress, userIds);

            // Now prepare the response 
            var author = userFullNames.Find(x => x.UserId == forumResponseModel.ForumModel.Author);
            forumResponseModel.AuthorName = author?.GivenName + " " + author?.Surname;

            // Prepare the students
            foreach (var forumPost in originalPosts)
            {
                // Get the total of child posts
                forumPost.ChildPostCount = forumPostChildren.Count(x => x.ParentPostId == forumPost.Id);

                var authorStudent = userFullNames.Find(x => x.UserId == forumPost.Author);
                forumResponseModel.ForumPostModels.Add(new Domain.Models.Generals.Responses.ForumPostResponseModel()
                {
                    AuthorName = authorStudent?.GivenName + " " + authorStudent?.Surname,
                    ShowArchive = forumPost.Author == userInSessionId || forum.Author == userInSessionId,
                    ShowEdit = forumPost.Author == userInSessionId,
                    ForumPostModel = forumPost,
                });
                forumPost.AuthorUsername = authorStudent?.Username;
            }

            // This is when we just want the posts and we want to save some data of what the user receives.
            if (!returnForumModel)
            {
                forumResponseModel.ForumModel = null;
            }

            forumResponseModel.TeacherGroupModels = teacherGroups;

            return forumResponseModel;
        }

        /// <summary>
        /// To get the student's forums.
        /// </summary>
        /// <param name="token">The student's token that is making the request.</param>
        /// <param name="ipAddress">The student's ip address.</param>
        /// <param name="userId">The user's id (student's id).</param>
        /// <param name="langCode">The language code of the user.</param>
        /// <param name="fromPage">The from page to read the results.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <param name="subject">The subject to filter. (optional)</param>
        /// <param name="level">The level fo filter. (optional)</param>
        /// <returns>Returns the forums of the user.</returns>
        public Domain.Models.Generals.Responses.ForumsResponseModel GetStudentForums(string token, string ipAddress, string userId, string langCode, int fromPage, int limiter, string subject = null, string level = null)
        {

            // Read the forums
            // Get the forums of the teacher.
            List<Domain.Models.Generals.Models.ForumModel> forumResponseModelList = Infrastructure.DapperHandler.TeacherPlans.GetStudentForums(userId, subject, level);
            if (forumResponseModelList.Count == 0)
            {
                return new Domain.Models.Generals.Responses.ForumsResponseModel();
            }

            /// Get the subject name from athenas.
            List<SubjectModel> subjects = AthenasHandler.GetAthenasSubjects(langCode);

            Domain.Models.Generals.Responses.ForumsResponseModel forumsResponseModel = new Domain.Models.Generals.Responses.ForumsResponseModel()
            {
                TotalItems = forumResponseModelList.Count,
                TotalPages = int.Parse(Math.Ceiling((double)forumResponseModelList.Count / limiter).ToString()),
            };

            forumResponseModelList = forumResponseModelList.Skip(fromPage * limiter).Take(limiter).ToList();

            // Get the forums users 
            List<string> userIdsToFind = new List<string>();
            foreach (Domain.Models.Teachers.ForumModel forum in forumResponseModelList)
            {
                userIdsToFind.Add(forum.Author);
            }

            List<UserFullNameModel> userFullnames = GetUserDetails.GetUsersFullNamesSameSchool(token, ipAddress, userIdsToFind);


            foreach (Domain.Models.Generals.Models.ForumModel forum in forumResponseModelList)
            {

                UserFullNameModel authorFullName = userFullnames.Find(x => x.UserId == forum.Author);

                forumsResponseModel.ForumModelList.Add(new Domain.Models.Generals.Responses.ForumModel()
                {
                    SubjectCode = forum.SubjectCode,
                    SubjectName = subjects.Find(x => x.Code == forum.SubjectCode)?.Name,
                    LevelCode = forum.LevelCode,
                    Active = forum.Active,
                    AuthorName = authorFullName?.GivenName + " " + authorFullName?.Surname,
                    Description = forum.Description,
                    CreatedAt = forum.CreatedAt,
                    Title = forum.Title,
                    Id = forum.Id,

                });
            }

            return forumsResponseModel;
        }

    }
}
