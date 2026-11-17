using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.GameUserReward
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            MessageList = new List<string>();
        }
        private bool _transactionValid;
        private List<string> _messageList;
        private RewardCodeModel _rewardCodeModel;
        private UserRewardModel _userRewardModel;
        private RewardBundleModel _rewardBundleModel;
        private List<string> _rewardGeneratedCodes;

        public bool TransactionValid { get => _transactionValid; set => _transactionValid = value; }
        public List<string> MessageList { get => _messageList; set => _messageList = value; }
        public RewardCodeModel RewardCodeModel { get => _rewardCodeModel; set => _rewardCodeModel = value; }
        public UserRewardModel UserRewardModel { get => _userRewardModel; set => _userRewardModel = value; }
        public RewardBundleModel RewardBundleModel { get => _rewardBundleModel; set => _rewardBundleModel = value; }
        public List<string> RewardGeneratedCodes { get => _rewardGeneratedCodes; set => _rewardGeneratedCodes = value; }
    }
}
