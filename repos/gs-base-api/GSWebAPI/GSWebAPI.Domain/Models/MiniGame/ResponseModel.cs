using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.MiniGame
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            MessageList = new List<string>();
        }
        private bool _transactionValid;
        private List<string> _messageList;
        private List<GameResponseModel> _gameResponseModelList;

        public bool TransactionValid { get => _transactionValid; set => _transactionValid = value; }
        public List<string> MessageList { get => _messageList; set => _messageList = value; }
        public List<GameResponseModel> GameResponseModelList { get => _gameResponseModelList; set => _gameResponseModelList = value; }
    }
}