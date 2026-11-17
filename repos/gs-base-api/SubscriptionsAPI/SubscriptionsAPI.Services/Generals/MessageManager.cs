using System.Collections.Generic;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Generals;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Generals
{
    /// <summary>
    /// To handle the messages from the DB.
    /// </summary>
    public class MessageManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code.</param>
        public MessageManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To get the message details.
        /// </summary>
        /// <param name="code">The code to find.</param>
        /// <returns>Returns the message string.</returns>
        public string GetMessageDetails(string code)
        {
            MessageModel messageModel = new MessageModel
            {
                LangCode = _langCode,
                Code = code
            };

            List<MessageModel> messageModelList = new MessageDAO().Read(_connectionStringRead, messageModel, true, true);

            if (messageModelList.Count == 0)
            {
                return null;
            }
            else
            {
                return messageModelList[0].Desc;
            }
        }

        /// <summary>
        /// To get the messages details list.
        /// </summary>
        /// <param name="codeList">The code list to find.</param>
        /// <returns>Returns a string of messages.</returns>
        public List<string> GetMessageDetails(List<string> codeList)
        {
            List<MessageModel> messageModelList = new List<MessageModel>();

            for (int i = 0; i < codeList.Count; i++)
            {
                MessageModel messageModel = new MessageModel
                {
                    LangCode = _langCode,
                    Code = codeList[i]
                };
                messageModelList.Add(messageModel);
            }

            List<MessageModel> messageModelListResult = new List<MessageModel>();

            MessageDAO messageDAO = new MessageDAO();
            messageModelListResult = messageDAO.Read(_connectionStringRead, messageModelList);

            if (messageModelListResult.Count == 0)
                return null;
            else
            {
                List<string> resultList = new List<string>();
                for (int i = 0; i < messageModelListResult.Count; i++)
                {
                    resultList.Add(messageModelListResult[i].Desc);
                }
                return resultList;
            }
        }
    }
}
