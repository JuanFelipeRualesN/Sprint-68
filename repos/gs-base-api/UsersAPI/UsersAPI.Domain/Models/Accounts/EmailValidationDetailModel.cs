namespace UsersAPI.UsersAPI.Domain.Models.Accounts
{
    public class EmailValidationDetailModel
    {
        private string _id;
        private string _subject;
        private string _header;
        private string _body;
        private string _url;
        private string _footer;
        private string _langCode;
        private string _emailTokenTypeId;
        private string _modifiedBy;
        private string _CreatedAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string Subject { get { return _subject; } set { _subject = value; } }
        public string Header { get { return _header; } set { _header = value; } }
        public string Body { get { return _body; } set { _body = value; } }
        public string Url { get { return _url; } set { _url = value; } }
        public string Footer { get { return _footer; } set { _footer = value; } }
        public string LangCode { get { return _langCode; } set { _langCode = value; } }
        public string EmailTokenTypeId { get { return _emailTokenTypeId; } set { _emailTokenTypeId = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _CreatedAt; } set { _CreatedAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}