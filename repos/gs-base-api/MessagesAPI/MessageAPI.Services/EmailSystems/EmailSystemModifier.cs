using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using MessagesAPI.MessageAPI.Infrastructure.Repositories.EmailSystems;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using MySqlConnector;

namespace MessagesAPI.MessageAPI.Services.EmailSystems
{
    /// <summary>
    /// To handle the communications in the message area.
    /// </summary>
    public class EmailSystemModifier
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language code of the user.</param>
        public EmailSystemModifier(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Sends a message to other users.
        /// </summary>
        /// <param name="emailMessageModel">The message model with the subject, body, etc.</param>
        /// <param name="emailAttachmentModels">The attachments URL.</param>
        /// <param name="emailRecipientModelList">The recipients.</param>
        /// <returns>Returns a response model with the transaction details.</returns>
        public ResponseModel SendMessage(EmailMessageModel emailMessageModel, List<EmailAttachmentModel> emailAttachmentModels, List<EmailMessageRecipientModel> emailRecipientModelList)
        {
            try
            {
                // First, validate.
                ResponseModel responseModel = ValidateEmail(emailMessageModel, emailAttachmentModels, emailRecipientModelList);
                if (!responseModel.Valid)
                {
                    return responseModel;
                }

                // Set the RE from reply if parent message id is not null and parent message id is not 0.
                if (!string.IsNullOrWhiteSpace(emailMessageModel.ParentMessageId) && emailMessageModel.ParentMessageId != "0")
                {
                    emailMessageModel.Subject = "RE: " + emailMessageModel.Subject;
                }

                // Check whether the message record already exists in the database before attempting to create it.
                // This is necessary because SendMessage is called once per page when sending to a large recipient list
                // (e.g. 570 users split into batches of 300). The message header must be created only on the first
                // batch; subsequent batches reuse the existing record and only insert new recipients.
                EmailMessageModel newEmailMessageModel = new EmailMessageModel()
                {
                    Id = emailMessageModel.Id,           // populated after the first batch creates the record
                    AuthorId = emailMessageModel.AuthorId,
                    ParentMessageId = emailMessageModel.ParentMessageId,
                    Body = emailMessageModel.Body,
                    Subject = emailMessageModel.Subject,
                };
                List<EmailMessageModel> validateIfEmailExist = new EmailMessageDAO().Read(_connectionString, newEmailMessageModel, true, false);

                emailMessageModel.ModifiedBy = emailMessageModel.AuthorId;
                if (validateIfEmailExist == null || validateIfEmailExist.Count == 0)
                {
                    // Message does not exist yet — create it and persist attachments.
                    emailMessageModel = new EmailMessageDAO().Create(_connectionString, emailMessageModel)[0];

                    // Attachments belong to the message header, so they are only inserted once.
                    if (emailAttachmentModels != null && emailAttachmentModels.Count > 0)
                    {
                        foreach (EmailAttachmentModel emailAttachmentModel in emailAttachmentModels)
                        {
                            // Strip the SAS token from Azure Blob URLs before storing; the token expires
                            // and the clean URL is what should be persisted long-term.
                            if (!string.IsNullOrEmpty(emailAttachmentModel.AttachmentUrl))
                            {
                                int questionMarkIndex = emailAttachmentModel.AttachmentUrl.IndexOf('?');
                                if (questionMarkIndex > -1)
                                {
                                    emailAttachmentModel.AttachmentUrl = emailAttachmentModel.AttachmentUrl.Substring(0, questionMarkIndex);
                                }
                            }

                            emailAttachmentModel.MessageId = emailMessageModel.Id;
                            emailAttachmentModel.ModifiedBy = emailMessageModel.AuthorId;
                        }

                        new EmailAttachmentDAO().Create(_connectionString, emailAttachmentModels);
                    }
                }
                else
                {
                    // Message already exists (subsequent batch) — reuse the existing record so all
                    // recipients are linked to the same message thread.
                    emailMessageModel = validateIfEmailExist[0];
                }

                // Link every recipient in this batch to the message and mark as unread.
                foreach (EmailMessageRecipientModel emailMessageRecipient in emailRecipientModelList)
                {
                    emailMessageRecipient.MessageId = emailMessageModel.Id;
                    emailMessageRecipient.IsRead = "0";
                    emailMessageRecipient.ModifiedBy = emailMessageModel.AuthorId;
                }

                // Insert all recipients for this batch in a single query for performance.
                BulkInsertRecipients(emailRecipientModelList);

                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                    {
                        new CustomClasses.MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:cm_message_successfully_sent"])
                    }
                };
            }
            catch (Exception)
            {
                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                    {
                        new CustomClasses.MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:cm_message_generic_error"])
                    }
                };
            }
        }

        /// <summary>
        /// Inserts recipients in a single bulk operation instead of individual INSERTs.
        /// </summary>
        /// <param name="recipients">The list of recipients to insert.</param>
        private void BulkInsertRecipients(List<EmailMessageRecipientModel> recipients)
        {
            if (recipients == null || recipients.Count == 0)
                return;

            DBConnection dbConn = new DBConnection(_connectionString);
            MySqlConnection connection = dbConn.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection
                };

                // Build a single multi-row INSERT to avoid the overhead of one round-trip per recipient.
                StringBuilder sb = new StringBuilder(
                    "INSERT INTO cm_email_message_recipients (`recipient_id`, `message_id`, `is_read`, `modified_by`) VALUES ");

                for (int i = 0; i < recipients.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append($"(@recipientId{i}, @messageId{i}, @isRead{i}, @modifiedBy{i})");
                    cmd.Parameters.AddWithValue($"@recipientId{i}", recipients[i].RecipientId);
                    cmd.Parameters.AddWithValue($"@messageId{i}", recipients[i].MessageId);
                    cmd.Parameters.AddWithValue($"@isRead{i}", recipients[i].IsRead);
                    cmd.Parameters.AddWithValue($"@modifiedBy{i}", recipients[i].ModifiedBy);
                }

                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
            finally
            {
                dbConn.CloseConnection(connection);
            }
        }

        /// <summary>
        /// Validates the message.
        /// </summary>
        /// <param name="emailMessageModel">The message model with the subject, body, etc.</param>
        /// <param name="emailAttachmentModels">The attachments' URLs if any.</param>
        /// <param name="recipientModels">The recipients.</param>
        /// <returns>Returns a Response Model with validation details.</returns>
        public ResponseModel ValidateEmail(EmailMessageModel emailMessageModel, List<EmailAttachmentModel> emailAttachmentModels, List<EmailMessageRecipientModel> recipientModels)
        {
            ResponseModel responseModel = new ResponseModel()
            {
                Valid = true,
                MessageCodes = new List<string>(),
                Messages = new List<string>()
            };

            // Validate email
            if (emailMessageModel == null)
            {
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_email_required"]);
            }

            if (string.IsNullOrWhiteSpace(emailMessageModel.AuthorId))
            {
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_author_required"]);
            }

            if (string.IsNullOrWhiteSpace(emailMessageModel.Body))
            {
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_body_required"]);
            }

            if (string.IsNullOrWhiteSpace(emailMessageModel.Subject))
            {
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_subject_required"]);
            }

            if (emailAttachmentModels != null && emailAttachmentModels.Count > 0)
            {
                foreach (EmailAttachmentModel emailAttachmentModel in emailAttachmentModels)
                {
                    if (string.IsNullOrWhiteSpace(emailAttachmentModel.AttachmentUrl))
                    {
                        responseModel.Valid = false;
                        responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_attachment_required"]);
                        break;
                    }
                }
            }

            if (recipientModels == null)
            {
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_recipient_required"]);
            }
            else
            {
                foreach (EmailMessageRecipientModel recipient in recipientModels)
                {
                    if (string.IsNullOrWhiteSpace(recipient.RecipientId))
                    {
                        responseModel.Valid = false;
                        responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_recipient_required"]);
                        break;
                    }
                }
            }

            if (!responseModel.Valid)
            {
                responseModel.Messages = new CustomClasses.MessageManager(_connectionString, _langCode).GetMessageDetails(responseModel.MessageCodes);
            }

            return responseModel;
        }
    }
}
