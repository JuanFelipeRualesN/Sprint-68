namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Emails
{
    public class EmailSettingModel
    {
        private string _id;
        private string _mailDomainCom;
        private string _emailSender;
        private string _senderPassword;
        private string _emailPort;
        private string _defaultCredential;
        private string _enableSsl;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string MailDomainCom { get { return _mailDomainCom; } set { _mailDomainCom = value; } }
        public string EmailSender { get { return _emailSender; } set { _emailSender = value; } }
        public string SenderPassword { get { return _senderPassword; } set { _senderPassword = value; } }
        public string EmailPort { get { return _emailPort; } set { _emailPort = value; } }
        public string DefaultCredential { get { return _defaultCredential; } set { _defaultCredential = value; } }
        public string EnableSsl { get { return _enableSsl; } set { _enableSsl = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}