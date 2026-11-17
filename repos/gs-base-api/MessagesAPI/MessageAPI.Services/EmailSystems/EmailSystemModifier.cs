using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MessagesAPI.MessageAPI.Infrastructure.Repositories.EmailSystems;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;

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

                // If everything is alright, then send it.
                emailMessageModel.ModifiedBy = emailMessageModel.AuthorId;
                emailMessageModel = new EmailMessageDAO().Create(_connectionString, emailMessageModel)[0];

                if (emailAttachmentModels != null && emailAttachmentModels.Count > 0)
                {
                    foreach (EmailAttachmentModel emailAttachmentModel in emailAttachmentModels)
                    {
                        emailAttachmentModel.MessageId = emailMessageModel.Id;
                        emailAttachmentModel.ModifiedBy = emailMessageModel.AuthorId;
                    }

                    new EmailAttachmentDAO().Create(_connectionString, emailAttachmentModels);
                }

                foreach (EmailMessageRecipientModel emailMessageRecipient in emailRecipientModelList)
                {
                    emailMessageRecipient.MessageId = emailMessageModel.Id;
                    emailMessageRecipient.IsRead = "0";
                    emailMessageRecipient.ModifiedBy = emailMessageModel.AuthorId;
                }

                new EmailMessageRecipientDAO().Create(_connectionString, emailRecipientModelList);

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
                // Email is Required
                // Return inmediatly
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_email_required"]);
            }

            if (string.IsNullOrWhiteSpace(emailMessageModel.AuthorId))
            {
                // User required
                // Return inmediately
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_author_required"]);
            }

            if (string.IsNullOrWhiteSpace(emailMessageModel.Body))
            {
                // Body required
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_body_required"]);
            }

            if (string.IsNullOrWhiteSpace(emailMessageModel.Subject))
            {
                // Subject required
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_subject_required"]);
            }

            if (emailAttachmentModels != null && emailAttachmentModels.Count > 0)
            {
                foreach (EmailAttachmentModel emailAttachmentModel in emailAttachmentModels)
                {
                    if (string.IsNullOrWhiteSpace(emailAttachmentModel.AttachmentUrl))
                    {
                        // URL Required
                        responseModel.Valid = false;
                        responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_attachment_required"]);
                        break;
                    }
                }
            }

            if (recipientModels == null)
            {
                // Recipient required
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:cm_message_code_recipient_required"]);
            }
            else
            {
                foreach (EmailMessageRecipientModel recipient in recipientModels)
                {

                    if (string.IsNullOrWhiteSpace(recipient.RecipientId))
                    {
                        // Recipient required
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
