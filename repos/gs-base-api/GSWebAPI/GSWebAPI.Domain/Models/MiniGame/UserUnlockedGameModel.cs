namespace GSWebAPI.GSWebAPI.Domain.Models.MiniGame
{
    public class UserUnlockedGameModel
    {
        private string _id;
        private string _userId;
        private string _gameCode;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string GameCode { get { return _gameCode; } set { _gameCode = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}