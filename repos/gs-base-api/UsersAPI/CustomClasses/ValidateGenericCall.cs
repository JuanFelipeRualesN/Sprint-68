using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Generals;
using UsersAPI.UsersAPI.Services.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;
using UsersAPI.CustomClasses;


namespace UsersAPI.CustomClasses
{
    /// <summary>
    /// To handle the generic calls (if session valid or not, etc).
    /// </summary>
    public class ValidateGenericCall : ControllerBase
    {

        /// <summary>
        /// Validates the full call.
        /// </summary>
        /// <param name="Request">The HTTP Request.</param>
        /// <param name="permission">Permission name.</param>
        /// <returns>Returns the validation details with the user id, role, etc, if successful. Otherwise will return SessionValid as false.</returns>
        public static ValidationResponseModel ValidateFullCall(HttpContext context, string permission, string ipAddress = null)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = IPAddressDetector.GetClientIpAddress(context);
            }
            // Check if the session token still valid
            /* Get the general values to process the request */
            string token = RequestHandler.GetToken(context);
            if (token == null)
            {
                return new ValidationResponseModel();
            }
            string langCode = RequestHandler.GetLanguage(context);
            /* End of general values to process the request */

            // First, we must check if the user has permission for it
            ValidationResponseModel generalResponseModel = Validate(token, permission, ipAddress, langCode, context);

            return new ValidationResponseModel()
            {
                IpAddress = ipAddress,
                LangCode = langCode,
                SessionValid = generalResponseModel.SessionValid,
                UserId = generalResponseModel.UserId,
                IsStaff = generalResponseModel.IsStaff,
                IsDirector = generalResponseModel.IsDirector,
                Messages = generalResponseModel.Messages,
                Token = token,
                RoleId = generalResponseModel.RoleId, 
                OreId = generalResponseModel.OreId
            };
        }

        /// <summary>
        /// Validate the generics around the classes (if the user can do the operation, if there is any user ID, etc).
        /// </summary>
        /// <param name="token">The token sent in the service.</param>
        /// <param name="controllerName">The controller name to check if the user can do the operation.</param>
        /// <param name="ipAddress">The IP Address.</param>
        /// <param name="langCode">The user's language.</param>
        /// <param name="ctx">The HTTP request's token.</param>
        /// <returns>Returns a general response with transaction details.</returns>
        public static ValidationResponseModel Validate(string token, string controllerName, string ipAddress, string langCode, HttpContext ctx)
        {
            // Validate JWT token. 

            var tokenJwtValidation = new TokenManager(
               secretKey: AppConfiguration.Configuration["AppSettings:SecretKey"],
               issuer: AppConfiguration.Configuration["AppSettings:issuer"], 
               durationValidInHours: int.Parse(AppConfiguration.Configuration["AppSettings:token_duration_in_hours"])).
               ValidateToken(token);

            if(tokenJwtValidation == null || tokenJwtValidation.Identity == null || 
                string.IsNullOrWhiteSpace(tokenJwtValidation.Identity.Name))
            {
                return new ValidationResponseModel()
                {
                    SessionValid = false,
                };
     
            }
            var validationResponse = new ValidationResponseModel();
            /*
            var authenticationCache = new CacheHandler.AuthenticationCache(ctx);
            // Added by POR 2021.10.11 to improve performance 
            try
            {
                validationResponse = authenticationCache.GetUserAccessDetails(token, controllerName, ipAddress);
                if (validationResponse != null)
                {
                    return validationResponse;
                }
            }
            catch (Exception)
            {
                // Ignore
            }
            // End of by POR 2021.10.11 to improve performance  
            */
            // Added by POR 2021.10.13 to improve performance: New functions and validations with dapper instead of Libraries 
            UserCanDoOperationModel userCanDoOperationModel = 
                Authentication.CheckIfUserCanDoOperation(
                token,
                int.Parse(AppConfiguration.Configuration["AppSettings:token_duration_in_hours"]) / 24,
                controllerName,
                AppConfiguration.Configuration["AppSettings:ur_role_director"],
                AppConfiguration.Configuration["AppSettings:ur_roles_allowed_to_employee_app"].Split(','));
            // End of added by POR 2021.10.13 to improve performance. 

            if (!userCanDoOperationModel.UserCanDoOperation)
            {
                return new ValidationResponseModel();
            }

            validationResponse = new ValidationResponseModel()
            {
                IsStaff = userCanDoOperationModel.IsStaff,
                SessionValid = true,
                IpAddress = ipAddress,
                LangCode = langCode,
                Token = token,
                UserId = userCanDoOperationModel.UserId,
                IsDirector = userCanDoOperationModel.IsDirector,
                RoleId = userCanDoOperationModel.RoleId,
                OreId = userCanDoOperationModel.OreId,
                FullName = userCanDoOperationModel.FullName,
                Email = userCanDoOperationModel.Email
            };

            // Added on 2021.10.11 by POR to improve performance.
            // TODO: Return the cache handling.
            try
            {
               // authenticationCache.SetAccessDetails(token, controllerName, ipAddress, validationResponse);
            }
            catch(Exception)
            {
                // Ignore
            }
            // End of 2021.10.11 by POR to improve performance.

            return validationResponse;

        }
    }
}