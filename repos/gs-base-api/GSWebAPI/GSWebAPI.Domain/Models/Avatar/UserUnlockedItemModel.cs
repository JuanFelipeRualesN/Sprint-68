namespace GSWebAPI.GSWebAPI.Domain.Models.Avatar
{
    public class UserUnlockedItemModel : IResponse
    {
        public UserUnlockedItemModel()
        {
            MessageList = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> MessageList { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AvatarItemCode { get; set; }
        public string AvatarPart { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}