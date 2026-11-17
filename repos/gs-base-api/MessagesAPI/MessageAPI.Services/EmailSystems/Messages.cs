using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySqlConnector;
using Dapper;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;

namespace MessagesAPI.MessageAPI.Services.EmailSystems
{
    /// <summary>
    /// To handle the messages.
    /// </summary>
    public static class Messages
    {
        private readonly static string connectionStringRead = AppConfiguration.Configuration["ConnectionStrings:EmailSystemManagerConnectionStringReader"];
        private readonly static string connectionStringWrite = AppConfiguration.Configuration["ConnectionStrings:EmailSystemManagerConnectionString"];

        /// <summary>
        /// To get the unread messages total.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the total of unread messages (int).</returns>
        public static int SearchUnreadMessagesTotal(string userId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { userId };
            string query = @"SELECT COUNT(*)
                FROM gs_communication_db_prod.cm_email_messages em
                INNER JOIN gs_communication_db_prod.cm_email_message_recipients re ON re.message_id = em.id
                WHERE re.recipient_id = @userId AND re.is_read = 0;";

            IEnumerable<int> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = connectionStringRead
            };
            connection.Open();

            try
            {
                items = connection.Query<int>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            if (items == null || items.AsList().Count == 0)
            {
                return 0;
            }

            return items.AsList()[0];

        }

        /// <summary>
        /// To search for the received messages of the user.
        /// </summary>
        /// <param name="userId">The user's id to find the messages.</param>
        /// <param name="fromPage">From what page.</param>
        /// <param name="limit">The limit of items per page.</param>
        /// <returns>The email response model with total items and messages details.</returns>
        public static ResponseModel SearchReceivedMessages(string userId, int fromPage, int limit)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { userId };
            string query = @"SELECT  ms.id, ms.subject, ms.body, ms.date_created, ms.parent_message_id, ms.author_id, re.is_read as `read`
                FROM gs_communication_db_prod.cm_email_messages ms 
                INNER JOIN `gs_communication_db_prod`.`cm_email_message_recipients` re ON re.message_id = ms.id
                WHERE re.recipient_id = @userId
                ORDER BY ms.date_created DESC
                ";

            IEnumerable<EmailMessageExtraDetail> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = connectionStringRead
            };
            connection.Open();
            try
            {
                items = connection.Query<EmailMessageExtraDetail>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            int totalItems = items != null ? items.Count() : 0;
            return new MessagesAPI.MessageAPI.Domain.Models.EmailSystem.ResponseModel()
            {
                Total = totalItems,
                EmailMessageModelList = items != null && items.Count() > 0 ? items.Skip(fromPage * limit).Take(limit > totalItems ? totalItems : limit).ToList() : items.ToList()
            };
        }

        /// <summary>
        /// To search for the sent messages by the user.
        /// </summary>
        /// <param name="userId">The user's id to find the messages.</param>
        /// <param name="fromPage">From what page.</param>
        /// <param name="limit">The limit of items per page.</param>
        /// <returns>The email response model with total items and messages details.</returns>
        public static ResponseModel SearchSentMessages(string userId, int fromPage, int limit)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT  ms.id, ms.subject, ms.body, ms.date_created, ms.parent_message_id, ms.author_id, 1 as `read`
                FROM gs_communication_db_prod.cm_email_messages ms 
                WHERE ms.author_id = @userId 
                ORDER BY ms.date_created DESC
                ";
            var parameters = new { userId };

            IEnumerable<EmailMessageExtraDetail> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = connectionStringRead
            };
            connection.Open();

            try
            {
                items = connection.Query<EmailMessageExtraDetail>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            int totalItems = items != null ? items.Count() : 0;
            return new MessagesAPI.MessageAPI.Domain.Models.EmailSystem.ResponseModel()
            {
                Total = totalItems,
                EmailMessageModelList = items != null && items.Count() > 0 ? items.Skip(fromPage * limit).Take(limit > totalItems ? totalItems : limit).ToList() : items.ToList()
            };
        }

        /// <summary>
        /// To get the full detail email message.
        /// </summary>
        /// <param name="userId">The recipient's id or autor's id</param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static EmailMessageFullDetailModel GetMessageDetail(string userId, string messageId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var parameters = new { messageId };
            string queryEmailMessage = @"SELECT id, subject, body, date_created, parent_message_id, author_id FROM gs_communication_db_prod.cm_email_messages WHERE id = @messageId; ";
            string queryEmailRecipients = @"SELECT id, recipient_id FROM gs_communication_db_prod.cm_email_message_recipients WHERE message_id = @messageId; ";
            string queryAttachments = @"SELECT id, attachment_name, attachment_url FROM gs_communication_db_prod.cm_email_attachments WHERE message_id = @messageId; ";

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = connectionStringRead
            };
            connection.Open();
            var message = connection.Query<EmailMessageModel>(queryEmailMessage, parameters);
            var emailMessageRecipients = connection.Query<EmailMessageRecipientModel>(queryEmailRecipients, parameters);
            var emailAttachments = connection.Query<EmailAttachmentModel>(queryAttachments, parameters);
            connection.Close();


            EmailMessageFullDetailModel emailFullDetail = new EmailMessageFullDetailModel()
            {
                EmailAttachmentModelList = emailAttachments != null ? emailAttachments.ToList() : new List<EmailAttachmentModel>(),
                EmailMessageRecipientModelList = emailMessageRecipients != null ? emailMessageRecipients.ToList() : new List<EmailMessageRecipientModel>(),
                EmailMessageModel = message != null ? message.First() : new EmailMessageModel()
            };

            // Check if user can access to the details
            if (emailFullDetail.EmailMessageRecipientModelList.Find(x => x.RecipientId == userId) != null || emailFullDetail.EmailMessageModel.AuthorId == userId)
            {
                // Prepare the things to return
                return emailFullDetail;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// To mark the message as read.
        /// </summary>
        /// <param name="messageId">The message's id.</param>
        /// <param name="userInSession">The user in session.</param>
        public static void MarkMessageAsRead(string messageId, string userInSession)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"UPDATE `gs_communication_db_prod`.`cm_email_message_recipients`
            SET `is_read` = 1,
            `date_read` = @dateRead,
            `modified_by` = @userInSession
            WHERE `message_id` = @messageId AND `recipient_id` = @userInSession;
            ";
            var parameters = new { dateRead = DateTime.Now, userInSession, messageId };
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = connectionStringWrite
            };
            connection.Open();

           
                try
                {
                    connection.Execute(query, parameters);
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();
                }
            
        }
    }
}
