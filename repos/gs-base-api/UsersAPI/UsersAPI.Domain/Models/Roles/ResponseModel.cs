using System.Collections.Generic;

namespace UsersAPI.UsersAPI.Domain.Models.Roles
{
    /// <summary>
    /// This class is used to send the validation and details from the library to the main web service
    /// </summary>
    public class ResponseModel
    {
        private bool _valid;
        private string _userId; 
        private List<string> _details;
        private List<string> _errorCodes; 

        public bool Valid { get { return _valid; } set { _valid = value; } }
        public string UserId {  get { return _userId; } set { _userId = value; } }
        public List<string> Details { get { return _details; } set { _details = value; } }
        public List<string> ErrorCodes {  get { return _errorCodes; } set { _errorCodes = value;  } }
    }
}
