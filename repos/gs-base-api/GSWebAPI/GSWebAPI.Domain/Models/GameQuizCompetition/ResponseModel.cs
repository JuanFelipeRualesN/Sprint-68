using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            MessageList = new List<string>();
        }

        private bool _transactionValid;
        private List<string> _messageList;

        public bool TransactionValid { get => _transactionValid; set => _transactionValid = value; }
        public List<string> MessageList { get => _messageList; set => _messageList = value; }
    }
}
