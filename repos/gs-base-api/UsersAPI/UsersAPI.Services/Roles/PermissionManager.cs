using System.Collections.Generic;
using UsersAPI.UsersAPI.Domain.Models.Roles.Responses;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Roles;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.Roles
{
    /// <summary>
    /// To handle permissions.
    /// </summary>
    public class PermissionManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Costructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's language code.</param>
        public PermissionManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// To get the permissions.
        /// </summary>
        /// <param name="permissionModels">Permissions to filter.</param>
        /// <returns>Return the permissions list.</returns>
        public List<PermissionModel> GetPermissions(List<PermissionModel> permissionModels)
        {
            return new PermissionDAO().Read(this._connectionStringRead, permissionModels);
        }

        /// <summary>
        /// Validates the permission.
        /// </summary>
        /// <param name="permissionModel">Permission model.</param>
        /// <param name="isUpdate">If it is update or not.</param>
        /// <returns>Returns the permission response detail.</returns>
        private PermissionResponseModel ValidatePermission(PermissionModel permissionModel, bool isUpdate)
        {
            if (permissionModel == null)
            {
                // Return permission required.
                return new PermissionResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_model_required"])
                    },
                };
            }

            if (string.IsNullOrWhiteSpace(permissionModel.Desc))
            {
                // Desc is required
                return new PermissionResponseModel()
                {
                    Desc = permissionModel.Desc,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_desc_required"])
                    },
                    Name = permissionModel.Name,
                    Valid = false,
                };
            }

            if (string.IsNullOrWhiteSpace(permissionModel.Name))
            {
                // Name is required.
                return new PermissionResponseModel()
                {
                    Desc = permissionModel.Desc,
                    Messages = new List<string>{
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_name_required"])
                    },
                    Name = permissionModel.Name,
                    Valid = false,
                };
            }


            if (isUpdate && string.IsNullOrWhiteSpace(permissionModel.Id))
            {
                return new PermissionResponseModel()
                {
                    Desc = permissionModel.Desc,
                    Messages = new List<string>
                    { new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_id_required"]) },
                    Name = permissionModel.Name,
                    Valid = false,
                };
            }

            return new PermissionResponseModel()
            {
                Valid = true,
            };
        }

        /// <summary>
        /// Creates the permission.
        /// </summary>
        /// <param name="permissionModel">Permission details.</param>
        /// <param name="userInSession">Id of user in session.</param>
        /// <returns>Return the permission response with transaction details.</returns>
        public PermissionResponseModel CreatePermission(PermissionModel permissionModel, string userInSession)
        {
            // First validate.
            PermissionResponseModel permissionResponseModelValidation = this.ValidatePermission(permissionModel, false);
            if (!permissionResponseModelValidation.Valid)
            {
                return permissionResponseModelValidation;
            }
            permissionModel.ModifiedBy = userInSession;

            // The create if valid.
            PermissionModel permissionCreate = new PermissionDAO().Create(this._connectionString, permissionModel)[0];
            return new PermissionResponseModel()
            {
                Id = permissionCreate.Id,
                CreatedAt = permissionCreate.CreatedAt,
                Desc = permissionCreate.Desc,
                Messages = new List<string>
                    { new MessageManager(this._connectionString, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_transaction_success"]) },
                ModifiedBy = permissionCreate.ModifiedBy,
                Name = permissionCreate.Name,
                UpdatedAt = permissionCreate.UpdatedAt,
                Valid = true,
            };
        }

        /// <summary>
        /// Updates the permission.
        /// </summary>
        /// <param name="permissionModel">Permission details.</param>
        /// <returns>Return the permission response with transaction details.</returns>
        public PermissionResponseModel UpdatePermission(PermissionModel permissionModel, string userInSession)
        {
            // First validate.
            PermissionResponseModel permissionResponseModelValidation = this.ValidatePermission(permissionModel, true);
            if (!permissionResponseModelValidation.Valid)
            {
                return permissionResponseModelValidation;
            }

            // The create if valid.
            PermissionModel permissionCreate = new PermissionDAO().Update(this._connectionString, new PermissionModel()
            {
                Desc = permissionModel.Desc,
                Name = permissionModel.Name,
                ModifiedBy = userInSession,
            }, new PermissionModel()
            {
                Id = permissionModel.Id,
            })[0];
            return new PermissionResponseModel()
            {
                Id = permissionCreate.Id,
                CreatedAt = permissionCreate.CreatedAt,
                Desc = permissionCreate.Desc,
                Messages = new List<string>
                {  new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_transaction_success"]) },
                ModifiedBy = permissionCreate.ModifiedBy,
                Name = permissionCreate.Name,
                UpdatedAt = permissionCreate.UpdatedAt,
                Valid = true,
            };
        }
    }
}
