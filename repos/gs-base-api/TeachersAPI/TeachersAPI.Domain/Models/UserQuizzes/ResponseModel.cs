using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.UserQuizzes
{
    /// <summary>
    /// This class is used to send the validation and details from the library to the main web service
    /// </summary>
    public class ResponseModel
    {
        public ResponseModel()
        {
            MessageList = new List<string>();
        }

        private bool _transactionValid;
        private List<string> _messageList;
        private UserQuizResultModel _userQuizResultModel;

        public bool TransactionValid { get => _transactionValid; set => _transactionValid = value; }
        public List<string> MessageList { get => _messageList; set => _messageList = value; }
        public UserQuizResultModel UserQuizResultModel { get => _userQuizResultModel; set => _userQuizResultModel = value; }
    }
}