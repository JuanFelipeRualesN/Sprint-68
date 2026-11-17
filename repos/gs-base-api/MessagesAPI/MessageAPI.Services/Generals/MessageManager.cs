using MessagesAPI.MessageAPI.Domain.Models.General;
using MessagesAPI.MessageAPI.Infrastructure.Repositories.Generals;

namespace MessagesAPI.MessageAPI.Services.EmailSystems.CustomClasses
{
    /// <summary>
    /// To handle the messages that are returned to the user.
    /// </summary>
    public class MessageManager
    {
        private readonly string _connectionString;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="langCode">The language code.</param>
        public MessageManager(string connectionString, string langCode)
        {
            _connectionString = connectionString;
            _langCode = langCode;
        }

        /// <summary>
        /// To get the message details from the database.
        /// </summary>
        /// <param name="code">The message's code.</param>
        /// <returns>Returns the message string.</returns>
        public string GetMessageDetails(string code)
        {
            var messageModel = new MessageModel
            {
                LangCode = _langCode,
                Code = code
            };

            var messageModelList = new MessageDAO().Read(_connectionString, messageModel, true, true);

            if (messageModelList.Count == 0)
                return null;
            else
                return messageModelList[0].Desc;
        }

        /// <summary>
        /// To get the message details by finding a code of list.
        /// </summary>
        /// <param name="codeList">The code list to find.</param>
        /// <returns>Returns a list of string of the messages.</returns>
        public List<string> GetMessageDetails(List<string> codeList)
        {
            var messageModelList = new List<MessageModel>();

            for (int i = 0; i < codeList.Count; i++)
            {
                MessageModel messageModel = new MessageModel
                {
                    LangCode = _langCode,
                    Code = codeList[i]
                };
                messageModelList.Add(messageModel);
            }

            var messageModelListResult = new MessageDAO().Read(_connectionString, messageModelList);

            if (messageModelListResult.Count == 0)
                return null;
            else
            {
                var resultList = new List<string>();
                for (int i = 0; i < messageModelListResult.Count; i++)
                {
                    resultList.Add(messageModelListResult[i].Desc);
                }
                return resultList;
            }
        }
    }
}