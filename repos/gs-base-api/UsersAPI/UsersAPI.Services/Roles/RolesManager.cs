using System.Collections.Generic;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Roles;
using UsersAPI.UsersAPI.Services.Generals;

namespace UsersAPI.UsersAPI.Services.Roles
{
    /// <summary>
    /// To handle the roles.
    /// </summary>
    public class RolesManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connections string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language's code.</param>
        public RolesManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// Returns the role types by role type list to filter.
        /// </summary>
        /// <param name="roleTypes">Role type list to filter.</param>
        /// <returns>Return a list of role types.</returns>
        public List<RoleTypeModel> GetRolesByRoleTypeModel(List<RoleTypeModel> roleTypes)
        {
            return new RoleTypeDAO().Read(this._connectionStringRead, roleTypes);
        }

        /// <summary>
        /// Get the role details, including the permissions.
        /// </summary>
        /// <param name="requestModel">Request details.</param>
        /// <returns>Return the reponse with role details.</returns>
        public UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel GetRoleDetails(UsersAPI.Domain.Models.Roles.Request.RoleRequestModel requestModel)
        {
            UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel roleResponseModel = new UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel();

            List<RoleTypeModel> roleTypeModels = new RoleTypeDAO().Read(this._connectionStringRead, new RoleTypeModel() { Id = requestModel.Id, CreatedAt = requestModel.CreatedAt, Desc = requestModel.Desc, Name = requestModel.Name, ModifiedBy = requestModel.ModifiedBy, UpdatedAt = requestModel.UpdatedAt }, true, true);
            if (roleTypeModels.Count > 0)
            {
                roleResponseModel.Id = roleTypeModels[0].Id;
                roleResponseModel.Desc = roleTypeModels[0].Desc;
                roleResponseModel.CreatedAt = roleTypeModels[0].CreatedAt;
                roleResponseModel.UpdatedAt = roleTypeModels[0].UpdatedAt;
                roleResponseModel.Name = roleTypeModels[0].Name;

                roleResponseModel.RolePermissionModelList = new RolePermissionDAO().Read(this._connectionStringRead, new RolePermissionModel() { RoleId = roleResponseModel.Id, }, false, false);

                if (roleResponseModel.RolePermissionModelList.Count > 0)
                {
                    List<PermissionModel> permissionModelsToFind = new List<PermissionModel>();
                    foreach (RolePermissionModel rolePermission in roleResponseModel.RolePermissionModelList)
                    {
                        permissionModelsToFind.Add(new PermissionModel()
                        {
                            Id = rolePermission.PermissionId,
                        });
                    }
                    roleResponseModel.PermissionModelList = new PermissionDAO().Read(this._connectionStringRead, permissionModelsToFind);
                }
            }

            return roleResponseModel;
        }

        /// <summary>
        /// Creates a new role type.
        /// </summary>
        /// <param name="requestModel">The role request details to create.</param>
        /// <param name="userInSession">The id of the user in session.</param>
        /// <returns>Returns a response model with transaction details.</returns>
        public UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel CreateRole(UsersAPI.Domain.Models.Roles.Request.RoleRequestModel requestModel, string userInSession)
        {
            // First validate.
            UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel responseModelValidation = this.ValidateRole(requestModel, false);
            if (!responseModelValidation.Valid)
            {
                return responseModelValidation;
            }

            requestModel.ModifiedBy = userInSession;

            // The create if valid.
            RoleTypeModel roleCreated = new RoleTypeDAO().Create(this._connectionString, requestModel)[0];

            // The create the role permissions
            foreach (RolePermissionModel rolePermission in requestModel.RolePermissionModelList)
            {
                rolePermission.RoleId = roleCreated.Id;
                rolePermission.ModifiedBy = userInSession;
            }

            if (requestModel.RolePermissionModelList.Count > 0)
            {
                requestModel.RolePermissionModelList = new RolePermissionDAO().Create(this._connectionString, requestModel.RolePermissionModelList);
            }

            return new UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel()
            {
                RolePermissionModelList = requestModel.RolePermissionModelList,
                Id = roleCreated.Id,
                CreatedAt = roleCreated.CreatedAt,
                Desc = roleCreated.Desc,
                Messages = new List<string>
                    { new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_transaction_success"]) },
                ModifiedBy = roleCreated.ModifiedBy,
                Name = roleCreated.Name,
                UpdatedAt = roleCreated.UpdatedAt,
                Valid = true,
            };
        }

        /// <summary>
        /// Updates role.
        /// </summary>
        /// <param name="requestModel">The role request details to create.</param>
        /// <param name="userInSession">The id of the user in session.</param>
        /// <returns>Returns a response model with transaction details.</returns>
        public UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel UpdateRole(UsersAPI.Domain.Models.Roles.Request.RoleRequestModel requestModel, string userInSession)
        {
            // First validate.
            UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel responseModelValidation = this.ValidateRole(requestModel, true);
            if (!responseModelValidation.Valid)
            {
                return responseModelValidation;
            }

            requestModel.ModifiedBy = userInSession;

            // The create if valid.
            RoleTypeModel roleUpdated = new RoleTypeDAO().Update(this._connectionString, new RoleTypeModel()
            {
                Desc = requestModel.Desc,
                Name = requestModel.Name,
                ModifiedBy = userInSession,
            }, new RoleTypeModel()
            {
                Id = requestModel.Id,
            })[0];

            // The create the role permissions
            foreach (RolePermissionModel rolePermission in requestModel.RolePermissionModelList)
            {
                rolePermission.RoleId = requestModel.Id;
                rolePermission.ModifiedBy = userInSession;
            }

            if (requestModel.RolePermissionModelList.Count > 0)
            {
                new RolePermissionDAO().Delete(this._connectionString, new RolePermissionModel()
                {
                    RoleId = requestModel.Id,
                });
                requestModel.RolePermissionModelList = new RolePermissionDAO().Create(this._connectionString, requestModel.RolePermissionModelList);
            }

            return new UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel()
            {
                RolePermissionModelList = requestModel.RolePermissionModelList,
                Id = roleUpdated.Id,
                CreatedAt = roleUpdated.CreatedAt,
                Desc = roleUpdated.Desc,
                Messages = new List<string>
                    { new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_transaction_success"]) },
                ModifiedBy = roleUpdated.ModifiedBy,
                Name = roleUpdated.Name,
                UpdatedAt = roleUpdated.UpdatedAt,
                Valid = true,
            };
        }

        /// <summary>
        /// Validation for the role.
        /// </summary>
        /// <param name="roleRequestModel">The role to update or create.</param>
        /// <param name="isUpdate">Whether is an update or not.</param>
        /// <returns>Returns the role with transaction details.</returns>
        private UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel ValidateRole(UsersAPI.Domain.Models.Roles.Request.RoleRequestModel roleRequestModel, bool isUpdate)
        {
            if (roleRequestModel == null)
            {
                // Return permission required.
                return new UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_model_required"])
                    },
                };
            }

            if (string.IsNullOrWhiteSpace(roleRequestModel.Desc))
            {
                // Desc is required
                return new UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel()
                {
                    RolePermissionModelList = roleRequestModel.RolePermissionModelList,
                    Desc = roleRequestModel.Desc,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_desc_required"])
                    },
                    Name = roleRequestModel.Name,
                    Valid = false,
                };
            }

            if (string.IsNullOrWhiteSpace(roleRequestModel.Name))
            {
                return new UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel()
                {
                    RolePermissionModelList = roleRequestModel.RolePermissionModelList,
                    Desc = roleRequestModel.Desc,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_name_required"])
                    },
                    Name = roleRequestModel.Name,
                    Valid = false,
                };
            }

            if (roleRequestModel.RolePermissionModelList == null || roleRequestModel.RolePermissionModelList.Count == 0)
            {
                return new UsersAPI.Domain.Models.Roles.Responses.RoleResponseModel()
                {
                    RolePermissionModelList = roleRequestModel.RolePermissionModelList,
                    Desc = roleRequestModel.Desc,
                    Messages = new List<string>()
                    {
                        new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_permissions_required"])
                    },
                    Name = roleRequestModel.Name,
                    Valid = false,
                };
            }

            if (isUpdate && string.IsNullOrWhiteSpace(roleRequestModel.Id))
            {
                return new Domain.Models.Roles.Responses.RoleResponseModel()
                {
                    RolePermissionModelList = roleRequestModel.RolePermissionModelList,
                    Desc = roleRequestModel.Desc,
                    Messages = new List<string>
                    { new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:ur_roles_id_required"]) },
                    Name = roleRequestModel.Name,
                    Valid = false,
                };
            }

            return new Domain.Models.Roles.Responses.RoleResponseModel()
            {
                Valid = true,
            };
        }
    }
}
