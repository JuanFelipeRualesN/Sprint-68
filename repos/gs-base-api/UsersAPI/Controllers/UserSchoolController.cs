using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models.Filters;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.FeignAPIs;
using UsersAPI.UsersAPI.Services.PersonalDetails;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To handle the relations between users and schools.
    /// </summary>
    [ApiController]
    [Route("usersapi/schools")]
    public class UsersSchoolController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UsersSchoolController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// To just get the users' schools ids.
        /// </summary>
        /// <param name="userId">The id of the user to check the school.</param>
        /// <returns>Returns the user's school ids list.</returns>
        [HttpGet("user/{userId}/school-ids")]
        public async Task<IActionResult> GetUserSchoolsIds(string userId)
        {
            // First, we must check if the user has permission for it
            /* Token validation in Users API */
            UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "UserSchoolsRead");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */

            // Get the user schools. 
            var userSchools = Users.SearchUserSchools(userId);
            // Get just the ids.
            List<string> userSchoolIds = new List<string>();
            foreach (var userSchool in userSchools)
            {
                userSchoolIds.Add(userSchool.SchoolId);
            }

            return Ok(userSchoolIds);
        }

        /// <summary>
        /// To get the user's schools with schools info, and if check if they are confirmed or not.
        /// </summary>
        /// <param name="userId">The id of the user to check the school.</param>
        /// <returns>Returns the user's school details with entity’s information as institution’s name, state, city, etc.</returns>
        [HttpGet("info/user_schools/{userId}")]
        public async Task<IActionResult> GetUserSchools(string userId)
        {
            // First, we must check if the user has permission for it
            /* Token validation in Users API */
            UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "UserSchoolsRead");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */

            // Check if the user has the permission to update the other student profile
            if (!Users.CheckIfUserCanHandleOtherUser(
                 new AccountGeneral()
                 {
                     IsClientAccount = sessionResponseModel.RoleId == "1",
                     UserAlphaId = sessionResponseModel.UserId,
                     IsDirector = sessionResponseModel.IsDirector,
                     IsStaff = sessionResponseModel.IsStaff,
                     UserIds = new List<string>() { userId },
                     OreId = sessionResponseModel.OreId,
                 }))
            {
                return Unauthorized();
            }

            List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchoolModelListResult = Users.SearchUserSchools(userId);

            // Now relate the user schools with school info.
            if (userSchoolModelListResult.Count == 0)
            {
                return Ok(new List<UserSchoolDetailResponseModel>());
            }

            // Get the schools details 
            List<string> entityIds = new List<string>();
            foreach (UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel userSchool in userSchoolModelListResult)
            {
                entityIds.Add(userSchool.SchoolId);
            }

            List<EntityGeneralDetailsResponseModel> entityGeneralDetails = new  SchoolDetailsHandler().GetEntityGeneralDetails(entityIds);

            // Set the return object.
            List<UserSchoolDetailResponseModel> userSchoolsDetails = new List<UserSchoolDetailResponseModel>();
            foreach (UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel userSchoolModel in userSchoolModelListResult)
            {
                UsersAPI.Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel entityGeneralDetailModel = entityGeneralDetails.Find(x => x.EntityId == userSchoolModel.SchoolId);

                userSchoolsDetails.Add(new UserSchoolDetailResponseModel()
                {
                    UserSchoolId = userSchoolModel.Id,
                    UserId = userSchoolModel.UserId,
                    InstitutionId = userSchoolModel.SchoolId,
                    InstitutionName = entityGeneralDetailModel?.EntityName,
                    InstitutionIdentifier = entityGeneralDetailModel?.EntityIdentifier,
                    State = entityGeneralDetailModel?.State,
                    City = entityGeneralDetailModel?.City,
                    Confirmed = userSchoolModel.Confirmed == "1" ? true : false,
                    Country = entityGeneralDetailModel?.Country,
                    ZipCode = entityGeneralDetailModel?.ZipCode
                });
            }

            return Ok(userSchoolsDetails);
        }

        /// <summary>
        /// To get the users' ids of the school sent in the route.
        /// </summary>
        /// <param name="schoolId">The school’s id to get the users.</param>
        /// <returns>Returns a list of User School Model with user id and school id’s relation.</returns>
        [HttpGet("users/{schoolId}")]
        public async Task<IActionResult> GetSchoolsUsers(string schoolId)
        {
            // First, we must check if the user has permission for it
            /* Token validation in Users API */
            UsersAPI.Domain.Models.Generals.ValidationResponseModel responseSession = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "UserSchoolsRead");
            // If the transaction is not valid, stop the operation
            if (!responseSession.SessionValid)
            {
                return Unauthorized();
            }

            if (!string.IsNullOrWhiteSpace(responseSession.OreId))
            {
                if (!ORESchoolUserCheck.CheckIfSchoolInOre(schoolId, responseSession.OreId))
                {
                    return Unauthorized();
                }
            }

            return Ok(
                new UserSchoolManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"], AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], null).GetUserSchools(
                new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>()
                {
                        new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                        {
                            SchoolId = schoolId,
                        },
                }));
        }

        /// <summary>
        /// To get the user making the request school's info, whether confirmed or not. 
        /// </summary>
        /// <returns>Returns a list with user school details’ response model.</returns>
        [HttpGet("info/by_token/user_schools/")]
        public async Task<IActionResult> GetUserSchoolsByToken()
        {

            // First, we must check if the user has permission for it
            /* Token validation in Users API */
            UsersAPI.Domain.Models.Generals.ValidationResponseModel responseSession = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "UserSchoolsRead");
            // If the transaction is not valid, stop the operation
            if (!responseSession.SessionValid)
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */

            List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchoolModelListResult = new 
                UserSchoolManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], RequestHandler.GetLanguage(HttpContext))
                .GetUserSchools(new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>() { new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel() { UserId = responseSession.UserId } });

            // Now relate the user schools with school info.
            if (userSchoolModelListResult.Count == 0)
            {
                return Ok(new List<UserSchoolDetailResponseModel>());
            }

            List<string> entityIds = new List<string>();
            foreach (UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel userSchool in userSchoolModelListResult)
            {
                entityIds.Add(userSchool.SchoolId);
            }
            List<EntityGeneralDetailsResponseModel> entityGeneralDetails = new SchoolDetailsHandler().GetEntityGeneralDetails(entityIds);

            // Set the return object.
            List<UserSchoolDetailResponseModel> userSchoolsDetails = new List<UserSchoolDetailResponseModel>();

            foreach (UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel userSchoolModel in userSchoolModelListResult)
            {
               EntityGeneralDetailsResponseModel entityGeneralDetailModel = entityGeneralDetails.Find(x => x.EntityId == userSchoolModel.SchoolId);

                userSchoolsDetails.Add(new UserSchoolDetailResponseModel()
                {
                    UserSchoolId = userSchoolModel.Id,
                    UserId = userSchoolModel.UserId,
                    InstitutionId = userSchoolModel.SchoolId,
                    InstitutionName = entityGeneralDetailModel?.EntityName,
                    InstitutionIdentifier = entityGeneralDetailModel?.EntityIdentifier,
                    State = entityGeneralDetailModel?.State,
                    City = entityGeneralDetailModel?.City,
                    Confirmed = userSchoolModel.Confirmed == "1" ? true : false,
                    Country = entityGeneralDetailModel?.Country,
                    ZipCode = entityGeneralDetailModel?.ZipCode
                });
            }

            return Ok(userSchoolsDetails);
        }

        /// <summary>
        /// To get the user schools by filtering by user school model list.
        /// </summary>
        /// <param name="userSchoolModelList">User schools to find.</param>
        /// <returns>Returns a list of user schools’ models.</returns>
        [HttpPost("user_schools/")]
        public async Task<IActionResult> GetUserSchoolsByUserIdList(List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchoolModelList)
        {

            UsersAPI.Domain.Models.Generals.ValidationResponseModel responseSession = ValidateGenericCall.ValidateFullCall(HttpContext, "UserSchoolsRead");
            // If the transaction is not valid, stop the operation
            if (!responseSession.SessionValid)
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */
            if (userSchoolModelList.Count == 0)
            {
                return BadRequest(new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>());
            }

            // Delete any user that is not in the school if ORE 
            if (!string.IsNullOrWhiteSpace(responseSession.OreId))
            {
                // Call the schools 
                List<String> allowedSchools = SchoolDetailsHandler.GetORESchoolIds(responseSession.OreId);
                List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> allowedUserSchools = new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>();
                foreach (UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel userSchool in userSchoolModelList)
                {
                    if (allowedSchools.Contains(userSchool.SchoolId))
                    {
                        allowedUserSchools.Add(userSchool);
                    }
                }

                userSchoolModelList = allowedUserSchools;

                if (userSchoolModelList.Count == 0)
                {
                    return BadRequest(new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>());
                }
            }


            return Ok(Users.SearchUserSchoolModelsByUserSchoolModelList(userSchoolModelList));
        }

        /// <summary>
        /// To confirm that an existing user is part of a school (for example a student that is part of a school, a teacher, principal, etc).
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="entityId">The entity to relate.</param>
        /// <param name="recordNo">The record number (SIE).</param>
        /// <returns>Returns just the HTTP code.</returns>
        [HttpPost("confirm/user_schools/{userId}/{entityId}/{recordNo}")]
        public async Task<IActionResult> ConfirmUserSchool(string userId, string entityId, string recordNo)
        {

            // First, we must check if the user has permission for it
            /* Token validation in Users API */
            UsersAPI.Domain.Models.Generals.ValidationResponseModel responseSession = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ConfirmUserEntity");
            // If the transaction is not valid, stop the operation
            if (!responseSession.SessionValid)
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(entityId))
            {
                return BadRequest();
            }


            // Check if user can handle the school 
            if (!string.IsNullOrWhiteSpace(responseSession.OreId))
            {
                if (!ORESchoolUserCheck.CheckIfSchoolInOre(entityId, responseSession.OreId))
                {
                    return Unauthorized();
                }

                if (!ORESchoolUserCheck.CheckIfStaffOreCanHandleUser(responseSession.OreId, userId))
                {
                    return Unauthorized();
                }
            }

            // Check if User record is available
            if (!new UserSchoolManager(
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                             responseSession.LangCode).CheckIfIdentifierAvailable(recordNo,
                        AppConfiguration.Configuration["AppSettings:global_school_identifier"] == "1" ? null : entityId))
            {
                return BadRequest(new GeneralResponseModel() { Messages = new List<string>()
                {
                    new MessageManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                           RequestHandler.GetLanguage(HttpContext)).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_user_record_exists"])
                }
                });
            }


            // If staff or principal, automatically removed the user from the older schools
            if (responseSession.IsDirector || responseSession.IsStaff)
            {
                new UserSchoolManager(
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                             responseSession.LangCode).
                             DeleteUserSchool(
                              new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>() {
                                new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                                {
                                    UserId = userId,
                                }
                            });
            }

            // Attach the user school.
            new UserSchoolManager(
                         AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                         AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                         responseSession.LangCode).
                         CreateUserSchools(
                    new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>() {
                        new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                        {
                            Confirmed = "1",
                            UserId = userId,
                            Identifier = recordNo,
                            SchoolId = entityId,
                            ModifiedBy = responseSession.UserId,
                        }
                    });

            return Ok();
        }

        /// <summary>
        /// To add a user to school or program.
        /// </summary>
        /// <param name="userId">The user’s id to add.</param>
        /// <param name="entityId">The entity’s id (School Id or Program Id).</param>
        /// <param name="identifier">User's record number for the school.</param>
        /// <returns>Returns a HTTP response only.</returns>
        [HttpPost("add/user_schools/{userId}/{entityId}/{identifier}")]
        public async Task<IActionResult> AddUserToSchool(string userId, string entityId, string identifier)
        {
            // First, we must check if the user has permission for it
            /* Token validation in Users API */
            UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "UserSchoolAddRequest");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid || string.IsNullOrWhiteSpace(sessionResponseModel.UserId))
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */


            // Check if user can handle the school 
            if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
            {
                if (!ORESchoolUserCheck.CheckIfSchoolInOre(entityId, sessionResponseModel.OreId))
                {
                    return Unauthorized();
                }

                if (!ORESchoolUserCheck.CheckIfStaffOreCanHandleUser(sessionResponseModel.OreId, userId))
                {
                    return Unauthorized();
                }
            }

            // Check if the user has the permission to update the other student profile
            if (!Users.CheckIfUserCanHandleOtherUser(
                 new AccountGeneral()
                 {
                     IsClientAccount = sessionResponseModel.RoleId == "1",
                     UserAlphaId = sessionResponseModel.UserId,
                     IsDirector = sessionResponseModel.IsDirector,
                     IsStaff = sessionResponseModel.IsStaff,
                     UserIds = new List<string>() { userId },
                     OreId = sessionResponseModel.OreId,
                 }))
            {
                return Unauthorized();
            }

            /* End of Token Validation in Users API */
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(entityId) || string.IsNullOrWhiteSpace(identifier))
            {
                return BadRequest();
            }

            // Validate the identifier and the school.
            // Check if User record is available
            if (!new UserSchoolManager(
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                             sessionResponseModel.LangCode).CheckIfIdentifierAvailable(identifier,
                        AppConfiguration.Configuration["AppSettings:global_school_identifier"] == "1" ? null : entityId))
            {
                return BadRequest(new GeneralResponseModel()
                {
                    Messages = new List<string>()
                {
                    new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                           RequestHandler.GetLanguage(HttpContext)).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_user_record_exists"])
                }
                });
            }

            // If staff or principal, automatically removed the user from the older schools
            if (sessionResponseModel.IsDirector || sessionResponseModel.IsStaff)
            {
                new UserSchoolManager(
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                             sessionResponseModel.LangCode).
                             DeleteUserSchool(
                              new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>() {
                                new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                                {
                                    UserId = userId,
                                }
                            });
            }

            // Attach the user school.
            new UserSchoolManager(
                         AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                         AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                         sessionResponseModel.LangCode).
                         CreateUserSchools(
                    new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>() {
                        new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                        {
                            Confirmed = sessionResponseModel.IsStaff || sessionResponseModel.IsDirector ? "1": "0", // Autoconfirm if staff.
                            UserId = userId,
                            Identifier = identifier,
                            SchoolId = entityId,
                            ModifiedBy = sessionResponseModel.UserId,
                        }
                    });

            return CreatedAtAction(nameof(GetUserSchools), new { userId = userId }, null);
        }

        /// <summary>
        /// To validate if a record number (SIE) is available or not.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="recordNo">The record number (SIE).</param>
        /// <returns>Returns true if available, else, returns false.</returns>
        [HttpGet("validate/user_schools/{entityId}/{recordNo}")]
        public async Task<IActionResult> ValidateIfUserSchoolRecordAvailable(string entityId, string recordNo)
        {
            // Check if User record is available
            // Now we can check if the identifier is global or just for school.
            if (!new UserSchoolManager(
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                             AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                             RequestHandler.GetLanguage(HttpContext)).CheckIfIdentifierAvailable(recordNo,
                        AppConfiguration.Configuration["AppSettings:global_school_identifier"] == "1" ? null : entityId))
            {
                return Ok(false);
            }
            else
            {
                return Ok(true);
            }
        }

        /// <summary>
        /// Deletes the relation between a user and school.
        /// </summary>
        /// <param name="userId">The user’s id to delete the relation.</param>
        /// <param name="entityId">The entity’s id (School Id or Program Id).</param>
        /// <returns>Returns a HTTP response only. </returns>
        [HttpPost("delete/user_schools/{userId}/{entityId}")]
        public async Task<IActionResult> DeleteUserSchool(string userId, string entityId)
        {
            // First, we must check if the user has permission for it
            /* Token validation in Users API */
            UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "DeleteUserSchool");
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid || string.IsNullOrWhiteSpace(sessionResponseModel.UserId))
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */

            // Check if user can handle the school 
            if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
            {
                if (!ORESchoolUserCheck.CheckIfSchoolInOre(entityId, sessionResponseModel.OreId))
                {
                    return Unauthorized();
                }
            }

            // Check if the user has the permission to update the other student profile
            if (!Users.CheckIfUserCanHandleOtherUser(
                 new AccountGeneral()
                 {
                     IsClientAccount = sessionResponseModel.RoleId == "1",
                     UserAlphaId = sessionResponseModel.UserId,
                     IsDirector = sessionResponseModel.IsDirector,
                     IsStaff = sessionResponseModel.IsStaff,
                     UserIds = new List<string>() { userId },
                     OreId = sessionResponseModel.OreId,
                 }))
            {
                return Unauthorized();
            }

            /* End of Token Validation in Users API */
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(entityId))
            {
                return BadRequest();
            }

            // Confirm the user school.
            new UserSchoolManager(
                         AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                         AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                         sessionResponseModel.LangCode).
                         DeleteUserSchool(
                      new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>() {
                        new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                        {
                            UserId = userId,
                            SchoolId = entityId,
                        }
                    });

            return Ok();
        }

        /// <summary>
        /// Check if users are the same school (user making the reuqest and user id in the route).
        /// </summary>
        /// <param name="userIdToCheck">The user's id to check if it is from the same school as the user in making the request.</param>
        /// <returns>Returns a boolean. True if same school, else, false.</returns>
        [HttpGet("users/check-same-school/{userIdToCheck}")]
        public async Task<IActionResult> CheckIfUserSameSchool(string userIdToCheck)
        {
            // First, we must check if the user has permission for it
            UsersAPI.Domain.Models.Generals.ValidationResponseModel responseSession = ValidateGenericCall.ValidateFullCall(HttpContext, "UserSchoolsRead", CustomClasses.IPAddressDetector.GetClientIpAddress(HttpContext));
            // If the transaction is not valid, stop the operation
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            /* End of Token Validation in Users API */

            return Ok(Users.CheckIfUsersInSameSchool(responseSession.UserId, userIdToCheck));
        }

        /// <summary>
        /// To relate the users of a subscription to a school.
        /// </summary>
        /// <param name="subscriptionId">The subscription id to find the users and to relate them to a school.</param>
        /// <returns>Returns a HTTP Response with response code.</returns>
        [HttpPost("users/relate-user-school-by-subscription/{subscriptionId}")]
        public async Task<IActionResult> RelateUserSchoolBySubscription(string subscriptionId)
        {
            try
            {
                // First, we must check if the user has permission for it
                /* Token validation in Users API */
                UsersAPI.Domain.Models.Generals.ValidationResponseModel responseSession = ValidateGenericCall.ValidateFullCall(HttpContext, "UserSchoolAddRequest");
                // If the transaction is not valid, stop the operation
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                /* End of Token Validation in Users API */

                // Now start the proccess. Call the subscription id for details. 
                string schoolId = new SchoolDetailsHandler().GetSchoolIdOfSubscription(subscriptionId);

                if (string.IsNullOrWhiteSpace(schoolId))
                {
                    return BadRequest();
                }

                // Check if user can handle the school 
                if (!string.IsNullOrWhiteSpace(responseSession.OreId))
                {
                    if (!ORESchoolUserCheck.CheckIfSchoolInOre(schoolId, responseSession.OreId))
                    {
                        return Unauthorized();
                    }
                }

                SubscriptionResponseModel subscriptionResponseModel = 
                    SubscriptionDetailsHandler.GetSubscriptionDetails(responseSession.Token, IPAddressDetector.GetClientIpAddress(HttpContext), subscriptionId, responseSession.LangCode);
                if (subscriptionResponseModel == null || subscriptionResponseModel.Subscriptions == null || subscriptionResponseModel.Subscriptions.Count == 0 || subscriptionResponseModel.Subscriptions[0].SubscriptionProfiles == null || subscriptionResponseModel.Subscriptions[0].SubscriptionProfiles.Count == 0)
                {
                    return BadRequest();
                }

                // Else, start to attach the users 
                List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchoolsToAttachList = new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>();

                foreach (SubscriptionProfileModel subscriptionProfile in subscriptionResponseModel.Subscriptions[0].SubscriptionProfiles)
                {
                    userSchoolsToAttachList.Add(new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                    {
                        SchoolId = schoolId,
                        UserId = subscriptionProfile.ProfileId,
                        Confirmed = "1",
                        ModifiedBy = responseSession.UserId,
                    });
                }

                List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userExistingSchoolList = new 
                    UserSchoolManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], responseSession.LangCode)
                    .GetUserSchools(new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>()
            {
                new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                {
                    SchoolId = schoolId,
                }
            });

                var result = userSchoolsToAttachList.Where(p => userExistingSchoolList.All(p2 => p2.UserId != p.UserId));
                userSchoolsToAttachList = result.ToList();

                if (userSchoolsToAttachList.Count > 0)
                {
                    new UserSchoolManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], responseSession.LangCode).
                        CreateUserSchools(userSchoolsToAttachList);
                }

                return CreatedAtAction(nameof(GetUserSchools), new { userId = userSchoolsToAttachList.First().UserId }, null);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the users' full names of the users that are in the same school as the user making the request. 
        /// This is used in an API to API call, to get the users' full names, related or not, but from the same school, for forums purposes.
        /// </summary>
        /// <param name="userListRequests">A list of users' ids.</param>
        /// <returns>Returns a list of the user full names model.</returns>
        [HttpPost("general/same-school")]
        public async Task<IActionResult> GetUsersGeneralDataOfSameSchools(List<string> userListRequests)
        {
            try
            {
                UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "SchoolEntitiesRead");

                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                if (userListRequests == null || userListRequests.Count == 0)
                {
                    // Return empty list.
                    return Ok(new List<UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel>());
                }

                return Ok(Users.SearchNamesOfUsersInListOfSameSchoolAsUserSession(sessionResponseModel.UserId, userListRequests));

            }
            catch (Exception)
            {
                return BadRequest(new List<UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel>());
            }
        }
    }
}
