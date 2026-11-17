using System;
using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Roles;


namespace UsersAPI.UsersAPI.Services.Roles.CustomClasses
{
    /// <summary>
    /// To handle the validations.
    /// </summary>
    public class ValidationManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode; 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language's code.</param>
        public ValidationManager(string connectionStringRead, string langCode)
        {
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode; 
        }

        /// <summary>
        /// To validate the role.
        /// </summary>
        /// <param name="roleId">The role's id.</param>
        /// <returns>Returns a transaction details if the role is valid or not.</returns>
        public ResponseModel ValidateRole(string roleId)
        {
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                ErrorCodes = new List<string>()
            };

            if (String.IsNullOrWhiteSpace(roleId))
            {
                responseModel.Valid = false;
                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_user_role_required"]);
            }
            else
            {

                // Check if role exists 
                RoleTypeModel roleTypeModel = new RoleTypeModel();
                roleTypeModel.Id = roleId;
                RoleTypeDAO roleTypeDAO = new RoleTypeDAO();
                List<RoleTypeModel> roleTypeModelList = new List<RoleTypeModel>();
                roleTypeModelList = roleTypeDAO.Read(this._connectionStringRead, roleTypeModel, false, false);

                if (String.IsNullOrWhiteSpace(roleId))
                {
                    // Role is required
                    responseModel.Valid = false;
                    responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_user_role_doesnt_exists"]);
                }
                if (roleTypeModelList.Count == 0)
                {
                    // Role doesn't exists 
                    responseModel.Valid = false;
                    responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_user_role_required"]);
                }
            }

            return responseModel; 
        }
    }
}
