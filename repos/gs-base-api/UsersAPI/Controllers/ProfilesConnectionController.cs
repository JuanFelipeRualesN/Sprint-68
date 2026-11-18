using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models.Filters;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;
using UsersAPI.UsersAPI.Services.Emails;
using UsersAPI.UsersAPI.Services.FeignAPIs;
using UsersAPI.UsersAPI.Services.Relations;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// Handles the profiles' connections.
    /// </summary>
    [ApiController]
    [Route("usersapi/profile-connections")]
    public class ProfilesConnectionController : ControllerBase
    {
        /// <summary>
        /// Gets the users' general information related to the user making the request.
        /// </summary>
        /// <param name="userListRequest">User id list with request details to filter.</param>
        /// <returns>Returns the users' details.</returns>
        [HttpPost("shared-details")]
        public async Task<IActionResult> GetUserRelationsSharedDetails([FromBody] UserListRequestModel userListRequest)
        {
            try
            {
                var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                if (userListRequest == null)
                {
                    userListRequest = new UserListRequestModel();
                }


                List<string> schoolIds = new List<string>();
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    schoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                }

                var result = Users.SearchRelations(
                    new AccountAdvance()
                    {
                        UserIds = userListRequest.UserIds,
                        RoleId = userListRequest.RoleId,
                        UserAlphaId = sessionResponseModel.UserId,
                        CallOnlyProfilesThatCanBeManaged = false,
                        IsStaff = sessionResponseModel.IsStaff,
                        IsDirector = sessionResponseModel.IsDirector,
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        SameSchoolRequired = false,
                        SchoolIds = schoolIds != null && schoolIds.Count > 0 ? schoolIds : null,
                    },
                    new Pagination()
                    {
                        ApplyPagination = false,
                    });

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets the users' names information related to the user making the request.
        /// </summary>
        /// <param name="userListRequest">User id list with request details to filter.</param>
        /// <returns>Returns the users' details.</returns>
        [HttpPost("names")]
       public async Task<IActionResult> GetUsersRelatedNames([FromBody] UserListRequestModel userListRequest)
        {
            try
            {
                var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections", 
                    string.IsNullOrWhiteSpace(userListRequest?.IpAddress) ? IPAddressDetector.GetClientIpAddress(HttpContext) 
                    : userListRequest?.IpAddress);
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                if (userListRequest == null)
                {
                    userListRequest = new UserListRequestModel();
                }

                List<string> schoolIds = new List<string>();
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    schoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                }

                var result = Users.GetUserFullNames(userListRequest.UserIds,
                   sessionResponseModel.UserId,
                   sessionResponseModel.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_teacher"],
                   sessionResponseModel.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_student"],
                   sessionResponseModel.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_director"],
                   sessionResponseModel.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_client"],
                   sessionResponseModel.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_parent"],
                   schoolIds);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets the user's relations by pagination. If the user Id (optional parameter) is sent, 
        /// then we will validate if the user connected is a staff. If it is a staff, then the staff could get the user relations. 
        /// If the parameter is not sent, then it will return the user in the session’s relations.
        /// </summary>
        /// <param name="fromPage">Current page.</param>
        /// <param name="limit">How many items per page.</param>
        /// <param name="isStudentHomeschool">If the user is a homeschool student. Optional.</param>
        /// <param name="userId">THe user's id. Optional.</param>
        /// <returns>Returns the users’ list.</returns>
        [HttpGet("shared-details/{fromPage}/{limit}/{isStudentHomeschool?}/{userId?}")]
       public async Task<IActionResult> GetUserRelationsSharedDetails([FromRoute] int fromPage, [FromRoute] int limit , [FromRoute] bool? isStudentHomeschool = null, [FromRoute] string userId = null)
        {
            try
            {
                var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");

                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                var schoolIds = new List<string>();
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    schoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                }

                var result = Users.SearchRelations(
                      new AccountAdvance()
                      {
                          RoleId = null,
                          UserAlphaId = !string.IsNullOrWhiteSpace(userId) ? userId : sessionResponseModel.UserId,
                          CallOnlyProfilesThatCanBeManaged = false,
                          IsStaff = sessionResponseModel.IsStaff,
                          IsDirector = sessionResponseModel.IsDirector,
                          IsClientAccount = sessionResponseModel.RoleId == "1",
                          IsStudentHomeschool = sessionResponseModel.RoleId == "3" && isStudentHomeschool == true,
                          SameSchoolRequired = false,
                          SchoolIds = schoolIds != null && schoolIds.Count > 0 ? schoolIds : null,
                      },
                     new Pagination()
                     {
                         ApplyPagination = true,
                         LimitRows = limit,
                         FromPage = fromPage
                     },
                     !string.IsNullOrWhiteSpace(userId),
                     userId);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets the user's connection requests by pagination (something like friend requests).
        /// </summary>
        /// <param name="fromPage">Current page.</param>
        /// <param name="limit">How many items per page.</param>
        /// <returns>Returns the users’ list.</returns>
        [HttpGet("requests/shared-details/{fromPage}/{limit}")]
       public async Task<IActionResult> GetUserConnectionRequests([FromRoute] int fromPage, [FromRoute] int limit)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                var result = Users.SearchRequests(
                     new AccountAdvance()
                     {
                         UserAlphaId = sessionResponseModel.UserId,
                     },
                    new Pagination()
                    {
                        ApplyPagination = true,
                        LimitRows = limit,
                        FromPage = fromPage
                    });

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the users that are connected to the user making the request. *Just the model, for faster readings.
        /// </summary>
        /// <returns>Returns a HTTP Response Message with all details. </returns>
        [HttpGet("models")]
       public async Task<IActionResult> GetUserRelationsWithOtherUsers()
        {
            try
            {
                var sessionResponseModel = 
                    CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // Return error about invalid request
                var schoolIds = new List<string>();
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    schoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                }

                var result = Users.SearchRelationsModels(
                    new AccountGeneral()
                    {
                        UserAlphaId = sessionResponseModel.UserId,
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        IsDirector = sessionResponseModel.IsDirector,
                        IsStaff = sessionResponseModel.IsStaff,
                        IsStudentHomeschool = sessionResponseModel.RoleId == "3" && SubscriptionDetailsHandler.CheckIStudentIsHomeschool(sessionResponseModel.Token),
                        SchoolIds = schoolIds != null && schoolIds.Count > 0 ? schoolIds : null,
                    });

                return Ok(result);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        ///  Returns the users that are connected with the user by filtering by role’s id. (Just the models)
        /// </summary>
        /// <param name="roleId">The role’s id to filter.</param>
        /// <returns>Returns a HTTP Response Message with the profile connection models </returns>
        [HttpGet("models/by-roles/{roleId}")]
       public async Task<IActionResult> GetUserRelationsWithOtherUsersFilterByRole([FromRoute] string roleId)
        {
            try
            {
                UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // Return error about invalid request
                List<string> schoolIds = new List<string>();
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    schoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                }


                var result = Users.SearchRelationsModels(new AccountGeneral()
                {
                    UserAlphaId = sessionResponseModel.UserId,
                    RoleId = roleId,
                    IsClientAccount = sessionResponseModel.RoleId == "1",
                    IsDirector = sessionResponseModel.IsDirector,
                    IsStaff = sessionResponseModel.IsStaff,
                    SchoolIds = schoolIds != null && schoolIds.Count > 0 ? schoolIds : null,
                });

                return Ok(result);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// To send an invitation code to another user, so the user can send messages, probably manage, etc. (Something like sending a friend request.).
        /// </summary>
        /// <param name="invitationCode">The invitation’s code to send.</param>
        /// <returns>Returns the general response details with transaction details.</returns>
        [HttpPost("request/{invitationCode}")]
       public async Task<IActionResult> SendConnectionRequest([FromRoute] string invitationCode)
        {
            try
            {
                UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // TODO: Refactorize this.
                UsersAPI.Domain.Models.Relations.ResponseModel responseConnectUsers = new RelatedAccountsManager(
                      AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                      AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                      sessionResponseModel.LangCode).ConnectUsers(
                    sessionResponseModel.UserId,
                    invitationCode,
                    AppConfiguration.Configuration["AppSettings:urel_restricted_connections"] == "1",
                    false,
                    sessionResponseModel.UserId
                    );

                UsersAPI.Domain.Models.Generals.Responses.GeneralResponseModel responseModel = new GeneralResponseModel()
                {
                    TransactionValid = responseConnectUsers.Valid,
                    Messages = responseConnectUsers.Messages
                };

                if (responseModel.TransactionValid)
                {
                    return CreatedAtAction(nameof(SendConnectionRequest), responseModel);
                }
                else
                {
                    return Ok(responseModel);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To send a connection request to another user with one selected user by email.
        /// Example of use: We want a child to be connected to his parent. So the userRequestModel.UserIds[0] is the id of the child, and the email is the parent’s email.
        /// </summary>
        /// <param name="userRequestModel">Details to create the connection.</param>
        /// <returns>Returns a HTTP Response Message.</returns>
        [HttpPost("request/by-email/create")]
       public async Task<IActionResult> SendConnectionRequestByEmail([FromBody] ProfileConnectionEmailRequestModel userRequestModel)
        {
            // First, check the token
            // Then check if the user is connected
            try
            {
                var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // Verify if user is related
                if (!Users.CheckIfUserCanHandleOtherUser(
                    new AccountGeneral()
                    {
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        UserAlphaId = sessionResponseModel.UserId,
                        IsDirector = sessionResponseModel.IsDirector,
                        IsStaff = sessionResponseModel.IsStaff,
                        UserIds = new List<string>() { userRequestModel.UserIds[0] },
                        OreId = sessionResponseModel.OreId,
                    }))
                {
                    return Unauthorized();
                }

                // Check if the user exists by email and automatically relate him.
                var userDetailsManager = new UserDetailsManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                      sessionResponseModel.LangCode);
                var userEmailResponse = userDetailsManager.GetUserIdByEmail(userRequestModel.Email);

                if (userEmailResponse.Valid)
                {
                    // Get user Invitation Code 
                    var userAccountModelInvited = userDetailsManager.GetUserAccountDetails(new List<UsersAPI.Domain.Models.Accounts.UserAccountModel>()
                    {
                        new UsersAPI.Domain.Models.Accounts.UserAccountModel()
                        {
                            Id = userRequestModel.UserIds[0],
                        }
                    });

                    // Insert directly
                    if (userAccountModelInvited.Count > 0)
                    {
                        // TODO: Refactorize this
                        var responseConnectUsers = new RelatedAccountsManager(
                          AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                          AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                          sessionResponseModel.LangCode).ConnectUsers(
                            userEmailResponse.UserId,
                            userAccountModelInvited[0].Token,
                           AppConfiguration.Configuration["AppSettings:urel_restricted_connections"] == "1",
                           true,
                           sessionResponseModel.UserId
                       );

                        if (responseConnectUsers.Valid)
                        {
                            return Created(
                                "",
                            new GeneralResponseModel()
                            {
                                Messages =
                                new List<string>()
                                {
                                       new UsersAPI.Services.Generals.MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], sessionResponseModel.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_message_code_success_request_directed"])
                                },
                            });
                        }

                    }
                }

                // Now, send it
                try
                {
                    // First, get the user id invitation code
                    UsersAPI.Domain.Models.Accounts.UserAccountModel userAccountModel = new UsersAPI.Domain.Models.Accounts.UserAccountModel();

                    List<UsersAPI.Domain.Models.Accounts.UserAccountModel> userAccountModelList =
                    userDetailsManager.GetUserAccountDetails(new List<UsersAPI.Domain.Models.Accounts.UserAccountModel>()
                    {
                        new UsersAPI.Domain.Models.Accounts.UserAccountModel()
                        {
                            Id = userRequestModel.UserIds[0],
                        },
                    });

                    string tokenEmail = TokenManager.GenerateAlphanumericToken(32);

                    var profilesConnectionManager = 
                        new ProfilesConnectionsRequestsManager(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]);

                    profilesConnectionManager.CreateProfileConnectionInvitationByEmail(
                        new List<UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel>() {
                        new UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel()
                        {
                            CreatedBy = sessionResponseModel.UserId,
                            ModifiedBy = sessionResponseModel.UserId,
                            UserCodeToConnectWith = userAccountModelList[0].Token,
                            Email = userRequestModel.Email,
                            Token = tokenEmail,
                        }
                        });

                    // Now send the email.
                    UsersAPI.Domain.Models.Generals.EmailMessageModel emailMessageModel = new UsersAPI.Infrastructure.Repositories.Generals.EmailMessageDAO().Read(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                        new UsersAPI.Domain.Models.Generals.EmailMessageModel()
                        {
                            Type = "INV",
                            LangCode = sessionResponseModel.LangCode,
                        },
                        true,
                        true)[0];

                    // Substitute the strings
                    emailMessageModel.Body = emailMessageModel.Body.Replace("{$1}", userAccountModelList[0].Token);
                    emailMessageModel.Url = emailMessageModel.Url.Replace("{$1}", userAccountModelList[0].Token);
                    emailMessageModel.Url = emailMessageModel.Url.Replace("{$0}", tokenEmail);
                    string sendDetails = EmailManager.SendEmailSimple(
                         AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                         new List<string> { userRequestModel.Email },
                         emailMessageModel.Subject,
                         emailMessageModel.Header + " " + emailMessageModel.Body + " " + emailMessageModel.Url + " " + emailMessageModel.Footer);

                    return Created(
                        "",
                       new GeneralResponseModel()
                       {
                           Messages =
                           new List<string>()
                           {
                               new UsersAPI.Services.Generals.MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], sessionResponseModel.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_message_code_success_request"]),
                           },
                       });
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes a connection sent.
        /// </summary>
        /// <param name="id">The id of the request to delete.</param>
        /// <returns>Returns the general response details with transaction details.</returns>
        [HttpPost("request/by-email/delete/{id}")]
        public async Task<IActionResult> DeleteConnectionRequestByEmail([FromRoute] string id)
        {

            UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }

            new ProfilesConnectionsRequestsManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]).
                DeleteProfileConnectionInvitationByEmail(new List<UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel>()
                {
                new UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel()
                {
                    Id = id,
                    CreatedBy = sessionResponseModel.UserId,
                }
               });

            return Ok();
        }

        /// <summary>
        /// Gets the connection requests that were sent by the user to emails.
        /// This connection requests by email are the ones that are sent by the teacher to a student tutor's email.
        /// </summary>
        /// <param name="fromPage">Current page.</param>
        /// <param name="limit">How many items per page.</param>
        /// <returns>Returns the profile connection invitation’s list.</returns>
        [HttpGet("request/by-email/{fromPage}/{limit}")]
       public async Task<IActionResult> GetConnectionRequestByEmail([FromRoute] int fromPage, [FromRoute] int limit)
        {
            UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }

            List<UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel> profileConnectionByEmailList =
            new ProfilesConnectionsRequestsManager(
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                      AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]).
                      GetProfileInvitationByEmailModelList(new List<UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel>()
            {
                 new UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel()
                {
                    CreatedBy = sessionResponseModel.UserId,
                }
            });

            ProfileConnectionInvitationResponseModel profileConnectionInvitationResponseModel = new ProfileConnectionInvitationResponseModel()
            {
                TotalPages = int.Parse(Math.Ceiling((double)profileConnectionByEmailList.Count / limit).ToString()),
            };
            profileConnectionInvitationResponseModel.ProfileConnectionInvitationByEmailModelList = profileConnectionByEmailList;
            profileConnectionInvitationResponseModel.ProfileConnectionInvitationByEmailModelList = profileConnectionInvitationResponseModel.ProfileConnectionInvitationByEmailModelList.Skip((fromPage - 1) * limit).Take(limit).ToList();

            return Ok(profileConnectionInvitationResponseModel);
        }

        /// <summary>
        /// Verifies if the user alpha can do any changes or access the data to user beta. 
        /// This is used to check if the user can access the other user's results, send messages, verifies grades, etc.
        /// </summary>
        /// <param name="userIdAlpha">The user’s id (alpha. Example: teacher, parent, admin, etc).</param>
        /// <param name="userIdBeta">The user’s id beta (Example: student; teacher if alpha is admin, etc).</param>
        /// <returns>Returns a HTTP Response Message with a boolean indicating if they are connected or not.</returns>
        [HttpGet("user-handle-user/{userIdAlpha}/{userIdBeta}")]
        public async Task<IActionResult> AreUsersConnected([FromRoute] string userIdAlpha, [FromRoute] string userIdBeta)
        {
            var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RelationsUsers");

            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(userIdAlpha) || string.IsNullOrWhiteSpace(userIdBeta))
            {
                return BadRequest(false);
            }

            return Ok(Users.CheckIfUserCanHandleOtherUser(
                    new AccountGeneral()
                    {
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        UserAlphaId = userIdAlpha,
                        IsDirector = sessionResponseModel.IsDirector,
                        IsStaff = sessionResponseModel.IsStaff,
                        UserIds = new List<string>() { userIdBeta },
                        OreId = sessionResponseModel.OreId,
                    }));
        }

        /// <summary>
        /// Returns the profile's main account (the client account).
        /// For Swagger: Just send the IP Address if applies.
        /// </summary>
        /// <param name="permissionSessionRequestModel">Includes permission session details.</param>
        /// <returns>Returns a HTTP Response Message with the ID of the main user account. </returns>
        [HttpPost("profile-main-account")]
       public async Task<IActionResult> GetProfileMainAccount([FromBody] PermissionSessionRequestModel permissionSessionRequestModel)
        {

            // First, we must check if the user has permission for it
            var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections",
                string.IsNullOrWhiteSpace(permissionSessionRequestModel?.IpAddress) ? IPAddressDetector.GetClientIpAddress(HttpContext) : permissionSessionRequestModel?.IpAddress);
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }

            var result = Users.SearchProfileMainAccount(sessionResponseModel.UserId);

            return Ok(result);
        }

        /// <summary>
        /// To add a connection between users (to be used by staff only).
        /// </summary>
        /// <param name="userIdA">The user’s id.</param>
        /// <param name="isUserIdAAlpha">If user A can handle user B.</param>
        /// <param name="userIdB">The user’s id b to relate with user a.</param>
        /// <returns>Returns a HTTP Response transaction details.</returns>
        [HttpPost("add-connection/{userIdA}/{isUserIdAAlpha}/{userIdB}")]
       public async Task<IActionResult> AddConnectionByStaff([FromRoute] string userIdA, [FromRoute] bool isUserIdAAlpha, [FromRoute] string userIdB)
        {

            var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid || !sessionResponseModel.IsStaff)
            {
                return Unauthorized();
            }

            if (!Users.CheckIfUserCanHandleOtherUser(
                 new AccountGeneral()
                 {
                     IsClientAccount = sessionResponseModel.RoleId == "1",
                     UserAlphaId = sessionResponseModel.UserId,
                     IsDirector = sessionResponseModel.IsDirector,
                     IsStaff = sessionResponseModel.IsStaff,
                     UserIds = new List<string>() { userIdA },
                     OreId = sessionResponseModel.OreId,
                 }) ||

                    !Users.CheckIfUserCanHandleOtherUser(
                 new AccountGeneral()
                 {
                     IsClientAccount = sessionResponseModel.RoleId == "1",
                     UserAlphaId = sessionResponseModel.UserId,
                     IsDirector = sessionResponseModel.IsDirector,
                     IsStaff = sessionResponseModel.IsStaff,
                     UserIds = new List<string>() { userIdB },
                     OreId = sessionResponseModel.OreId,
                 }))
            {
                return Unauthorized();
            }

            var responseModel = new RelatedAccountsManager(
             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
             sessionResponseModel.LangCode).ConnectUsersDirect(userIdA, userIdB,
             isUserIdAAlpha,
             sessionResponseModel.UserId);

            if (responseModel.Valid)
            {
                return Created("",
                 new GeneralResponseModel() { Messages = responseModel.Messages });
            }
            else
            {
                return BadRequest(
               new GeneralResponseModel() { Messages = responseModel.Messages });
            }
        }

        /// <summary>
        /// To relate the profile to the main account. This is used when relating an user to a subscription.
        /// </summary>
        /// <param name="userIdMainAccount">The user id of the main account.</param>
        /// <param name="userIdProfile">The user id (profile).</param>
        /// <returns>Returns a HTTP response with the transaction details.</returns>
        [HttpPost("add-profile-main-account/{userIdMainAccount}/{userIdProfile}")]
       public async Task<IActionResult> AddProfileToMainAccount([FromRoute] string userIdMainAccount, [FromRoute] string userIdProfile)
        {
            var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid || !sessionResponseModel.IsStaff)
            {
                return Unauthorized();
            }

            if (!Users.CheckIfUserCanHandleOtherUser(
                 new AccountGeneral()
                 {
                     IsClientAccount = sessionResponseModel.RoleId == "1",
                     UserAlphaId = sessionResponseModel.UserId,
                     IsDirector = sessionResponseModel.IsDirector,
                     IsStaff = sessionResponseModel.IsStaff,
                     UserIds = new List<string>() { userIdMainAccount },
                     OreId = sessionResponseModel.OreId,
                 }) ||

                    !Users.CheckIfUserCanHandleOtherUser(
                 new AccountGeneral()
                 {
                     IsClientAccount = sessionResponseModel.RoleId == "1",
                     UserAlphaId = sessionResponseModel.UserId,
                     IsDirector = sessionResponseModel.IsDirector,
                     IsStaff = sessionResponseModel.IsStaff,
                     UserIds = new List<string>() { userIdProfile },
                     OreId = sessionResponseModel.OreId,
                 }))
            {
                return Unauthorized();
            }

            var responseModel = new RelatedAccountsManager(
             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
             sessionResponseModel.LangCode).AssociateAccountToMainSubscriptionUser(userIdMainAccount, userIdProfile, sessionResponseModel.UserId);

            if (responseModel.Valid)
            {
                return Created("", responseModel);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes a connection. It does not delete the user from the subscription. The user will stay, however, the connection will be deleted. 
        /// The associated account will not be associated (in the case of the main subscriptions). 
        /// And the profile connection will be deleted. It will delete the connection between the user in session and user Id.
        /// </summary>
        /// <param name="userIdA">User Id to delete.</param>
        /// <param name="userIdB">Optional: The other user to delete relation (only available if the transaction is being made by the staff, who can choose two users). If not sent, it will delete the user in session with the userIdA. </param>
        /// <returns>Returns a Http Response Message.</returns>
        [HttpPost("delete-connection/{userIdA}/{userIdB?}")]
        public async Task<IActionResult> DeleteConnection([FromRoute] string userIdA, [FromRoute] string userIdB = null)
        {

            var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }

            var responseModel = new UsersAPI.Domain.Models.Relations.ResponseModel();
            // If the user is staff and userId was sent, then delete what was specified. 
            // If not, just delete the relation of the userIdA with the user in session.
            if (sessionResponseModel.IsStaff && !string.IsNullOrWhiteSpace(userIdB))
            {
                responseModel = new RelatedAccountsManager(
                 AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                 AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                 sessionResponseModel.LangCode).DeleteConnection(userIdB, userIdA, sessionResponseModel.UserId, sessionResponseModel.IsStaff);
            }
            else
            {
                responseModel = new RelatedAccountsManager(
                       AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                       AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                       sessionResponseModel.LangCode).DeleteConnection(sessionResponseModel.UserId, userIdA, sessionResponseModel.UserId, sessionResponseModel.IsStaff);

            }

            return Ok(new GeneralResponseModel() { Messages = responseModel.Messages });
        }

        /// <summary>
        /// Deletes a connection request.
        /// </summary>
        /// <param name="userId">User Id to delete the request.</param>
        /// <returns>Returns a Http Response Message.</returns>
        [HttpPost("delete-request/{userId}")]
        public async Task<IActionResult> DeleteConnectionRequest([FromRoute] string userId)
        {

            var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }

            new ProfilesConnectionsRequestsManager(
                   AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                   AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]).
                   DeleteConnectionRequest(sessionResponseModel.UserId, userId);

            return Ok();
        }

        /// <summary>
        /// To accept a connection request. Example: When a teacher accept a request by a student's tutor.
        /// </summary>
        /// <param name="userId">User's id to accept a connection request.</param>
        /// <returns>Returns just the HTTP Code.</returns>
        [HttpPost("accept-request/{userId}")]
        public async Task<IActionResult> AcceptConnectionRequest([FromRoute] string userId)
        {
            // Since this call is available from API to API feed, validate the original IP Address, else, get the fe IP.
            var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }

            new ProfilesConnectionsRequestsManager(
                   AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                   AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]).
                   AcceptConnectionRequestNoAlphas(sessionResponseModel.UserId, userId);

            return Ok();
        }

        /// <summary>
        /// Gets the total number of distinct students enrolled at the same school(s)
        /// as the specified teacher, based on their profile connections.
        /// </summary>
        /// <returns>
        /// <para><see cref="OkObjectResult"/> (200) with a int of total of students</c> on success.</para>
        /// <para><see cref="UnauthorizedResult"/> (401) if the session is invalid,
        /// the caller is not staff, or the caller is not authorized to access the
        /// target teacher's data.</para>
        /// is null, empty, or whitespace.</para>
        /// <para>500 Internal Server Error if the underlying query fails.</para>
        /// </returns>
        [HttpGet("teacher-students-count/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTeacherStudentsCount()
        {
            UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel =
                CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileConnections");

            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }

            int studentsCount;
            try
            {
                studentsCount = await new RelatedAccountsManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext)).GetTeacherStudentsCountAsync(sessionResponseModel.UserId);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(studentsCount);
        }
    }
}
