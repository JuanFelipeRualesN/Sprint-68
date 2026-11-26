using System;
using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.PersonalDetails
{
    /// <summary>
    /// To handle the user-schools details.
    /// </summary>
    public class UserSchoolManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;
        private readonly Random _random;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's language code.</param>
        public UserSchoolManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
            this._random = new Random();
        }

        /// <summary>
        /// Checks if users are from the same school.
        /// </summary>
        /// <param name="userIdA">User ID A.</param>
        /// <param name="userIdB">User ID B.</param>
        /// <returns>Returns a boolean if user are from the same school.</returns>
        public bool CheckIfUsersSameSchool(string userIdA, string userIdB)
        {
            // Compare schools of users
            var userSchoolManager = new UserSchoolManager(this._connectionString, this._connectionStringRead, null);
            var requesterSchools = userSchoolManager.GetUserSchools(new List<UserSchoolModel>()
            {
                new UserSchoolModel()
                {
                    UserId = userIdA,
                },
            });

            var requestedSchools = userSchoolManager.GetUserSchools(new List<UserSchoolModel>()
            {
                new UserSchoolModel()
                {
                    UserId = userIdB,
                },
            });

            // Compare schools
            if (requestedSchools.Count != 0 && requesterSchools.Count != 0)
            {
                foreach (var requesterSchool in requesterSchools)
                {
                    if (requestedSchools.Find(x => x.SchoolId == requesterSchool.SchoolId) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Return the user schools detals.
        /// </summary>
        /// <param name="userSchoolModelList">User schools to find (students, teachers, etc).</param>
        /// <returns>Returns the user school model.</returns>
        public List<UserSchoolModel> GetUserSchools(List<UserSchoolModel> userSchoolModelList)
        {
            return new UserSchoolDAO().Read(this._connectionStringRead, userSchoolModelList);
        }

        /// <summary>
        /// Verifies if an identifier is available or not.
        /// </summary>
        /// <param name="userSchoolIdentifier">User School Identifier.</param>
        /// <param name="schoolId">School Id.</param>
        /// <returns>Returns true if available, else, false.</returns>
        public bool CheckIfIdentifierAvailable(string userSchoolIdentifier, string schoolId)
        {
            List<UserSchoolModel> idenfierCheckIfExists = new UserSchoolDAO().Read(this._connectionStringRead, new UserSchoolModel()
            {
                Identifier = userSchoolIdentifier,
                SchoolId = schoolId,
            }, false, false);

            return idenfierCheckIfExists.Count == 0;
        }

        /// <summary>
        /// Relates the user to school.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="schoolId">The school id.</param>
        /// <param name="confirmed">If user is confirmed or not.</param>
        /// <param name="userSessionId">The user session.</param>
        /// <param name="userSchoolIdentifier">The user school identifier (SIE).</param>
        /// <returns>Returns the response model.</returns>
        public ResponseModel RelateUserToSchool(string userId, string schoolId, bool confirmed, string userSessionId, string userSchoolIdentifier)
        {
            try
            {
                // First, check the identifier
                List<UserSchoolModel> idenfierCheckIfExists = new UserSchoolDAO().Read(this._connectionStringRead, new UserSchoolModel()
                {
                    Identifier = userSchoolIdentifier,
                    SchoolId = schoolId,
                }, false, false);

                if (idenfierCheckIfExists.Count > 0)
                {
                    // Check if the user id is the same 
                    if (userId != idenfierCheckIfExists[0].UserId)
                    {
                        // User exists already. 
                        return new ResponseModel()
                        {
                            Valid = false,
                            Details = new List<string>()
                        {
                            new MessageManager(this._connectionString, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_user_record_exists"]),
                        }
                        };
                    }
                    else
                    {
                        return new ResponseModel()
                        {
                            Valid = true,
                            Details = new List<string>()
                        {
                             new MessageManager(this._connectionString, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_user_school_related"]),
                        }
                        };
                    }
                }

                CreateUserSchools(new List<UserSchoolModel>()
            {
                new UserSchoolModel()
                {
                    Identifier = userSchoolIdentifier,
                    SchoolId = schoolId,
                    Confirmed = confirmed ? "1": "0",
                    UserId = userId,
                    ModifiedBy = userSessionId,
                }
            });

                return new ResponseModel()
                {
                    Valid = true,
                    Details = new List<string>()
                        {
                             new MessageManager(this._connectionString, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_user_school_related"]),
                        }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ResponseModel()
                {
                    Valid = false,
                    Details = new List<string>()
                        {
                             new MessageManager(this._connectionString, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gus_general_error"]),
                        }
                };
            }
        }

        /// <summary>
        /// Creates the user schools list.
        /// </summary>
        /// <param name="userSchoolModelList">User schools to create.</param>
        public void CreateUserSchools(List<UserSchoolModel> userSchoolModelList)
        {
            try
            {
                new UserSchoolDAO().Create(this._connectionString, userSchoolModelList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Creating user schools failed.");
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// To delete user-school relation.
        /// </summary>
        /// <param name="userSchoolsToDelete">User school delete list.</param>
        public void DeleteUserSchool(List<UserSchoolModel> userSchoolsToDelete)
        {
            new UserSchoolDAO().Delete(this._connectionString, userSchoolsToDelete);
        }
    }
}
