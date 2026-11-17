namespace GSWebAPI.GSWebAPI.Domain.Models.Avatar
{
    public class UserAvatarModel : IResponse
    {
        public UserAvatarModel()
        {
            MessageList = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> MessageList { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Base { get; set; }
        public string BaseHex { get; set; }
        public string Sex { get; set; }
        public string BackAccesory { get; set; }
        public string Eyebrows { get; set; }
        public string Eyes { get; set; }
        public string EyesHex { get; set; }
        public string FacePaint { get; set; }
        public string Glasses { get; set; }
        public string Hair { get; set; }
        public string HairHex { get; set; }
        public string Hat { get; set; }
        public string Jacket { get; set; }
        public string Mask { get; set; }
        public string Mouth { get; set; }
        public string Nose { get; set; }
        public string Pants { get; set; }
        public string Shirt { get; set; }
        public string Shoes { get; set; }
        public string Socks { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}