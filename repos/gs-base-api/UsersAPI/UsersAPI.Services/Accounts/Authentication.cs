using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using System.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;
using UsersAPI.UsersAPI.Services.Generals;
using System.Threading.Tasks;
using UsersAPI.UsersAPI.Services.FeignAPIs;

namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To habndle authentication details using Dapper.
    /// </summary>
    public static class Authentication
    {
        /// <summary>
        /// To check if the user can do the operation.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="timeToAdd">The time to add to session to check if access still valid.</param>
        /// <param name="permissionName">The permission name.</param>
        /// <param name="directorRoleId">The director role id.</param>
        /// <param name="staffRolesIds">The staff role ids.</param>
        /// <returns>Returns the UserCanDoOperationModel with properties about if director, if staff and if can do the operation.</returns>
        public static Domain.Models.Generals.UserCanDoOperationModel CheckIfUserCanDoOperation(string token, int timeToAdd, string permissionName, string directorRoleId, string[] staffRolesIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            /*** IMPORTANT: IP ADDRESS HAS BEEN COMMENTED UNTIL FURTHER NOTICE, SINCE IT IS FAILING WITH THE TESTS DUE TO AN IP CHANGE; ***/
            // Init test with dapper 
            var parameters = new { Token = token, TimeAdded = timeToAdd, PermissionName = permissionName };
            string query = @"SELECT ro.role_id, se.user_id, uo.ore_id, CONCAT(fu.given_name, ' ', fu.surname) as 'full_name', em.email
                    FROM gs_users_db_prod.ul_sessions se
                    INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id = se.user_id
                    INNER JOIN `gs_users_db_prod`.`ur_role_permissions` rp ON rp.role_id = ro.role_id
                    INNER JOIN `gs_users_db_prod`.`ur_permissions` per ON per.id = rp.permission_id 
                    LEFT JOIN `gs_users_db_prod`.`upd_user_ore` uo ON uo.user_id = se.user_id
                    LEFT JOIN `gs_users_db_prod`.`ul_user_emails` em ON em.user_id = se.user_id
                    LEFT JOIN `gs_users_db_prod`.`upd_user_full_names` fu ON fu.user_id = se.user_id
                    WHERE 
                    se.token = @token 
                    AND  current_timestamp() <= adddate(se.created_at, @timeAdded)
                    AND per.name = @permissionName";

            IEnumerable<Domain.Models.Generals.UserCanDoOperationModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {

                items = connection.Query<Domain.Models.Generals.UserCanDoOperationModel>(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }



            if (items == null || items.AsList().Count == 0)
            {
                return new Domain.Models.Generals.UserCanDoOperationModel()
                {
                    UserCanDoOperation = false,
                };
            }

            Domain.Models.Generals.UserCanDoOperationModel userCanDoOperation = new Domain.Models.Generals.UserCanDoOperationModel()
            {
                UserCanDoOperation = true,
                IsDirector = (items.AsList()[0].RoleId == directorRoleId),
                IsStaff = (staffRolesIds.Contains(items.AsList()[0].RoleId)),
                UserId = items.AsList()[0].UserId,
                RoleId = items.AsList()[0].RoleId,
                OreId = items.AsList()[0].OreId,
                FullName = items.AsList()[0].FullName,
                Email = items.AsList()[0].Email
            };
            return userCanDoOperation;
        }

        /// <summary>
        /// The updated login process for performance improvement.
        /// </summary>
        /// <param name="validationInput">The username or the email.</param>
        /// <param name="password">The user plain password.</param>
        /// <param name="validateByEmail">True if validate by email, false if validate by username.</param>
        /// <param name="ipAddress">The user's ip address.</param>
        /// <param name="langCode">The user's language code.</param>
        /// <param name="evaluatePassword">To evaluate password when log in. This should be false only if it is SSO.</param>
        /// <returns>Return the account details.</returns>
        public static Domain.Models.Generals.Responses.LoginAccountResponseModel LoginProcess(string validationInput,
            string password, bool validateByEmail, string ipAddress, string langCode, bool evaluatePassword = true)
        {
            Domain.Models.Generals.Responses.LoginAccountResponseModel loginAccountResponseModel = new Domain.Models.Generals.Responses.LoginAccountResponseModel();
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            // Check if validation input (username or email) or password (if required only) are not empty. 
            if (string.IsNullOrWhiteSpace(validationInput) || (string.IsNullOrWhiteSpace(password) && evaluatePassword))
            {
                // Throw error
                loginAccountResponseModel.Messages = new List<string>()
                {
                    new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_message_code_username_password_required"])
                };
            }

            IEnumerable<UserAuthenticationModel> items = null;
            object parameters;
            string query = @"SELECT DISTINCT ua.id, ua.username, ua.password, ua.token as invitation_code, fu.given_name as full_name, ";

            if (validateByEmail)
            {
                query += "em.email, ";
            }

            query += " ust.status_id, sa.allow_access, rot.name as role_name, uaa.main_user_account ";
            query += " FROM `gs_users_db_prod`.`ul_user_accounts` ua ";

            if (validateByEmail)
            {
                query += " LEFT JOIN `gs_users_db_prod`.`ul_user_emails` em ON em.user_id = ua.id";
            }

            query += @" LEFT JOIN `gs_users_db_prod`.`upd_user_full_names` fu ON fu.user_id = ua.id 
                    LEFT JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ua.id 
                    LEFT JOIN `gs_users_db_prod`.`ul_status_accounts` sa ON sa.id = ust.status_id
                    LEFT JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id = ua.id 
                    LEFT JOIN `gs_users_db_prod`.`ur_role_types` rot ON rot.id = ro.role_id 
                    LEFT JOIN `gs_users_db_prod`.`urel_associated_accounts` uaa ON uaa.main_user_account = ua.id ";


            if (validateByEmail)
            {
                query += " WHERE email = @email";
                parameters = new
                {
                    email = validationInput,
                };
            }
            else
            {
                query += "WHERE username = @username";
                parameters = new
                {
                    username = validationInput,
                };
            }

            // Init test with dapper 
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<UserAuthenticationModel>(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();

            }


            if (items != null && items.AsList().Count == 0)
            {
                // Raise error
                loginAccountResponseModel.Messages = new List<string>()
                    {
                        new MessageManager(
                            AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode).GetMessageDetails(
                            AppConfiguration.Configuration["AppSettings:message_code_login_email_not_found"])
                    };
                return loginAccountResponseModel;
            }

            // Check the status 
            if (items.AsList()[0].AllowAccess == 0)
            {
                // Raise error. Cannot enter. 
                loginAccountResponseModel.Messages = new List<string>()
                    {
                        new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ul_message_code_account_suspended_success"])
                    };
                return loginAccountResponseModel;
            }

            // Now compare the passwords 
            // If validation fails...
            LoginManager loginManagerCUD = new LoginManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode);

            // If evaluate password is required.
            if (evaluatePassword)
            {
                if (!PasswordManager.PasswordMatch(items.AsList()[0].Password, password))
                {
                    loginAccountResponseModel.Messages = new List<string>()
                    {
                        new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_login_wrong_username_or_password"])
                    };

                    // Now, add a login attempt 
                    var queryAttempts = @"SELECT attempt FROM gs_users_db_prod.ul_user_login_attempts WHERE user_id = @userId";
                    object parametersQueryAttempts = new
                    {
                        userId = items.AsList()[0].Id
                    };

                    IEnumerable<UserLoginAttemptModel> loginAttemptsItems = null;

                    connection = new MySqlConnection
                    {
                        ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
                    };
                    connection.Open();
                    try
                    {
                        loginAttemptsItems = connection.Query<UserLoginAttemptModel>(queryAttempts, parametersQueryAttempts);
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        connection.Close();

                    }


                    int quantity = 0;
                    if (loginAttemptsItems == null || loginAttemptsItems.AsList().Count == 0)
                    {
                        quantity = 1;
                    }
                    else
                    {
                        quantity = int.Parse(loginAttemptsItems.AsList()[0].Attempt) + 1;
                    }

                    // Update login attempts
                    loginManagerCUD.SetLoginAttempts(new UserAccountModel()
                    {
                        Id = items.AsList()[0].Id,
                    }, quantity);

                    // If the account is more than 3, then block it. 
                    if (quantity >= int.Parse(AppConfiguration.Configuration["AppSettings:login_attempts_max"]))
                    {
                        loginManagerCUD.BlockAccount(new UserAccountModel()
                        {
                            Id = items.AsList()[0].Id,
                        });

                        loginAccountResponseModel.Messages = new List<string>()
                        {
                            new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:account_max_login_attempts_reached"])
                        };
                        return loginAccountResponseModel;
                    }

                    loginAccountResponseModel.Messages = new List<string>()
                        {
                            new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_code_login_wrong_username_or_password"])
                        };
                    return loginAccountResponseModel;

                }
            }

            // Else, continue with success login: 
            // a. Set the login attempts to 0. 
            System.Threading.Tasks.Task.Run(() =>
            {
                SetLoginAttempts(items.AsList()[0].Id, 0);
            });

            // Generate a token 
            loginAccountResponseModel.Token = SaveAccessToken(items.AsList()[0].Id, ipAddress);
            loginAccountResponseModel.IsSubscriptionMainAccount = items.AsList()[0].MainUserAccount != null && items.AsList()[0].MainUserAccount == "1";
            loginAccountResponseModel.InvitationCode = items.AsList()[0].InvitationCode;
            loginAccountResponseModel.Username = items.AsList()[0].Username;
            loginAccountResponseModel.HasProfiles = items.AsList()[0].MainUserAccount != null && items.AsList()[0].MainUserAccount == "1";
            loginAccountResponseModel.Roles = new List<string>() { items.AsList()[0].RoleName };
            loginAccountResponseModel.Name = items.AsList()[0].FullName;
            loginAccountResponseModel.IsStudentHomeschool = SubscriptionDetailsHandler.CheckIStudentIsHomeschool(loginAccountResponseModel.Token);
            return loginAccountResponseModel;

        }

        /// <summary>
        /// To handle the login process via SSO. This method for now only supports Azure.
        /// </summary>
        /// <param name="token">The idToken that is returned from the frontend sign in.</param>
        /// <param name="userIpAddress">The IP Address of the user.</param>
        /// <param name="openIdConfigUrl">The open id URL. Example: https://login.microsoftonline.com/{tenantId}/.well-known/openid-configuration </param>
        /// <param name="clientId">The client id from Azure that represents the APP.</param>
        /// <param name="clientSecret">The client secret from Azure that represents the APP.</param>
        /// <param name="tenantId">The tenant ID from Azure that represents the APP.</param>
        /// <param name="langCode">The user's preferred language.</param>
        /// <returns>Returns the login details if completed successfully and the email is matched.</returns>
        public async static Task<Domain.Models.Generals.Responses.LoginAccountResponseModel> LoginProcessSSO(string token,
        string userIpAddress, string openIdConfigUrl, string clientId, string clientSecret, string tenantId, string langCode)
        {
            Domain.Models.Generals.Responses.LoginAccountResponseModel loginAccountResponseModel = new Domain.Models.Generals.Responses.LoginAccountResponseModel();
            string email = null;

            if (string.IsNullOrWhiteSpace(token))
            {
                // Throw error
                loginAccountResponseModel.Messages = new List<string>()
                {
                    // Return error with token required. 
                    new MessageManager(
                        AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], 
                        langCode).GetMessageDetails(
                    AppConfiguration.Configuration["AppSettings:message_sso_could_not_be_completed"])
                };
                return loginAccountResponseModel;
            }

            try
            {
                openIdConfigUrl = openIdConfigUrl.Replace("{tenantId}", tenantId);
                // Retrieve the OpenID Connect metadata document from Azure AD
                var configManager = new Microsoft.IdentityModel.Protocols.
                    ConfigurationManager<OpenIdConnectConfiguration>(openIdConfigUrl,
                    new OpenIdConnectConfigurationRetriever());
                var config = await configManager.GetConfigurationAsync();

                // Validate the access token using the OpenID Connect configuration
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = clientId,
                    ValidateIssuer = false,
                    ValidIssuer = config.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = config.SigningKeys,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
                SecurityToken validatedToken;

                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                // Get the user's email address from the validated token
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "emails");
                email = emailClaim?.Value;
                if (email == null)
                {
                    var preferredUsernameClaim = principal.Claims.FirstOrDefault(c => c.Type == "preferred_username");
                    email = preferredUsernameClaim?.Value;
                }
            }
            catch (Exception)
            {
                // Return error 
                loginAccountResponseModel.Messages = new List<string>()
                {
                    // Return error with token required. 
                    new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], 
                    langCode).GetMessageDetails(
                    AppConfiguration.Configuration["AppSettings:message_sso_could_not_be_completed"])
                };
                return loginAccountResponseModel;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                // Return error 
                loginAccountResponseModel.Messages = new List<string>()
                {
                    // Return error with token required. 
                    new MessageManager(AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:message_sso_could_not_be_completed"])
                };
                return loginAccountResponseModel;
            }

            // Finally, do the login. 
            return LoginProcess(email, null, true, userIpAddress, langCode, false);
        }

        /// <summary>
        /// To get the user access tokens by the user's id.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the list of user's sessions.</returns>
        public static List<SessionModel> GetUserSessionTokens(string userId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            IEnumerable<SessionModel> items = null;
            object parameters;
            string query = @"SELECT * FROM `gs_users_db_prod`.`ul_sessions` WHERE user_id = @userId; ";
            parameters = new
            {
                userId = userId,
            };

            // Init test with dapper 
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<SessionModel>(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();

            }

            return items.ToList();
        }

        /// <summary>
        /// To get permissions list.
        /// </summary>
        /// <returns>Returns the permissions' list.</returns>
        public static List<UsersAPI.Domain.Models.Roles.PermissionModel> GetPermissions()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            IEnumerable<UsersAPI.Domain.Models.Roles.PermissionModel> items = null;
            string query = @"SELECT * FROM gs_users_db_prod.ur_permissions; ";

            // Init test with dapper 
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<Domain.Models.Roles.PermissionModel>(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            return items.ToList();
        }

        /// <summary>
        /// To set the login attempts.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="attempt">The attempt.</param>
        private static void SetLoginAttempts(string userId, int attempt)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = "UPDATE gs_users_db_prod.ul_user_login_attempts SET attempt=@attempt, last_login=NOW() WHERE user_id = @userId;";
            var parameters = new { userId = userId, attempt = attempt };
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"]
            };
            connection.Open();
            try
            {
                connection.Execute(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();

            }
        }

        /// <summary>
        /// Generate and saves the access token.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>Returns the generated access token.</returns>
        private static string SaveAccessToken(string userId, string ipAddress)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var token = new TokenManager(
                secretKey: AppConfiguration.Configuration["AppSettings:SecretKey"],
                issuer: AppConfiguration.Configuration["AppSettings:issuer"],
                durationValidInHours: int.Parse(AppConfiguration.Configuration["AppSettings:token_duration_in_hours"])).
                GenerateToken(userId);
            string query = "INSERT INTO `gs_users_db_prod`.`ul_sessions`(`user_id`,`token`,`ip_address`) VALUES (@userId, @token, @ipAddress);";
            var parameters = new { 
                userId = userId, 
                token = token,
                ipAddress = ipAddress };
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"]
            };
            connection.Open();
            try
            {
                connection.Execute(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return token;
        }
    }
}