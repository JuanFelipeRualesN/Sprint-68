using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UsersAPI.CustomClasses.CacheHandler;
using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models.Filters;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.Accounts;
using UsersAPI.UsersAPI.Services.FeignAPIs;
using UsersAPI.UsersAPI.Services.PersonalDetails;
using UsersAPI.UsersAPI.Services.Generals;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Services.Roles;
using UsersAPI.UsersAPI.Domain.Models.Generals; // Add this for ValidationResponseModel

namespace UsersAPI.Controllers
{
    /// <summary>
    /// Manages the users by admin.
    /// </summary>
    [ApiController]
    [Route("usersapi/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration = AppConfiguration.Configuration;

        /// <summary>
        /// To return a generic message when user upload bulk controller fails. 
        /// </summary>
        /// <returns>Returns a generic message.</returns>
        private GeneralResponseModel ReturnGenericMessageBulk(string languageCode = "en")
        {
            var generalResponseModel = new GeneralResponseModel()
            {
                Messages = new List<string>(),
            };

            // Message no DB
            if (languageCode == "es")
            {
                generalResponseModel.Messages.Add(_configuration["message_error_no_db_es"]);
            }
            else
            {
                generalResponseModel.Messages.Add(_configuration["message_error_no_db_en"]);
            }

            return generalResponseModel;
        }

        /// <summary>
        /// Validates the session for the given permission and optional parameters like IP address, token, and language code.
        /// </summary>
        /// <param name="permission">The required permission for the session.</param>
        /// <param name="ipAddress">Optional. The IP address of the user.</param>
        /// <returns>A ValidationResponseModel indicating the result of the validation.</returns>
        private ValidationResponseModel ValidateSession(string permission, string ipAddress = null)
        {
            var request = HttpContext.Request;
            string token = request.Headers["Token"].FirstOrDefault() ?? request.Query["Token"].FirstOrDefault();
            string langCode = request.Headers["LangCode"].FirstOrDefault() ?? request.Query["LangCode"].FirstOrDefault() ?? "en";
            ipAddress ??= HttpContext.Connection.RemoteIpAddress?.ToString();
            // You may need to refactor ValidateGenericCall to accept these parameters, or mimic its logic here
            return CustomClasses.ValidateGenericCall.Validate(token, permission, ipAddress, langCode, null);
        }

        /// <summary>
        /// To retrieve the user roles models by users' ids list.
        /// </summary>
        /// <param name="usersIds">The users ids to call.</param>
        /// <returns>Returns the users' roles.</returns>
        [HttpPost("users-roles")]
        public async Task<IActionResult> GetUserRolesModelsByUserIds([FromBody] List<string> usersIds)
        {
            try
            {
                var sessionResponseModel = ValidateSession("UsersAdvanceRead");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                List<UserRoleModel> userRolesToCall = usersIds.Select(userId => new UserRoleModel() { UserId = userId }).ToList();
                var userRolesManager = new UserRolesManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                    sessionResponseModel.LangCode);
                var result = userRolesManager.GetUserRolesByRolesList(userRolesToCall);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Return the list of users. This is meant to be used just for the Admin. Do not allow other roles to enter here.
        /// For Swagger: We do not use the SubscriptionId property for this endpoint.
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="generalRequestModel">General request details including filters.</param>
        /// <returns>Returns a list of user with the details.</returns>
        [HttpPost]
        public async Task<IActionResult> UserAdvanceRead([FromBody] UserRequestModel generalRequestModel)
        {
            try
            {
                var sessionResponseModel = ValidateSession("UsersAdvanceRead");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    if (generalRequestModel.SchoolIds == null || generalRequestModel.SchoolIds.Count == 0)
                    {
                        generalRequestModel.SchoolIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                    }
                    else
                    {
                        List<string> schoolsIds = SchoolDetailsHandler.GetORESchoolIds(sessionResponseModel.OreId);
                        generalRequestModel.SchoolIds = generalRequestModel.SchoolIds.Where(schoolId => schoolsIds.Contains(schoolId)).ToList();
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
                var result = Users.SearchUsers(
                    new AccountAdvance()
                    {
                        UserIds = generalRequestModel?.UserIds?.Count > 0 ? generalRequestModel.UserIds : null,
                        CallOnlyProfilesThatCanBeManaged = !sessionResponseModel.IsDirector && !sessionResponseModel.IsStaff,
                        CreatedFrom = generalRequestModel?.CreatedFrom,
                        CreatedTo = generalRequestModel?.CreatedTo,
                        IsDirector = sessionResponseModel.IsDirector,
                        IsStaff = sessionResponseModel.IsStaff,
                        SameSchoolRequired = sessionResponseModel.RoleId != "1" && !sessionResponseModel.IsStaff && sessionResponseModel.RoleId != "7",
                        IsClientAccount = sessionResponseModel.RoleId == "1",
                        SchoolIds = generalRequestModel?.SchoolIds,
                        LastName = generalRequestModel?.LastName,
                        Email = generalRequestModel?.Email,
                        FirstName = generalRequestModel?.FirstName,
                        PhoneNumber = generalRequestModel?.PhoneNumber,
                        RoleId = generalRequestModel?.RoleId,
                        UserAlphaId = sessionResponseModel.UserId,
                        Username = generalRequestModel?.Username,
                        LastSessionFrom = generalRequestModel?.LastSessionFrom,
                        LastSessionTo = generalRequestModel?.LastSessionTo,
                        UserSchoolRecord = generalRequestModel?.UserSchoolRecord,
                    },
                    new Pagination()
                    {
                        ApplyPagination = true,
                        FromPage = generalRequestModel?.PageFrom ?? 0,
                        LimitRows = generalRequestModel?.Limiter ?? 50,
                    });
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To modify the user by the employee.
        /// The properties AddressChange, UsernameChange, PasswordChange, EmailChange, NameChange,
        /// PhoneChange, AddressChange, RoleChange, OreChange should be true if any of those details are changed. 
        /// For example, if we are requesting a change in the address, AddressChange should be true.
        /// Note that OreId is required only if RoleId is 8 (Admin-ore).
        /// </summary>
        /// <param name="requestModel">The request with all details.</param>
        /// <returns>Returns a HTTPResponseMessage with the transaction details. </returns>
        [HttpPost("modify")]
        public async Task<IActionResult> ModifyUserByAdmin([FromBody] UserInfoUpdateRequestModel requestModel)
        {
            try
            {
                GeneralResponseModel generalResponseModel = new GeneralResponseModel()
                {
                    TransactionValid = true,
                };
                var sessionResponseModel = ValidateSession("UserInfoUpdate");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                if (string.IsNullOrWhiteSpace(requestModel.UserId))
                {
                    return BadRequest();
                }
                if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    if (!ORESchoolUserCheck.CheckIfStaffOreCanHandleUser(sessionResponseModel.OreId, requestModel.UserId))
                    {
                        return Unauthorized();
                    }
                }
                if (requestModel.AddressChange)
                {
                    var userUpdateAddressModel = AddressUserManager.UpdateUserAddress(
                        requestModel.AddressLine1,
                        requestModel.AddressLine2,
                        requestModel.City,
                        requestModel.CountryIso,
                        requestModel.Country,
                        requestModel.ZipCode,
                        requestModel.State ?? requestModel.StateIso,
                        requestModel.StateIso ?? requestModel.State,
                        requestModel.UserId,
                        sessionResponseModel.LangCode,
                        sessionResponseModel.UserId);
                    generalResponseModel.TransactionValid = userUpdateAddressModel.TransactionValid;
                    generalResponseModel.Messages.AddRange(userUpdateAddressModel.Messages);
                }
                var userModifyManager = new UserModifyManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                    sessionResponseModel.LangCode);
                if (requestModel.EmailChange)
                {
                    var responseEmailModel = userModifyManager.UpdateEmailProcess(
                        requestModel.UserId,
                        requestModel.Email,
                        sessionResponseModel.UserId);
                    if (generalResponseModel.TransactionValid)
                    {
                        generalResponseModel.TransactionValid = responseEmailModel.Valid;
                    }
                    generalResponseModel.Messages.AddRange(responseEmailModel.Details);
                }
                var userPersonalDetailsManager = new UserPersonalDetailsManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                    sessionResponseModel.LangCode);
                if (requestModel.NameChange)
                {
                    var responseUserDetails = userPersonalDetailsManager.UpdateOrInsertUserNameAndLastnameWithValidation(
                        requestModel.UserId,
                        requestModel.FirstName,
                        requestModel.LastName,
                        null,
                        sessionResponseModel.UserId);
                    if (generalResponseModel.TransactionValid)
                    {
                        generalResponseModel.TransactionValid = responseUserDetails.Valid;
                    }
                    generalResponseModel.Messages.AddRange(responseUserDetails.Details);
                }
                if (requestModel.PasswordChange)
                {
                    var passwordAccountManager = new PasswordAccountManager(
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), sessionResponseModel.LangCode);
                    var responsePassword = passwordAccountManager.UpdatePasswordWithoutEntryOld(requestModel.UserId, requestModel.Password, requestModel.PasswordRetype, sessionResponseModel.UserId);
                    if (generalResponseModel.TransactionValid)
                    {
                        generalResponseModel.TransactionValid = responsePassword.Valid;
                    }
                    generalResponseModel.Messages.AddRange(responsePassword.Details);
                }
                if (requestModel.PhoneChange)
                {
                    var responseUserDetails = userPersonalDetailsManager.InsertUserPhone(new UsersAPI.Domain.Models.PersonalDetails.UserPhoneNumberModel()
                    {
                        UserId = requestModel.UserId,
                        PhoneNumber = requestModel.Phone,
                        ModifiedBy = sessionResponseModel.UserId,
                    });
                    if (generalResponseModel.TransactionValid)
                    {
                        generalResponseModel.TransactionValid = responseUserDetails.Valid;
                    }
                    generalResponseModel.Messages.AddRange(responseUserDetails.Details);
                }
                if (requestModel.RoleChange)
                {
                    if (sessionResponseModel.IsStaff && sessionResponseModel.RoleId == _configuration["ur_role_admin_ore"])
                    {
                        string[] nonAllowedRolesForORE = _configuration["ur_roles_not_allowed_to_register_for_ore_admin"].Split(',');
                        if (nonAllowedRolesForORE.Contains(requestModel.RoleId))
                        {
                            generalResponseModel.Messages = new List<string>()
                            {
                                new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), GetLanguage()).GetMessageDetails(_configuration["ur_roles_this_role_cannot_be_registered"])
                            };
                            return BadRequest(generalResponseModel);
                        }
                    }
                    if (requestModel.RoleId != _configuration["ur_role_admin_ore"])
                    {
                        var userPreviousRole = Users.SearchUsersRolesByUserIds(new List<string>() { requestModel.UserId });
                        if (userPreviousRole[0].RoleId == _configuration["ur_role_admin_ore"])
                        {
                            Users.DeleteAmdminOreRelation(requestModel.UserId);
                        }
                    }
                    if (requestModel.RoleId == _configuration["ur_role_admin_ore"])
                    {
                        Users.RelateUserAdminToOreId(requestModel.UserId, requestModel.OreId, sessionResponseModel.UserId);
                        generalResponseModel.Messages = new List<string>()
                        {
                            new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), GetLanguage()).GetMessageDetails(_configuration["upd_message_code_ore_updated"])
                        };
                    }
                    var userRolesManager = new UserRolesManager(
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                        sessionResponseModel.LangCode);
                    var userRoleResponseModel = userRolesManager.UpdateUserRole(requestModel.UserId, requestModel.RoleId, sessionResponseModel.UserId);
                    if (_configuration["ur_role_client"] == requestModel.RoleId)
                    {
                        SubscriptionDetailsHandler.CreateCustomer(sessionResponseModel.IpAddress, sessionResponseModel.LangCode, requestModel.Email, requestModel.UserId, true);
                    }
                    if (generalResponseModel.TransactionValid)
                    {
                        generalResponseModel.TransactionValid = userRoleResponseModel.Valid;
                    }
                    generalResponseModel.Messages.AddRange(userRoleResponseModel.Details);
                }
                if (requestModel.UsernameChange)
                {
                    var responseUsername = userModifyManager.UpdateUsername(
                        requestModel.UserId,
                        requestModel.Username,
                        sessionResponseModel.UserId);
                    generalResponseModel.TransactionValid = responseUsername.Valid;
                    generalResponseModel.Messages.AddRange(responseUsername.Details);
                }
                if (!string.IsNullOrWhiteSpace(requestModel.StatusId) && !string.IsNullOrWhiteSpace(requestModel.UserId))
                {
                    userModifyManager.UserAccountStatusUpdate(new UsersAPI.Domain.Models.Accounts.UserAccountStatusModel()
                    {
                        UserId = requestModel.UserId,
                        StatusId = requestModel.StatusId,
                        ModifiedBy = sessionResponseModel.UserId,
                    });
                    try
                    {
                        new SessionTokenManager(
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader")).DeleteAllSessionsOfTheUser(requestModel.UserId);
                    }
                    catch (Exception)
                    {
                        // Just continue
                    }
                }
                if (requestModel.OreChange)
                {
                    if (requestModel.RoleId != _configuration["ur_role_admin_ore"])
                    {
                        generalResponseModel.Messages = new List<string>()
                        {
                            new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), GetLanguage()).GetMessageDetails(_configuration["upd_message_code_ore_only_for_admin_ore"])
                        };
                        return BadRequest(generalResponseModel);
                    }
                    else
                    {
                        Users.RelateUserAdminToOreId(requestModel.UserId, requestModel.OreId, sessionResponseModel.UserId);
                        generalResponseModel.Messages = new List<string>()
                        {
                            new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), GetLanguage()).GetMessageDetails(_configuration["upd_message_code_ore_updated"])
                        };
                    }
                }
                if (generalResponseModel.TransactionValid)
                {
                    // Cache update logic (can be refactored to async if needed)
                }
                if (!generalResponseModel.TransactionValid)
                {
                    return BadRequest(generalResponseModel);
                }
                return Ok(generalResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the user competition rival general details. We should be careful of what we share. That is why this is a different call.
        /// The UserId property is required. IpAddress is optional, unless the IP is evaluated again for access tokens.
        /// For Swagger: 
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="permissionSessionRequest">Permission request including the the user id, permissions, etc.</param>
        /// <returns>Returns a HTTP Response with the share details.</returns>
        [HttpPost("competition/rival")]
        public async Task<IActionResult> GetUserCompetitionRivalGeneralDetails([FromBody] PermissionSessionRequestModel permissionSessionRequest)
        {
            try
            {
                var sessionResponseModel = ValidateSession("GameQuizCompetitionSearchRival", permissionSessionRequest.IpAddress);
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var userGenderModel = new UserPersonalDetailsManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                    sessionResponseModel.LangCode).GetUserGender(permissionSessionRequest.UserId);
                var userAccountModelList = new UserDetailsManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), sessionResponseModel.LangCode)
                    .GetUserAccountDetails(new List<UsersAPI.Domain.Models.Accounts.UserAccountModel>
                    {
                        new UsersAPI.Domain.Models.Accounts.UserAccountModel { Id = permissionSessionRequest.UserId }
                    });
                var response = new UserRivalResponseModel
                {
                    UserRivalId = permissionSessionRequest.UserId,
                    Username = userAccountModelList[0].Username,
                    Gender = userGenderModel?.Gender ?? "M"
                };
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To upload users bulk. This also edits the users if they exist (if it is the same school record). 
        /// However, this does not update the passwords in bulk. If you want to update the password, you should use the password update.
        /// This is not a regular JSON. 
        /// There are three parameters that should be sent. The roleId (Role of the users), enrollmentId (The subscription's id), schoolId (The id of the school).
        /// In the body, it should be a form-data with the file to upload and a "send" action. 
        /// There are additional parameters that are required.
        /// </summary>
        /// <returns>Returns a response with transaction’s details.</returns>
        [HttpPost("bulk/upload/user-schools")]
        public async Task<IActionResult> UploadUsersBulk()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                string token = httpRequest.Form["Token"].ToString();
                string langCode = httpRequest.Form["LangCode"].ToString();
                string roleId = httpRequest.Form["RoleId"].ToString();
                string subscriptionId = httpRequest.Form["SubscriptionId"].ToString();
                string ipAddress = IPAddressDetector.GetClientIpAddress(HttpContext);

                UsersAPI.Services.Generals.MessageManager messagesGSWeb =
                new UsersAPI.Services.Generals.MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);


                // Check if role Id is allowed to be upload as bulk
                if (!_configuration["ur_roles_can_upload_as_bulk"].Split(',').Contains(roleId))
                {
                    return BadRequest(ReturnGenericMessageBulk());
                }

                UserBulkResponseModel generalResponseModel = new UserBulkResponseModel()
                {
                    Messages = new List<string>(),
                    UserErrors = new List<UserErrorModel>(),
                };
                UsersAPI.Services.Generals.MessageManager messageManager = new UsersAPI.Services.Generals.MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"), langCode);

                string invalidFormatError = messageManager.GetMessageDetails(_configuration["gus_message_code_wrong_format"]);

                // Now save the file. We do not read it directly because it can be a lot of users, and it's worst to mantain all that data in the Memory than the file.
                if (httpRequest.HasFormContentType && httpRequest.Form.Files.Count > 0)
                {
                    // Check if Logged User can do the operation
                    UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = new UsersAPI.Domain.Models.Generals.ValidationResponseModel();
                    sessionResponseModel = ValidateSession("UploadProfilesBulk");
                    generalResponseModel.Messages = new List<string>();
                    generalResponseModel.Messages = sessionResponseModel.Messages;

                    // If the transaction is not valid, stop the operation
                    if (!sessionResponseModel.SessionValid)
                    {
                        return Unauthorized();
                    }


                    // Call the subscription school

                    string schoolId = new SchoolDetailsHandler().GetSchoolIdOfSubscription(subscriptionId);
                    if (string.IsNullOrWhiteSpace(schoolId))
                    {
                        return BadRequest(ReturnGenericMessageBulk());
                    }

                    // Check if user can handle the school 
                    if (!string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                    {
                        if (!ORESchoolUserCheck.CheckIfSchoolInOre(schoolId, sessionResponseModel.OreId))
                        {
                            return Unauthorized();
                        }
                    }

                    // Get the users of the school
                    UsersAPI.Services.PersonalDetails.UserSchoolManager userSchoolManagerReader = new
                        UsersAPI.Services.PersonalDetails.UserSchoolManager(
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                       null);
                    List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchoolModelListResult =
                        userSchoolManagerReader.GetUserSchools(new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>(){
                            new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel() { SchoolId = schoolId }});


                    UsersAPI.Services.PersonalDetails.UserPersonalDetailsManager userPersonalDetailsManagerReader =
                    new UsersAPI.Services.PersonalDetails.UserPersonalDetailsManager(
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);
                    UsersAPI.Services.PersonalDetails.UserPersonalDetailsManager userPersonalDetailsManagerCUD =
                   new UsersAPI.Services.PersonalDetails.UserPersonalDetailsManager(
                       _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                       _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);

                    var docfiles = new List<string>();
                    foreach (var postedFile in httpRequest.Form.Files)
                    {
                        // Check if the file is a CSV file
                        var fileFormat = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1).ToLower();

                        // Is a valid CSV?
                        if (fileFormat != "csv")
                        {
                            // Wrong format
                            return BadRequest(ReturnGenericMessageBulk());
                        }

                        // Call the subscriptions details
                        SubscriptionResponseModel subscriptionResponseModel = SubscriptionDetailsHandler.GetSubscriptionDetails(token, ipAddress, subscriptionId, langCode);

                        // Get the user address
                        UsersAPI.Domain.Models.PersonalDetails.UserAddressModel userAddressModel = RegisterUserManager.GetMainAccountMainAddress(subscriptionResponseModel.MainAccountUserId, langCode);

                        // Now call the profiles
                        UsersAPI.Services.Accounts.UserDetailsManager userDetailsManager = new UserDetailsManager(
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            langCode);

                        /** KIU To check if exists **/
                        List<UsersAPI.Domain.Models.Accounts.UserAccountModel> userAccountCheckIfUsernameExistsListToFind = new List<UsersAPI.Domain.Models.Accounts.UserAccountModel>();
                        List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailCheckIfEmailExistsListToFind = new List<UsersAPI.Domain.Models.Accounts.UserEmailModel>();
                        List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchoolsCheckIfExistsRecordNoToFind = new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>();
                        /** KU End of check if exists **/

                        // Save the file in the upload folder
                        List<string> lines = new List<string>();
                        using (StreamReader csvReader = new StreamReader(postedFile.OpenReadStream()))
                        {
                            while (!csvReader.EndOfStream)
                            {
                                lines.Add(csvReader.ReadLine());
                            }
                        }


                        // var filePath = HttpContext.Current.Server.MapPath(AppConfiguration.Configuration["AppSettings:gus_path_bulk_files"] + string.Format(@"{0}.csv", DateTime.Now.Ticks));
                        // postedFile.SaveAs(filePath);
                        // docfiles.Add(filePath);
                        // var lines = File.ReadAllLines(filePath);

                        generalResponseModel.UserErrors = new List<UserErrorModel>();

                        List<UsersAPI.Domain.Models.Generals.Requests.RegisterRequestModel> registerRequestList = new List<RegisterRequestModel>();
                        UsersAPI.Services.Accounts.CustomClasses.ValidationManager validationManager = new UsersAPI.Services.Accounts.CustomClasses.ValidationManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"), langCode);

                        List<string> usernamesInListToAvoidDuplicates = new List<string>();
                        List<string> emailsInListToAvoidDuplicates = new List<string>();
                        List<string> userIdentifierInRecordToAvoudDuplicates = new List<string>();

                        // Check the lines length
                        if (lines.Count > int.Parse(_configuration["max_bulk_users_processing_allowed"]))
                        {
                            // The line does not have the full requirements. 
                            generalResponseModel.Messages = new List<string>
                            {
                                messagesGSWeb.GetMessageDetails(_configuration["message_file_upload_too_long"])
                            };
                            return BadRequest(generalResponseModel);
                        }

                        if(lines.Count <= 1)
                        {
                            // Empty file
                            generalResponseModel.Messages = new List<string>
                            {
                                messagesGSWeb.GetMessageDetails(_configuration["message_file_empty_file"])
                            };
                            return BadRequest(generalResponseModel);
                        }

                        // Read the lines from the file and do a pre-evaluation and fixing
                        for (var i = 1; i < lines.Count; i++)
                        {
                            var user = lines[i];
                            string[] userArray = user.Split(',');

                            // CSV example: Username = [0], Password = [1], Firstname = [2], Lastname = [3], DoB = [4], Gender = [5], SchoolUserIdentifier = [6], Email = [7]
                            try
                            {
                                if(userArray.Length != 8)
                                {
                                    // The line does not have the full requirements. 
                                    generalResponseModel.Messages = new List<string>
                                    {
                                        messagesGSWeb.GetMessageDetails(_configuration["message_not_valid_columns_users_upload_bulk"]) + " " + i.ToString()
                                    };
                                    return BadRequest(generalResponseModel);
                                }

                                UsersAPI.Domain.Models.Generals.Requests.RegisterRequestModel userRegister = new UsersAPI.Domain.Models.Generals.Requests.RegisterRequestModel()
                                {
                                    Username = userArray[0],
                                    DoB = userArray.Length > 3 ? userArray[4] : null,
                                    Gender = userArray.Length > 4 ? userArray[5] : null,
                                    Password = userArray[1],
                                    PasswordRetype = userArray[1],
                                    FirstName = userArray[2],
                                    LastName = userArray[3],
                                    SchoolUserIdentifier = userArray[6],
                                    Email = userArray.Length > 7 ? userArray[7] : null,
                                };

                                // Fix text
                                if (!string.IsNullOrWhiteSpace(userRegister.Username))
                                {
                                    userRegister.Username = TextFixer.RemoveSpecialCharacters(TextFixer.RemoveDiacritics(TextFixer.ReplaceCSVWeirdCharacters(userRegister.Username)));
                                    // Validate
                                    UsersAPI.Domain.Models.Accounts.ResponseModel usernameValidation = validationManager.ValidateUsername(userRegister.Username, false);
                                    if (!usernameValidation.Valid)
                                    {
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages = usernameValidation.Details;
                                        generalResponseModel.UserErrors.Add(userErrorModel);
                                        return BadRequest(generalResponseModel);
                                    }

                                    if (usernamesInListToAvoidDuplicates.Find(x => x == userRegister.Username) != null)
                                    {
                                        // The username is already in this list. Please, avoid duplicates 
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages.Add(messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_username_duplicated_in_list"]));
                                        generalResponseModel.UserErrors.Add(userErrorModel);
                                        return BadRequest(generalResponseModel);
                                    }
                                    usernamesInListToAvoidDuplicates.Add(userRegister.Username);

                                    userAccountCheckIfUsernameExistsListToFind.Add(new UsersAPI.Domain.Models.Accounts.UserAccountModel()
                                    {
                                        Username = userRegister.Username,
                                    });

                                }
                                else
                                {
                                    // Return error that all usernames are required
                                    UserErrorModel userErrorModel = new UserErrorModel()
                                    {
                                        ErrorDetails = new List<string>(),
                                        Username = userArray?[0],
                                        DoB = userArray.Length > 3 ? userArray[4] : null,
                                        Gender = userArray.Length > 4 ? userArray[5] : null,
                                        Password = userArray?[1],
                                        PasswordRetype = userArray?[1],
                                        FirstName = userArray?[2],
                                        LastName = userArray?[3],
                                        SchoolUserIdentifier = userArray?[6],
                                        Email = userArray?[7],
                                    };
                                    userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                    generalResponseModel.Messages = new List<string>();
                                    generalResponseModel.Messages.Add(messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_all_usernames_required"]));
                                    generalResponseModel.UserErrors.Add(userErrorModel);

                                    return BadRequest(userErrorModel);
                                }

                                if (string.IsNullOrWhiteSpace(userRegister.Password))
                                {
                                    // Return error that all usernames are required
                                    UserErrorModel userErrorModel = new UserErrorModel()
                                    {
                                        ErrorDetails = new List<string>(),
                                        Username = userArray?[0],
                                        DoB = userArray.Length > 3 ? userArray[4] : null,
                                        Gender = userArray.Length > 4 ? userArray[5] : null,
                                        Password = userArray?[1],
                                        PasswordRetype = userArray?[1],
                                        FirstName = userArray?[2],
                                        LastName = userArray?[3],
                                        SchoolUserIdentifier = userArray?[6],
                                        Email = userArray?[7],
                                    };
                                    userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                    generalResponseModel.Messages = new List<string>();
                                    generalResponseModel.Messages.Add(messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_all_passwords_required"]));
                                    generalResponseModel.UserErrors.Add(userErrorModel);

                                    return BadRequest(userErrorModel);
                                }
                                else
                                {
                                    UsersAPI.Domain.Models.Accounts.ResponseModel passwordValidation = validationManager.ValidatePassword(userRegister.Password, userRegister.Password);
                                    if (!passwordValidation.Valid)
                                    {
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages = passwordValidation.Details;
                                        generalResponseModel.UserErrors.Add(userErrorModel);
                                        return BadRequest(generalResponseModel);
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(userRegister.FirstName))
                                {
                                    userRegister.FirstName = TextFixer.RemoveSpecialCharacterExceptWhiteSpaces(TextFixer.RemoveDiacritics(TextFixer.ReplaceCSVWeirdCharacters(userRegister.FirstName)));

                                    UsersAPI.Domain.Models.PersonalDetails.ResponseModel nameValidation = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateName(userRegister.FirstName);
                                    if (!nameValidation.Valid)
                                    {
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages = nameValidation.Details;
                                        generalResponseModel.UserErrors.Add(userErrorModel);

                                        return BadRequest(userErrorModel);
                                    }
                                }
                                else
                                {
                                    // Return error that all First names
                                    UserErrorModel userErrorModel = new UserErrorModel()
                                    {
                                        ErrorDetails = new List<string>(),
                                        Username = userArray?[0],
                                        DoB = userArray.Length > 3 ? userArray[4] : null,
                                        Gender = userArray.Length > 4 ? userArray[5] : null,
                                        Password = userArray?[1],
                                        PasswordRetype = userArray?[1],
                                        FirstName = userArray?[2],
                                        LastName = userArray?[3],
                                        SchoolUserIdentifier = userArray?[6],
                                        Email = userArray?[7],
                                    };
                                    userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                    generalResponseModel.Messages = new List<string>();
                                    generalResponseModel.Messages.Add(messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_all_first_names_required"]));
                                    generalResponseModel.UserErrors.Add(userErrorModel);

                                    return BadRequest(userErrorModel);
                                }

                                if (!string.IsNullOrWhiteSpace(userRegister.LastName))
                                {
                                    userRegister.LastName = TextFixer.RemoveSpecialCharacterExceptWhiteSpaces(TextFixer.RemoveDiacritics(TextFixer.ReplaceCSVWeirdCharacters(userRegister.LastName)));

                                    UsersAPI.Domain.Models.PersonalDetails.ResponseModel nameValidation = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateSurname(userRegister.LastName);
                                    if (!nameValidation.Valid)
                                    {
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages = nameValidation.Details;
                                        generalResponseModel.UserErrors.Add(userErrorModel);

                                        return BadRequest(userErrorModel);
                                    }
                                }
                                else
                                {
                                    // Return error that all Last names are required
                                    UserErrorModel userErrorModel = new UserErrorModel()
                                    {
                                        ErrorDetails = new List<string>(),
                                        Username = userArray?[0],
                                        DoB = userArray.Length > 3 ? userArray[4] : null,
                                        Gender = userArray.Length > 4 ? userArray[5] : null,
                                        Password = userArray?[1],
                                        PasswordRetype = userArray?[1],
                                        FirstName = userArray?[2],
                                        LastName = userArray?[3],
                                        SchoolUserIdentifier = userArray?[6],
                                        Email = userArray?[7],
                                    };
                                    userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                    generalResponseModel.Messages = new List<string>();
                                    generalResponseModel.Messages.Add(messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_all_last_names_required"]));
                                    generalResponseModel.UserErrors.Add(userErrorModel);

                                    return BadRequest(userErrorModel);

                                }

                                if (!string.IsNullOrWhiteSpace(userRegister.SchoolUserIdentifier))
                                {
                                    userRegister.SchoolUserIdentifier = TextFixer.RemoveSpecialCharacters(TextFixer.RemoveDiacritics(TextFixer.ReplaceCSVWeirdCharacters(userRegister.SchoolUserIdentifier)));
                                    userSchoolsCheckIfExistsRecordNoToFind.Add(new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                                    {
                                        Identifier = userRegister.SchoolUserIdentifier,
                                        SchoolId = _configuration["global_school_identifier"] == "0" ? schoolId : null
                                    });

                                    if (userIdentifierInRecordToAvoudDuplicates.Find(x => x == userRegister.SchoolUserIdentifier) != null)
                                    {
                                        // The username is already in this list. Please, avoid duplicates 
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages.Add(messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_school_identifier_duplicated_in_list"]));
                                        generalResponseModel.UserErrors.Add(userErrorModel);
                                        return BadRequest(generalResponseModel);
                                    }
                                    userIdentifierInRecordToAvoudDuplicates.Add(userRegister.SchoolUserIdentifier);

                                }
                                else
                                {
                                    // Return error that all School Identifiers are required
                                    UserErrorModel userErrorModel = new UserErrorModel()
                                    {
                                        ErrorDetails = new List<string>(),
                                        Username = userArray?[0],
                                        DoB = userArray.Length > 3 ? userArray[4] : null,
                                        Gender = userArray.Length > 4 ? userArray[5] : null,
                                        Password = userArray?[1],
                                        PasswordRetype = userArray?[1],
                                        FirstName = userArray?[2],
                                        LastName = userArray?[3],
                                        SchoolUserIdentifier = userArray?[6],
                                        Email = userArray?[7],
                                    };
                                    userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                    generalResponseModel.Messages = new List<string>();
                                    generalResponseModel.Messages.Add(messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_all_school_identifiers_required"]));
                                    generalResponseModel.UserErrors.Add(userErrorModel);

                                    return BadRequest(generalResponseModel);

                                }

                                if (!string.IsNullOrWhiteSpace(userRegister.Gender))
                                {
                                    userRegister.Gender = TextFixer.FixSex(TextFixer.ReplaceCSVWeirdCharacters(TextFixer.RemoveSpecialCharacters(userRegister.Gender)));
                                    UsersAPI.Domain.Models.PersonalDetails.ResponseModel genderValidation = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateGender(userRegister.Gender);
                                    if (!genderValidation.Valid)
                                    {
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages = genderValidation.Details;
                                        generalResponseModel.UserErrors.Add(userErrorModel);

                                        return BadRequest(generalResponseModel);
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(userRegister.DoB))
                                {
                                    userRegister.DoB = userRegister.DoB.Trim();
                                    UsersAPI.Domain.Models.PersonalDetails.ResponseModel dobValidation = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateDOB(userRegister.DoB);
                                    if (!dobValidation.Valid)
                                    {
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages = dobValidation.Details;
                                        generalResponseModel.UserErrors.Add(userErrorModel);

                                        return BadRequest(generalResponseModel);
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(userRegister.Email))
                                {
                                    userRegister.Email = userRegister.Email.Trim();
                                    userEmailCheckIfEmailExistsListToFind.Add(new UsersAPI.Domain.Models.Accounts.UserEmailModel()
                                    {
                                        Email = userRegister.Email,
                                    });

                                    // Validate
                                    UsersAPI.Domain.Models.Accounts.ResponseModel emailValidation = validationManager.ValidateEmail(userRegister.Email, false);
                                    if (!emailValidation.Valid)
                                    {
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages = emailValidation.Details;
                                        generalResponseModel.UserErrors.Add(userErrorModel);
                                        return BadRequest(generalResponseModel);
                                    }

                                    if (emailsInListToAvoidDuplicates.Find(x => x == userRegister.Email) != null)
                                    {
                                        // The username is already in this list. Please, avoid duplicates 
                                        UserErrorModel userErrorModel = new UserErrorModel()
                                        {
                                            ErrorDetails = new List<string>(),
                                            Username = userArray?[0],
                                            DoB = userArray.Length > 3 ? userArray[4] : null,
                                            Gender = userArray.Length > 4 ? userArray[5] : null,
                                            Password = userArray?[1],
                                            PasswordRetype = userArray?[1],
                                            FirstName = userArray?[2],
                                            LastName = userArray?[3],
                                            SchoolUserIdentifier = userArray?[6],
                                            Email = userArray?[7],
                                        };
                                        userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                        generalResponseModel.Messages = new List<string>();
                                        generalResponseModel.Messages.Add(messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_email_duplicated_in_list"]));
                                        generalResponseModel.UserErrors.Add(userErrorModel);
                                        return BadRequest(generalResponseModel);
                                    }
                                    emailsInListToAvoidDuplicates.Add(userRegister.Email);
                                }

                                registerRequestList.Add(userRegister);

                            }
                            catch (Exception)
                            {
                                UserErrorModel userErrorModel = new UserErrorModel()
                                {
                                    ErrorDetails = new List<string>(),
                                    Username = userArray?[0],
                                    DoB = userArray.Length > 3 ? userArray[4] : null,
                                    Gender = userArray.Length > 4 ? userArray[5] : null,
                                    Password = userArray?[1],
                                    PasswordRetype = userArray?[1],
                                    FirstName = userArray?[2],
                                    LastName = userArray?[3],
                                    SchoolUserIdentifier = userArray?[6],
                                    Email = userArray?[7],
                                };
                                userErrorModel.ErrorDetails.Add(invalidFormatError + " " + i);
                                generalResponseModel.UserErrors.Add(userErrorModel);
                                return BadRequest(generalResponseModel);
                            }
                        }

                        // Validate that there is no role changes. 
                        List<UserSchoolRoleModel> userRolesSchoolsToDetectChanges = Users.SearchUserRolesByUserSchoolRecord(userIdentifierInRecordToAvoudDuplicates, _configuration["global_school_identifier"] == "0" ? schoolId : null);
                        // Now, check if the role being processed is different that the user existing role. 
                        if(userRolesSchoolsToDetectChanges.Count > 0)
                        {
                            List<UserSchoolRoleModel> userSchoolRoleModelsThatAreSupposedToChangeRoles = userRolesSchoolsToDetectChanges.FindAll(x => x.RoleId != roleId);
                            if(userSchoolRoleModelsThatAreSupposedToChangeRoles.Count > 0)
                            {
                                string siesWithError = "";
                                for(int i=0; i <  userSchoolRoleModelsThatAreSupposedToChangeRoles.Count; i++)
                                {
                                    if(i == userSchoolRoleModelsThatAreSupposedToChangeRoles.Count -1)
                                    {
                                        siesWithError += userSchoolRoleModelsThatAreSupposedToChangeRoles[i].Identifier + ". ";
                                    }
                                    else
                                    {
                                        siesWithError += userSchoolRoleModelsThatAreSupposedToChangeRoles[i].Identifier + ", ";
                                    }
                                }

                                generalResponseModel.Messages = new List<string>()
                                {
                                    messagesGSWeb.GetMessageDetails(_configuration["message_file_contains_change_of_role"]) + siesWithError,
                                };

                                // Return error.
                                return BadRequest(generalResponseModel);
                            }                          
                        }

                        // Now, call the emails, accounts, and user school identifiers to check if it's the same user or not. 
                        // Or if the user is trying to update an account that alread exists. 
                        List<UsersAPI.Domain.Models.Accounts.UserAccountModel> userAccountCheckIfUsernameExistsListToFindResult = userDetailsManager.GetUserAccountDetails(userAccountCheckIfUsernameExistsListToFind);
                        List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailCheckIfEmailExistsListToFindResult = userDetailsManager.GetUsersEmailByUserEmailList(userEmailCheckIfEmailExistsListToFind);
                        List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchoolsCheckIfExistsRecordNoToFindResult = userSchoolManagerReader.GetUserSchools(userSchoolsCheckIfExistsRecordNoToFind);

                        UsersAPI.Services.Accounts.PasswordAccountManager passwordAccountManager = new UsersAPI.Services.Accounts.PasswordAccountManager(
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            langCode);


                        List<string> accountsToCheckIfProfileSubscribedModelList = new List<string>();
                        var userProfilesToAssociateInAccountList = new List<UsersAPI.Domain.Models.Relations.AssociatedAccountModel>();
                        // To edit
                        List<UsersAPI.Domain.Models.Accounts.UserAccountModel> userAccountModelToUpdate = new List<UsersAPI.Domain.Models.Accounts.UserAccountModel>();
                        List<UsersAPI.Domain.Models.Accounts.UserAccountModel> userAccountModeltToUpdateWhere = new List<UsersAPI.Domain.Models.Accounts.UserAccountModel>();
                        List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailModelToUpdate = new List<UsersAPI.Domain.Models.Accounts.UserEmailModel>();
                        List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailModelToUpdateWhere = new List<UsersAPI.Domain.Models.Accounts.UserEmailModel>();
                        List<UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel> userFullNameToUpdate = new List<UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel>();
                        List<UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel> userFullNameToUpdateWhere = new List<UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel>();
                        List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel> userDobToUpdate = new List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel>();
                        List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel> userDobToUpdateWhere = new List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel>();
                        List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel> userGenderToUpdate = new List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel>();
                        List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel> userGenderToUpdateWhere = new List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel>();

                        // To add 
                        List<RegisterRequestModel> userToRegisterAddList = new List<RegisterRequestModel>();

                        // Evaluate each user to register
                        foreach (RegisterRequestModel userToRegister in registerRequestList)
                        {
                            // Check if the user school exists and it is not the same user 
                            UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel userSchoolCheckIfExists = userSchoolsCheckIfExistsRecordNoToFindResult.Find(x => x.Identifier == userToRegister.SchoolUserIdentifier);
                            UsersAPI.Domain.Models.Accounts.UserAccountModel userAccountModelToCheckIfExists = userAccountCheckIfUsernameExistsListToFindResult.Find(x => x.Username == userToRegister.Username);
                            UsersAPI.Domain.Models.Accounts.UserEmailModel userEmailCheckIfExists = userEmailCheckIfEmailExistsListToFindResult.Find(x => x.Email == userToRegister.Email);

                            // Username taken
                            if (userAccountModelToCheckIfExists != null && userSchoolCheckIfExists == null)
                            {
                                generalResponseModel.UserErrors.Add(new UserErrorModel()
                                {
                                    Username = userToRegister.Username,
                                    DoB = userToRegister.DoB,
                                    Gender = userToRegister.Gender,
                                    Password = userToRegister.Password,
                                    PasswordRetype = userToRegister.Password,
                                    FirstName = userToRegister.FirstName,
                                    LastName = userToRegister.LastName,
                                    SchoolUserIdentifier = userToRegister.SchoolUserIdentifier,
                                    Email = userToRegister.Email,
                                });

                                generalResponseModel.Messages = new List<string>()
                                {
                                    messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_all_username_taken"])
                                };

                                return BadRequest(generalResponseModel);
                            }

                            // Email taken
                            if (userEmailCheckIfExists != null && userSchoolCheckIfExists == null)
                            {
                                generalResponseModel.UserErrors.Add(new UserErrorModel()
                                {
                                    Username = userToRegister.Username,
                                    DoB = userToRegister.DoB,
                                    Gender = userToRegister.Gender,
                                    Password = userToRegister.Password,
                                    PasswordRetype = userToRegister.Password,
                                    FirstName = userToRegister.FirstName,
                                    LastName = userToRegister.LastName,
                                    SchoolUserIdentifier = userToRegister.SchoolUserIdentifier,
                                    Email = userToRegister.Email,
                                });

                                generalResponseModel.Messages = new List<string>()
                                {
                                    messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_all_email_taken"])
                                };

                                return BadRequest(generalResponseModel);
                            }

                            // Verify if it is the same user. 
                            if (userSchoolCheckIfExists != null)
                            {
                                // If the username is different, update it 
                                if (userAccountModelToCheckIfExists != null && userAccountModelToCheckIfExists.Id == userSchoolCheckIfExists.UserId)
                                {
                                    if (userAccountModelToCheckIfExists.Username != userToRegister.Username)
                                    {
                                        userAccountModelToUpdate.Add(new UsersAPI.Domain.Models.Accounts.UserAccountModel()
                                        {
                                            Username = userToRegister.Username,
                                            Password = UsersAPI.Services.Accounts.CustomClasses.PasswordManager.HashedPassword(userToRegister.Password),
                                            ModifiedBy = sessionResponseModel.UserId,
                                        });
                                        userAccountModeltToUpdateWhere.Add(new UsersAPI.Domain.Models.Accounts.UserAccountModel()
                                        {
                                            Id = userAccountModelToCheckIfExists.Id
                                        });
                                    }
                                }

                                // Same with the email 
                                if (userEmailCheckIfExists != null && userEmailCheckIfExists.UserId == userSchoolCheckIfExists.UserId)
                                {
                                    if (userEmailCheckIfExists.Email != userToRegister.Email)
                                    {
                                        userEmailModelToUpdate.Add(new UsersAPI.Domain.Models.Accounts.UserEmailModel()
                                        {
                                            UserId = userSchoolCheckIfExists.UserId,
                                            Email = userToRegister.Email,
                                            ModifiedBy = sessionResponseModel.UserId,
                                        });

                                        userEmailModelToUpdateWhere.Add(new UsersAPI.Domain.Models.Accounts.UserEmailModel()
                                        {
                                            UserId = userEmailCheckIfExists.UserId
                                        });
                                    }
                                }
                                else if (userEmailCheckIfExists != null && userEmailCheckIfExists.UserId != userSchoolCheckIfExists.UserId)
                                {
                                    // Email Taken
                                    generalResponseModel.UserErrors.Add(new UserErrorModel()
                                    {
                                        Username = userToRegister.Username,
                                        DoB = userToRegister.DoB,
                                        Gender = userToRegister.Gender,
                                        Password = userToRegister.Password,
                                        PasswordRetype = userToRegister.Password,
                                        FirstName = userToRegister.FirstName,
                                        LastName = userToRegister.LastName,
                                        SchoolUserIdentifier = userToRegister.SchoolUserIdentifier,
                                        Email = userToRegister.Email,
                                    });

                                    generalResponseModel.Messages = new List<string>()
                                {
                                    messagesGSWeb.GetMessageDetails(_configuration["gus_error_message_bulk_all_email_taken"])
                                };

                                    return BadRequest(generalResponseModel);
                                }
                                else if (userEmailCheckIfExists == null) // The email does not exist and is updated.
                                {
                                    userEmailModelToUpdate.Add(new UsersAPI.Domain.Models.Accounts.UserEmailModel()
                                    {
                                        UserId = userSchoolCheckIfExists.UserId,
                                        Email = userToRegister.Email,
                                        ModifiedBy = sessionResponseModel.UserId,
                                    });

                                    userEmailModelToUpdateWhere.Add(new UsersAPI.Domain.Models.Accounts.UserEmailModel()
                                    {
                                        UserId = userSchoolCheckIfExists.UserId
                                    });

                                }

                                // User fullname to update
                                userFullNameToUpdate.Add(new UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel()
                                {
                                    UserId = userSchoolCheckIfExists.UserId,
                                    Surname = userToRegister.LastName,
                                    GivenName = userToRegister.FirstName,
                                    ModifiedBy = sessionResponseModel.UserId,
                                });
                                userFullNameToUpdateWhere.Add(new UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel()
                                {
                                    UserId = userSchoolCheckIfExists.UserId,
                                });

                                // User DOB if anything
                                if (!string.IsNullOrWhiteSpace(userToRegister.DoB))
                                {
                                    userDobToUpdate.Add(new UsersAPI.Domain.Models.PersonalDetails.UserDobModel()
                                    {
                                        UserId = userSchoolCheckIfExists.UserId,
                                        Dob = userToRegister.DoB,
                                        ModifiedBy = sessionResponseModel.UserId,
                                    });
                                    userDobToUpdateWhere.Add(new UsersAPI.Domain.Models.PersonalDetails.UserDobModel()
                                    {
                                        UserId = userSchoolCheckIfExists.UserId,
                                    });
                                }

                                // User Gender if anything
                                if (!string.IsNullOrWhiteSpace(userToRegister.Gender))
                                {
                                    userGenderToUpdate.Add(new UsersAPI.Domain.Models.PersonalDetails.UserGenderModel()
                                    {
                                        UserId = userSchoolCheckIfExists.UserId,
                                        Gender = userToRegister.Gender,
                                        ModifiedBy = sessionResponseModel.UserId,
                                    });
                                    userGenderToUpdateWhere.Add(new UsersAPI.Domain.Models.PersonalDetails.UserGenderModel()
                                    {
                                        UserId = userSchoolCheckIfExists.UserId,
                                    });
                                }

                                accountsToCheckIfProfileSubscribedModelList.Add(userSchoolCheckIfExists.UserId);
                                userProfilesToAssociateInAccountList.Add(new UsersAPI.Domain.Models.Relations.AssociatedAccountModel()
                                {
                                    MainUserAccount = subscriptionResponseModel.MainAccountUserId,
                                    ProfileUserAccount = userSchoolCheckIfExists.UserId
                                });
                            }
                            else
                            {
                                // Add user to register
                                userToRegisterAddList.Add(userToRegister);
                            }
                        }


                        // Before making the insertions and updates, check if the users fits in subscription limit. 
                        List<string> subscriptionsProfilesIdsToAddList = new List<string>();
                        if (accountsToCheckIfProfileSubscribedModelList.Count > 0)
                        {
                            foreach (string profileId in accountsToCheckIfProfileSubscribedModelList)
                            {
                                SubscriptionProfileModel subscriptionProfile = subscriptionResponseModel.Subscriptions[0].SubscriptionProfiles.Find(x => x.ProfileId == profileId);
                                // If the profile is not attached to the subscription, then it means that we should attached it.
                                if (subscriptionProfile == null)
                                {
                                    subscriptionsProfilesIdsToAddList.Add(profileId);
                                }

                            }
                        }

                        // Now verify the limits
                        if (_configuration["gus_discount_only_students"] == "1" && roleId == _configuration["ur_role_client_child"])
                        {
                            // Check from the existing profiles which are students. 
                            List<UsersAPI.Domain.Models.Roles.UserRoleModel> userRoleModelListToCheckStudents = new List<UsersAPI.Domain.Models.Roles.UserRoleModel>();
                            foreach (SubscriptionProfileModel subscriptionProfileModel in subscriptionResponseModel.Subscriptions[0].SubscriptionProfiles)
                            {
                                userRoleModelListToCheckStudents.Add(new UsersAPI.Domain.Models.Roles.UserRoleModel()
                                {
                                    UserId = subscriptionProfileModel.ProfileId,
                                    RoleId = _configuration["ur_role_client_child"],
                                });
                            }

                            if (userRoleModelListToCheckStudents.Count > 0)
                            {
                                // Now check the user roles
                                UsersAPI.Services.Roles.UserRolesManager userRolesManager = new
                                    UsersAPI.Services.Roles.UserRolesManager(
                                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);
                                List<UsersAPI.Domain.Models.Roles.UserRoleModel> userRolesOfUsersStudentsThatExistsList = userRolesManager.GetUserRolesByRolesList(userRoleModelListToCheckStudents);

                                // Check if the is space, adding the: 
                                // (Users from the subscription profiles that are students) + (New Users) + (Users that exists, but we have to attach to the subscription) > (The profile limit of the subscription)
                                if (userRolesOfUsersStudentsThatExistsList.Count + userToRegisterAddList.Count + subscriptionsProfilesIdsToAddList.Count > int.Parse(subscriptionResponseModel.Subscriptions[0].ProfilesTotalQuantity))
                                {
                                    // Sorry. No space 
                                    generalResponseModel.Messages = new List<string>();
                                    generalResponseModel.Messages.Add(messageManager.GetMessageDetails(_configuration["gus_message_code_add_profile_error"]));
                                    return BadRequest(generalResponseModel);
                                }
                            }
                        }
                        else if (_configuration["gus_discount_only_students"] == "0")
                        {
                            if (subscriptionResponseModel.Subscriptions[0].SubscriptionProfiles.Count + userToRegisterAddList.Count + subscriptionsProfilesIdsToAddList.Count > int.Parse(subscriptionResponseModel.Subscriptions[0].ProfilesTotalQuantity))
                            {
                                // Return the limit has bean reached
                                generalResponseModel.Messages = new List<string>();
                                generalResponseModel.Messages.Add(messageManager.GetMessageDetails(_configuration["gus_message_code_add_profile_error"]));
                                return BadRequest(generalResponseModel);
                            }
                        }

                        // If everything is alright, just make the updates and the insertions 

                        // Lets start with the updates 
                        UserModifyManager userModifyManager = new UserModifyManager(
                           _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                           _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);

                        if (userAccountModelToUpdate.Count > 0)
                        {
                            userModifyManager.UpdateUserAccountListWithoutValidation(userAccountModelToUpdate, userAccountModeltToUpdateWhere);
                        }
                        if (userEmailModelToUpdate.Count > 0)
                        {
                            // Check if exists, if they do not exists, add them, otherwise updated
                            // Get the existings 
                            List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailModelsExisting = userDetailsManager.GetUsersEmailByUserEmailList(userEmailModelToUpdateWhere);
                            List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailModelsToInsertList = new List<UsersAPI.Domain.Models.Accounts.UserEmailModel>();
                            List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailModelToUpdateOfficialList = new List<UsersAPI.Domain.Models.Accounts.UserEmailModel>();
                            List<UsersAPI.Domain.Models.Accounts.UserEmailModel> userEmailModelToUpdateWhereOfficialList = new List<UsersAPI.Domain.Models.Accounts.UserEmailModel>();


                            foreach (UsersAPI.Domain.Models.Accounts.UserEmailModel userEmail in userEmailModelToUpdate)
                            {
                                // If the user exists, put in the where list
                                // Else, put in the insert list. 
                                if (!string.IsNullOrWhiteSpace(userEmail.Email) && userEmail.Email.Trim().Length != 0)
                                {
                                    userEmail.Email = userEmail.Email.Trim();
                                    if (userEmailModelsExisting.Find(x => x.UserId == userEmail.UserId) == null)
                                    {
                                        userEmailModelsToInsertList.Add(new UsersAPI.Domain.Models.Accounts.UserEmailModel()
                                        {
                                            UserId = userEmail.UserId,
                                            Email = userEmail.Email,
                                            ModifiedBy = userEmail.ModifiedBy,
                                        });
                                    }
                                }
                            }

                            if (userEmailModelsToInsertList.Count > 0)
                            {
                                userModifyManager.InsertEmailModelWithoutValidation(userEmailModelsToInsertList);
                            }

                            if (userEmailModelToUpdateOfficialList.Count > 0)
                            {
                                userModifyManager.UpdateUserEmailListWithoutValidation(userEmailModelToUpdateOfficialList, userEmailModelToUpdateWhereOfficialList);
                            }

                        }

                        // Continue with: 
                        // 1. Update the gender, the dob and the full name. 
                        // There full name is always required, so we do not have to insert anything new.
                        // 1.b. The gender are optional, so we have to call the existings one to update, and the others to create. 
                        // 1.c Same process for user Dob 
                        if (userFullNameToUpdate.Count > 0)
                        {
                            userPersonalDetailsManagerCUD.UpdateUserNameListWithoutValidation(userFullNameToUpdate, userFullNameToUpdateWhere);
                        }
                        if (userGenderToUpdate.Count > 0)
                        {
                            // Get the existings 
                            List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel> userGenderModelsExisting = userPersonalDetailsManagerReader.GetUserGenderModelList(userGenderToUpdateWhere);
                            List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel> userGenderModelsToInsertList = new List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel>();
                            List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel> userGenderModelToUpdateOfficialList = new List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel>();
                            List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel> userGenderModelToUpdateWhereOfficialList = new List<UsersAPI.Domain.Models.PersonalDetails.UserGenderModel>();


                            foreach (UsersAPI.Domain.Models.PersonalDetails.UserGenderModel userGender in userGenderToUpdate)
                            {
                                // If the user exists, put in the where list
                                // Else, put in the insert list. 
                                if (userGenderModelsExisting.Find(x => x.UserId == userGender.UserId) == null)
                                {
                                    userGenderModelsToInsertList.Add(new UsersAPI.Domain.Models.PersonalDetails.UserGenderModel()
                                    {
                                        UserId = userGender.UserId,
                                        Gender = userGender.Gender,
                                        ModifiedBy = userGender.ModifiedBy,
                                    });
                                }
                                else
                                {
                                    userGenderModelToUpdateOfficialList.Add(new UsersAPI.Domain.Models.PersonalDetails.UserGenderModel()
                                    {
                                        Gender = userGender.Gender,
                                        ModifiedBy = userGender.ModifiedBy,
                                    });
                                    userGenderModelToUpdateWhereOfficialList.Add(new UsersAPI.Domain.Models.PersonalDetails.UserGenderModel()
                                    {
                                        UserId = userGender.UserId,
                                    });
                                }
                            }


                            // Do the insertions and updates 
                            if (userGenderModelsToInsertList.Count > 0)
                            {
                                userPersonalDetailsManagerCUD.InsertGenderListWithoutValidation(userGenderModelsToInsertList);
                            }

                            if (userGenderModelToUpdateOfficialList.Count > 0)
                            {
                                userPersonalDetailsManagerCUD.UpdateGenderListWithoutValidation(userGenderModelToUpdateOfficialList, userGenderModelToUpdateWhereOfficialList);
                            }

                        }
                        if (userDobToUpdate.Count > 0)
                        {
                            // Get the existings 
                            List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel> userDobModelsExisting = userPersonalDetailsManagerReader.GetUserDobList(userDobToUpdateWhere);
                            List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel> userDobModelsToInsertList = new List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel>();
                            List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel> userDobModelToUpdateOfficialList = new List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel>();
                            List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel> userDobModelToUpdateWhereOfficialList = new List<UsersAPI.Domain.Models.PersonalDetails.UserDobModel>();

                            foreach (UsersAPI.Domain.Models.PersonalDetails.UserDobModel userDob in userDobToUpdate)
                            {
                                try
                                {
                                    DateTime dobConverted = DateTime.Parse(userDob.Dob);
                                    // If the user exists, put in the where list
                                    // Else, put in the insert list. 
                                    if (userDobModelsExisting.Find(x => x.UserId == userDob.UserId) == null)
                                    {
                                        userDobModelsToInsertList.Add(new UsersAPI.Domain.Models.PersonalDetails.UserDobModel()
                                        {
                                            UserId = userDob.UserId,
                                            Dob = dobConverted.ToString("yyyy-MM-dd HH:mm:ss"),
                                            ModifiedBy = userDob.ModifiedBy,
                                        });
                                    }
                                    else
                                    {
                                        userDobModelToUpdateOfficialList.Add(new UsersAPI.Domain.Models.PersonalDetails.UserDobModel()
                                        {
                                            Dob = dobConverted.ToString("yyyy-MM-dd HH:mm:ss"),
                                            ModifiedBy = userDob.ModifiedBy,
                                        });
                                        userDobModelToUpdateWhereOfficialList.Add(new UsersAPI.Domain.Models.PersonalDetails.UserDobModel()
                                        {
                                            UserId = userDob.UserId,
                                        });
                                    }
                                }
                                catch (Exception)
                                {
                                    // Ignore because date bad format
                                }
                            }


                            // Do the insertions and updates 
                            if (userDobModelsToInsertList.Count > 0)
                            {
                                userPersonalDetailsManagerCUD.InsertDobListWithoutValidation(userDobModelsToInsertList);
                            }

                            if (userDobModelToUpdateOfficialList.Count > 0)
                            {
                                userPersonalDetailsManagerCUD.UpdateDobListWithoutValidation(userDobModelToUpdateOfficialList, userDobModelToUpdateWhereOfficialList);
                            }

                        }

                        // 2. Create the new users.
                        List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchoolModelsToRelateList = new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>();

                        // Do the insertion, everything has been validated. This will kill the performance since we need the Ids. 
                        // to be inserted. We could make a stored procedure for this, but we want to maintain the business logic 
                        // in the API. 
                        // You could also limit the users to register entry.
                        foreach (RegisterRequestModel registerRequest in userToRegisterAddList)
                        {

                            RegisterProfileResponseModel registerProfileResponseModel = RegisterUserManager.RegisterUserProfile(
                                registerRequest,
                                sessionResponseModel.UserId,
                                ipAddress,
                                userAddressModel,
                                subscriptionResponseModel,
                                roleId,
                                langCode);

                            if (registerProfileResponseModel.TransactionValid)
                            {
                                userProfilesToAssociateInAccountList.Add(new UsersAPI.Domain.Models.Relations.AssociatedAccountModel()
                                {
                                    MainUserAccount = subscriptionResponseModel.MainAccountUserId,
                                    ProfileUserAccount = registerProfileResponseModel.UserId
                                });
                                subscriptionsProfilesIdsToAddList.Add(registerProfileResponseModel.UserId);
                                userSchoolModelsToRelateList.Add(new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                                {
                                    UserId = registerProfileResponseModel.UserId,
                                    Identifier = registerRequest.SchoolUserIdentifier,
                                    SchoolId = schoolId,
                                    ModifiedBy = sessionResponseModel.UserId,
                                    Confirmed = "1"
                                });
                            }
                            else
                            {
                                // Add as incorrect data
                                generalResponseModel.UserErrors.Add(new UserErrorModel()
                                {
                                    Username = registerRequest.Username,
                                    DoB = registerRequest.DoB,
                                    Gender = registerRequest.Gender,
                                    Password = registerRequest.Password,
                                    PasswordRetype = registerRequest.Password,
                                    FirstName = registerRequest.FirstName,
                                    LastName = registerRequest.LastName,
                                    SchoolUserIdentifier = registerRequest.SchoolUserIdentifier,
                                    ErrorDetails = registerProfileResponseModel.Messages,
                                    Email = registerRequest.Email,
                                });
                            }
                        }

                        // Attach the profiles to the subscription
                        if (subscriptionsProfilesIdsToAddList.Count > 0)
                        {
                            SubscriptionDetailsHandler.AttachProfilesToSubscription(ipAddress, subscriptionsProfilesIdsToAddList, subscriptionId, langCode, token);
                        }

                        // Attach the profiles to the main account of the subscription.
                        if (userProfilesToAssociateInAccountList.Count > 0)
                        {
                            // Relate the account to the main user id 
                            List<string> userIdsToAssociate = new List<string>();

                            foreach (var associatedAccountModel in userProfilesToAssociateInAccountList)
                            {
                                userIdsToAssociate.Add(associatedAccountModel.ProfileUserAccount);
                            }

                            if (userIdsToAssociate.Count > 0)
                            {
                                // This function already evaluates if they exists or not to avoid duplications.
                                new UsersAPI.Services.Relations.RelatedAccountsManager(
                                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                                        langCode).AssociateAccounts(subscriptionResponseModel.MainAccountUserId, userIdsToAssociate, sessionResponseModel.UserId);
                            }
                        }

                        // Attach users to school
                        if (userSchoolModelsToRelateList.Count > 0)
                        {
                            new UserSchoolManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                                _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            null).CreateUserSchools(userSchoolModelsToRelateList);
                        }


                        if (generalResponseModel.UserErrors != null && generalResponseModel.UserErrors.Count > 0)
                        {
                            return BadRequest(generalResponseModel);
                        }

                        generalResponseModel.Messages = new List<string>
                            {
                                // Send successful
                                messageManager.GetMessageDetails(_configuration["gus_message_code_users_bulk_uploaded_successfuly"])
                            };

                        return Created("", generalResponseModel);
                    }

                    if (generalResponseModel.UserErrors != null && generalResponseModel.UserErrors.Count > 0)
                    {
                        return BadRequest(generalResponseModel);
                    }

                    return Created("", generalResponseModel);
                }
                else
                {
                    return BadRequest(ReturnGenericMessageBulk());
                }
            }
            catch (Exception)
            {
                
                return BadRequest(ReturnGenericMessageBulk());
            }
        }

        private string GetLanguage()
        {
            var request = HttpContext.Request;
            return request.Headers["LangCode"].FirstOrDefault() ?? request.Query["LangCode"].FirstOrDefault() ?? "en";
        }
    }
}
