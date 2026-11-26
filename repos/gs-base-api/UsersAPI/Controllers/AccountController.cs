using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.Accounts;
using UsersAPI.UsersAPI.Services.Emails;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To manage account details.
    /// </summary>
    [ApiController]
    [Route("usersapi")]
    public class AccountController : ControllerBase 
    {
        /// <summary>
        /// To verify if the email is available. The email should be unique.
        /// </summary>
        /// <param name="email">The email to check if it is available or not.</param>
        /// <returns>Returns a boolean whether is available or not. </returns>
        [HttpGet]
        [Route("accounts/email/availability/")]
        public async Task<IActionResult> CheckEmailAvailability(string email)
        {
            try
            {
                // Check if the email is available
                var result = new EmailValidationManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext)
                ).IsEmailAvailable(email);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To verify if the username is available. The username should be unique.
        /// </summary>
        /// <param name="username">The username to validate if it is available.</param>
        /// <returns>Returns a boolean whether is available or not.</returns>
        [HttpGet]
        [Route("accounts/username/availability/")]
        public async Task<IActionResult> CheckUsernameAvailability(string username)
        {
            try
            {
                // Check if the username is available
                var result = new UsernameValidationManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext)).IsUsernameAvailable(username);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To modify the password by a reset token.
        /// For Swagger: Just send the Password, PasswordRetype and Token in the request model for this endpoint. 
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="passwordResetByRecoveryRequestModel">Password reset recovery details that hsould include the password and password retype.</param>
        /// <returns>Returns a HTTP Response message with transaction details. If the transaction was successful, there user session is sent. </returns>
        [HttpPost]
        [Route("accounts/password/reset/modify")]
        public async Task<IActionResult> InsertNewPassword([FromBody] PasswordResetRequestModel passwordResetByRecoveryRequestModel)
        {
            try
            {
                string langCode = RequestHandler.GetLanguage(HttpContext);
                var responseModel = new LoginAccountResponseModel();

                // The password, password retype, and the token cannot be null or empty
                if (!string.IsNullOrWhiteSpace(passwordResetByRecoveryRequestModel.Password)
                    && !string.IsNullOrWhiteSpace(passwordResetByRecoveryRequestModel.PasswordRetype)
                    && !string.IsNullOrWhiteSpace(passwordResetByRecoveryRequestModel.Token))
                {
                    string ipAddress = IPAddressDetector.GetClientIpAddress(HttpContext);
                    var responseModelManagerLib = new PasswordAccountManager(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                        langCode).UpdatePasswordByRecoveryToken(passwordResetByRecoveryRequestModel.Token, passwordResetByRecoveryRequestModel.Password, passwordResetByRecoveryRequestModel.PasswordRetype);
                    responseModel.Messages = responseModelManagerLib.Details;

                    // If transaction was valid, connect the client
                    if (responseModelManagerLib.Valid)
                    {
                        var emailValidationUserList = new PasswordAccountManager(
                            AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                            AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                            langCode).GetEmailValidationByEmailValidation(new UsersAPI.Domain.Models.Accounts.EmailValidationUserModel()
                        {
                            Token = passwordResetByRecoveryRequestModel.Token,
                        });
                        try
                        {
                            // Make Login 
                            var loginAccountResponseModel = Authentication.LoginProcess(emailValidationUserList[0].Email, passwordResetByRecoveryRequestModel.Password, true, 
                                IPAddressDetector.GetClientIpAddress(HttpContext), RequestHandler.GetLanguage(HttpContext));
                            return Ok(loginAccountResponseModel);
                        }
                        catch (Exception)
                        {
                            // If the login fails, it's okay, since the password was updated already.
                            return Ok();
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                return BadRequest(responseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        ///To make a petition to a password reset. An email is sent with a token, so the user will receive a form to update the password.
        /// For Swagger: Just send the Email in the request model for this endpoint.
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="passwordResetModel">Password reset request model. Send only the email field. None of the other properties are required.</param>
        /// <returns>Returns a HTTPResponseMessage with transaction details.</returns>
        [HttpPost]
        [Route("accounts/password/reset/petition")]
        public async Task<IActionResult> AskPasswordReset([FromBody] PasswordResetRequestModel passwordResetModel)
        {
            try
            {
                var responseModel = new GeneralResponseModel();
                if (passwordResetModel != null && !string.IsNullOrWhiteSpace(passwordResetModel.Email))
                {
                    // Validate email  
                    var responseEmailValidationModel = new EmailValidationManager(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                        RequestHandler.GetLanguage(HttpContext)
                        ).SendResetPasswordEmail(passwordResetModel.Email);
                    responseModel.Messages = responseEmailValidationModel.Details;
                    responseModel.TransactionValid = responseEmailValidationModel.Valid;

                    if (!responseModel.TransactionValid)
                    {
                        return BadRequest(responseModel);
                    }
                    return Ok(responseModel);
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To update a password by account configuration.
        /// For Swagger: Just send the OldPassword, Password, and PasswordRetype in the request model for this endpoint.
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="requestModel">Request model that should include the current password (OldPassword), 
        /// the new password (Password), and password retype (PasswordRetype).</param>
        /// <returns>Returns the transaction details.</returns>
        [HttpPost]
        [Route("accounts/password/modify")]
        public async Task<IActionResult> PasswordUpdateByUser([FromBody] PasswordResetRequestModel requestModel)
        {
            try
            {
                var generalResponseModel = new GeneralResponseModel();
                var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "PasswordUpdateByUser");
                // If the transaction is not valid, stop the operation
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                // If we ware here, we can update the password
                var responseUpdate = new PasswordAccountManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], sessionResponseModel.LangCode)
                    .UpdatePassword(sessionResponseModel.UserId, requestModel.OldPassword, requestModel.Password, requestModel.PasswordRetype);
                // Return the response
                generalResponseModel.TransactionValid = responseUpdate.Valid;
                generalResponseModel.Messages = responseUpdate.Details;
                if (!generalResponseModel.TransactionValid)
                {
                    return BadRequest(generalResponseModel);
                }
                return Ok(generalResponseModel);
            }
            catch (Exception)
            {
                return BadRequest(new GeneralResponseModel());
            }
        }

        /// <summary>
        /// One of the most used endpoints (it is used by the majority of the other APIs). 
        /// To get the user id by token and confirm if the token is valid or not. It sends many account's details such as user's id, 
        /// role, etc.
        /// </summary>
        /// <param name="permissionSessionRequestModel">Permission request with token, permission's name, ip address and lang code details.</param>
        /// <returns>Returns the user Id by token and other additional details if valid.</returns>
        [HttpPost]
        [Route("accounts/get_user_by_token")]
        public async Task<IActionResult> GetUserIdByToken([FromBody] PermissionSessionRequestModel permissionSessionRequestModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(permissionSessionRequestModel.Token)
                    || string.IsNullOrWhiteSpace(permissionSessionRequestModel.PermissionName)
                    || string.IsNullOrWhiteSpace(permissionSessionRequestModel.IpAddress))
                {
                    return BadRequest();
                }
                var sessionResponseModel = ValidateGenericCall.Validate(
                    permissionSessionRequestModel.Token,
                    permissionSessionRequestModel.PermissionName,
                    permissionSessionRequestModel.IpAddress,
                    RequestHandler.GetLanguage(HttpContext),
                    HttpContext);
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                return Ok(sessionResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the user’s email by the token.
        /// For Swagger: Just send the UserId property in the request model for this endpoint.
        /// Since we cannot create an unique example for same model (this is a known issue for Swashbuckle), you may have to modify the model's properties a little bit: 
        /// https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// </summary>
        /// <param name="permissionSessionRequestModel">Permission request with token, permission, and IP Address.</param>
        /// <returns>Returns the user's email by token.</returns>
        [HttpPost]
        [Route("accounts/client/get_user_email")]
        public async Task<IActionResult> GetUserEmailByToken([FromBody] PermissionSessionRequestModel permissionSessionRequestModel)
        {

            var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "UserGenericRead",
                string.IsNullOrWhiteSpace(permissionSessionRequestModel.IpAddress) ?
                IPAddressDetector.GetClientIpAddress(HttpContext) : permissionSessionRequestModel.IpAddress);
            // If the transaction is not valid, stop the operation
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }
            // If is a staff and there is an specific user Id to request, send the email of the requested user id, otherwise send the email of the connected user.
            if (sessionResponseModel.IsStaff && !string.IsNullOrWhiteSpace(permissionSessionRequestModel.UserId))
            {
                var email = new UserDetailsManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], null).GetUserEmail(permissionSessionRequestModel.UserId);
                return Ok(email);
            }
            var userEmail = new UserDetailsManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], null).GetUserEmail(sessionResponseModel.UserId);
            return Ok(userEmail);
        }

        /// <summary>
        /// This is used to test the email sending process from this API.
        /// </summary>
        /// <param name="email">The email to send the message.</param>
        /// <param name="subject">The email’s subject.</param>
        /// <param name="body">The email’s body.</param>
        /// <returns>Return a message transaction details.</returns>
        [HttpPost]
        [Route("email/test/{email}/{subject}/{body}")]
        public async Task<IActionResult> SendEmail(string email, string subject, string body)
        {
            try
            {
                string emailStr = EmailManager.SendEmailSimple(
                            AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                            new List<string> { email },
                            subject,
                            body);
                return Ok(new GeneralResponseModel()
                {
                    Messages = { emailStr }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseModel()
                {
                    Messages = { ex.ToString() }
                });
            }
        }

        /// <summary>
        /// To get the emails of the users’ ids that are sent in the parameter.
        /// </summary>
        /// <param name="userIds">A list of user ids to get the emails.</param>
        /// <returns>Return a HTTP Response with email details if successful.</returns>
        [HttpPost]
        [Route("account/emails")]
        public async Task<IActionResult> GetEmailsByUserList([FromBody] List<string> userIds)
        {
            try
            {
                if (userIds == null || userIds.Count == 0)
                {
                    return BadRequest();
                }
                if (!ValidateGenericCall.ValidateFullCall(HttpContext, "GetEmailsByList").SessionValid)
                {
                    return Unauthorized();
                }
                var emails = Users.SearchUserEmailsByUserIds(userIds);
                return Ok(emails);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}