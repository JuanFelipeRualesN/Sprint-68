namespace UsersAPI.UsersAPI.Domain.Models.Roles
{
    public class UserRoleModel
    {
        private string _id;
        private string _roleId;
        private string _userId;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string RoleId { get { return _roleId; } set { _roleId = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}