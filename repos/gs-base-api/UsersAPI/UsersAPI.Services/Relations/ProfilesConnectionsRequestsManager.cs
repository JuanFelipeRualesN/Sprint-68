using System.Collections.Generic;
using UsersAPI.UsersAPI.Domain.Models.Relations;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Relations;


namespace UsersAPI.UsersAPI.Services.Relations
{
    /// <summary>
    /// To handle the connection request by emails (Example, when a teacher invites a parent to connect with the child.)
    /// </summary>
    public class ProfilesConnectionsRequestsManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilesConnectionsRequestsManager"/> class.
        /// Profiles Connection Manager to manage the user connections.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        public ProfilesConnectionsRequestsManager(string connectionString, string connectionStringRead)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
        }

        /// <summary>
        /// Returns the profile invitation by email models.
        /// </summary>
        /// <param name="profileConnectionInvitationByEmailModels">Profile Connection Invitation by email model list.</param>
        /// <returns>Returns a profile connection according to search.</returns>
        public List<ProfileConnectionInvitationByEmailModel> GetProfileInvitationByEmailModelList(List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailModels)
        {
            return new ProfileConnectionInvitationByEmailDAO().Read(this._connectionStringRead, profileConnectionInvitationByEmailModels);
        }

        /// <summary>
        /// Creates a profile connection invitation by email in DB.
        /// </summary>
        /// <param name="profileConnectionModels">Profile connection models.</param>
        public void CreateProfileConnectionInvitationByEmail(List<ProfileConnectionInvitationByEmailModel> profileConnectionModels)
        {
            new ProfileConnectionInvitationByEmailDAO().Create(
                       this._connectionString,
                       profileConnectionModels);

        }

        /// <summary>
        /// Delete profile connection invitation by email in DB.
        /// </summary>
        /// <param name="profileConnectionModels">Profile connection models.</param>
        public void DeleteProfileConnectionInvitationByEmail(List<ProfileConnectionInvitationByEmailModel> profileConnectionModels)
        {
            new ProfileConnectionInvitationByEmailDAO().Delete(
                       this._connectionString,
                       profileConnectionModels);

        }

        /// <summary>
        /// Deletes a connection request.
        /// </summary>
        /// <param name="userA">User A.</param>
        /// <param name="userB">User B.</param>
        public void DeleteConnectionRequest(string userA, string userB)
        {
            new ProfileConnectionRequestDAO().Delete(this._connectionString, new List<ProfileConnectionRequestModel> {
                new ProfileConnectionRequestModel()
                {
                    UserRequestedId = userA,
                    UserRequesterId = userB,
                },
                new ProfileConnectionRequestModel()
                {
                    UserRequestedId = userB,
                    UserRequesterId = userA,
                }
                });
        }

        /// <summary>
        /// Process to accept connection request. The users are not alpha against another. Example: Teacher and parent.
        /// </summary>
        /// <param name="userA">User A.</param>
        /// <param name="userB">User B.</param>
        public void AcceptConnectionRequestNoAlphas(string userA, string userB)
        {
            // Check if exists
            List<ProfileConnectionRequestModel> profileConnectionRequestModels = new ProfileConnectionRequestDAO().Read(this._connectionStringRead,
                new List<ProfileConnectionRequestModel>() {
                new ProfileConnectionRequestModel()
                {
                    UserRequestedId = userA,
                    UserRequesterId = userB
                },
                new ProfileConnectionRequestModel()
                {
                    UserRequestedId = userB,
                    UserRequesterId = userA
                }
                });

            if(profileConnectionRequestModels.Count == 0)
            {
                return; // Just ignore
            }

            new ProfileConnectionDAO().Create(this._connectionString, new ProfileConnectionModel()
            {
                UserIdA = userA,
                UserIdB = userB
            });


            new ProfileConnectionRequestDAO().Delete(this._connectionString, new List<ProfileConnectionRequestModel> {
                new ProfileConnectionRequestModel()
                {
                    UserRequestedId = userA,
                    UserRequesterId = userB,
                },
                new ProfileConnectionRequestModel()
                {
                    UserRequestedId = userB,
                    UserRequesterId = userA,
                }
                });
        }
    }
}