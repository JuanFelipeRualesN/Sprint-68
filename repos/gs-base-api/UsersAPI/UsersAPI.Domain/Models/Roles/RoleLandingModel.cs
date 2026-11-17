namespace UsersAPI.UsersAPI.Domain.Models.Roles
{
    public class RoleLandingModel
    {
        private string _id;
        private string _roleTypeId;
        private string _landing;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string RoleTypeId { get { return _roleTypeId; } set { _roleTypeId = value; } }
        public string Landing { get { return _landing; } set { _landing = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}