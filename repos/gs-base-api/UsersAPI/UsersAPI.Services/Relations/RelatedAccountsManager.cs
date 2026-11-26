using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using UsersAPI.UsersAPI.Domain.Models.Relations;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Relations;
using UsersAPI.UsersAPI.Services.Accounts;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.Relations
{
    /// <summary>
    /// To handle relations between the main account and profiles. Some of the classes includes connection between profiles.
    /// They were made separatly because at the point that Profiles Connections were requested, we had many tables, functions, working with the main account (the account with
    /// subcription) and the profile.
    /// </summary>
    public class RelatedAccountsManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedAccountsManager"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's language code..</param>
        public RelatedAccountsManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// Associates multiple profiles to the user main account. The user main account is the account that can manages the subscription.
        /// </summary>
        /// <param name="mainAccountId">The main account id (user's id).</param>
        /// <param name="profileUserAccounts">Profile user accounts to associate with the main account..</param>
        /// <param name="sessionUserId">The user that is modifying the records..</param>
        public void AssociateAccounts(string mainAccountId, List<string> profileUserAccounts, string sessionUserId)
        {
            // Check if the exists to avoid duplicate. You may ask: "HEY! we could put a unique index in the DB and problem solved."
            // Yes, we know, but we want to avoid any business logic in the DB. 
            // So if one day the company decided to change to another RDBMS, it would be easier.

            List<AssociatedAccountModel> associatedAccountModelsToCreate = new List<AssociatedAccountModel>();
            List<AssociatedAccountModel> associatedAccountModelsToCheck = new List<AssociatedAccountModel>();
            for (int i = 0; i < profileUserAccounts.Count; i++)
            {
                associatedAccountModelsToCreate.Add(new AssociatedAccountModel()
                {
                    MainUserAccount = mainAccountId,
                    ProfileUserAccount = profileUserAccounts[i],
                    Available = "1",
                    ModifiedBy = sessionUserId
                });
                associatedAccountModelsToCheck.Add(new AssociatedAccountModel()
                {
                    MainUserAccount = mainAccountId,
                    ProfileUserAccount = profileUserAccounts[i],
                    Available = "1"
                });
            }

            List<AssociatedAccountModel> associatedAccountsExistent = new AssociatedAccountDAO().Read(this._connectionStringRead, associatedAccountModelsToCheck);
            if (associatedAccountsExistent.Count != associatedAccountModelsToCreate.Count)
            {
                var result = associatedAccountModelsToCreate.Where(p => associatedAccountsExistent.All(p2 => p2.ProfileUserAccount != p.ProfileUserAccount));
                associatedAccountModelsToCreate = result.ToList();

            }

            if (associatedAccountModelsToCreate.Count > 0)
            {
                new AssociatedAccountDAO().Create(this._connectionString, associatedAccountModelsToCreate);
            }
        }

        /// <summary>
        /// Returns the profiles of the main account (the main account is client).
        /// </summary>
        /// <param name="mainAccountUserId">Main account user's id.</param>
        /// <returns>Returns the associated account model list.</returns>
        public List<AssociatedAccountModel> GetMainAccountRelatedProfiles(string mainAccountUserId)
        {

            if (string.IsNullOrWhiteSpace(mainAccountUserId))
            {
                return new List<AssociatedAccountModel>();
            }
            // Get the list of the children
            AssociatedAccountModel associatedAccountModel = new AssociatedAccountModel
            {
                Available = "1",
                MainUserAccount = mainAccountUserId,
            };

            AssociatedAccountDAO associatedAccountDAO = new AssociatedAccountDAO();
            return associatedAccountDAO.Read(this._connectionStringRead, associatedAccountModel, false, false);
        }

        /// <summary>
        /// Associates the profile account (teacher, student, child, etc), to the client of the subscription account. 
        /// NOTE: DO NOT confuse this with the profiles connections, which works like friends on FB. This is the equivalent of an Admin.
        /// </summary>
        /// <param name="mainAccountUserId">The user of the main account (administrator).</param>
        /// <param name="profileUserAccounts">The user profile (teacher, student, etc).</param>
        /// <param name="sessionUserId">The user that is creating the row.</param>
        public void AssociateAccountToMainSubscriptionUser(string mainAccountUserId, List<string> profileUserAccounts, string sessionUserId)
        {
            List<AssociatedAccountModel> associatedAccountModels = new List<AssociatedAccountModel>();
            for (int i = 0; i < profileUserAccounts.Count; i++)
            {
                associatedAccountModels.Add(new AssociatedAccountModel()
                {
                    MainUserAccount = mainAccountUserId,
                    ProfileUserAccount = profileUserAccounts[i],
                    Available = "1",
                    ModifiedBy = sessionUserId
                });
            }
            new AssociatedAccountDAO().Create(this._connectionString, associatedAccountModels);
        }

        /// <summary>
        /// To related the associated account of the subscription to the profile.
        /// </summary>
        /// <param name="mainAccountUserId">The main account id.</param>
        /// <param name="profileId">The profile id.</param>
        /// <param name="sessionUserId">The user that is making the request.</param>
        public ResponseModel AssociateAccountToMainSubscriptionUser(string mainAccountUserId, string profileId, string sessionUserId)
        {
            // First, check if exists 
            var associatedAccountDao = new AssociatedAccountDAO();
            var profileFoundList = associatedAccountDao.Read(this._connectionStringRead, new AssociatedAccountModel()
            {
                MainUserAccount = mainAccountUserId,
                ProfileUserAccount = profileId,
            }, true, true);

            if (profileFoundList.Count > 0)
            {
                return new ResponseModel()
                {
                    Valid = true,
                };
            }

            new AssociatedAccountDAO().Create(this._connectionString, new AssociatedAccountModel()
            {
                MainUserAccount = mainAccountUserId,
                ProfileUserAccount = profileId,
                ModifiedBy = sessionUserId,
            });

            return new ResponseModel()
            {
                Valid = true,
            };
        }

        /// <summary>
        /// Verifies if user are connected. Verifies the associated account and the profile connections.
        /// </summary>
        /// <param name="userIdA">User A.</param>
        /// <param name="userIdB">User B.</param>
        /// <returns>Returns true if connected, false if not.</returns>
        public bool AreUserConnected(string userIdA, string userIdB)
        {
            List<ProfileConnectionModel> profileConnectionModels = new ProfileConnectionDAO().Read(
                this._connectionStringRead,
                new List<ProfileConnectionModel>()
                {
                    new ProfileConnectionModel()
                    {
                        UserIdA = userIdA,
                        UserIdB = userIdB,
                    },
                    new ProfileConnectionModel()
                    {
                        UserIdA = userIdB,
                        UserIdB = userIdA,
                    },
                });

            List<AssociatedAccountModel> associatedAccountModels = new AssociatedAccountDAO().Read(this._connectionStringRead, new AssociatedAccountModel()
            {
                MainUserAccount = userIdA,
                ProfileUserAccount = userIdB,
                Available = "1"
            }, true, true);


            return profileConnectionModels.Count > 0 || associatedAccountModels.Count > 0;
        }

        /// <summary>
        /// Deletes a user connection (equivalent to delete a friend).
        /// </summary>
        /// <param name="userRemover">The user that is making the removal.</param>
        /// <param name="userToRemove">The user to be removed.</param>
        /// <param name="userSessionId">The user that is deleting the row.</param>
        /// <param name="isStaff">If user is staff.</param>
        public ResponseModel DeleteConnection(string userRemover, string userToRemove, string userSessionId, bool isStaff)
        {
            // Both arguments are required. If not, stop the operation.
            if (string.IsNullOrWhiteSpace(userRemover) || string.IsNullOrWhiteSpace(userToRemove))
            {
                return new ResponseModel();
            }

            new ProfileConnectionDAO().Delete(this._connectionString,
                new List<ProfileConnectionModel>()
                {
                    new ProfileConnectionModel()
                    {
                        UserIdA = userRemover,
                        UserIdB = userToRemove
                    },
                    new ProfileConnectionModel()
                    {
                        UserIdA = userToRemove,
                        UserIdB = userRemover
                    }
            });

            // Check if user to remove is an associated account
            if (new AssociatedAccountDAO().Read(this._connectionStringRead, new AssociatedAccountModel() { MainUserAccount = userToRemove }, true, true).Count > 0)
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:urel_message_code_cannot_delete_connection"])
                    }
                };
            }

            new AssociatedAccountDAO().Update(this._connectionString,

                new List<AssociatedAccountModel>()
                {
                    new AssociatedAccountModel()
                    {
                        Available = "0",
                        ModifiedBy = userSessionId,
                    },
                    new AssociatedAccountModel()
                    {
                        Available = "0",
                        ModifiedBy = userSessionId,
                    }
                },
                new List<AssociatedAccountModel>() {

                new AssociatedAccountModel()
                {
                    MainUserAccount = userRemover,
                    ProfileUserAccount = userToRemove
                },
                new AssociatedAccountModel()
                {
                    MainUserAccount = userToRemove,
                    ProfileUserAccount = userRemover
                }
                }
                );

            return new ResponseModel()
            {
                Valid = true,
            };
        }

        /// <summary>
        /// Connect users directly.
        /// </summary>
        /// <param name="userIdA">User Id A.</param>
        /// <param name="userIdB">User Id B.</param>
        /// <param name="isUserIdAAlpha">If user a can handle user b.</param>
        /// <param name="userSessionId">The user that is creating the row.</param>
        /// <returns></returns>
        public ResponseModel ConnectUsersDirect(string userIdA, string userIdB, bool isUserIdAAlpha, string userSessionId)
        {
            // Check if the same 
            if (userIdA == userIdB)
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead,
                        this._langCode).
                        GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_cannot_connect_with_yourself"]),
                    },
                };
            }

            // Check the role of the userIdA
            var userRolesManager = new UsersAPI.Services.Roles.UserRolesManager(this._connectionStringRead, this._connectionStringRead, this._langCode);
            var userRoles = userRolesManager.GetUserRolesByRolesList(new List<UsersAPI.Domain.Models.Roles.UserRoleModel>()
            {
                new UsersAPI.Domain.Models.Roles.UserRoleModel()
                {
                    UserId = userIdA,
                }
            });

            bool isUserAClient = userRoles.Find(x => x.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_client"] && userIdA == x.UserId) != null;

            // If the client has control over the selected user, use the "urel_associated_accounts" table.
            // Otherwise, for all other roles (teachers, students, etc.) or when the client lacks control, 
            // use the "urel_profile_connections" table.
            if (isUserAClient && isUserIdAAlpha)
            {
                // Check if connected already as associated account
                var associatedAccoutsExisting = new AssociatedAccountDAO().Read(this._connectionStringRead, new AssociatedAccountModel()
                {
                    MainUserAccount = userIdA,
                    ProfileUserAccount = userIdB,
                }, false, false);

                if(associatedAccoutsExisting.Count > 0)
                {
                    if (associatedAccoutsExisting[0].Available == "0")
                    {
                        // Update it. 
                        new AssociatedAccountDAO().Update(this._connectionString,
                            new AssociatedAccountModel()
                            {
                                Available = "1",
                                ModifiedBy = userSessionId,
                            }, new AssociatedAccountModel()
                        {
                            Id = associatedAccoutsExisting[0].Id,
                        });
                    }
                    else
                    {
                        return new ResponseModel()
                        {
                            Valid = false,
                            Messages = new List<string>()
                            {
                                new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                                                                                              AppConfiguration.Configuration["AppSettings:urel_message_code_already_connected"]),
                            },
                        };
                    } 
                }
                else
                {
                    // Register the associated account
                    new AssociatedAccountDAO().Create(this._connectionString, new AssociatedAccountModel()
                    {
                        MainUserAccount = userIdA,
                        ProfileUserAccount = userIdB,
                        Available = "1",
                        ModifiedBy = userSessionId,
                    });

                    return new ResponseModel()
                    {
                        Valid = true,
                        Messages = new List<string>()
                    {
                          new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_connection_made"]),
                    },
                    };
                }
            }
            else
            {
                // Check if they are connected already
                if (AreUserConnected(userIdA, userIdB))
                {
                    return new ResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_already_connected"]),
                    },
                    };
                }

                new ProfileConnectionDAO().Create(this._connectionString, new ProfileConnectionModel()
                {
                    ModifiedBy = userSessionId,
                    UserAAlpha = isUserIdAAlpha ? "1" : "0",
                    UserIdA = userIdA,
                    UserIdB = userIdB,
                });
            }

            return new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
                    {
                          new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_connection_made"]),
                    },
            };
        }

        /// <summary>
        /// This connects the users. If the users are from the same school and the requester user is a teacher, then the connection is automatically made.
        /// If the user is not a teacher, then it is store as a request (Friend request, for example).
        /// </summary>
        /// <param name="userRequesterId">User who send the request Id.</param>
        /// <param name="invitationCode">The invitation code of the requested user.</param>
        /// <param name="allowChildToChild">If restrict children connections. (Cannot send requests).</param>
        /// <param name="allowedByTeacher">If allowed by teacher, it is going to be more flexible.</param>
        /// <param name="userSessionId">The user that is creating the row.</param>
        /// <returns>Returns a response model if was successful or not. Parent to child not allowed without invitation, children to children not allowed. </returns>
        public ResponseModel ConnectUsers(string userRequesterId, string invitationCode, bool allowChildToChild, bool allowedByTeacher, string userSessionId)
        {
            // Search the user by invitation code
            UserDetailsManager userDetailsManager = new UserDetailsManager(
                  this._connectionStringRead,
                  this._langCode);
            List<UsersAPI.Domain.Models.Accounts.UserAccountModel> userRequestedAccountModelList =
            userDetailsManager.GetUserAccountDetails(new List<UsersAPI.Domain.Models.Accounts.UserAccountModel>()
            {
                new UsersAPI.Domain.Models.Accounts.UserAccountModel()
                {
                    Token = invitationCode,
                },
            });

            if (userRequestedAccountModelList.Count == 0)
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead,
                        this._langCode).
                        GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_invalid_code"]),
                    },
                };
            }

            // Check if the same 
            if (userRequesterId == userRequestedAccountModelList[0].Id)
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead,
                        this._langCode).
                        GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_cannot_connect_with_yourself"]),
                    },
                };
            }


            // Check if they are connected already
            if (AreUserConnected(userRequesterId, userRequestedAccountModelList[0].Id))
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_already_connected"]),
                    },
                };
            }

            // If it's valid, verify if the user A is a teacher and it is from the same school of the user b, and check if school has a subscription.
            UsersAPI.Services.Roles.UserRolesManager userRolesManager = new UsersAPI.Services.Roles.UserRolesManager(this._connectionStringRead, this._connectionStringRead, this._langCode);
            List<UsersAPI.Domain.Models.Roles.UserRoleModel> userRoles = userRolesManager.GetUserRolesByRolesList(new List<UsersAPI.Domain.Models.Roles.UserRoleModel>()
            {
                new UsersAPI.Domain.Models.Roles.UserRoleModel()
                {
                    UserId = userRequesterId,
                },
                new UsersAPI.Domain.Models.Roles.UserRoleModel()
                {
                    UserId = userRequestedAccountModelList[0].Id,
                }
            });


            // Check if requester is teacher, to evaluate if requester is an alpha (has power against the other user).
            bool isRequesterTeacher = userRoles.Find(x => x.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_teacher"] && userRequesterId == x.UserId) != null;
            bool isRequestedAChild = userRoles.Find(x => x.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_client_child"] && userRequestedAccountModelList[0].Id == x.UserId) != null;
            bool isRequesterAChild = userRoles.Find(x => x.RoleId == AppConfiguration.Configuration["AppSettings:ur_role_client_child"] && userRequesterId == x.UserId) != null;

            if (isRequesterTeacher && isRequestedAChild)
            {
                // Check if platform settings is allowing this type of connection. 
                if (!FeignAPIs.GsWebAPIHandler.GetInvitationStudentStatus())
                {
                    return new ResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                                                           AppConfiguration.Configuration["AppSettings:message_invitation_is_not_allowed_at_this_time"]),
                        },
                    };
                }
            }

            // Do not allow children to connect with other children or adults if not a teacher
            // If the adult is the parent, the process is different. 
            if (!isRequesterTeacher && isRequestedAChild && !allowChildToChild && !allowedByTeacher)
            {
                // Return error. You cannot allow. 
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                          new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_cannot_stablish_connection_children_to_children_or_adult"]),
                    },
                };
            }

            // Check if both users are from the same school. If they are from the same school, then automatically associate the account.
            // Verify if they are from the same schools
            PersonalDetails.UserSchoolManager userSchoolManager = new PersonalDetails.UserSchoolManager(this._connectionString, this._connectionStringRead, this._langCode);
            bool sameSchool = userSchoolManager.CheckIfUsersSameSchool(userRequesterId, userRequestedAccountModelList[0].Id);
            if (sameSchool && isRequesterTeacher)
            {
                // Auto attach
                new ProfileConnectionDAO().Create(this._connectionString, new ProfileConnectionModel()
                {
                    ModifiedBy = userSessionId,
                    UserAAlpha = isRequesterTeacher && isRequestedAChild ? "1" : "0",
                    UserIdA = userRequesterId,
                    UserIdB = userRequestedAccountModelList[0].Id,
                });

                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                    {
                          new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_connection_made"]),
                    },
                };

            }
            else if (allowedByTeacher && !isRequesterAChild)
            {
                new ProfileConnectionDAO().Create(this._connectionString, new ProfileConnectionModel()
                {
                    ModifiedBy = userSessionId,
                    UserAAlpha = !isRequesterAChild && isRequestedAChild ? "1" : "0",
                    UserIdA = userRequesterId,
                    UserIdB = userRequestedAccountModelList[0].Id,
                });

                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                    {
                          new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_connection_made"]),
                    },
                };
            }
            else
            {

                // Send a "friend request"
                new ProfileConnectionRequestDAO().Create(this._connectionString, new ProfileConnectionRequestModel()
                {
                    ModifiedBy = userSessionId,
                    UserRequesterId = userRequesterId,
                    UserRequestedId = userRequestedAccountModelList[0].Id,
                });
                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                    {
                          new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:urel_message_code_success_request"]),
                    },
                };
            }
        }

        /// <summary>
        /// Gets the total number of distinct students who attend the same
        /// school as the given teacher.
        /// </summary>
        /// <param name="teacherUserId">The teacher's user id.</param>
        /// <returns>The total number of students.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="teacherUserId"/> is null or empty.</exception>
        public async Task<int> GetTeacherStudentsCountAsync(string teacherUserId)
        {
            if (string.IsNullOrWhiteSpace(teacherUserId))
            {
                throw new ArgumentException("Teacher user id must be provided.", nameof(teacherUserId));
            }

            const string query = @"
                SELECT COUNT(DISTINCT pc.user_id_b)
                FROM urel_profile_connections pc
                INNER JOIN upd_user_schools ustudent
                    ON ustudent.user_id = pc.user_id_b
                INNER JOIN upd_user_schools usteacher
                    ON usteacher.user_id = pc.user_id_a
                INNER JOIN ur_user_roles ur
                    ON ur.user_id = ustudent.user_id AND ur.role_id = 3
                WHERE pc.user_a_alpha = 1
                  AND ustudent.school_id = usteacher.school_id
                  AND pc.user_id_a = @teacherUserId;";

            var parameters = new DynamicParameters();
            parameters.Add("@teacherUserId", teacherUserId);

            await using var connection = new MySqlConnection(this._connectionStringRead);
            await connection.OpenAsync();

            // Let it throw on failure — the caller needs to know the query failed
            // rather than silently getting 0.
            return await connection.QuerySingleOrDefaultAsync<int>(query, parameters);
        }
    }
}
