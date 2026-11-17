using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals;
using GSWebAPI.GSWebAPI.Domain.Models.General;

namespace GSWebAPI.CustomClasses
{
    /// <summary>
    /// Manages the messages of the service.
    /// </summary>
    public class MessageManager
    {
        private readonly string _connectionString;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="langCode">Language code.</param>
        public MessageManager(string connectionString, string langCode)
        {
            _connectionString = connectionString;
            _langCode = langCode;
        }

        /// <summary>
        /// Get the message details.
        /// </summary>
        /// <param name="code">The message code.</param>
        /// <returns>Return the message details.</returns>
        public string GetMessageDetails(string code)
        {
            MessageModel messageModel = new MessageModel
            {
                LangCode = _langCode,
                Code = code,
            };

            List<MessageModel> messageModelList = new MessageDAO().Read(_connectionString, messageModel, true, true);

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
        /// Get the message details by code list.
        /// </summary>
        /// <param name="codeList">The list of codes.</param>
        /// <returns>Return a list of messages.</returns>
        public List<string> GetMessageDetails(List<string> codeList)
        {
            List<MessageModel> messageModelList = new List<MessageModel>();

            for (int i = 0; i < codeList.Count; i++)
            {
                MessageModel messageModel = new MessageModel
                {
                    LangCode = _langCode,
                    Code = codeList[i],
                };
                messageModelList.Add(messageModel);
            }

            List<MessageModel> messageModelListResult = new MessageDAO().Read(_connectionString, messageModelList);

            if (messageModelListResult.Count == 0)
            {
                return null;
            }
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
