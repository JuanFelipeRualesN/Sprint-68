using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.Accounts;
using UsersAPI.UsersAPI.Services.FeignAPIs;
using UsersAPI.UsersAPI.Services.PersonalDetails;
using UsersAPI.UsersAPI.Services.Generals;
using UsersAPI.UsersAPI.Services.Relations;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests.APIs;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace UsersAPI.Controllers
{
    [ApiController]
    [Route("usersapi/register")]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Registers a free account as teachers (that want to implement Genial to their school. Not valid for teachers from schools that has Genial Skills Web) and parents.
        /// </summary>
        /// <param name="requestModel">The request with details to register.</param>
        /// <returns>Returns the subscription response model with transaction details.</returns>
        [HttpPost("free")]
        public async Task<IActionResult> RegisterFreeAccount([FromBody] RegisterRequestModel requestModel)
        {
            try
            {
                var generalResponseModel = new UsersAPI.Domain.Models.Generals.Responses.APIs.SubscriptionResponseModel()
                {
                    Messages = new List<string>(),
                };
                string langCode = RequestHandler.GetLanguage(HttpContext);


                // Validate if the school identifier exists for the school.
                if (!string.IsNullOrWhiteSpace(requestModel.SchoolId) && !string.IsNullOrWhiteSpace(requestModel.SchoolUserIdentifier))
                {
                    if (!new UserSchoolManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode).
                        CheckIfIdentifierAvailable(requestModel.SchoolUserIdentifier,
                        _configuration["global_school_identifier"] == "1" ? null : requestModel.SchoolId))
                    {
                        generalResponseModel.Messages = new List<string>(){
                        new MessageManager(
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            langCode).GetMessageDetails(_configuration["gus_user_record_exists"])
                        };
                        return BadRequest(generalResponseModel);
                    }
                }

                string ipAddress = IPAddressDetector.GetClientIpAddress(HttpContext);
                bool validTransaction = true;

                // Set a limit of the roles that can be subscribed for free.
                // This is very important to avoid hackers send a role that they should not.
                // DO NOT modified unless you know what you are doing.
                string[] allowedSubscribersForFreeArray = _configuration["ur_roles_can_subscribe_for_free"].Split(',');
                if (!allowedSubscribersForFreeArray.Contains(requestModel.RoleId))
                {
                    return BadRequest(generalResponseModel);
                }

                List<string> errorCodes = new List<string>();
                bool valid = true;

                // Create the username by email
                string username = requestModel.Email.Substring(0, requestModel.Email.IndexOf('@'));
                DateTime dateTimeStr = DateTime.Now;
                string date = dateTimeStr.ToString("yyMMdds");
                username += date;
                requestModel.Username = username;

                // Validate email
                var response = new UsersAPI.Domain.Models.Accounts.ResponseModel();
                response = new UsersAPI.Services.Accounts.CustomClasses.ValidationManager(
                   _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode).ValidateEmail(requestModel.Email, true);
                if (!response.Valid)
                {
                    valid = false;
                    errorCodes.AddRange(response.ErrorCodes);
                }

                if (errorCodes != null && errorCodes.Count > 0 && !valid)
                {
                    // Get the message of the errors
                    validTransaction = false;
                    generalResponseModel.Messages.AddRange(new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode).GetMessageDetails(errorCodes));
                }

                errorCodes = new List<string>();
                UsersAPI.Domain.Models.PersonalDetails.ResponseModel responseUserDetails = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel();

                // Validate address
                var responseAddressModel = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel();
                var userAddressModel = new UsersAPI.Domain.Models.PersonalDetails.UserAddressModel
                {
                    Country = requestModel.Country,
                    State = requestModel.State,
                    StateIsoCode = requestModel.StateIso,
                    CountryIsoCode2 = requestModel.CountryIso,
                };

                AddressManager addressManager = new AddressManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);
                responseAddressModel = addressManager.ValidateAddress(userAddressModel, false, false, false, false, true);
                if (!responseAddressModel.Valid)
                {
                    validTransaction = false;
                    generalResponseModel.Messages.AddRange(responseAddressModel.Details);
                }

                var messageManagerWS = new UsersAPI.Services.Generals.MessageManager(
                    _configuration["GenialSkillsUsersAPIDBConnection"], langCode);

                // Validate name(we would use only one name.It is sufficient)
                responseUserDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateName(requestModel.FirstName);
                if (!responseUserDetails.Valid)
                {
                    validTransaction = false;
                    errorCodes = responseUserDetails.ErrorCodes;
                    MessageManager messageManagerPersonalDetails =
                        new MessageManager(
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            langCode);
                    generalResponseModel.Messages.AddRange(messageManagerPersonalDetails.GetMessageDetails(responseUserDetails.ErrorCodes));
                }

                responseUserDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateSurname(requestModel.LastName);
                if (!responseUserDetails.Valid)
                {
                    validTransaction = false;
                    errorCodes = responseUserDetails.ErrorCodes;
                    MessageManager messageManagerPersonalDetails =
                        new MessageManager(
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            langCode);
                    generalResponseModel.Messages.AddRange(messageManagerPersonalDetails.GetMessageDetails(responseUserDetails.ErrorCodes));
                }

                // If not valid, return the error
                if (!validTransaction)
                {
                    generalResponseModel.Messages.Add("Error 2");
                    return BadRequest(generalResponseModel);
                }

                // Else: Continue:
                // A. Register User
                // TODO: Refactor this.
                UserModifyManager userModifyManager = new UserModifyManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                    langCode);
                var responseUser =
                    userModifyManager.RegisterUserFullProccessWithoutValidation(
                    requestModel.Username,
                    requestModel.Password,
                    ipAddress,
                    requestModel.Email,
                    requestModel.RoleId,
                    requestModel.FirstName,
                    requestModel.LastName,
                    requestModel.Phone,
                    null,
                    requestModel.AddressLine1,
                    requestModel.AddressLine2,
                    requestModel.City,
                    requestModel.State,
                    requestModel.Country,
                    null,
                    requestModel.CountryIso,
                    null,
                    requestModel.StateIso ?? requestModel.State,
                    null,
                    true);

                if (responseUser.Valid)
                {
                    // Generate the session token
                    LoginAccountResponseModel loginAccountResponseModel =
                    Authentication.LoginProcess(requestModel.Username, requestModel.Password, false, ipAddress, langCode);


                    // If there is a registration request token (for example, it is the parent (no client) who is being registered and an invitation token for another user was sent.
                    if (!string.IsNullOrWhiteSpace(requestModel.RegistrationConnectionRequestToken))
                    {
                        // Connect user
                        var profilesConnectionsRequestsManager = new ProfilesConnectionsRequestsManager(
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"));

                        List<UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailModelList =
                        profilesConnectionsRequestsManager.GetProfileInvitationByEmailModelList(new List<UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel>()
                        {
                            new UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel()
                            {
                                Email = requestModel.Email,
                                Token = requestModel.RegistrationConnectionRequestToken,
                            }
                        });

                        if (profileConnectionInvitationByEmailModelList.Count > 0)
                        {
                            // Connect the user
                            RelatedAccountsManager profilesConnectionManager = new RelatedAccountsManager(
                             _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                             _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                             langCode);

                            var responseConnectUsers = profilesConnectionManager.ConnectUsers(
                               responseUser.UserId,
                               profileConnectionInvitationByEmailModelList[0].UserCodeToConnectWith,
                               _configuration["urel_restricted_connections"] == "1",
                               true,
                                responseUser.UserId
                               );

                            // Once connected, delete the invitation.
                            profilesConnectionsRequestsManager.DeleteProfileConnectionInvitationByEmail(new List<UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel>()
                            {
                                new UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel()
                                {
                                    Email = requestModel.Email,
                                    Token = requestModel.RegistrationConnectionRequestToken,
                                }
                            });
                        }
                    }

                    // Enter school if any.
                    if (!string.IsNullOrWhiteSpace(requestModel.SchoolId))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            new UserSchoolManager(
                                _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                                _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode).RelateUserToSchool(
                                responseUser.UserId, requestModel.SchoolId, false, responseUser.UserId, requestModel.SchoolUserIdentifier);
                        });
                    }

                    return Created("", loginAccountResponseModel);
                }
                else
                {
                    if (!validTransaction)
                    {
                        return BadRequest(generalResponseModel);
                    }

                    return Ok(generalResponseModel);
                }
            }
            catch (Exception ex)
            {
                var generalResponseModel = new GeneralResponseModel
                {
                    Messages = new System.Collections.Generic.List<string>(),
                };
                generalResponseModel.Messages.Add(ex.ToString());

                return BadRequest(generalResponseModel);
            }
        }

        /// <summary>
        /// To register the user by the employee.
        /// For Swagger: Required properties: 
        /// RoleId, OreId (Optional, but required if Admin ORE), 
        /// SchoolId, SchoolUserIdentifier (if sent), 
        /// Username, Password, PasswordRetype, Email, RoleId,
        /// FirstName,vLastName, Phone, DoB (Optional),
        /// AddressLine1, AddressLine2, City, State, Country,
        /// ZipCode, CountryIso, Gender (Optional),
        /// StateIso (Optional)
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="requestModel">The request with all details.</param>
        /// <param name="stripeRequired">True if Stripe is required.</param>
        /// <returns>Returns a HTTP Response. </returns>
        [HttpPost]
        [Route("admin/users/register")]
        public async Task<IActionResult> RegisterUserByAdmin([FromBody] RegisterRequestModel requestModel, bool stripeRequired = false)
        {
            try
            {
                RegisterProfileResponseModel generalResponseModel = new RegisterProfileResponseModel();
                UsersAPI.Domain.Models.Generals.ValidationResponseModel sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "UserRegister");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                // If the user making the request is an Admin ORE, evaluate which roles the user cannot register.
                if(sessionResponseModel.IsStaff && sessionResponseModel.RoleId == _configuration["ur_role_admin_ore"])
                {
                    // Check if user cannot registered the other 
                    string[] nonAllowedRolesForORE = _configuration["ur_roles_not_allowed_to_register_for_ore_admin"].Split(',');
                    if(nonAllowedRolesForORE.Contains(requestModel.RoleId))
                    {
                        generalResponseModel.Messages = new List<string>()
                        {
                            new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            RequestHandler.GetLanguage(HttpContext)).GetMessageDetails(_configuration["ur_roles_this_role_cannot_be_registered"])
                        };
                        // School identifier already exists.
                        return BadRequest(generalResponseModel);
                    }
                }

                // Check if role can be added in this area. 
                string[] allowedRoles = _configuration["ur_roles_that_can_be_register_without_subscriptions"].Split(',');
                if (!allowedRoles.Contains(requestModel.RoleId))
                {
                    generalResponseModel.Messages = new List<string>()
                        {
                            new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            RequestHandler.GetLanguage(HttpContext)).GetMessageDetails(_configuration["gus_user_record_exists"])
                        };
                    // School identifier already exists.
                    return BadRequest(generalResponseModel);
                }

                // Validate if admin is ore and ore id was sent 
                if (requestModel.RoleId == _configuration["ur_role_admin_ore"] && string.IsNullOrWhiteSpace(requestModel.OreId))
                {
                    generalResponseModel.Messages = new List<string>()
                    {
                         new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                           RequestHandler.GetLanguage(HttpContext)).GetMessageDetails(_configuration["upd_message_code_ore_required"])
                    };
                    // ORE is required 
                    return BadRequest(generalResponseModel);
                }


                // Validate school identifier 
                if (!string.IsNullOrWhiteSpace(requestModel.SchoolId) && !string.IsNullOrWhiteSpace(requestModel.SchoolExternalIdentifier))
                {
                    requestModel.SchoolExternalIdentifier = requestModel.SchoolExternalIdentifier.Trim();
                    if (!new UserSchoolManager(
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), RequestHandler.GetLanguage(HttpContext)).
                        CheckIfIdentifierAvailable(requestModel.SchoolExternalIdentifier,
                        _configuration["global_school_identifier"] == "1" ? null : requestModel.SchoolId))
                    {
                        generalResponseModel.Messages = new List<string>()
                        {
                            new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                            RequestHandler.GetLanguage(HttpContext)).GetMessageDetails(_configuration["gus_user_record_exists"])
                        };
                        // School identifier already exists.
                        return BadRequest(generalResponseModel);
                    }
                }


                // Else, continue with the registration process

                // First, we search the country standard name (instead of ISO)

                requestModel.Country = new AddressDetailsHandler().GetCountryStandardName(requestModel.CountryIso);
                if (string.IsNullOrWhiteSpace(requestModel.Country))
                {
                    requestModel.Country = requestModel.CountryIso;
                }

                // Then we continue with the registration
                // TODO: Refactor Register overall
                UsersAPI.Domain.Models.Accounts.ResponseModel responseModel = new UsersAPI.Domain.Models.Accounts.ResponseModel();
                UserModifyManager userModifyManager = new UserModifyManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                    sessionResponseModel.LangCode);

                responseModel = userModifyManager.RegisterUserFullProccess(
                    requestModel.Username,
                    requestModel.Password,
                    requestModel.PasswordRetype,
                    sessionResponseModel.IpAddress,
                    requestModel.Email,
                    requestModel.RoleId,
                    requestModel.FirstName,
                    requestModel.LastName,
                    requestModel.Phone,
                    requestModel.DoB,
                    requestModel.AddressLine1,
                    requestModel.AddressLine2,
                    requestModel.City,
                    requestModel.State,
                    requestModel.Country,
                    requestModel.ZipCode,
                    requestModel.CountryIso,
                    requestModel.Gender,
                    _configuration["upd_parent_dob_required"] == "1",
                    _configuration["upd_parent_gender_required"] == "1",
                    requestModel.StateIso ?? requestModel.State,
                    sessionResponseModel.UserId);

                generalResponseModel.Messages = responseModel.Details;
                generalResponseModel.TransactionValid = responseModel.Valid;

                if (generalResponseModel.TransactionValid)
                {
                    // If the subscriptions is handled by stripe, register the user in Stripe
                    if (requestModel.RoleId == _configuration["ur_role_client"])
                    {
                        // Register the user in Stripe, and set the customer in Subscription API.
                        SubscriptionDetailsHandler.CreateCustomer(sessionResponseModel.IpAddress, sessionResponseModel.LangCode, requestModel.Email, responseModel.UserId, stripeRequired);

                    }
                    generalResponseModel.UserId = responseModel.UserId;

                    if (requestModel.RoleId == _configuration["ur_role_admin_ore"])
                    {
                        // Relate user admin to ore.
                        Users.RelateUserAdminToOreId(generalResponseModel.UserId, requestModel.OreId, sessionResponseModel.UserId);
                    }

                    if (!string.IsNullOrWhiteSpace(requestModel.SchoolId) && !string.IsNullOrWhiteSpace(requestModel.SchoolExternalIdentifier))
                    {
                        // Insert the school and identifier. 
                        new UserSchoolManager(
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                            _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), 
                            RequestHandler.GetLanguage(HttpContext)).
                            RelateUserToSchool(responseModel.UserId, requestModel.SchoolId, true, sessionResponseModel.UserId, requestModel.SchoolExternalIdentifier);
                    }

                    return Created("", generalResponseModel);
                }
                else
                {
                    return BadRequest(generalResponseModel);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Registers the name, last name, and addresses of the user.
        /// </summary>
        /// <param name="requestModel">The request with the password details and token.</param>
        /// <returns>Return the details of transaction.</returns>
        [HttpPost("contact")]
        public async Task<IActionResult> RegisterStepContact([FromBody] RegisterRequestModel requestModel)
        {
            try
            {
                var generalResponseModel = new GeneralResponseModel();
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "ProfileUpdateInformation");

                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

          
                // Do the validations
                var messageManagerPersonalDetails = new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"), sessionResponseModel.LangCode);
                var responsePersonalDetails = new UsersAPI.Domain.Models.PersonalDetails.ResponseModel
                {
                    Valid = true,
                    ErrorCodes = new List<string>(),
                };

                bool validRequest = true;

                // Name details
                List<string> messagesCodesName = new List<string>();
                if (!string.IsNullOrWhiteSpace(requestModel.FirstName))
                {
                    responsePersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateName(requestModel.FirstName);
                    if (!responsePersonalDetails.Valid)
                    {
                        validRequest = false;
                        messagesCodesName.AddRange(responsePersonalDetails.ErrorCodes);
                    }
                }

                if (!string.IsNullOrWhiteSpace(requestModel.LastName))
                {
                    responsePersonalDetails = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateSurname(requestModel.LastName);
                    if (!responsePersonalDetails.Valid)
                    {
                        validRequest = false;
                        messagesCodesName.AddRange(responsePersonalDetails.ErrorCodes);
                    }
                }


                if (!validRequest)
                {
                    generalResponseModel.Messages.AddRange(messageManagerPersonalDetails.GetMessageDetails(messagesCodesName));
                    generalResponseModel.Messages.Add("Error Here");
                    return BadRequest(generalResponseModel);
                }

                var userPersonalDetailsManager =
                    new UserPersonalDetailsManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), sessionResponseModel.LangCode);

                if(!string.IsNullOrWhiteSpace(requestModel.Phone))
                {
                    string digitsOnlyPhone = Regex.Replace(requestModel.Phone, @"\D", ""); // \D matches any non-digit

                    userPersonalDetailsManager.InsertUserPhone(new UserPhoneNumberModel
                    {
                        PhoneNumber = digitsOnlyPhone,
                        UserId = sessionResponseModel.UserId,
                        ModifiedBy = sessionResponseModel.UserId,
                    });
                }

                // Update name
                if(!string.IsNullOrWhiteSpace(requestModel.FirstName) && !string.IsNullOrWhiteSpace(requestModel.LastName))
                {
                    userPersonalDetailsManager.UpdateOrInsertUserNameAndLastnameWithValidation(
                    sessionResponseModel.UserId,
                    requestModel.FirstName,
                    requestModel.LastName,
                    null,
                    sessionResponseModel.UserId);
                }
                


                // Update the Address
                var addressManager = new AddressManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), sessionResponseModel.LangCode);

                var userAddressModel = new UserAddressModel();
                userAddressModel = addressManager.GetUserAddress(new List<UserAddressModel>()
                {
                    new UserAddressModel()
                    {
                        UserId = sessionResponseModel.UserId,
                    },
                })[0];

                try
                {
                    addressManager.UpdateUserAddress(
                        new UserAddressModel()
                        {
                            PersonAddressName = "",
                            City = string.IsNullOrWhiteSpace(requestModel.City) ? " " : requestModel.City,
                            Country = userAddressModel.Country,
                            CountryIsoCode2 = userAddressModel.CountryIsoCode2,
                            State = userAddressModel.State,
                            StateIsoCode = userAddressModel.StateIsoCode,
                            MainAddress = "1",
                            Enable = "1",
                            UserId = sessionResponseModel.UserId,
                        }, false, false, true, false, false);
                }
                catch (Exception ex)
                {
                    // Ignore this.
                    /*if (!generalResponseModel.TransactionValid)
                    {
                        generalResponseModel.Messages.Add("Error 2.");
                        return Request.CreateResponse(
                           HttpStatusCode.BadRequest,
                           generalResponseModel,
                           Configuration.Formatters.JsonFormatter);
                    } */
                };



                return Ok(generalResponseModel);
            }
            catch (Exception ex)
            {
                var generalResponseModel = new GeneralResponseModel()
                {
                    Messages = new List<string>(),
                };
                generalResponseModel.Messages.Add(ex.ToString());
                return BadRequest(generalResponseModel);
            }
        }

        /// <summary>
        /// Registers the password as the second step when the user is subscribing.
        /// </summary>
        /// <param name="requestModel">The request with the password details and token.</param>
        /// <returns>Return the details of transaction.</returns>
        [HttpPost("password")]
        public async Task<IActionResult> RegisterStepPassword([FromBody] RegisterRequestModel requestModel)
        {
            try
            {
                var generalResponseModel = new GeneralResponseModel();
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "PasswordUpdateByUser");

                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }

                var passwordAccountManager = new PasswordAccountManager(
                         _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                        _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"),
                        sessionResponseModel.LangCode);

                var responseUpdate = passwordAccountManager.UpdatePasswordWithoutEntryOld(
                    sessionResponseModel.UserId, requestModel.Password, requestModel.PasswordRetype);

                generalResponseModel.TransactionValid = responseUpdate.Valid;
                generalResponseModel.Messages = responseUpdate.Details;

                if (!responseUpdate.Valid)
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
        /// Version 2 registration, which is a simple way to register, asking only for the needs.
        /// </summary>
        /// <param name="isPromo">1 if it's promo 0 if not.</param>
        /// <param name="requestModel">The request with details.</param>
        /// <returns>Return a HTTP Response with the transaction details.</returns>
        [HttpPost("v2/register/{isPromo}")]
        public async Task<IActionResult> RegisterV2(int isPromo, [FromBody] RegisterRequestModel requestModel)
        {
            try
            {
                var generalResponseModel = new UsersAPI.Domain.Models.Generals.Responses.GeneralResponseModel()
                {
                    Messages = new List<string>()
                };

                string ipAddress = IPAddressDetector.GetClientIpAddress(HttpContext);
                string langCode = RequestHandler.GetLanguage(HttpContext);

                var messageManager = new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);

                // Validate coupon
                if (!string.IsNullOrWhiteSpace(requestModel.CouponPlatformId))
                {
                    // Check if the coupon is valid
                    var validateCouponResponse = SubscriptionDetailsHandler.ValidateCouponByPlatform(requestModel.CouponPlatformId, langCode);
                    if (!validateCouponResponse.CouponValid)
                    {
                        return Ok(validateCouponResponse);
                    }
                }

                bool validTransaction = true;
                var validations = new UsersAPI.Services.Accounts.CustomClasses.ValidationManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);

                List<string> errorCodes = new List<string>();

                // Create the username by email
                string username = requestModel.Email.Substring(0, requestModel.Email.IndexOf('@'));
                DateTime dateTimeStr = DateTime.Now;
                string date = dateTimeStr.ToString("yyMMdds");
                username += date;
                requestModel.Username = username;

                // Validate email
                var response = new UsersAPI.Domain.Models.Accounts.ResponseModel();
                response = validations.ValidateEmail(requestModel.Email, true);
                if (!response.Valid)
                {
                    validTransaction = false;
                    errorCodes.AddRange(response.ErrorCodes);
                    generalResponseModel.Messages.AddRange(new MessageManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode).GetMessageDetails(errorCodes));

                }

                // Temporary password
                requestModel.Password = CustomClasses.TemporaryPasswordGenerator.CreatePassword(6);
                requestModel.PasswordRetype = requestModel.Password;


                // Validate quantity of children
                if (requestModel.SubscriptionsQuantity <= 0)
                {
                    requestModel.SubscriptionsQuantity = 1;
                }

                errorCodes = new List<string>();
   
                var userAddressModel = new UserAddressModel
                {
                    Country = requestModel.Country,
                    State = requestModel.State,
                    StateIsoCode = requestModel.StateIso,
                    CountryIsoCode2 = requestModel.CountryIso,
                };

                var addressManager = new AddressManager(
                   _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                   _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);

                var addressValidation = addressManager.ValidateAddress(userAddressModel, false, false, false, false, true);
                if (!addressValidation.Valid)
                {
                    validTransaction = false;
                    generalResponseModel.Messages.AddRange(addressValidation.Details);
                }

                if (isPromo == 0 && string.IsNullOrWhiteSpace(requestModel.StripeToken))
                {
                    validTransaction = false;

                    // Add the error about that stripe token cannot be empty
                    generalResponseModel.Messages.Add(messageManager.GetMessageDetails(_configuration["gus_message_code_invalid_payment_format"]));
                }

                // Validate name(we would use only one name.It is sufficient)
                var nameValidation = UsersAPI.Services.PersonalDetails.CustomClasses.ValidationManager.ValidateName(requestModel.FirstName); 
              
                if (!nameValidation.Valid)
                {
                    validTransaction = false;
                    errorCodes = nameValidation.ErrorCodes;
                    generalResponseModel.Messages.AddRange(messageManager.GetMessageDetails(nameValidation.ErrorCodes));
                }

                if (string.IsNullOrWhiteSpace(requestModel.LastName))
                {
                    requestModel.LastName = " ";
                }

                // If not valid, return the error
                if (!validTransaction)
                {
                    return BadRequest(generalResponseModel);
                }

                // Else: Continue:
                // A. Register User
                var userModifyManager = new UserModifyManager(
                    _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                   _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);

                var responseUser =
                    userModifyManager.RegisterUserFullProccessWithoutValidation(
                    requestModel.Username,
                    requestModel.Password,
                    ipAddress,
                    requestModel.Email,
                    _configuration["ur_role_client"],
                    requestModel.FirstName,
                    requestModel.LastName,
                    null,
                    null,
                    null,
                    null,
                    null,
                    requestModel.State,
                    requestModel.Country,
                    null,
                    requestModel.CountryIso,
                    null,
                    requestModel.StateIso ?? requestModel.State,
                    null,
                    true);

                // B. Register user in Stripe.
                var subscriptionResponseModel = SubscriptionDetailsHandler.CreateClientAndSubscriptionInStripe(new ClientSubscriptionRegisterRequestModel()
                {
                    UserId = responseUser.UserId,
                    StateIso = requestModel.StateIso,
                    CountryIsoCode = requestModel.CountryIso,
                    StripeToken = requestModel.StripeToken,
                    SubscriptionsQuantity = requestModel.SubscriptionsQuantity,
                    UserSessionId = responseUser.UserId,
                    CouponPlatformId = requestModel.CouponPlatformId,
                    Email = requestModel.Email,
                    PlanId = requestModel.PlanId,
                    TemporaryPassword = requestModel.Password,
                    IsPromo = isPromo,
                    MaxTriesExam = requestModel.MaxTriesExam,
                }, langCode);

                if (subscriptionResponseModel.Subscriptions == null || subscriptionResponseModel.Subscriptions.Count == 0 || string.IsNullOrWhiteSpace(subscriptionResponseModel.Subscriptions?[0].SubscriptionId))
                {

                    // Delete the user details you created
                    if (!string.IsNullOrWhiteSpace(responseUser.UserId))
                    {
                        Users.DeleteUserById(responseUser.UserId);
                    }

                    subscriptionResponseModel.Messages.Add(
                        messageManager.GetMessageDetails(
                        _configuration["gus_message_code_invalid_payment_format"]));
                    subscriptionResponseModel.Messages.Add(
                        JsonConvert.SerializeObject(requestModel));

                    // Return payment failed.
                    return Ok(subscriptionResponseModel);
                }


                // Auto connect the user after the registration
                var loginManager = new LoginManager(_configuration.GetConnectionString("GenialSkillsUsersAPIDBConnection"),
                   _configuration.GetConnectionString("GenialSkillsUsersAPIDBConnectionReader"), langCode);
               var responseLoginModel = new LoginResponseModel();

                // Generate the session token
                var loginAccountResponseModel =
                Authentication.LoginProcess(requestModel.Username, requestModel.Password, false, ipAddress, langCode);
                loginAccountResponseModel.RecentSubscriptionId = subscriptionResponseModel.Subscriptions?[0].SubscriptionId;

                return Created("", loginAccountResponseModel);
            }
            catch (Exception ex)
            {
                var generalResponseModel = new GeneralResponseModel
                {
                    Messages = new System.Collections.Generic.List<string>(),
                };
                generalResponseModel.Messages.Add(ex.ToString());

                return BadRequest(generalResponseModel);
            }
        }
    }
}