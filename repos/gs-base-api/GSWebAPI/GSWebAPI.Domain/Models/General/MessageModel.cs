using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General
{
    public class MessageModel
    {
        private string _id;
        private string _code;
        private string _desc;
        private string _langCode;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string Code { get { return _code; } set { _code = value; } }
        public string Desc { get { return _desc; } set { _desc = value; } }
        public string LangCode { get { return _langCode; } set { _langCode = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}