using System.Text.RegularExpressions;
using UsersAPI.Utils;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Services.FeignAPIs;
using UsersAPI.UsersAPI.Services.PersonalDetails;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler
{
    /// <summary>
    /// To handle the users.
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// To search the users emails only. Note, there are just some filters that can be applied. Check the code for more info.
        /// </summary>
        /// <param name="accountFilter">The filters.</param>
        /// <returns>Returns the emails with the user id and email.</returns>
        public static List<Models.UserEmailModel> SearchUsersEmails(Models.Filters.AccountAdvance accountFilter)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            // Prepare the query
            Tuple<string, DynamicParameters> queryAndParams = PrepareSelectForUsersManagedEmails(accountFilter);
            parameters.AddDynamicParams(queryAndParams.Item2);
            IEnumerable<Models.UserEmailModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<Models.UserEmailModel>(queryAndParams.Item1 + ";", parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items.ToList();
        }

        /// <summary>
        /// To delete the user by id. It deletes all the information related to the user, including the user account.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        public static void DeleteUserById(string userId)
        {
            using (var connection = new MySqlConnection(
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"]))
            {
                connection.Open();

                string query = @"
            DELETE FROM gs_users_db_prod.user_main_groups WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.urel_profile_connections WHERE user_id_a = @UserId;
            DELETE FROM gs_users_db_prod.urel_profile_connections WHERE user_id_b = @UserId;
            DELETE FROM gs_users_db_prod.urel_profile_connection_requests WHERE user_requester_id = @UserId;
            DELETE FROM gs_users_db_prod.urel_profile_connection_requests WHERE user_requested_id = @UserId;
            DELETE FROM gs_users_db_prod.urel_profile_connection_invitation_by_emails 
                WHERE user_code_to_connect_with = @UserId;
            DELETE FROM gs_users_db_prod.urel_associated_accounts WHERE main_user_account = @UserId;
            DELETE FROM gs_users_db_prod.urel_associated_accounts WHERE profile_user_account = @UserId;
            DELETE FROM gs_users_db_prod.ur_user_roles WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.upd_user_schools WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.upd_user_phone_numbers WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.upd_user_ore WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.upd_user_genders WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.upd_user_full_names WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.upd_user_dobs WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.upd_user_addresses WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.ul_user_login_attempts WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.ul_user_emails WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.ul_user_account_statuses WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.ul_sessions WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.ul_ip_address_users WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.ul_email_validation_users WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.gus_activity_date_reports WHERE user_id = @UserId;
            DELETE FROM gs_users_db_prod.ul_user_accounts WHERE id = @UserId;
        ";

                try
                {
                    connection.Execute(query, new { UserId = userId });
                }
                catch (Exception ex)
                {
                    // Log or rethrow based on your error handling policy
                    Console.Error.WriteLine("Error while deleting user: " + ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// To search for users applying many filters. This should be used for staff only, however, it is ready to be used for other type of roles. 
        /// It retrieves the users that can be handled only. In the case of staff, they can handle every user. If principal (director), can handle every user of the school, 
        /// and for other type of roles, it is returned only the users that can be managed by them.
        /// </summary>
        /// <param name="accountFilter">A class with many properties to filter. See AccountAdvance for more details.</param>
        /// <param name="paginationFilters">To apply pagination.</param>
        /// <returns>Returns the user advance response model with page details and users information.</returns>
        public static Models.UserAdvanceResponseModel SearchUsers(Models.Filters.AccountAdvance accountFilter, Models.Filters.Pagination paginationFilters)
        {
            Models.UserAdvanceResponseModel userAdvanceResponseModel = new Models.UserAdvanceResponseModel()
            {
                UserInfo = new List<Models.UserAdvance>(),
            };

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            if (paginationFilters != null && paginationFilters.LimitRows == 0) // To avoid divide by zero
                paginationFilters.LimitRows = 500;

            // Prepare the query
            Tuple<string, DynamicParameters> whereTuple = PrepareWhereUsersSearch(accountFilter);
            string query = PrepareSelectForUsersManaged(accountFilter) + " " + whereTuple.Item1 + " ORDER BY na.given_name, na.surname";
            parameters.AddDynamicParams(whereTuple.Item2);
            IEnumerable<Models.UserAdvance> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<Models.UserAdvance>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            int totalItems = items != null ? items.Count() : 0;
            if (items != null && totalItems > 0)
            {
                userAdvanceResponseModel = new Models.UserAdvanceResponseModel()
                {
                    PageTotals = paginationFilters == null ? 1 : int.Parse(Math.Ceiling(decimal.Parse(totalItems.ToString()) / decimal.Parse(paginationFilters.LimitRows.ToString())).ToString()),
                    UserInfo = paginationFilters != null && paginationFilters.ApplyPagination ? items.Skip(paginationFilters.FromPage * paginationFilters.LimitRows).Take(paginationFilters.LimitRows > totalItems ? totalItems : paginationFilters.LimitRows).ToList() : items.ToList()
                };
            }

            return userAdvanceResponseModel;
        }

        /// <summary>
        /// To get the user full names.
        /// </summary>
        /// <param name="userIds">The users' ids to call.</param>
        /// <param name="userRequesterId">Requester id (required for same school validation).</param>
        /// <param name="isClientAccount">To get the client's account.</param>
        /// <param name="isPrincipal">If the user making the request is a principal, then the users from their school can be called.</param>
        /// <param name="isStudent">If it is a student, the users from the school and personal's connections.</param>
        /// <param name="isTeacher">If is a teacher, the users from the school and the personal's connections.</param>
        /// <param name="isParentAccount">If it is a parent, the personal connections only.</param>
        /// <param name="schoolIds">School's ids.</param>
        /// <returns>Returns the full name list of the users that has been asked.</returns>
        public static List<UserFullNameModel> GetUserFullNames(List<string> userIds, string userRequesterId,
            bool isTeacher, bool isStudent, bool isPrincipal, bool isClientAccount,
            bool isParentAccount, List<string> schoolIds)
        {

            string query = @"
             SELECT DISTINCT
                na.user_id,
                na.given_name, 
                na.surname
                FROM `gs_users_db_prod`.`upd_user_full_names` na 
                INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  na.user_id    
                ";

            query += " LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = na.user_id ";
            // For example: Parent to student; student to student; Teacher to student; They have to be related.
            query += " LEFT JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON pro.user_id_b = na.user_id ";
            query += " LEFT JOIN `gs_users_db_prod`.`urel_associated_accounts` aa ON aa.profile_user_account = na.user_id ";


            // Now prepare WHERE 
            bool whereUsed = false;
            DynamicParameters parameters = new DynamicParameters();
            if (userIds != null && userIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.user_id IN (";
                for (int i = 0; i < userIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), userIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < userIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (schoolIds != null && schoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id IN (";
                for (int i = 0; i < schoolIds.Count; i++)
                {
                    query += string.Concat(" @schoolId", i.ToString());
                    parameters.Add("@schoolId" + i.ToString(), schoolIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < schoolIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            // If client account
            if (isClientAccount)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ((aa.main_user_account = @userAlphaId OR aa.profile_user_account = @userAlphaId) OR (pro.user_id_a = @userAlphaId OR pro.user_id_b = @userAlphaId)) ";
                whereUsed = true;
            }

            if (isParentAccount)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " (pro.user_id_a = @userAlphaId OR pro.user_id_b = @userAlphaId) ";
            }

            if (isParentAccount || isClientAccount)
            {
                parameters.Add("@userAlphaId", userRequesterId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            if (isTeacher || isStudent || isPrincipal)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += @" (us.school_id IN (
                 SELECT school_id
                 FROM `gs_users_db_prod`.`upd_user_schools`
                 WHERE user_id = @userRequesterId
                )";

                parameters.Add("@userRequesterId", userRequesterId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

                if (isTeacher || isStudent)
                {
                    // Or get the user connections
                    query += " OR (pro.user_id_a = @userTeacherOrStudentId OR pro.user_id_b = @userTeacherOrStudentId)";
                    parameters.Add("@userTeacherOrStudentId", userRequesterId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                }

                query += ")";

            }


            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();

            var userFullNames = new List<UserFullNameModel>();
            try
            {
                userFullNames = connection.Query<UserFullNameModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return userFullNames;

        }

        /// <summary>
        /// To search the users that are related to another user.
        /// </summary>
        /// <param name="accountFilter">A class with many properties to filter. See AccountGeneral for more details.</param>
        /// <param name="paginationFilters">To apply pagination.</param>
        /// <returns>Returns the user response model with user general info (not so detailed as useradvance) and pagination details.</returns>
        public static Models.UserResponseModel SearchRelations(
           Models.Filters.AccountGeneral accountFilter,
           Models.Filters.Pagination paginationFilters,
           bool isForOtherUser = false,
           string userIdToMakeQuery = null)
        {
            Models.UserResponseModel userResponseModel = new Models.UserResponseModel()
            {
                UserInfo = new List<Models.UserResponse>(),
            };

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            if (paginationFilters != null && paginationFilters.LimitRows == 0) // To avoid divide by zero
                paginationFilters.LimitRows = 500;

            IEnumerable<Models.UserResponse> items = null;
            // Prepare the query
            Tuple<string, DynamicParameters> selectTuple =
                PrepareQueryForUsersRelations(accountFilter, isForOtherUser, userIdToMakeQuery);

            // If empty, return empty
            if (string.IsNullOrWhiteSpace(selectTuple.Item1))
            {
                return userResponseModel;
            }

            parameters.AddDynamicParams(selectTuple.Item2);
            var connection = new MySqlConnection
            {
                ConnectionString =AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();


            try
            {
                items = connection.Query<Models.UserResponse>(selectTuple.Item1, parameters);
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }


            int totalItems = items != null ? items.Count() : 0;
            if (items != null && totalItems > 0)
            {
                userResponseModel = new Models.UserResponseModel()
                {
                    PageTotals = int.Parse(Math.Ceiling(decimal.Parse(totalItems.ToString()) /
                                                        decimal.Parse(paginationFilters.LimitRows.ToString()))
                        .ToString()),
                    UserInfo = paginationFilters != null && paginationFilters.ApplyPagination
                        ? items.Skip(paginationFilters.FromPage * paginationFilters.LimitRows)
                            .Take(paginationFilters.LimitRows > totalItems ? totalItems : paginationFilters.LimitRows)
                            .ToList()
                        : items.ToList()
                };
            }

            return userResponseModel;
        }

        /// <summary>
        /// To search for friend requests.
        /// </summary>
        /// <param name="accountFilter">A class with many properties to filter. See AccountGeneral for more details.</param>
        /// <param name="paginationFilters">To apply pagination.</param>
        /// <returns>Returns the information of the people that have sent an invitation to users.</returns>
        public static Models.UserResponseModel SearchRequests(Models.Filters.AccountGeneral accountFilter, Models.Filters.Pagination paginationFilters)
        {
            var userResponse = new Models.UserResponseModel()
            {
                UserInfo = new List<Models.UserResponse>(),
            };

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            if (paginationFilters != null && paginationFilters.LimitRows == 0) // To avoid divide by zero
                paginationFilters.LimitRows = 500;

            IEnumerable<Models.UserResponse> items = null;
            // Prepare the query
            string selectTuple = PrepareSelectFriendRequests();
            Tuple<string, DynamicParameters> whereTuple = PrepareWhereForFriendRequests(accountFilter);
            parameters.AddDynamicParams(whereTuple.Item2);

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<Models.UserResponse>(selectTuple + " " + whereTuple.Item1, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            int totalItems = items != null ? items.Count() : 0;
            if (items != null && totalItems > 0)
            {
                userResponse =
                new Models.UserResponseModel()
                {
                    PageTotals = int.Parse(Math.Ceiling(decimal.Parse(totalItems.ToString()) / decimal.Parse(paginationFilters.LimitRows.ToString())).ToString()),
                    UserInfo = paginationFilters != null && paginationFilters.ApplyPagination ? items.Skip(paginationFilters.FromPage * paginationFilters.LimitRows).Take(paginationFilters.LimitRows > totalItems ? totalItems : paginationFilters.LimitRows).ToList() : items.ToList()
                };
            }

            return userResponse;
        }

        /// <summary>
        /// To search for the users that are related to another user (just the models for query simplification)
        /// </summary>
        /// <param name="accountFilter">A class with many properties to filter. See AccountGeneral for more details.</param>
        /// <returns>Returns the profile connection model list.</returns>
        public static List<Relations.ProfileConnectionModel> SearchRelationsModels(Models.Filters.AccountGeneral accountFilter)
        {
            var profileConnectionModels = new List<Relations.ProfileConnectionModel>();
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            // Prepare the query
            Tuple<string, DynamicParameters> selectTuple = PrepareSelectAndWhereForUsersRelationsProfileConnectionModels(accountFilter);
            parameters.AddDynamicParams(selectTuple.Item2);

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                profileConnectionModels = connection.Query<Relations.ProfileConnectionModel>(selectTuple.Item1, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return profileConnectionModels;
        }

        /// <summary>
        /// To search the user roles by user school records.
        /// </summary>
        /// <param name="userSchoolRecords">User school records list (SIEs).</param>
        /// <param name="schoolId">Optional: School Id.</param>
        /// <returns>Returns the user school role model list.</returns>
        public static List<DapperHandler.Models.UserSchoolRoleModel> SearchUserRolesByUserSchoolRecord(List<string> userSchoolRecords, string schoolId = null)
        {
            List<DapperHandler.Models.UserSchoolRoleModel> userSchoolRecordsResults = new List<Models.UserSchoolRoleModel>();
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            DynamicParameters parameters = new DynamicParameters();

            // Prepare query
            string query = @"SELECT us.identifier, us.school_id, ro.role_id, us.user_id 
            FROM gs_users_db_prod.upd_user_schools us 
            INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id = us.user_id ";

            bool useAnd = false;
            if (userSchoolRecords.Count > 0)
            {
                query += " WHERE us.identifier IN ( ";
                for (int i = 0; i < userSchoolRecords.Count; i++)
                {
                    query += string.Concat(" @userSchoolRecordToFilter", i.ToString());
                    parameters.Add("@userSchoolRecordToFilter" + i.ToString(), userSchoolRecords[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < userSchoolRecords.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
                useAnd = true;
            }

            if(!string.IsNullOrWhiteSpace(schoolId))
            {
                if(useAnd)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }

                query += " us.school_id = @schoolId";
                parameters.Add("@schoolId", schoolId);
            }

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                userSchoolRecordsResults = connection.Query<DapperHandler.Models.UserSchoolRoleModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return userSchoolRecordsResults;
        }

        /// <summary>
        /// To get the user email model of the user ids requested.
        /// </summary>
        /// <param name="userIds">The users' ids to retrieve the users' emails.</param>
        /// <returns>Return a list of users' emails models.</returns>
        public static List<Accounts.UserEmailModel> SearchUserEmailsByUserIds(List<string> userIds)
        {
            List<Accounts.UserEmailModel> userEmailModels = new List<Accounts.UserEmailModel>();

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            // Prepare query
            string query = "SELECT * FROM gs_users_db_prod.ul_user_emails WHERE user_id IN (";
            for (int i = 0; i < userIds.Count; i++)
            {
                query += string.Concat(" @userIdToFilter", i.ToString());
                parameters.Add("@userIdToFilter" + i.ToString(), userIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                if (i < userIds.Count - 1)
                {
                    query += ",";
                }
            }
            query += ") ";

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                userEmailModels = connection.Query<Accounts.UserEmailModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return userEmailModels;
        }

        /// <summary>
        /// To get the user school models by user Id.
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <returns>Returns a list of User Schools.</returns>
        public static List<PersonalDetails.UserSchoolModel> SearchUserSchools(string userId)
        {
            List<PersonalDetails.UserSchoolModel> userSchools = new List<PersonalDetails.UserSchoolModel>();

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = "SELECT * FROM gs_users_db_prod.upd_user_schools WHERE user_id = @userId";
            var parameters = new { userId };
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                userSchools = connection.Query<PersonalDetails.UserSchoolModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return userSchools;
        }

        /// <summary>
        /// TO get the user full names of the users in the list sent in as argument. It has to be in the same school of the user in session.
        /// </summary>
        /// <param name="userInSessionId">The user that is making the call.</param>
        /// <param name="usersIdsNamesToFindList">The list of user ids to find the name.</param>
        /// <returns>Return a list of user full names.</returns>
        public static List<PersonalDetails.UserFullNameModel> SearchNamesOfUsersInListOfSameSchoolAsUserSession(string userInSessionId, List<string> usersIdsNamesToFindList)
        {
            List<PersonalDetails.UserFullNameModel> userFullNames = new List<PersonalDetails.UserFullNameModel>();

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            // Prepare query
            string query = @"SELECT fu.user_id, fu.given_name, fu.surname, fu.suffix 
                FROM gs_users_db_prod.upd_user_schools us
                INNER JOIN gs_users_db_prod.upd_user_full_names fu ON fu.user_id = us.user_id
                WHERE us.user_id IN(";

            for (int i = 0; i < usersIdsNamesToFindList.Count; i++)
            {
                query += string.Concat(" @userIdToFilter", i.ToString());
                parameters.Add("@userIdToFilter" + i.ToString(), usersIdsNamesToFindList[i], System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                if (i < usersIdsNamesToFindList.Count - 1)
                {
                    query += ",";
                }
            }
            query += ") AND us.school_id = (SELECT school_id FROM  gs_users_db_prod.upd_user_schools WHERE user_id = @userAlphaId)";
            parameters.Add("@userAlphaId", userInSessionId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                userFullNames = connection.Query<PersonalDetails.UserFullNameModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return userFullNames;
        }

        /// <summary>
        /// To find the user schools by user school model list.
        /// </summary>
        /// <param name="userSchoolModelListToFind">The list of the user school models to find.</param>
        /// <returns>Returns the user school model list.</returns>
        public static List<PersonalDetails.UserSchoolModel> SearchUserSchoolModelsByUserSchoolModelList(List<PersonalDetails.UserSchoolModel> userSchoolModelListToFind)
        {
            List<PersonalDetails.UserSchoolModel> userSchools = new List<PersonalDetails.UserSchoolModel>();

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            // Prepare query
            string query = @"SELECT *
                FROM gs_users_db_prod.upd_user_schools 
                WHERE user_id IN(";

            for (int i = 0; i < userSchoolModelListToFind.Count; i++)
            {
                query += string.Concat(" @userIdToFilter", i.ToString());
                parameters.Add("@userIdToFilter" + i.ToString(), userSchoolModelListToFind[i].UserId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                if (i < userSchoolModelListToFind.Count - 1)
                {
                    query += ",";
                }
            }
            query += ")";

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                userSchools = connection.Query<PersonalDetails.UserSchoolModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return userSchools;
        }

        /// <summary>
        /// To check if user A is from the same school as User B.
        /// </summary>
        /// <param name="userIdA">User Id A.</param>
        /// <param name="userIdB">User Id B.</param>
        /// <returns>Returns true if from same school, else false.</returns>
        public static bool CheckIfUsersInSameSchool(string userIdA, string userIdB)
        {
            bool sameSchool = false;
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT sh1.school_id FROM gs_users_db_prod.upd_user_schools sh1 
                    INNER JOIN gs_users_db_prod.upd_user_schools sh2 ON sh2.school_id = sh1.school_id
                    WHERE 
                    sh1.user_id = @userIdA AND sh2.user_id =  @userIdB ";
            var parameters = new { userIdA, userIdB };

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                sameSchool = connection.Query<string>(query, parameters).Count() > 0;
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return sameSchool;
        }

        /// <summary>
        /// To search the profile main account. The main account is the client that is related the subscription and the one that can handle the subscription.
        /// </summary>
        /// <param name="userId">The profile's id.</param>
        /// <returns>Return the associated account model with details.</returns>
        public static Relations.AssociatedAccountModel SearchProfileMainAccount(string userId)
        {
            Relations.AssociatedAccountModel associatedAccountModel = new Relations.AssociatedAccountModel();
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT * FROM `gs_users_db_prod`.`urel_associated_accounts` WHERE profile_user_account = @userId";
            var parameters = new { userId };
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                associatedAccountModel = connection.Query<Relations.AssociatedAccountModel>(query, parameters).First();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return associatedAccountModel;
        }

        /// <summary>
        /// To searcg the role ids according to user id list. The user id list is optional.
        /// </summary>
        /// <param name="userIds">Optional. A list of user ids to find.</param>
        /// <returns>Returns the user role model list.</returns>
        public static List<Roles.UserRoleModel> SearchUsersRolesByUserIds(List<string> userIds)
        {

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT role_id, user_id FROM gs_users_db_prod.ur_user_roles ";
            DynamicParameters parameters = new DynamicParameters();
            List<Roles.UserRoleModel> userRoleModelList = new List<Roles.UserRoleModel>();

            if (userIds != null && userIds.Count > 0)
            {
                query += " WHERE user_id IN (";
                for (int i = 0; i < userIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), userIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < userIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ")";

                MySqlConnection connection = new MySqlConnection
                {
                    ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
                };
                connection.Open();
                try
                {
                    userRoleModelList = connection.Query<Roles.UserRoleModel>(query, parameters).ToList();
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();
                }

            }
            else
            {

                MySqlConnection connection = new MySqlConnection
                {
                    ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
                };
                connection.Open();
                try
                {
                    userRoleModelList = connection.Query<Roles.UserRoleModel>(query).ToList();
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();
                }

            }

            return userRoleModelList;
        }

        /// <summary>
        /// Check if user can handle the other user.
        /// </summary>
        /// <param name="account">Account filter should include the userAlphaId, IsStaff, IsDirector, IsClientAccount and UserIds with a single element representing the user id of the user beta.</param>
        /// <returns>Returns a boolean, true if can be handled, else, false.</returns>
        public static bool CheckIfUserCanHandleOtherUser(Models.Filters.AccountGeneral account)
        {

            if (account.IsStaff)
            {
                if (string.IsNullOrWhiteSpace(account.OreId))
                {
                    return true;
                }
                else
                {
                    // If Ore is not null, then validate the ore
                    return ORESchoolUserCheck.CheckIfStaffOreCanHandleUser(account.OreId, account.UserIds[0]);

                }
            }

            if (account.IsDirector)
            {
                return CheckIfUsersInSameSchool(account.UserAlphaId, account.UserIds[0]);
            }

            // If other type of user, just check the relation
            string query = @"
            SELECT 
            ac.id
            FROM `gs_users_db_prod`.`ul_user_accounts` ac    
            ";

            if (account.IsClientAccount)
            {
                // The regular client can be related or associated.
                query += " LEFT JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON pro.user_id_b = ac.id ";
                query += " INNER JOIN `gs_users_db_prod`.`urel_associated_accounts` aa ON aa.profile_user_account = ac.id ";
                query += @" WHERE ";
                query += " (aa.main_user_account = @userAlphaId AND aa.profile_user_account = @profileId) OR (pro.user_id_a = @userAlphaId AND pro.user_a_alpha = 1 AND pro.user_id_b = @profileId) ";
            }
            else
            {
                query += " INNER JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON pro.user_id_b = ac.id ";
                query += @" WHERE ";
                query += " pro.user_id_a = @userAlphaId AND pro.user_a_alpha = 1 AND pro.user_id_b = @profileId";
            }
            var parameters = new { userAlphaId = account.UserAlphaId, profileId = account.UserIds[0] };

            bool canHandleUser = false;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                canHandleUser = connection.Query<string>(query, parameters).Count() > 0;
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return canHandleUser;
        }

        /// <summary>
        /// To relate user admin to ore (for ore staff).
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="oreId">The ore's id.</param>
        public static void RelateUserAdminToOreId(string userId, string oreId, string userInSessionId)
        {
            // First, we check if exists 
            string querySelect = @"SELECT user_id FROM gs_users_db_prod.upd_user_ore WHERE user_id = @userId";
            bool userExists = false;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                userExists = connection.Query<string>(querySelect, new { userId }).Count() > 0;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }


            if (!userExists)
            {
                string query = @"INSERT INTO `gs_users_db_prod`.`upd_user_ore` (`user_id`, `ore_id`, `modified_by`) VALUES (@userId, @oreId, @userInSessionId);";
                connection = new MySqlConnection
                {
                    ConnectionString = 
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"]
                };
                connection.Open();
                try
                {
                    connection.Execute(query, new { userId, oreId, userInSessionId });
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }

            }
            else
            {
                string query = @"UPDATE `gs_users_db_prod`.`upd_user_ore` SET `ore_id` = @oreId, `modified_by` = @userInSessionId WHERE `user_id` = @userId;";
                connection = new MySqlConnection
                {
                    ConnectionString = 
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"]
                };
                connection.Open();
                try
                {
                    connection.Execute(query, new { userId, oreId, userInSessionId });
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }

            }
        }

        /// <summary>
        /// Just the user id, because it should be one ORE anyways for admin. If this is deleted, it meas the 
        /// user has changed the role.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public static void DeleteAmdminOreRelation(string userId)
        {
            string query = @"DELETE FROM  gs_users_db_prod.upd_user_ore WHERE user_id = @userId";
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"]
            };
            connection.Open();
            try
            {
                connection.Execute(query, new { userId });
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

        }


        /// <summary>
        /// Prepare select, just emails.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private static Tuple<string, DynamicParameters> PrepareSelectForUsersManagedEmails(Models.Filters.AccountAdvance accountFilter)
        {
            string query = @"
            SELECT DISTINCT
            ac.id as user_id,
            em.email
            FROM `gs_users_db_prod`.`ul_user_accounts` ac 
            LEFT JOIN `gs_users_db_prod`.`ul_user_emails` em  ON em.user_id = ac.id
            INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id = ac.id ";

            if (accountFilter.IsDirector || !accountFilter.IsDirector && accountFilter.SameSchoolRequired)
            {
                query += " INNER JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id ";
            }
            else
            {
                query += " LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id ";
            }

            if (!accountFilter.IsStaff && !accountFilter.IsDirector && !accountFilter.IsClientAccount)
            {
                // For example: Parent to student; student to student; Teacher to student; They have to be related.
                query += " INNER JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON pro.user_id_b = ac.id ";
            }

            if (accountFilter.IsClientAccount)
            {
                // The regular client can be related or associated.
                query += " LEFT JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON pro.user_id_b = ac.id ";
                query += " INNER JOIN `gs_users_db_prod`.`urel_associated_accounts` aa ON aa.profile_user_account = ac.id ";
            }

            // Prepare WHERE
            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;
            bool userAlphaParameterAdded = false;
            // If not a staff and not a principal, check the relations 
            if (accountFilter != null && accountFilter.CallOnlyProfilesThatCanBeManaged && !accountFilter.IsStaff && !accountFilter.IsDirector)
            {
                query += @" WHERE ";
                if (accountFilter.IsClientAccount) // This table is used only for clients. Clients should look both tables.
                {
                    query += " (aa.main_user_account = @userAlphaId OR (pro.user_id_a = @userAlphaId AND pro.user_a_alpha = 1)) ";
                }
                else
                {
                    query += " (pro.user_id_a = @userAlphaId AND pro.user_a_alpha = 1) ";

                }
                if (!userAlphaParameterAdded)
                {
                    parameters.Add("@userAlphaId", accountFilter.UserAlphaId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    userAlphaParameterAdded = true;
                }
                whereUsed = true;
            }

            // Now apply the filters
            if (accountFilter != null && accountFilter.SameSchoolRequired && !accountFilter.IsStaff)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id = (SELECT us2.school_id FROM  `gs_users_db_prod`.`upd_user_schools` us2 WHERE us2.user_id = @userAlphaId) ";
                if (!userAlphaParameterAdded)
                {
                    parameters.Add("@userAlphaId", accountFilter.UserAlphaId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    userAlphaParameterAdded = true;
                }
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.RoleId))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ro.role_id = @roleId ";
                parameters.Add("@roleId", accountFilter.RoleId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.UserSchoolRecord))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.identifier = @userSchoolRecord ";
                parameters.Add("@userSchoolRecord", accountFilter.UserSchoolRecord, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && accountFilter.SchoolIds != null && accountFilter.SchoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id IN (";
                for (int i = 0; i < accountFilter.SchoolIds.Count; i++)
                {
                    query += string.Concat(" @schoolId", i.ToString());
                    parameters.Add("@schoolId" + i.ToString(), accountFilter.SchoolIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.SchoolIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (accountFilter != null && accountFilter.UserIds != null && accountFilter.UserIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.id IN (";
                for (int i = 0; i < accountFilter.UserIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), accountFilter.UserIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.UserIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Email))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " em.email LIKE @email ";
                parameters.Add("@email", "%" + accountFilter.Email + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            return Tuple.Create(query, parameters);
        }


        /// <summary>
        /// To prepare the select for the users that can be handle. 
        /// </summary>
        /// <param name="account">A class with many properties to filter. See AccountAdvance for more details.</param>
        /// <returns>Returns the query (string) generated.</returns>
        private static string PrepareSelectForUsersManaged(Models.Filters.AccountAdvance account)
        {
            string query = @"
            SELECT DISTINCT
            ac.id as `user_id`, 
            ac.token,
            ac.username, 
            ac.created_at,
            na.given_name, 
            na.surname,
            em.email, 
            adr.address_line_1, 
            adr.address_line_2,
            adr.city,
            adr.state_iso_code,
            adr.country,
            adr.country_iso_code_2,
            adr.zip,
            ore.ore_id,
            pho.phone_number as `phone`,
            ty.id as `role_id`,
            ty.name as `role`,
            1 as `user_has_control`, 
            stt.desc as 'status',
            stt.id as `status_id`,
            act.created_at AS 'last_session',
            us.identifier AS 'user_school_record'
            FROM `gs_users_db_prod`.`ul_user_accounts` ac 
            INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id 
            INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id 
            INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
            LEFT JOIN `gs_users_db_prod`.`ul_user_emails` em ON em.user_id = ac.id 
            LEFT JOIN `gs_users_db_prod`.`upd_user_ore` ore ON ore.user_id = ac.id
            LEFT JOIN `gs_users_db_prod`.`upd_user_addresses` adr ON adr.user_id = ac.id  AND adr.main_address = 1 AND adr.enable = 1
            LEFT JOIN `gs_users_db_prod`.`upd_user_phone_numbers` pho ON pho.user_id = ac.id
            INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = ac.id) 
            INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
            LEFT JOIN `gs_users_db_prod`.`gus_activity_date_reports` act ON act.user_id = ac.id AND act.created_at = (SELECT MAX(act2.created_at) FROM `gs_users_db_prod`.`gus_activity_date_reports` act2 WHERE act2.user_id = ac.id)    
            ";

            if (account.IsDirector || !account.IsDirector && account.SameSchoolRequired)
            {
                query += " INNER JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id ";
            }
            else
            {
                query += " LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id ";
            }

            if (!account.IsStaff && !account.IsDirector && !account.IsClientAccount)
            {
                // For example: Parent to student; student to student; Teacher to student; They have to be related.
                query += " INNER JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON pro.user_id_b = ac.id ";
            }

            if (account.IsClientAccount)
            {
                // The regular client can be related or associated.
                query += " LEFT JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON pro.user_id_b = ac.id ";
                query += " INNER JOIN `gs_users_db_prod`.`urel_associated_accounts` aa ON aa.profile_user_account = ac.id ";
            }


            return query;
        }

        /// <summary>
        /// To prepare the where string with the parameters for users that can be handled.
        /// </summary>
        /// <param name="accountFilter">A class with many properties to filter. See AccountAdvance for more details.</param>
        /// <returns>Returns the query with the dynamic parameters.</returns>
        private static Tuple<string, DynamicParameters> PrepareWhereUsersSearch(Models.Filters.AccountAdvance accountFilter)
        {
            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;
            bool userAlphaParameterAdded = false;
            string query = "";

            // If not a staff and not a principal, check the relations 
            if (accountFilter != null && accountFilter.CallOnlyProfilesThatCanBeManaged && !accountFilter.IsStaff && !accountFilter.IsDirector)
            {
                query += @" WHERE ";
                if (accountFilter.IsClientAccount) // This table is used only for clients. Clients should look both tables.
                {
                    query += " (aa.main_user_account = @userAlphaId OR (pro.user_id_a = @userAlphaId AND pro.user_a_alpha = 1)) ";
                }
                else
                {
                    query += " (pro.user_id_a = @userAlphaId AND pro.user_a_alpha = 1) ";

                }
                if (!userAlphaParameterAdded)
                {
                    parameters.Add("@userAlphaId", accountFilter.UserAlphaId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    userAlphaParameterAdded = true;
                }
                whereUsed = true;
            }

            // Now apply the filters
            if (accountFilter != null && accountFilter.SameSchoolRequired && !accountFilter.IsStaff)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id = (SELECT us2.school_id FROM  `gs_users_db_prod`.`upd_user_schools` us2 WHERE us2.user_id = @userAlphaId) ";
                if (!userAlphaParameterAdded)
                {
                    parameters.Add("@userAlphaId", accountFilter.UserAlphaId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    userAlphaParameterAdded = true;
                }
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.RoleId))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ty.id = @roleId ";
                parameters.Add("@roleId", accountFilter.RoleId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.UserSchoolRecord))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.identifier = @userSchoolRecord ";
                parameters.Add("@userSchoolRecord", accountFilter.UserSchoolRecord, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && accountFilter.SchoolIds != null && accountFilter.SchoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id IN (";
                for (int i = 0; i < accountFilter.SchoolIds.Count; i++)
                {
                    query += string.Concat(" @schoolId", i.ToString());
                    parameters.Add("@schoolId" + i.ToString(), accountFilter.SchoolIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.SchoolIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (accountFilter != null && accountFilter.UserIds != null && accountFilter.UserIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.id IN (";
                for (int i = 0; i < accountFilter.UserIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), accountFilter.UserIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.UserIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Username))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += @" ac.username LIKE @username ";
                parameters.Add("@username", "%" + accountFilter.Username + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.FirstName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.given_name LIKE @givenName ";
                parameters.Add("@givenName", "%" + accountFilter.FirstName + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.LastName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.surname LIKE @surname ";
                parameters.Add("@surname", "%" + accountFilter.LastName + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Email))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " em.email LIKE @email ";
                parameters.Add("@email", "%" + accountFilter.Email + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.PhoneNumber))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " pho.phone_number LIKE @phoneNumber ";
                parameters.Add("@phoneNumber", "%" + accountFilter.PhoneNumber + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.LastSessionFrom))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " act.created_at >= @dateConnectionFrom  ";
                parameters.Add("@dateConnectionFrom", DateTime.Parse(accountFilter.LastSessionFrom).ToString("yyyy-MM-dd HH:mm:ss"), System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.LastSessionTo))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " act.created_at <= @dateConnectionTo  ";
                parameters.Add("@dateConnectionTo", DateTime.Parse(accountFilter.LastSessionTo).ToString("yyyy-MM-dd HH:mm:ss"), System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.CreatedFrom))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.created_at >= @dateCreatedAccountFrom  ";
                parameters.Add("@dateCreatedAccountFrom", DateTime.Parse(accountFilter.CreatedFrom).ToString("yyyy-MM-dd HH:mm:ss"), System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.CreatedTo))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.created_at <= @dateCreatedAccountTo  ";
                parameters.Add("@dateCreatedAccountTo", DateTime.Parse(accountFilter.CreatedTo).ToString("yyyy-MM-dd HH:mm:ss"), System.Data.DbType.String, System.Data.ParameterDirection.Input);

            }

            return Tuple.Create(query, parameters);
        }

        /// <summary>
        /// To prepare the select for the users that are related to another user.
        /// </summary>
        /// <param name="account">A class with many properties to filter. See AccountGeneral for more details.</param>
        /// <returns>Returns the query with the dynamic parameters.</returns>
        private static Tuple<string, DynamicParameters> PrepareSelectForUsersRelations(Models.Filters.AccountGeneral account)
        {
            string query = @"
            SELECT 
            ac.id as `user_id`, 
            ac.token,
            ac.username, 
            ac.created_at,
            na.given_name, 
            na.surname,
            ue.email,
            ty.name as `role`,";
            if (account.IsStaff || account.IsDirector)
            {
                query += " 1  as `user_has_control`, ";
            }
            else
            {
                query += " CASE ";

                if (account.IsClientAccount)
                {
                    query += " WHEN aa.main_user_account = @userAlphaId THEN 1 ";
                }

                query += @" WHEN pro.user_id_a = @userAlphaId AND pro.user_a_alpha = 1 THEN 1 ELSE 0 END  as `user_has_control`, ";
            }
            query += @"
            us.identifier  AS 'user_school_record'
            FROM `gs_users_db_prod`.`ul_user_accounts` ac 
            INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id 
             LEFT JOIN `gs_users_db_prod`.`ul_user_emails` ue ON ue.user_id = ac.id
            INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id 
            INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
            INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = ac.id) 
            INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id   
            LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id
            ";


            if (!account.IsStaff && !account.IsDirector && !account.IsClientAccount || (account.IsStaff || account.IsDirector) && account.UserIds != null && account.UserIds.Count > 0)
            {
                // For example: Parent to student; student to student; Teacher to student; They have to be related.
                query += " INNER JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON  (pro.user_id_a = ac.id AND pro.user_id_b = @userAlphaId)  OR (pro.user_id_b = ac.id AND pro.user_id_a = @userAlphaId)";
            }

            if (account.IsClientAccount)
            {
                // The regular client can be related or associated.
                query += " LEFT JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON (pro.user_id_a = ac.id AND pro.user_id_b = @userAlphaId)  OR (pro.user_id_b = ac.id AND pro.user_id_a = @userAlphaId) ";
                query += " INNER JOIN `gs_users_db_prod`.`urel_associated_accounts` aa ON aa.profile_user_account = ac.id ";
            }

            DynamicParameters parameters = new DynamicParameters();

            if ((account.IsStaff || account.IsDirector) && account.UserIds != null && account.UserIds.Count > 0)
            {
                parameters.Add("@userAlphaId", account.UserIds[0], System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }
            else
            {
                parameters.Add("@userAlphaId", account.UserAlphaId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            return Tuple.Create(query, parameters);
        }

        /// <summary>
        /// To prepare the WHERE for the users that are related to another user.
        /// </summary>
        /// <param name="accountFilter">A class with many properties to filter. See AccountGeneral for more details.</param>
        /// <returns>Returns the query with the dynamic parameters.</returns>
        private static Tuple<string, DynamicParameters> PrepareWhereForUsersRelations(Models.Filters.AccountGeneral accountFilter)
        {
            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;
            string query = "";

            // If not a staff and not a principal, check the relations 
            if (!accountFilter.IsStaff && !accountFilter.IsDirector)
            {
                query += @" WHERE ";
                if (accountFilter.IsClientAccount) // This table is used only for clients. Clients should look both tables.
                {
                    query += " ((aa.main_user_account = @userAlphaId OR aa.profile_user_account = @userAlphaId) OR (pro.user_id_a = @userAlphaId OR pro.user_id_b = @userAlphaId)) ";
                }
                else
                {
                    query += " (pro.user_id_a = @userAlphaId OR pro.user_id_b = @userAlphaId) ";

                }
                whereUsed = true;
            }

            if (accountFilter.IsDirector || accountFilter.SameSchoolRequired) // If director, the users from schools are automatically rellated
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id = (SELECT us2.school_id FROM  `gs_users_db_prod`.`upd_user_schools` us2 WHERE us2.user_id = @userAlphaId) ";
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.RoleId))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ty.id = @roleId ";
                parameters.Add("@roleId", accountFilter.RoleId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.UserSchoolRecord))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.identifier = @userSchoolRecord ";
                parameters.Add("@userSchoolRecord", accountFilter.UserSchoolRecord, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && accountFilter.SchoolIds != null && accountFilter.SchoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id IN (";
                for (int i = 0; i < accountFilter.SchoolIds.Count; i++)
                {
                    query += string.Concat(" @schoolId", i.ToString());
                    parameters.Add("@schoolId" + i.ToString(), accountFilter.SchoolIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.SchoolIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (accountFilter != null && accountFilter.UserIds != null && accountFilter.UserIds.Count > 0 && (!accountFilter.IsStaff && !accountFilter.IsDirector))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.id IN (";
                for (int i = 0; i < accountFilter.UserIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), accountFilter.UserIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.UserIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Username))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += @" ac.username LIKE @username ";
                parameters.Add("@username", "%" + accountFilter.Username + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.FirstName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.given_name LIKE @givenName ";
                parameters.Add("@givenName", "%" + accountFilter.FirstName + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.LastName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.surname LIKE @surname ";
                parameters.Add("@surname", "%" + accountFilter.LastName + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }


            if (!whereUsed)
            {
                query += " WHERE ";
                whereUsed = true;
            }
            else
                query += " AND ";

            query += " stt.allow_access = 1 ";

            return Tuple.Create(query, parameters);
        }

        /// <summary>
        /// Prepares the select query for friend requests. (Users' relations).
        /// </summary>
        /// <returns>Returns the query string.</returns>
        private static string PrepareSelectFriendRequests()
        {
            string query = @"
            SELECT 
            ac.id as `user_id`, 
            ac.token,
            ac.username, 
            ac.created_at,
            na.given_name, 
            na.surname,
            ty.name as `role`,
            us.identifier  AS 'user_school_record'
            FROM `gs_users_db_prod`.`ul_user_accounts` ac 
            INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id 
            INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id 
            INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
            INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = ac.id) 
            INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id   
            LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id
            INNER JOIN `gs_users_db_prod`.`urel_profile_connection_requests` pro ON pro.user_requester_id = ac.id 
            ";
            return query;
        }

        /// <summary>
        /// Prepares the where query for friend requests. (Users' relations).
        /// </summary>
        /// <param name="accountFilter">A class with many properties to filter. See AccountGeneral for more details.</param>
        /// <returns>Returns the query string with dynamic parameters.</returns>
        private static Tuple<string, DynamicParameters> PrepareWhereForFriendRequests(Models.Filters.AccountGeneral accountFilter)
        {
            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;
            string query = "";
            bool alphaAdded = false;

            // If not a staff and not a principal, check the relations 
            if (!string.IsNullOrWhiteSpace(accountFilter.UserAlphaId))
            {

                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " pro.user_requested_id = @userAlphaId ";

                if (!alphaAdded)
                {
                    parameters.Add("@userAlphaId", accountFilter.UserAlphaId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                }
            }

            if (accountFilter.IsDirector || accountFilter.SameSchoolRequired) // If director, the users from schools are automatically rellated
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                if (!alphaAdded)
                {
                    query += " us.school_id = (SELECT us2.school_id FROM  `gs_users_db_prod`.`upd_user_schools` us2 WHERE us2.user_id = @userAlphaId) ";
                }
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.RoleId))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ty.id = @roleId ";
                parameters.Add("@roleId", accountFilter.RoleId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.UserSchoolRecord))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.identifier = @userSchoolRecord ";
                parameters.Add("@userSchoolRecord", accountFilter.UserSchoolRecord, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && accountFilter.SchoolIds != null && accountFilter.SchoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id IN (";
                for (int i = 0; i < accountFilter.SchoolIds.Count; i++)
                {
                    query += string.Concat(" @schoolId", i.ToString());
                    parameters.Add("@schoolId" + i.ToString(), accountFilter.SchoolIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.SchoolIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (accountFilter != null && accountFilter.UserIds != null && accountFilter.UserIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.id IN (";
                for (int i = 0; i < accountFilter.UserIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), accountFilter.UserIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.UserIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Username))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += @" ac.username LIKE @username ";
                parameters.Add("@username", "%" + accountFilter.Username + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.FirstName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.given_name LIKE @givenName ";
                parameters.Add("@givenName", "%" + accountFilter.FirstName + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.LastName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.surname LIKE @surname ";
                parameters.Add("@surname", "%" + accountFilter.LastName + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }


            if (!whereUsed)
            {
                query += " WHERE ";
                whereUsed = true;
            }
            else
                query += " AND ";

            query += " stt.allow_access = 1 ";

            return Tuple.Create(query, parameters);
        }

        /// <summary>
        /// Prepares the full query for get the models of users' profiles connections.
        /// </summary>
        /// <param name="account">A class with many properties to filter. See AccountGeneral for more details.</param>
        /// <returns>Returns the query string with dynamic parameters.</returns>
        private static Tuple<string, DynamicParameters> PrepareSelectAndWhereForUsersRelationsProfileConnectionModels(Models.Filters.AccountGeneral account)
        {
            string query = "";

            if (account.IsStaff)
            {
                query += @"SELECT
                @userAlphaId AS user_id_a, 
                ur.user_id AS user_id_b,
                1 as user_a_alpha
                FROM  `gs_users_db_prod`.`ur_user_roles` ur
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ur.user_id AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = ur.user_id) 
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                WHERE  
                allow_access = 1 
                AND ur.user_id != @userAlphaId ";
            }
            else if (account.IsDirector)
            {
                query += @"SELECT @userAlphaId AS user_id_a, 
                ur.user_id AS user_id_b,
                1 as user_a_alpha 
                FROM gs_users_db_prod.upd_user_schools us
                INNER JOIN `gs_users_db_prod`.`ur_user_roles` ur ON ro.user_id = us.user_id
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ur.user_id AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = ur.user_id) 
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                WHERE  
                allow_access = 1 
                AND school_id = (SELECT school_id FROM upd_user_schools WHERE user_id = @userAlphaId) AND ur.user_id != @userAlphaId";
            }
            else if (account.IsClientAccount)
            {

                query += @"SELECT
                CASE
                WHEN aa.main_user_account IS NOT NULL THEN aa.main_user_account
                WHEN pro.user_id_a IS NOT NULL THEN pro.user_id_a
                END as user_id_a,
                CASE
                WHEN aa.profile_user_account IS NOT NULL THEN aa.profile_user_account
                WHEN pro.user_id_b IS NOT NULL THEN pro.user_id_b
                END as user_id_b,
                CASE
                WHEN aa.main_user_account IS NOT NULL THEN 1
                WHEN pro.user_a_alpha IS NOT NULL THEN pro.user_a_alpha
                END as user_a_alpha
                FROM
                `gs_users_db_prod`.`ur_user_roles` ur
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ur.user_id AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = ur.user_id) 
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                LEFT JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON (pro.user_id_a = ur.user_id AND pro.user_id_b = @userAlphaId)  OR (pro.user_id_b = ur.user_id AND pro.user_id_a = @userAlphaId)
                INNER JOIN `gs_users_db_prod`.`urel_associated_accounts` aa ON (aa.profile_user_account = ur.user_id AND aa.main_user_account = @userAlphaId) OR (aa.main_user_account = ur.user_id AND aa.profile_user_account = @userAlphaId)
                WHERE allow_access = 1 AND ((aa.main_user_account = @userAlphaId OR aa.profile_user_account = @userAlphaId) OR (pro.user_id_a = @userAlphaId OR pro.user_id_b = @userAlphaId)) ";
            }
            else if (account.IsStudentHomeschool)
            {
                query += @"SELECT
                CASE
                WHEN pro.user_id_a IS NOT NULL THEN pro.user_id_a
                END as user_id_a,
                CASE
                WHEN pro.user_id_b IS NOT NULL THEN pro.user_id_b
                END as user_id_b,
                CASE
                WHEN pro.user_a_alpha IS NOT NULL THEN pro.user_a_alpha
                END as user_a_alpha
                FROM
                `gs_users_db_prod`.`ur_user_roles` ur
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ur.user_id AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = ur.user_id)
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                INNER JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON (pro.user_id_a = ur.user_id AND pro.user_id_b = @userAlphaId)  OR (pro.user_id_b = ur.user_id AND pro.user_id_a = @userAlphaId)
                WHERE allow_access = 1 AND (pro.user_id_a = @userAlphaId OR pro.user_id_b = @userAlphaId)
                UNION
                SELECT
                aa.main_user_account as user_id_a,
                aa.profile_user_account as user_id_b,
                1 as user_a_alpha
                FROM
                `gs_users_db_prod`.`urel_associated_accounts` aa
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = aa.main_user_account AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = aa.main_user_account)
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                WHERE allow_access = 1 AND aa.available = '1' AND aa.profile_user_account = @userAlphaId ";
            }
            else
            {
                // Teacher, student, parents, etc
                query += @"SELECT
                CASE
                WHEN pro.user_id_a IS NOT NULL THEN pro.user_id_a
                END as user_id_a,
                CASE
                WHEN pro.user_id_b IS NOT NULL THEN pro.user_id_b
                END as user_id_b,
                CASE
                WHEN pro.user_a_alpha IS NOT NULL THEN pro.user_a_alpha
                END as user_a_alpha
                FROM
                `gs_users_db_prod`.`ur_user_roles` ur
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ur.user_id AND ust.id = (SELECT MAX(ust2.id) FROM `gs_users_db_prod`.`ul_user_account_statuses` ust2 WHERE ust2.user_id = ur.user_id)
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                INNER JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON (pro.user_id_a = ur.user_id AND pro.user_id_b = @userAlphaId)  OR (pro.user_id_b = ur.user_id AND pro.user_id_a = @userAlphaId)
                WHERE allow_access = 1 AND (pro.user_id_a = @userAlphaId OR pro.user_id_b = @userAlphaId) ";
            }

            DynamicParameters parameters = new DynamicParameters();
            if (!long.TryParse(account.UserAlphaId, out var userAlphaId))
                throw new ArgumentException("Invalid user id");

            parameters.Add("@userAlphaId", userAlphaId, DbType.Int64);

            if (!string.IsNullOrWhiteSpace(account.RoleId) && int.TryParse(account.RoleId, out var roleId))
            {
                query += " AND ur.role_id = @roleId";
                parameters.Add("@roleId", roleId, DbType.Int32);
            }

            return Tuple.Create(query, parameters);
        }

        private static Tuple<string, DynamicParameters> PrepareQueryForUsersRelations(
           Models.Filters.AccountGeneral account,
           bool isForOtherUser = false,
           string userIdToCallDetails = null)
        {
            if (isForOtherUser && string.IsNullOrWhiteSpace(userIdToCallDetails))
            {
                // Throw error.
                throw new Exception("The user id to call details is required.");
            }
            if (isForOtherUser)
            {
                // Call the role of the user id to call details.
                string queryUserOtherDetails = @"SELECT role_id FROM gs_users_db_prod.ur_user_roles WHERE user_id = @userId";
                var connection = new MySqlConnection
                {
                    ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
                };
                connection.Open();
                int roleId = connection.Query<int>(queryUserOtherDetails, new { userId = userIdToCallDetails }).FirstOrDefault();
                connection.Close();

                // If role id is 0, throw an error.
                if (roleId == 0)
                {
                    throw new Exception("The user id to call details is not valid.");
                }

                if (roleId == int.Parse(AppConfiguration.Configuration["AppSettings:ur_role_client"]))
                {
                    account.IsClientAccount = true;
                }
                else if (roleId == int.Parse(AppConfiguration.Configuration["AppSettings:ur_role_director"]))
                {
                    account.IsDirector = true;
                }
                else if (roleId == int.Parse(AppConfiguration.Configuration["AppSettings:ur_role_admin_ore"])
                    || roleId == int.Parse(AppConfiguration.Configuration["AppSettings:ur_role_admin"]))
                {
                    account.IsStaff = true;
                }
                else
                {
                    account.IsClientAccount = false;
                    account.IsDirector = false;
                    account.IsStaff = false;
                }

                // If ore, filter by the schools.
                if(account.IsStaff && roleId == int.Parse(AppConfiguration.Configuration["AppSettings:ur_role_admin_ore"]))
                {
                    // First, select the region. 
                    string queryRegion = @"SELECT ore_id FROM gs_users_db_prod.upd_user_ore WHERE user_id = @userId;";
                    connection.Open();
                    string oreId = connection.Query<string>(queryRegion, new { userId = userIdToCallDetails }).FirstOrDefault();
                    connection.Close();

                    var schools = SchoolDetailsHandler.GetORESchoolIds(oreId);
                    if (schools != null && schools.Count > 0)
                    {
                        account.SchoolIds = schools;
                    }
                    else
                    {
                        // Return an empty list to the user since the Admin ORE needs to have a ore_id.
                        return Tuple.Create("", new DynamicParameters());
                    }
                }
            }

            var parameters = new DynamicParameters();
            var schoolsDirectors = new List<int>();

            string query = "";
            if (account.IsDirector)
            {
                // Call the users of the same school.
                query = @"SELECT 
                ac.id as `user_id`, 
                ac.token,
                ac.username, 
                ac.created_at,
                na.given_name, 
                na.surname,
                ue.email,
                ty.name as `role`,
                 1  as `user_has_control`,
                  ac.token  AS 'user_school_record'
                FROM `gs_users_db_prod`.`ul_user_accounts` ac 
                INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id 
                LEFT JOIN `gs_users_db_prod`.`ul_user_emails` ue ON ue.user_id = ac.id
                INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id 
                INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id   
                INNER JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id ";
            }
            else if (account.IsClientAccount)
            {
                // Call the associated accounts and the profiles.
                query = @"SELECT 
                ac.id as `user_id`, 
                ac.token,
                ac.username, 
                ac.created_at,
                na.given_name, 
                na.surname,
                ue.email,
                ty.name as `role`,
                 1  as `user_has_control`,
                  ac.token  AS 'user_school_record'
                FROM `gs_users_db_prod`.`ul_user_accounts` ac 
                INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id 
                LEFT JOIN `gs_users_db_prod`.`ul_user_emails` ue ON ue.user_id = ac.id
                INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id 
                INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                INNER JOIN `gs_users_db_prod`.`urel_associated_accounts` aa ON aa.profile_user_account = ac.id AND aa.main_user_account = @userAlphaId
                LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id ";

                parameters.Add("@userAlphaId", userIdToCallDetails, System.Data.DbType.Int32,
                 System.Data.ParameterDirection.Input);
            }
            else if (account.IsStudentHomeschool)
            {
                // Call the associated accounts and the profiles.
                query = @"SELECT 
                ac.id as `user_id`, 
                ac.token,
                ac.username, 
                ac.created_at,
                na.given_name, 
                na.surname,
                ue.email,
                ty.name as `role`,
                 1  as `user_has_control`,
                  ac.token  AS 'user_school_record'
                FROM `gs_users_db_prod`.`ul_user_accounts` ac 
                INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id 
                LEFT JOIN `gs_users_db_prod`.`ul_user_emails` ue ON ue.user_id = ac.id
                INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id 
                INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
                INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id
                INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                INNER JOIN `gs_users_db_prod`.`urel_associated_accounts` aa ON aa.profile_user_account = @userAlphaId AND aa.main_user_account = ac.id
                LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id ";

                if (isForOtherUser)
                {
                    parameters.Add("@userAlphaId", userIdToCallDetails, System.Data.DbType.Int32,
                        System.Data.ParameterDirection.Input);
                }
                else
                {
                    parameters.Add("@userAlphaId", account.UserAlphaId, System.Data.DbType.Int32,
                        System.Data.ParameterDirection.Input);
                }
            }
            else if (account.IsStaff)
            {
                // Call all the users.
                query = @"SELECT 
                    ac.id as `user_id`, 
                    ac.token,
                    ac.username, 
                    ac.created_at,
                    na.given_name, 
                    na.surname,
                    ue.email,
                    ty.name as `role`,
                     1  as `user_has_control`,
                      ac.token  AS 'user_school_record'
                    FROM `gs_users_db_prod`.`ul_user_accounts` ac 
                    INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id 
                    LEFT JOIN `gs_users_db_prod`.`ul_user_emails` ue ON ue.user_id = ac.id
                    INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id 
                    INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
                    INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id
                    INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                    LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id ";
            }
            else
            {
                // Call only the related users. 
                query = @"SELECT 
                    ac.id as `user_id`, 
                    ac.token,
                    ac.username, 
                    ac.created_at,
                    na.given_name, 
                    na.surname,
                    ue.email,
                    ty.name as `role`,
                     1  as `user_has_control`,
                      ac.token  AS 'user_school_record'
                    FROM `gs_users_db_prod`.`ul_user_accounts` ac 
                    INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id 
                    LEFT JOIN `gs_users_db_prod`.`ul_user_emails` ue ON ue.user_id = ac.id
                    INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id 
                    INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
                    INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id
                    INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
                    LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id 
                    INNER JOIN `gs_users_db_prod`.`urel_profile_connections` pro ON  (pro.user_id_a = ac.id AND pro.user_id_b = @userAlphaId)  
                    OR (pro.user_id_b = ac.id AND pro.user_id_a = @userAlphaId) ";

                if (isForOtherUser)
                {
                    parameters.Add("@userAlphaId", userIdToCallDetails, System.Data.DbType.Int32,
                        System.Data.ParameterDirection.Input);
                }
                else
                {
                    parameters.Add("@userAlphaId", account.UserAlphaId, System.Data.DbType.Int32,
                        System.Data.ParameterDirection.Input);
                }
            }

            // WHERE 
            bool whereUsed = false;

            // If not a staff and not a principal, check the relations 
            if (!account.IsStaff && !account.IsDirector && !account.IsStudentHomeschool)
            {
                query += @" WHERE ";
                if (account
                    .IsClientAccount) // This table is used only for clients. Clients should look both tables.
                {
                    query +=
                        " (aa.main_user_account = @userAlphaId OR aa.profile_user_account = @userAlphaId) ";
                    if (isForOtherUser)
                    {
                        parameters.Add("@userAlphaId", userIdToCallDetails, System.Data.DbType.Int32,
                            System.Data.ParameterDirection.Input);
                    }
                    else
                    {
                        parameters.Add("@userAlphaId", account.UserAlphaId, System.Data.DbType.Int32,
                            System.Data.ParameterDirection.Input);
                    }
                }
                else
                {
                    query += " (pro.user_id_a = @userAlphaId OR pro.user_id_b = @userAlphaId) ";
                }

                whereUsed = true;
            }

            if (account.IsDirector) // If director, the users from schools are automatically rellated
            {
                // Call the schools of the director. 
                var querySchools = @"SELECT us.school_id FROM `gs_users_db_prod`.`upd_user_schools` us WHERE us.user_id = @userAlphaId";
                var connection = new MySqlConnection
                {
                    ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
                };
                connection.Open();

                try
                {
                    schoolsDirectors = connection.Query<int>(querySchools, new { userAlphaId = account.UserAlphaId }).ToList();
                }
                catch (Exception)
                {
                }
                finally
                {
                    connection.Close();
                }

                if (schoolsDirectors.Count > 0)
                {
                    if (!whereUsed)
                    {
                        query += " WHERE ";
                        whereUsed = true;
                    }
                    else
                        query += " AND ";

                    query += " us.school_id IN (";
                    for (int i = 0; i < schoolsDirectors.Count; i++)
                    {
                        query += string.Concat(" @schoolId", i.ToString());
                        parameters.Add("@schoolId" + i.ToString(), schoolsDirectors[i], System.Data.DbType.String,
                                                       System.Data.ParameterDirection.Input);
                        if (i < schoolsDirectors.Count - 1)
                        {
                            query += ",";
                        }
                    }

                    query += ") ";
                }
            }

            if (account != null && !string.IsNullOrWhiteSpace(account.RoleId))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ty.id = @roleId ";
                parameters.Add("@roleId", account.RoleId, System.Data.DbType.Int32,
                    System.Data.ParameterDirection.Input);
            }

            if (account != null && !string.IsNullOrWhiteSpace(account.UserSchoolRecord))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.token = @userSchoolRecord ";
                parameters.Add("@userSchoolRecord", account.UserSchoolRecord, System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (account != null && account.SchoolIds != null && account.SchoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " us.school_id IN (";
                for (int i = 0; i < account.SchoolIds.Count; i++)
                {
                    query += string.Concat(" @schoolId", i.ToString());
                    parameters.Add("@schoolId" + i.ToString(), account.SchoolIds[i], System.Data.DbType.String,
                        System.Data.ParameterDirection.Input);
                    if (i < account.SchoolIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (account != null && account.UserIds != null && account.UserIds.Count > 0 &&
                (!account.IsStaff && !account.IsDirector && !account.IsStudentHomeschool))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.id IN (";
                for (int i = 0; i < account.UserIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), account.UserIds[i],
                        System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < account.UserIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (account != null && !string.IsNullOrWhiteSpace(account.Username))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += @" ac.username LIKE @username ";
                parameters.Add("@username", "%" + account.Username + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (account != null && !string.IsNullOrWhiteSpace(account.FirstName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.given_name LIKE @givenName ";
                parameters.Add("@givenName", "%" + account.FirstName + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (account != null && !string.IsNullOrWhiteSpace(account.LastName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.surname LIKE @surname ";
                parameters.Add("@surname", "%" + account.LastName + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (!whereUsed)
            {
                query += " WHERE ";
                whereUsed = true;
            }
            else
                query += " AND ";

            query += " stt.allow_access = 1 ";

            // Order by first name and last name.
            query += " ORDER BY na.given_name, na.surname ";

            return Tuple.Create(query, parameters);
        }
    
        /// <summary>
        /// To search the users for admin.
        /// </summary>
        /// <param name="accountFilter">The account to filter.</param>
        /// <param name="paginationFilters">The pagination filters.</param>
        /// <returns>Returns the user advance response model.</returns>
        public static Models.UserAdvanceResponseModel SearchUsersForAdmin(Models.Filters.AccountAdvance accountFilter,
            Models.Filters.Pagination paginationFilters)
        {
            var userAdvanceResponseModel = new Models.UserAdvanceResponseModel()
            {
                UserInfo = new List<Models.UserAdvance>(),
            };

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            if (paginationFilters != null && paginationFilters.LimitRows == 0) // To avoid divide by zero
                paginationFilters.LimitRows = 500;

            // Prepare the query
            Tuple<string, DynamicParameters> whereTuple = PrepareWhereUsersSearchAdmin(accountFilter);
            Tuple<string, DynamicParameters> whereTupleSubQuery = PrepareWhereAndJoinsUsersSearchAdminForSubQuery(accountFilter);


            string query = PrepareSelectForUsersManagedAdmin(accountFilter);
            query = Regex.Replace(
                query,
                @"\bFROM\s+`gs_users_db_prod`\.`ul_user_accounts`\s+ac\b",
                whereTupleSubQuery.Item1,
                RegexOptions.IgnoreCase
            );
            query += " " + whereTuple.Item1 + " ORDER BY ac.id";

            parameters.AddDynamicParams(whereTuple.Item2);

            // Add subquery parameters that are not already present in the outer WHERE parameters
            foreach (var paramName in whereTupleSubQuery.Item2.ParameterNames)
            {
                if (!parameters.ParameterNames.Contains(paramName))
                {
                    parameters.Add(paramName, whereTupleSubQuery.Item2.Get<object>(paramName));
                }
            }

            IEnumerable<Models.UserAdvance> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();

            var queryBuilder = PaginationQueryBuilder.BuildPagedQueryAsync(
                query,
                paginationFilters.FromPage,
                paginationFilters.LimitRows);

            try
            {
                items = connection.Query<Models.UserAdvance>(queryBuilder.Query, parameters);
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }


            if (items != null)
            {
                userAdvanceResponseModel = new Models.UserAdvanceResponseModel()
                {
                    UserInfo = items.ToList()
                };
            }

            return userAdvanceResponseModel;
        }

        /// <summary>
        /// To get total items
        /// </summary>
        /// <param name="accountFilter">A class with many properties to filter. See AccountAdvance for more details.</param>
        /// <param name="paginationFilters">To apply pagination.</param>
        /// <returns>Returns the user advance response model with page details and users information.</returns>
        public static int TotalItemsSearchUsersForAdmin(Models.Filters.AccountAdvance accountFilter,
            Models.Filters.Pagination paginationFilters)
        {
            var userAdvanceResponseModel = new Models.UserAdvanceResponseModel()
            {
                UserInfo = new List<Models.UserAdvance>(),
            };

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            if (paginationFilters != null && paginationFilters.LimitRows == 0) // To avoid divide by zero
                paginationFilters.LimitRows = 500;

            // Prepare the query
            Tuple<string, DynamicParameters> whereTupleSubQuery = PrepareWhereAndJoinsUsersSearchAdminForSubQuery(accountFilter, true);

            string query = whereTupleSubQuery.Item1;
            parameters.AddDynamicParams(whereTupleSubQuery.Item2);

            string connectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"];

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = connectionString
            };
            connection.Open();

            var totalItems = PaginationQueryBuilder.BuildCountQuery(
                query,
                parameters,
                connectionString
                );

            int totalPages = (int)Math.Ceiling(totalItems / (decimal)paginationFilters.LimitRows);

            return totalPages;

        }

        /// <summary>
        /// To prepare the select for the users that can be handle. (For Admin).
        /// </summary>
        /// <param name="account">A class with many properties to filter. See AccountAdvance for more details.</param>
        /// <returns>Returns the query (string) generated.</returns>
        private static string PrepareSelectForUsersManagedAdmin(Models.Filters.AccountAdvance account)
        {
            string query = @"
            SELECT DISTINCT
            ac.id as `user_id`,
            ac.token,
            ac.username,
            ac.created_at,
            umg.group_code,
            na.given_name,
            na.surname,
            em.email,
            adr.address_line_1,
            adr.address_line_2,
            adr.city,
            adr.state_iso_code,
            adr.country,
            adr.country_iso_code_2,
            adr.zip,
            ore.ore_id,
            pho.phone_number as `phone`,
            ty.id as `role_id`,
            ty.name as `role`,
            1 as `user_has_control`,
            stt.desc as 'status',
            stt.id as `status_id`,
            act.created_at AS 'last_session',
            ac.token AS 'user_school_record'
            FROM `gs_users_db_prod`.`ul_user_accounts` ac
            INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id
            INNER JOIN `gs_users_db_prod`.`ur_user_roles` ro ON ro.user_id =  ac.id
            INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id
            LEFT JOIN `gs_users_db_prod`.`ul_user_emails` em ON em.user_id = ac.id
            LEFT JOIN `gs_users_db_prod`.`upd_user_ore` ore ON ore.user_id = ac.id
            LEFT JOIN `gs_users_db_prod`.`upd_user_addresses` adr ON adr.user_id = ac.id  AND adr.main_address = 1 AND adr.enable = 1
            LEFT JOIN `gs_users_db_prod`.`upd_user_phone_numbers` pho ON pho.user_id = ac.id
            LEFT JOIN `gs_users_db_prod`.`user_main_groups` umg ON umg.user_id = ac.id
            INNER JOIN `gs_users_db_prod`.`ul_user_account_statuses` ust ON ust.user_id = ac.id
            INNER JOIN `gs_users_db_prod`.`ul_status_accounts` stt On stt.id = ust.status_id
            LEFT JOIN `gs_users_db_prod`.`gus_activity_date_reports` act ON act.user_id = ac.id AND act.created_at = (SELECT MAX(act2.created_at) FROM `gs_users_db_prod`.`gus_activity_date_reports` act2 WHERE act2.user_id = ac.id)  ";
            
            return query;
        }

        /// <summary>
        /// Where for search admin.
        /// </summary>
        /// <param name="accountFilter">The account details to filter.</param>
        /// <returns>Returns the part of the query.</returns>
         private static Tuple<string, DynamicParameters> PrepareWhereUsersSearchAdmin(
            Models.Filters.AccountAdvance accountFilter)
        {
            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;
            string query = "";
            

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.RoleId))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ty.id = @roleId ";
                parameters.Add("@roleId", accountFilter.RoleId, System.Data.DbType.Int32,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.UserSchoolRecord))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.token LIKE @userSchoolRecord ";
                parameters.Add("@userSchoolRecord", "%" + accountFilter.UserSchoolRecord + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            // SchoolIds filter removed from outer WHERE - the outer query does not have
            // a JOIN with user_schools, so referencing us.school_id here caused an error for admin-ore when fetching users
            // The subquery already filters by SchoolIds with the correct JOIN

            if (accountFilter != null && accountFilter.UserIds != null && accountFilter.UserIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.id IN (";
                for (int i = 0; i < accountFilter.UserIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), accountFilter.UserIds[i],
                        System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.UserIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Username))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += @" ac.username LIKE @username ";
                parameters.Add("@username", "%" + accountFilter.Username + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.FirstName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.given_name LIKE @givenName ";
                parameters.Add("@givenName", "%" + accountFilter.FirstName + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.LastName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " na.surname LIKE @surname ";
                parameters.Add("@surname", "%" + accountFilter.LastName + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Email))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " em.email LIKE @email ";
                parameters.Add("@email", "%" + accountFilter.Email + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.CreatedFrom))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.created_at >= @dateCreatedAccountFrom  ";
                parameters.Add("@dateCreatedAccountFrom",
                    DateTime.Parse(accountFilter.CreatedFrom).ToString("yyyy-MM-dd HH:mm:ss"),
                    System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.CreatedTo))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.created_at <= @dateCreatedAccountTo  ";
                parameters.Add("@dateCreatedAccountTo",
                    DateTime.Parse(accountFilter.CreatedTo).ToString("yyyy-MM-dd HH:mm:ss"), System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            return Tuple.Create(query, parameters);
        }

        /// <summary>
        /// Where for search admin.
        /// </summary>
        /// <param name="accountFilter">The account details to filter.</param>
        /// <returns>Returns the part of the query.</returns>
        private static Tuple<string, DynamicParameters> PrepareWhereAndJoinsUsersSearchAdminForSubQuery(
           Models.Filters.AccountAdvance accountFilter, bool isForTotalItems = false)
        {
            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;
            string query = "";
            string schoolRelation = string.Empty;

            if (isForTotalItems)
            {
                schoolRelation = $@"
                        SELECT DISTINCT ro.user_id
                        FROM `gs_users_db_prod`.`ur_user_roles` ro
                        INNER JOIN `gs_users_db_prod`.`ul_user_accounts` ac ON ac.id = ro.user_id ";

            }
            else
            {
                schoolRelation = $@"
                    FROM (
                        SELECT DISTINCT ro.user_id
                        FROM `gs_users_db_prod`.`ur_user_roles` ro
                        INNER JOIN `gs_users_db_prod`.`ul_user_accounts` ac ON ac.id = ro.user_id ";
            }


            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.RoleId))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                schoolRelation += "INNER JOIN `gs_users_db_prod`.`ur_role_types` ty ON ro.role_id = ty.id\n\r";
                query += " ty.id = @roleId ";
                parameters.Add("@roleId", accountFilter.RoleId, System.Data.DbType.Int32,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.UserSchoolRecord))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.token LIKE @userSchoolRecord ";
                parameters.Add("@userSchoolRecord", "%" + accountFilter.UserSchoolRecord + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && accountFilter.SchoolIds != null && accountFilter.SchoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                schoolRelation += "LEFT JOIN `gs_users_db_prod`.`upd_user_schools` us ON us.user_id = ac.id\n\r";
                query += " us.school_id IN (";
                for (int i = 0; i < accountFilter.SchoolIds.Count; i++)
                {
                    query += string.Concat(" @schoolId", i.ToString());
                    parameters.Add("@schoolId" + i.ToString(), accountFilter.SchoolIds[i], System.Data.DbType.String,
                        System.Data.ParameterDirection.Input);
                    if (i < accountFilter.SchoolIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (accountFilter != null && accountFilter.UserIds != null && accountFilter.UserIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.id IN (";
                for (int i = 0; i < accountFilter.UserIds.Count; i++)
                {
                    query += string.Concat(" @userIdToFilter", i.ToString());
                    parameters.Add("@userIdToFilter" + i.ToString(), accountFilter.UserIds[i],
                        System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < accountFilter.UserIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Username))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += @" ac.username LIKE @username ";
                parameters.Add("@username", "%" + accountFilter.Username + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.FirstName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                schoolRelation += "INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id\n\r";
                query += " na.given_name LIKE @givenName ";
                parameters.Add("@givenName", "%" + accountFilter.FirstName + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.LastName))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                schoolRelation += "INNER JOIN `gs_users_db_prod`.`upd_user_full_names` na ON na.user_id = ac.id\n\r";
                query += " na.surname LIKE @surname ";
                parameters.Add("@surname", "%" + accountFilter.LastName + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.Email))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                schoolRelation += "LEFT JOIN `gs_users_db_prod`.`ul_user_emails` em ON em.user_id = ac.id\n\r";
                query += " em.email LIKE @email ";
                parameters.Add("@email", "%" + accountFilter.Email + "%", System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.CreatedFrom))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.created_at >= @dateCreatedAccountFrom  ";
                parameters.Add("@dateCreatedAccountFrom",
                    DateTime.Parse(accountFilter.CreatedFrom).ToString("yyyy-MM-dd HH:mm:ss"),
                    System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            if (accountFilter != null && !string.IsNullOrWhiteSpace(accountFilter.CreatedTo))
            {
                if (!whereUsed)
                {
                    query += " WHERE ";
                    whereUsed = true;
                }
                else
                    query += " AND ";

                query += " ac.created_at <= @dateCreatedAccountTo  ";
                parameters.Add("@dateCreatedAccountTo",
                    DateTime.Parse(accountFilter.CreatedTo).ToString("yyyy-MM-dd HH:mm:ss"), System.Data.DbType.String,
                    System.Data.ParameterDirection.Input);
            }

            schoolRelation += query;
            if (isForTotalItems)
            {
                schoolRelation += "ORDER BY ro.user_id\r\n";
            }
            else
            {
                schoolRelation += "ORDER BY ro.user_id\r\n) ids\r\n INNER JOIN `gs_users_db_prod`.`ul_user_accounts` ac ON ac.id = ids.user_id";
            }

            return Tuple.Create(schoolRelation, parameters);
        }
}
}