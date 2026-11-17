using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Roles;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.Roles
{
    /// <summary>
    /// To handle user roles details.
    /// </summary>
    public class UserRolesManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode; 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's language code.</param>
        public UserRolesManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode; 
        }

        /// <summary>
        /// To get the users'roles.
        /// </summary>
        /// <param name="userRoleModelList">Roles to filter.</param>
        /// <returns>Returns the users'roles list.</returns>
        public List<UserRoleModel> GetUserRolesByRolesList(List<UserRoleModel> userRoleModelList)
        {
            return new UserRoleDAO().Read(this._connectionStringRead, userRoleModelList); 
        }

        /// <summary>
        /// To create an user's role.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="roleId">The role's id.</param>
        public void RegisterUserRole(string userId, string roleId)
        {
            new UserRoleDAO().Create(this._connectionString, new UserRoleModel()
            {
                UserId = userId,
                RoleId = roleId
            }); 
        }

        /// <summary>
        /// To update the user's role.
        /// </summary>
        /// <param name="userId">The user's id to update.</param>
        /// <param name="roleId">The role's id.</param>
        /// <param name="userModifiedBy">The user's id of the user that is modifying the record.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel UpdateUserRole(string userId, string roleId, string userModifiedBy)
        {
            new UserRoleDAO().Update(this._connectionString, new UserRoleModel()
            {
                RoleId = roleId, 
                ModifiedBy = userModifiedBy
            }, new UserRoleModel()
            {
                UserId = userId
            });

            ResponseModel responseModel = new ResponseModel()
            {
                Valid = true,
                Details = new List<string>()
            };

            responseModel.Details.Add(this.GetMessage(AppConfiguration.Configuration["AppSettings:ur_message_role_updated"]));
            return responseModel; 
        }

        /// <summary>
        /// To get the messages related to user-roles.
        /// </summary>
        /// <param name="messageCode">The message's code to find.</param>
        /// <returns>Returns the message's details.</returns>
        public string GetMessage(string messageCode)
        {
            return new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(messageCode);
        }
    }
}