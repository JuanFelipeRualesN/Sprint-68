namespace GSWebAPI.GSWebAPI.Domain.Models.MiniGame
{
    public class GameModel
    {
        private string _id;
        private string _code;
        private string _unlockedValue;
        private string _unlockedType;
        private string _playValue;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string Code { get { return _code; } set { _code = value; } }
        public string UnlockedValue { get { return _unlockedValue; } set { _unlockedValue = value; } }
        public string UnlockedType { get { return _unlockedType; } set { _unlockedType = value; } }
        public string PlayValue { get { return _playValue; } set { _playValue = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}