using System.Collections.Generic;

namespace UsersAPI.UsersAPI.Domain.Models.PersonalDetails
{
    /// <summary>
    /// This class is used to send the validation and details from the library to the main web service
    /// </summary>
    public class ResponseModel
    {
        public ResponseModel()
        {
            Details = new List<string>();
            ErrorCodes = new List<string>();
        }

        private bool _valid;
        private List<string> _details;
        private List<string> _errorCodes;

        public bool Valid { get { return _valid; } set { _valid = value; } }
        public List<string> Details { get { return _details; } set { _details = value; } }
        public List<string> ErrorCodes { get { return _errorCodes; } set { _errorCodes = value; } }
    }
}