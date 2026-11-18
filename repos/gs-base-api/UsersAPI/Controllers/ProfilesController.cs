using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models.Filters;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.Accounts;
using UsersAPI.UsersAPI.Services.FeignAPIs;
using UsersAPI.UsersAPI.Services.PersonalDetails;
using UsersAPI.UsersAPI.Services.Relations;
using UsersAPI.UsersAPI.Services.Roles;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To Handle profiles.
    /// </summary>
    [ApiController]
    [Route("usersapi/profiles")]
    public class ProfilesController : ControllerBase
    {
        /// <summary>
        /// To read the emails of the profiles. If Admin is making the request, the admin can access to every email. If ORE, just the users related to the ORE, 
        /// the principal (director) just the users from the school, and the other users just the user's related to.
        /// For Swagger: You can send to this endpoint the following properties: SchoolIds, Email, RoleId, UserIds, UserSchoolRecord, PageFrom, Limiter
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="generalRequestModel">The filters, such as RoleId, Email, UserSchoolRecord, and so on. </param>
        /// <returns>Returns the profiles' emails.</returns>
        [HttpPost("emails")]
        public async Task<IActionResult> ReadEmailsProfiles([FromBody] UserRequestModel generalRequestModel)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfilesRead");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // If has an ore, then call the school ids of that ore only
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    // Check if school ids is not null
                    if (generalRequestModel.SchoolIds == null || generalRequestModel.SchoolIds.Count == 0)
                    {
                        generalRequestModel.SchoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                    }
                    else
                    {
                        List<string> schoolsIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                        List<string> filteredSchoolIds = new List<string>();
                        foreach (string schoolId in generalRequestModel.SchoolIds)
                        {
                            if (schoolsIds.Contains(schoolId))
                            {
                                filteredSchoolIds.Add(schoolId);
                            }
                        }
                        generalRequestModel.SchoolIds = filteredSchoolIds;
                    }

                    if (generalRequestModel.SchoolIds == null || generalRequestModel.SchoolIds.Count == 0)
                    {
                        return Ok(new List<UserEmailModel>());
                    }
                }

                List<UserEmailModel> userEmailModels = Users.SearchUsersEmails(
                    new AccountAdvance()
                    {
                        CallOnlyProfilesThatCanBeManaged = true,
                        IsDirector = sessionResponseModel.IsDirector,
                        IsStaff = sessionResponseModel.IsStaff,
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        SameSchoolRequired = sessionResponseModel.RoleId != "1" && !sessionResponseModel.IsStaff && sessionResponseModel.RoleId != "7",
                        SchoolIds = generalRequestModel?.SchoolIds,
                        Email = generalRequestModel?.Email,
                        RoleId = generalRequestModel?.RoleId,
                        UserAlphaId = sessionResponseModel.UserId,
                        UserIds = generalRequestModel?.UserIds != null && generalRequestModel.UserIds.Count > 0 ? generalRequestModel.UserIds : null,
                        UserSchoolRecord = generalRequestModel?.UserSchoolRecord,
                    });

                // Returns the users 
                return Ok(userEmailModels.Skip(generalRequestModel.PageFrom * generalRequestModel.Limiter).Take(generalRequestModel.Limiter > userEmailModels.Count ? userEmailModels.Count : generalRequestModel.Limiter).ToList());
            }
            catch (Exception)
            {
                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// To read the users. It will read the users according to the roles. For example, Admin/Employee (staff) can read every user. Teachers, Parents, and clients can read the users related to them. 
        /// Just read the users that the user making the request can handle.
        /// For Swagger: We do not use the SubscriptionId property for this endpoint.
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="generalRequestModel">Request with filters, such a FirstName, LastName, RoleId, etc.</param>
        /// <returns>Returns the HTTP Response with profiles details.</returns>
        [HttpPost]
        public async Task<IActionResult> ReadProfiles([FromBody] UserRequestModel generalRequestModel)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfilesRead");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // If has an ore, then call the school ids of that ore only
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    // Check if school ids is not null
                    if (generalRequestModel.SchoolIds == null || generalRequestModel.SchoolIds.Count == 0)
                    {
                        generalRequestModel.SchoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                    }
                    else
                    {
                        List<string> schoolsIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                        List<string> filteredSchoolIds = new List<string>();
                        foreach (string schoolId in generalRequestModel.SchoolIds)
                        {
                            if (schoolsIds.Contains(schoolId))
                            {
                                filteredSchoolIds.Add(schoolId);
                            }
                        }
                        generalRequestModel.SchoolIds = filteredSchoolIds;
                    }

                    if (generalRequestModel.SchoolIds == null || generalRequestModel.SchoolIds.Count == 0)
                    {
                        return Ok(new UserAdvanceResponseModel()
                        {
                            PageTotals = 0,
                            UserInfo = new List<UserAdvance>(),
                        });
                    }
                }

                // Returns the users 
                return Ok(Users.SearchUsers(
                    new AccountAdvance()
                    {
                        CallOnlyProfilesThatCanBeManaged = true,
                        CreatedFrom = generalRequestModel != null ? generalRequestModel?.CreatedFrom : null,
                        CreatedTo = generalRequestModel != null ? generalRequestModel?.CreatedTo : null,
                        IsDirector = sessionResponseModel.IsDirector,
                        IsStaff = sessionResponseModel.IsStaff,
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        SameSchoolRequired = sessionResponseModel.RoleId != "1" && !sessionResponseModel.IsStaff && sessionResponseModel.RoleId != "7",
                        SchoolIds = generalRequestModel != null ? generalRequestModel?.SchoolIds : null,
                        LastName = generalRequestModel != null ? generalRequestModel?.LastName : null,
                        Email = generalRequestModel != null ? generalRequestModel?.Email : null,
                        FirstName = generalRequestModel != null ? generalRequestModel?.FirstName : null,
                        PhoneNumber = generalRequestModel != null ? generalRequestModel?.PhoneNumber : null,
                        RoleId = generalRequestModel != null ? generalRequestModel?.RoleId : null,
                        UserAlphaId = sessionResponseModel.UserId,
                        UserIds = generalRequestModel != null && generalRequestModel.UserIds != null && generalRequestModel.UserIds.Count > 0 ? generalRequestModel.UserIds : null,
                        Username = generalRequestModel != null ? generalRequestModel?.Username : null,
                        LastSessionFrom = generalRequestModel != null ? generalRequestModel?.LastSessionFrom : null,
                        LastSessionTo = generalRequestModel != null ? generalRequestModel?.LastSessionTo : null,
                        UserSchoolRecord = generalRequestModel != null ? generalRequestModel?.UserSchoolRecord : null,
                    },
                    generalRequestModel.Limiter == 0 ? null :
                    new Pagination()
                    {
                        ApplyPagination = true,
                        FromPage = generalRequestModel != null ? generalRequestModel.PageFrom : 0,
                        LimitRows = generalRequestModel != null ? generalRequestModel.Limiter : 50,
                    }));
            }
            catch (Exception ex)
            {
                // Return error about invalid json
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To read the profile's details.
        /// </summary>
        /// <param name="profileId">The profile's id to read the details.</param>
        /// <returns>Returns the HTTP Response Message.</returns>
        [HttpGet("{profileId}")]
        public async Task<IActionResult> ReadProfile(string profileId)
        {
            try
            {
                UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileInformationRead");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // If has an ore, then call the school ids of that ore only
                List<string> schoolIds = new List<string>();
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    // Call the ore schools 
                    schoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                }

                var userList = Users.SearchUsers(
                new AccountAdvance()
                {
                    CallOnlyProfilesThatCanBeManaged = true,
                    IsDirector = sessionResponseModel.IsDirector,
                    IsStaff = sessionResponseModel.IsStaff,
                    SameSchoolRequired = sessionResponseModel.RoleId != "1" && !sessionResponseModel.IsStaff && sessionResponseModel.RoleId != "7",
                    IsClientAccount = sessionResponseModel.RoleId == "1",
                    UserAlphaId = sessionResponseModel.UserId,
                    UserIds = new List<string> { profileId },
                    SchoolIds = schoolIds
                },
                new Pagination()
                {
                    ApplyPagination = true,
                    FromPage = 0,
                    LimitRows = 1,
                });

                if (userList.UserInfo != null && userList.UserInfo.Count > 0)
                {
                    // Returns the users 
                    return Ok(userList.UserInfo[0]);
                }

                return BadRequest(new UserAdvance());
            }
            catch (Exception)
            {
                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the profiles of a subscription (for the admin (staff) when accesing to the subscription's details).
        /// For Swagger: 
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="generalRequestModel">The user request with the details to filter. </param>
        /// <returns>Returns a HTTP Response with the users related to the subscription. </returns>
        [HttpPost("subscriptions/profiles")]
        public async Task<IActionResult> GetSubscriptionProfiles([FromBody] UserRequestModel generalRequestModel)
        {
            try
            {
                UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "UsersAdvanceRead");

                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                if (string.IsNullOrWhiteSpace(generalRequestModel.SubscriptionId))
                {
                    return BadRequest();
                }

                // If has an ore, then call the school ids of that ore only
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    // Call the ore schools 
                    generalRequestModel.SchoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                }

                // Check the users of the subscription by id.
                List<UsersAPI.Domain.Models.Generals.Responses.APIs.SubscriptionProfileModel> subscriptionProfiles =
                    SubscriptionDetailsHandler.GetSubscriptionsProfileModels(sessionResponseModel.IpAddress, sessionResponseModel.LangCode, sessionResponseModel.Token, 
                    new List<string> { generalRequestModel.SubscriptionId });

                if (subscriptionProfiles.Count == 0)
                {
                    return Ok(new UserAdvanceResponseModel());
                }

                // Prepare the list of user ids
                List<string> userIds = new List<string>();
                foreach (UsersAPI.Domain.Models.Generals.Responses.APIs.SubscriptionProfileModel subscriptionProfile in subscriptionProfiles)
                {
                    userIds.Add(subscriptionProfile.ProfileId);
                }

                // Returns the users 
                return Ok(Users.SearchUsers(
                    new AccountAdvance()
                    {
                        CallOnlyProfilesThatCanBeManaged = !sessionResponseModel.IsDirector && !sessionResponseModel.IsStaff,
                        CreatedFrom = generalRequestModel != null ? generalRequestModel?.CreatedFrom : null,
                        CreatedTo = generalRequestModel != null ? generalRequestModel?.CreatedTo : null,
                        IsDirector = sessionResponseModel.IsDirector,
                        IsStaff = sessionResponseModel.IsStaff,
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        SameSchoolRequired = false,
                        SchoolIds = generalRequestModel != null ? generalRequestModel?.SchoolIds : null,
                        LastName = generalRequestModel != null ? generalRequestModel?.LastName : null,
                        Email = generalRequestModel != null ? generalRequestModel?.Email : null,
                        FirstName = generalRequestModel != null ? generalRequestModel?.FirstName : null,
                        PhoneNumber = generalRequestModel != null ? generalRequestModel?.PhoneNumber : null,
                        RoleId = generalRequestModel != null ? generalRequestModel?.RoleId : null,
                        UserAlphaId = sessionResponseModel.UserId,
                        UserIds = userIds,
                        Username = generalRequestModel != null ? generalRequestModel?.Username : null,
                        LastSessionFrom = generalRequestModel != null ? generalRequestModel?.LastSessionFrom : null,
                        LastSessionTo = generalRequestModel != null ? generalRequestModel?.LastSessionTo : null,
                        UserSchoolRecord = generalRequestModel != null ? generalRequestModel?.UserSchoolRecord : null,
                    },
                    new Pagination()
                    {
                        ApplyPagination = true,
                        FromPage = generalRequestModel != null ? generalRequestModel.PageFrom : 0,
                        LimitRows = generalRequestModel != null ? generalRequestModel.Limiter : 50,
                    }));
            }
            catch (Exception)
            {
                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the users related to the main account without caring about the subscription. However, a subscription's id is asked to identify the user of the main account.
        /// For Swagger: 
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="generalRequestModel">The user request with the details to filter.</param>
        /// <returns>Returns a HTTP Response with the users related to the subscription. </returns>
        [HttpPost("subscriptions/client/profiles")]
        public async Task<IActionResult> GetSubscriptionProfilesByMainUser([FromBody] UserRequestModel generalRequestModel)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "UsersAdvanceRead");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // If has an ore, then call the school ids of that ore only
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    // Call the ore schools 
                    generalRequestModel.SchoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                }

                if (string.IsNullOrWhiteSpace(generalRequestModel.SubscriptionId))
                {
                    return BadRequest();
                }

                // Get the main user of the subscription
                var subscriptionUserResponseModel =
                SubscriptionDetailsHandler.GetSubscriptionDetails(sessionResponseModel.Token, sessionResponseModel.IpAddress, generalRequestModel.SubscriptionId, sessionResponseModel.LangCode);
                if (subscriptionUserResponseModel == null || subscriptionUserResponseModel.Subscriptions == null || subscriptionUserResponseModel.Subscriptions.Count == 0)
                {
                    return Ok(new UserAdvanceResponseModel());
                }

                // Get the ID of the school 
                string schoolId = new SchoolDetailsHandler().GetSchoolIdOfSubscription(generalRequestModel.SubscriptionId);
                if(!string.IsNullOrWhiteSpace(schoolId))
                {
                    if(generalRequestModel.SchoolIds == null)
                    {
                        generalRequestModel.SchoolIds = new List<string>()
                        {
                            schoolId,
                        };
                    }
                    else
                    {
                        generalRequestModel.SchoolIds.Add(schoolId);
                    }
                }

                // Returns the users 
                return Ok(Users.SearchUsers(
                    new AccountAdvance()
                    {
                        CallOnlyProfilesThatCanBeManaged = false,  // We are calling this for the client
                        CreatedFrom = generalRequestModel != null ? generalRequestModel?.CreatedFrom : null,
                        CreatedTo = generalRequestModel != null ? generalRequestModel?.CreatedTo : null,
                        IsDirector = false, // We are calling this for the client
                        IsStaff = sessionResponseModel.IsStaff,  // We are calling this for the client
                        IsClientAccount = sessionResponseModel.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_client"],  // We are calling this for the client
                        SameSchoolRequired = false, // If role is not a client, apply the logic of search by schools.,
                        SchoolIds = generalRequestModel != null ? generalRequestModel?.SchoolIds : null,
                        LastName = generalRequestModel != null ? generalRequestModel?.LastName : null,
                        Email = generalRequestModel != null ? generalRequestModel?.Email : null,
                        FirstName = generalRequestModel != null ? generalRequestModel?.FirstName : null,
                        PhoneNumber = generalRequestModel != null ? generalRequestModel?.PhoneNumber : null,
                        RoleId = generalRequestModel != null ? generalRequestModel?.RoleId : null,
                        UserAlphaId = subscriptionUserResponseModel.MainAccountUserId, // NOTE: This argument is different here. Do not confuse it or change it.
                        Username = generalRequestModel != null ? generalRequestModel?.Username : null,
                        LastSessionFrom = generalRequestModel != null ? generalRequestModel?.LastSessionFrom : null,
                        LastSessionTo = generalRequestModel != null ? generalRequestModel?.LastSessionTo : null,
                        UserSchoolRecord = generalRequestModel != null ? generalRequestModel?.UserSchoolRecord : null,
                    },
                    new Pagination()
                    {
                        ApplyPagination = true,
                        FromPage = generalRequestModel != null ? generalRequestModel.PageFrom : 0,
                        LimitRows = generalRequestModel != null ? generalRequestModel.Limiter : 50,
                    }));
            }
            catch (Exception)
            {
                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// Registers the profile to the subscription. If the subscription has a school attached, the user will be related to the school and/or programs automatically.
        /// For Swagger: The following properties are required: 
        /// RoleId, SchoolId, SubscriptionId, Username, SchoolUserIdentifier, Password, PasswordRetype, FirstName,
        /// LastName, Email, (Dob and Gender are processed if sent, but not required).
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="requestModel">Request details.</param>
        /// <param name="isHomeSchool">If it is homeschooling, then true, else, false.</param>
        /// <returns>Returns a HTTP Response with profile registration.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterProfile([FromBody] RegisterRequestModel requestModel, bool isHomeSchool = false)
        {
            try
            {
                var generalResponseModel = new RegisterProfileResponseModel();
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RegisterProfile");

                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // If homeschool generate a random UserSchoolIdentifier
                if (isHomeSchool && string.IsNullOrWhiteSpace(requestModel.SchoolUserIdentifier))
                {
                    requestModel.SchoolUserIdentifier = Guid.NewGuid().ToString("N").Substring(0, 10);
                }

                if (string.IsNullOrWhiteSpace(requestModel.Username) || string.IsNullOrWhiteSpace(requestModel.Password))
                {
                    generalResponseModel.Messages = new List<string>(){
                            new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                            sessionResponseModel.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_register_profile_retype_pass_username"])
                        };
                    return BadRequest(generalResponseModel);

                }

                // First, call the subscriptions details 
                // Check if there is any space left for the subscription
                var subscriptionDetails = SubscriptionDetailsHandler.GetSubscriptionDetails(sessionResponseModel.Token, sessionResponseModel.IpAddress, requestModel.SubscriptionId, sessionResponseModel.LangCode);
                if (subscriptionDetails == null || subscriptionDetails.Subscriptions == null || subscriptionDetails.Subscriptions.Count == 0)
                {
                    return BadRequest();
                }
                var mainAccountUserId = subscriptionDetails.MainAccountUserId;
                if (!sessionResponseModel.IsStaff)
                {
                    mainAccountUserId = sessionResponseModel.UserId;
                }

                // If subscrptions profiles is not null and more than 0, then evaluate if there is any space left
                if (subscriptionDetails.Subscriptions[0].SubscriptionProfiles != null && subscriptionDetails.Subscriptions[0].SubscriptionProfiles.Count > 0)
                {
                    if (subscriptionDetails.Subscriptions[0].SubscriptionProfiles.Count + 1 > int.Parse(subscriptionDetails.Subscriptions[0].ProfilesTotalQuantity))
                    {
                        if (AppConfiguration.Configuration["AppSettings:gus_discount_only_students"] == "1" && requestModel.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_client_child"])
                        {
                            // Discount the users who are students
                            // Count only the students
                            List<UsersAPI.Domain.Models.Roles.UserRoleModel> profileRoleIdList = new List<UsersAPI.Domain.Models.Roles.UserRoleModel>();
                            foreach (SubscriptionProfileModel profile in subscriptionDetails.Subscriptions[0].SubscriptionProfiles)
                            {
                                profileRoleIdList.Add(new UsersAPI.Domain.Models.Roles.UserRoleModel()
                                {
                                    UserId = profile.ProfileId,
                                    RoleId = AppConfiguration.Configuration["AppSettings:ur_role_client_child"],
                                });
                            }
                            var userRolesManager = new UserRolesManager(
                                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], 
                                sessionResponseModel.LangCode);
                            var profileRoles = userRolesManager.GetUserRolesByRolesList(profileRoleIdList);

                            int studentsTotal = 0;
                            foreach (var profileRole in profileRoles)
                            {
                                studentsTotal++;
                            }

                            if (studentsTotal > int.Parse(subscriptionDetails.Subscriptions[0].ProfilesTotalQuantity))
                            {
                                generalResponseModel.Messages.Add(new UsersAPI.Services.Generals.MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"], sessionResponseModel.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_message_code_add_profile_error"]));
                                return BadRequest(generalResponseModel);
                            }
                        }
                        else if (AppConfiguration.Configuration["AppSettings:gus_discount_only_students"] == "0" && requestModel.RoleId != AppConfiguration.Configuration["AppSettings:ur_role_client_child"])
                        {
                            generalResponseModel.Messages.Add(new UsersAPI.Services.Generals.MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"], sessionResponseModel.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_message_code_add_profile_error"]));
                            return Ok(generalResponseModel);
                        }
                    }
                }

                // Verify if the school identifier exists or not.
                var userSchoolManager = new UserSchoolManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], sessionResponseModel.LangCode);

                // Check if subscription has a school related to not duplicate the identifier 
                requestModel.SchoolId = new SchoolDetailsHandler().GetSchoolIdOfSubscription(requestModel.SubscriptionId);
                if (!string.IsNullOrWhiteSpace(requestModel.SchoolId))
                {
                    // Check if user is allowed to add to the ore
                    if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                    {
                        if (!ORESchoolUserCheck.CheckIfSchoolInOre(requestModel.SchoolId, sessionResponseModel.OreId))
                        {
                            return Unauthorized();
                        }
                    }

                    // Validate if school user identifier was sent 
                    if (string.IsNullOrWhiteSpace(requestModel.SchoolUserIdentifier))
                    {
                        generalResponseModel.Messages = new List<string>(){
                            new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                            sessionResponseModel.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_user_record_exists"])
                        };
                        return BadRequest(generalResponseModel);
                    }

                    // If was sent, check if available 
                    if (!userSchoolManager.CheckIfIdentifierAvailable(requestModel.SchoolUserIdentifier, 
                        AppConfiguration.Configuration["AppSettings:global_school_identifier"] == "1" ? null : requestModel.SchoolId))
                    {
                        generalResponseModel.Messages = new List<string>(){
                            new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                            sessionResponseModel.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_user_record_exists"])
                        };
                        return BadRequest(generalResponseModel);
                    }
                }


                // Get the parent address to insert the children address
                UsersAPI.Domain.Models.PersonalDetails.UserAddressModel userAddressModel = RegisterUserManager.GetMainAccountMainAddress(mainAccountUserId, sessionResponseModel.LangCode);
                // Now, continue with the child registration

                // If subscruptions profiles is not null and more than 0, then evaluate if there is any space left
                RegisterProfileResponseModel userChildRegisterResponseModel = RegisterUserManager.RegisterUserProfile(
                        requestModel,
                        sessionResponseModel.UserId,
                        sessionResponseModel.IpAddress,
                        userAddressModel,
                    subscriptionDetails,
                    requestModel.RoleId,
                    sessionResponseModel.LangCode);

                if (!userChildRegisterResponseModel.TransactionValid)
                {
                    generalResponseModel.TransactionValid = userChildRegisterResponseModel.TransactionValid;
                    generalResponseModel.Messages = userChildRegisterResponseModel.Messages;
                    return BadRequest(generalResponseModel);
                }

                // Attach the profile to the subscription
                var messageAttch = SubscriptionDetailsHandler.AttachProfileToSubscriptionEmergency(requestModel.SubscriptionId, userChildRegisterResponseModel.UserId);
                // SubscriptionDetailsHandler.AttachProfileToSubscription(sessionResponseModel.IpAddress, AppConfiguration.Configuration["AppSettings:ur_role_client_child"], userChildRegisterResponseModel.UserId, requestModel.RoleId, requestModel.SubscriptionId, sessionResponseModel.LangCode, sessionResponseModel.Token);


                // Now associate the accounts
                RelatedAccountsManager profilesConnectionManager = new RelatedAccountsManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    sessionResponseModel.LangCode);
                profilesConnectionManager.AssociateAccountToMainSubscriptionUser(mainAccountUserId, new List<string>() { userChildRegisterResponseModel.UserId }, sessionResponseModel.UserId);

                if (!string.IsNullOrWhiteSpace(requestModel.SchoolId))
                {
                    // Check if the subscription is attached to a school
                    userSchoolManager.RelateUserToSchool(userChildRegisterResponseModel.UserId, requestModel.SchoolId, sessionResponseModel.IsStaff, sessionResponseModel.UserId, requestModel.SchoolUserIdentifier);
                }

        
                // Return the response
                generalResponseModel.Messages = userChildRegisterResponseModel.Messages;
                generalResponseModel.Messages.Add(messageAttch);
                generalResponseModel.SpacesInSubscriptionLeft = userChildRegisterResponseModel.SpacesInSubscriptionLeft; // The new space available
                generalResponseModel.TransactionValid = true;
                return Created("", generalResponseModel);
            }
            catch (Exception)
            {
                return BadRequest(new UsersAPI.Domain.Models.Generals.Responses.GeneralResponseModel());
            }
        }

        /// <summary>
        /// To modify the password profiles. It can be modified if the user making the request can handle the other user (example: teacher can handle the student).
        /// For Swagger: These are the only properties that will be processed if sent: ProfileId, 
        /// Password, PasswordRetype (all of them required).
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="requestModel">Request model with details.</param>
        /// <returns>Returns a HttpResponseMessage.</returns>
        [HttpPost("password/modify")]
        public async Task<IActionResult> ModifyPasswordProfile([FromBody] ProfileInformationRequestModel requestModel)
        {
            GeneralResponseModel generalResponseModel = new GeneralResponseModel();
            try
            {
                UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfilePasswordReset");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }


                // Check if the user has the permission to update the other student profile
                if (!Users.CheckIfUserCanHandleOtherUser(
                    new AccountGeneral()
                    {
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        UserAlphaId = sessionResponseModel.UserId,
                        IsDirector = sessionResponseModel.IsDirector,
                        IsStaff = sessionResponseModel.IsStaff,
                        UserIds = new List<string>() { requestModel.ProfileId },
                        OreId = sessionResponseModel.OreId,
                    }))
                {
                    return Unauthorized();
                }


                generalResponseModel.Messages = new List<string>();

                // If we are here, it means that the user has de permission to update the data
                PasswordAccountManager passwordAccountManager = new PasswordAccountManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], 
                    sessionResponseModel.LangCode);
                UsersAPI.Domain.Models.Accounts.ResponseModel responseModel =
                passwordAccountManager.UpdatePasswordWithoutEntryOld(requestModel.ProfileId, requestModel.Password, requestModel.PasswordRetype);

                generalResponseModel.Messages = responseModel.Details;
                if (!responseModel.Valid)
                {
                    return BadRequest(generalResponseModel);
                }

                return Ok(generalResponseModel);
            }
            catch (Exception)
            {
                // Return error about invalid json
                return BadRequest(generalResponseModel);
            }
        }

        /// <summary>
        /// Modifies profile information. It can be modified if the user making the request 
        /// can handle the other user (example: teacher can handle the student).
        /// For Swagger: These are the only properties that will be processed if sent: ProfileId (Required), 
        /// FirstName, LastName, Email, DoB, Gender.
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="requestModel">Request details.</param>
        /// <returns>Return a HTTPResponse with result datails.</returns>
        [HttpPost("profile/modify")]
        public async Task<IActionResult> ModifyProfileInformation([FromBody] ProfileInformationRequestModel requestModel)
        {
            GeneralResponseModel generalResponseModel = new GeneralResponseModel();
            try
            {

                UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileUpdateInformation");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                bool transactionValid = true;

                // Check if the user has the permission to update the other student profile
                if (!Users.CheckIfUserCanHandleOtherUser(
                     new AccountGeneral()
                     {
                         IsClientAccount = sessionResponseModel.RoleId == "1",
                         UserAlphaId = sessionResponseModel.UserId,
                         IsDirector = sessionResponseModel.IsDirector,
                         IsStaff = sessionResponseModel.IsStaff,
                         UserIds = new List<string>() { requestModel.ProfileId }, 
                         OreId = sessionResponseModel.OreId,
                     }))
                {
                    return Unauthorized();
                }

                // Now do the updates
                if (!string.IsNullOrWhiteSpace(requestModel.FirstName) && !string.IsNullOrWhiteSpace(requestModel.LastName))
                {
                    // Update the first name and last name
                    UserPersonalDetailsManager userModifyManager = new UserPersonalDetailsManager(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], 
                        sessionResponseModel.LangCode);
                    UsersAPI.Domain.Models.PersonalDetails.ResponseModel response =
                    userModifyManager.UpdateOrInsertUserNameAndLastnameWithValidation(
                        requestModel.ProfileId,
                        requestModel.FirstName,
                        requestModel.LastName,
                        null,
                        sessionResponseModel.UserId);

                    if (!response.Valid)
                    {
                        transactionValid = false;
                    }

                    generalResponseModel.Messages.AddRange(response.Details);
                }

                if (!string.IsNullOrWhiteSpace(requestModel.Email))
                {
                    // TODO: Refactor this.
                    UserModifyManager userModifyManager = new UserModifyManager(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], sessionResponseModel.LangCode);
                    UsersAPI.Domain.Models.Accounts.ResponseModel response = userModifyManager.UpdateEmailProcess(requestModel.ProfileId, requestModel.Email, sessionResponseModel.UserId);
                    if (!response.Valid)
                    {
                        transactionValid = false;
                    }
                    generalResponseModel.Messages.AddRange(response.Details);
                }

                // Update the DOB if it's not null
                if (!string.IsNullOrWhiteSpace(requestModel.DoB))
                {
                    UserPersonalDetailsManager userModifyManager = new UserPersonalDetailsManager(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], sessionResponseModel.LangCode);
                    UsersAPI.Domain.Models.PersonalDetails.UserDobModel userDobModelToInsert = new UsersAPI.Domain.Models.PersonalDetails.UserDobModel
                    {
                        UserId = requestModel.ProfileId,
                        Dob = requestModel.DoB,
                    };
                    UsersAPI.Domain.Models.PersonalDetails.ResponseModel response =
                    userModifyManager.InsertOrUpdateUserDobProcessWithValidation(userDobModelToInsert);
                    if (!response.Valid)
                    {
                        transactionValid = false;
                    }

                    generalResponseModel.Messages.AddRange(response.Details);
                }

                // Updates the gender if is not null.
                if (!string.IsNullOrWhiteSpace(requestModel.Gender))
                {
                    // Update gender
                    UserPersonalDetailsManager userModifyManager = new UserPersonalDetailsManager(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], sessionResponseModel.LangCode);
                    UsersAPI.Domain.Models.PersonalDetails.UserGenderModel userGenderToInsert = new UsersAPI.Domain.Models.PersonalDetails.UserGenderModel
                    {
                        UserId = requestModel.ProfileId,
                        Gender = requestModel.Gender,
                    };
                    UsersAPI.Domain.Models.PersonalDetails.ResponseModel response =
                    userModifyManager.InsertOrUpdateUserGenderProcessWithValidation(userGenderToInsert);

                    if (!response.Valid)
                    {
                        transactionValid = false;
                    }

                    generalResponseModel.Messages.AddRange(response.Details);
                }

                if (!transactionValid)
                {
                    return BadRequest(generalResponseModel);
                }

                return Ok(generalResponseModel);
            }
            catch (Exception)
            {
                // Return error about invalid json
                return BadRequest(generalResponseModel);
            }
        }
    }
}