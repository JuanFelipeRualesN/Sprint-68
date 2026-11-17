using System;
using System.Collections.Generic;
using System.Configuration;
using MessagesAPI.CustomClasses;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using MessagesAPI.MessageAPI.Domain.Models.General.APIs;
using MessagesAPI.MessageAPI.Domain.Models.General;
using MessagesAPI.MessageAPI.Services.Emails;
using MessagesAPI.MessageAPI.Services.EmailSystems;
using MessagesAPI.MessageAPI.Services.FeignAPIs;

using System.Linq;
using MessagesAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagesAPI.Controllers
{
    /// <summary>
    /// To handle messages (as emails).
    /// </summary>
    [ApiController]
    [Route("messagesapi")]
    public class MessageController : ControllerBase
    {
        private readonly static string connectionStringRead = AppConfiguration.Configuration["ConnectionStrings:EmailSystemManagerConnectionStringReader"];
        private readonly static string connectionStringWrite = AppConfiguration.Configuration["ConnectionStrings:EmailSystemManagerConnectionString"];
        private readonly static string configNotificationEmail = AppConfiguration.Configuration["AppSettings:notification_email"];
        private readonly static string configNotificationEmailBody = AppConfiguration.Configuration["AppSettings:notification_email_body"];
        private readonly static string configMessageMessageSent = AppConfiguration.Configuration["AppSettings:cm_message_successfully_sent"];
        
        /// <summary>
        /// To send a message by the selected role. This will also send an email.
        /// For example, if we want to send a message for students, we chose the students role the message
        /// will be sent to them. Note: If the user is a principal (director), the message will be sent only to the users of their school. 
        /// The same with the ORE Admin, which will be sent to the users of their ORE. 
        /// The Admin messages will be sent global.
        /// </summary>
        /// <param name="request">The message request model with details as recipients (in this case roles), subject, body, among others.</param>
        /// <param name="sendEmail">To send emails.</param>
        /// <returns>A response with transaction details. See ResponseModel for more information.</returns>
        [HttpPost("messages/send-by-role")]
        public async Task<IActionResult> SendMessageByRole([FromBody] MessageRequestModel request, [FromQuery] bool sendEmail = false)
        {
            try
            {
                // Check the access token
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "CommunicationMessage");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                if (string.IsNullOrWhiteSpace(request.RoleId) ||
                    request.EmailMessageFullDetailModel?.EmailMessageModel == null ||
                    string.IsNullOrWhiteSpace(request.EmailMessageFullDetailModel.EmailMessageModel.Subject))
                {
                    return BadRequest();
                }

                // Assign author
                request.EmailMessageFullDetailModel.EmailMessageModel.AuthorId = responseSession.UserId;

                _ = Task.Run(() =>
                {
                    bool done = false;
                    int pageFrom = 0;
                    const int limiter = 300;

                    while (!done)
                    {
                        var userEmailResponse = GetUserDetails.GetUsersIdsThatUserCanHandleByRole(
                            responseSession.Token, request.RoleId, pageFrom, limiter);
                        pageFrom++;

                        if (userEmailResponse == null || userEmailResponse.Count == 0)
                            done = true;

                        var emailsRecipients = new List<string>();
                        var emailRecipientsAllowed = new List<EmailMessageRecipientModel>();

                        foreach (var userEmail in userEmailResponse)
                        {
                            emailRecipientsAllowed.Add(new EmailMessageRecipientModel
                            {
                                RecipientId = userEmail.UserId
                            });

                            if (!string.IsNullOrWhiteSpace(userEmail.Email))
                            {
                                emailsRecipients.Add(userEmail.Email);
                            }
                        }

                        var emailSystemModifier = new EmailSystemModifier(
                            connectionStringWrite,
                            connectionStringRead,
                            RequestHandler.GetLanguage(HttpContext));

                        var responseEmail = emailSystemModifier.SendMessage(
                            request.EmailMessageFullDetailModel.EmailMessageModel,
                            request.EmailMessageFullDetailModel.EmailAttachmentModelList ?? new List<EmailAttachmentModel>(),
                            emailRecipientsAllowed
                        );

                        if (emailsRecipients.Count > 0 && sendEmail)
                        {
                            try
                            {
                                EmailManager.SendEmailSimple(
                                    connectionStringRead,
                                    emailsRecipients,
                                    configNotificationEmail + request.EmailMessageFullDetailModel?.EmailMessageModel?.Subject,
                                    configNotificationEmailBody + "\"" + request.EmailMessageFullDetailModel?.EmailMessageModel?.Body + "\""
                                );
                            }
                            catch
                            {
                                // Silent failure for notifications
                            }
                        }
                    }
                });

                var responseModel = new MessagesAPI.MessageAPI.Domain.Models.General.ResponseModel
                {
                    SessionValid = true,
                    TransactionValid = true,
                    Messages = new List<string>
            {
                new MessagesAPI.MessageAPI.Services.EmailSystems.CustomClasses.MessageManager(
                    connectionStringRead,
                    RequestHandler.GetLanguage(HttpContext)
                ).GetMessageDetails(configMessageMessageSent)
            }
                };

                return StatusCode(StatusCodes.Status201Created, responseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To send a message and an email to the users. The users will be validated if the main user can send the message to them.
        /// For Swagger, ignore the "RoleId" property, since it reuse the same example model. We cannot specify a specific example, and it is 
        /// a known issue: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="request">The message request model with details as recipients, subject, body, among others.</param>
        /// <param name="sendEmail">To send email.</param>
        /// <returns>A response with transaction details. See ResponseModel for more information.</returns>
        [HttpPost("messages/send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequestModel request, [FromQuery] bool sendEmail = false)
        {
            try
            {
                // Validate the access token
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "CommunicationMessage");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                request.EmailMessageFullDetailModel.EmailMessageModel.AuthorId = responseSession.UserId;

                var emailRecipientsAllowed = new List<EmailMessageRecipientModel>();
                var userIdsToCall = new List<string>();

                // If it is a response
                if (!string.IsNullOrWhiteSpace(request.EmailMessageFullDetailModel.EmailMessageModel.ParentMessageId) &&
                    request.EmailMessageFullDetailModel.EmailMessageModel.ParentMessageId != "0")
                {
                    emailRecipientsAllowed = request.EmailMessageFullDetailModel.EmailMessageRecipientModelList;
                }
                else
                {
                    var profileConnectionModels = GetUserDetails.GetUsersThatUserCanInteractWith(responseSession.Token);

                    if (profileConnectionModels == null || profileConnectionModels.Count == 0)
                    {
                        return Unauthorized();
                    }

                    foreach (var emailRecipient in request.EmailMessageFullDetailModel.EmailMessageRecipientModelList)
                    {
                        var profileConnection = profileConnectionModels.Find(x =>
                            x.UserIdA == emailRecipient.RecipientId || x.UserIdB == emailRecipient.RecipientId);

                        if (profileConnection != null && !emailRecipientsAllowed.Any(x => x.RecipientId == emailRecipient.RecipientId))
                        {
                            emailRecipientsAllowed.Add(emailRecipient);
                            userIdsToCall.Add(emailRecipient.RecipientId);
                        }
                    }
                }

                if (emailRecipientsAllowed.Count == 0)
                {
                    return Unauthorized();
                }

                var emailSystemModifier = new EmailSystemModifier(
                    connectionStringWrite,
                    connectionStringRead,
                    RequestHandler.GetLanguage(HttpContext));

                var responseEmail = emailSystemModifier.SendMessage(
                    request.EmailMessageFullDetailModel.EmailMessageModel,
                    request.EmailMessageFullDetailModel.EmailAttachmentModelList ?? new List<EmailAttachmentModel>(),
                    emailRecipientsAllowed);

                if (sendEmail)
                {
                    _ = Task.Run(() =>
                    {
                        if (userIdsToCall.Count > 0)
                        {
                            var userEmails = GetUserDetails.GetUserEmailModels(userIdsToCall, responseSession.Token);
                            var userEmailsList = userEmails
                                .Where(e => !string.IsNullOrWhiteSpace(e.Email))
                                .Select(e => e.Email)
                                .ToList();

                            try
                            {
                                EmailManager.SendEmailSimple(
                                    connectionStringRead,
                                    userEmailsList,
                                    configNotificationEmail + request.EmailMessageFullDetailModel?.EmailMessageModel?.Subject,
                                    configNotificationEmailBody + "\"" + request.EmailMessageFullDetailModel?.EmailMessageModel?.Body + "\""
                                );
                            }
                            catch
                            {
                                // Silent catch for optional notifications
                            }
                        }
                    });
                }

                var responseModel = new MessagesAPI.MessageAPI.Domain.Models.General.ResponseModel
                {
                    SessionValid = true,
                    TransactionValid = responseEmail.Valid,
                    Messages = responseEmail.Messages
                };

                if (!responseModel.TransactionValid)
                {
                    return BadRequest(responseModel);
                }

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To retrieve the received messages by the current user.
        /// </summary>
        /// <param name="fromPage">The current page to read (for pagination purposes).</param>
        /// <param name="limit">How many objects per page. Example: 10 items.</param>
        /// <param name="langCode">The language's code of the user's interface. Example: en for English. Note: This should be removed in the future and use the header's application language.</param>
        /// <returns>A response with messages' details. See MessageResponseModel for more information.</returns>
        [HttpGet("messages/received/{fromPage}/{limit}/{langCode}")]
        public async Task<IActionResult> GetReceivedMessages(int fromPage, int limit, string langCode)
        {
            try
            {
                // Validate the access token
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "CommunicationMessage");

                if (limit > 20)
                    limit = 20;

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get received messages
                var emailReadResultsResponse = Messages.SearchReceivedMessages(responseSession.UserId, fromPage, limit);

                if (emailReadResultsResponse == null ||
                    emailReadResultsResponse.EmailMessageModelList == null ||
                    emailReadResultsResponse.EmailMessageModelList.Count == 0)
                {
                    return Ok(new MessageResponseModel
                    {
                        TransactionValid = true,
                        SessionValid = true,
                        EmailMessageModelList = new List<EmailMessageModel>(),
                        TotalPages = 0
                    });
                }

                // Prepare author IDs for lookup
                var userIdsFullNameToGet = emailReadResultsResponse.EmailMessageModelList
                    .Select(e => e.AuthorId)
                    .Distinct()
                    .ToList();

                UserAdvanceResponseModel userSharedDetailsResponseModel = new UserAdvanceResponseModel();
                try
                {
                    userSharedDetailsResponseModel = GetUserDetails.GetUserFullNamesAPI(responseSession.Token, userIdsFullNameToGet);
                }
                catch
                {
                    // Not critical if this fails
                }

                var messageResponseModel = new MessageResponseModel
                {
                    TransactionValid = true,
                    SessionValid = true,
                    EmailMessageModelList = new List<EmailMessageModel>(),
                    TotalPages = (int)Math.Ceiling((double)emailReadResultsResponse.Total / limit)
                };

                foreach (var emailMessage in emailReadResultsResponse.EmailMessageModelList)
                {
                    var userFullNameModel = userSharedDetailsResponseModel?.UserInfo?.Find(x => x.UserId == emailMessage.AuthorId);

                    messageResponseModel.EmailMessageModelList.Add(new EmailMessageModel
                    {
                        Id = emailMessage.Id,
                        ParentMessageId = emailMessage.ParentMessageId,
                        Subject = emailMessage.Subject,
                        DateCreated = emailMessage.DateCreated,
                        UserFullName = userFullNameModel != null
                            ? $"{userFullNameModel.GivenName} {userFullNameModel.Surname}"
                            : null,
                        Read = emailMessage.Read
                    });
                }

                return Ok(messageResponseModel);
            }
            catch
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// To get the sent messages by the user.
        /// </summary>
        /// <param name="fromPage">The current page to read (for pagination purposes).</param>
        /// <param name="limit">How many objects per page. Example: 10 items.</param>
        /// <param name="langCode">The language's code of the user's interface. Example: en for English. Note: This should be removed in the future and use the header's application language.</param>
        /// <returns> A response with message details. See MessageResponseModel for more information. Use only the total pages' property.</returns>
        [HttpGet("messages/sent/{fromPage}/{limit}/{langCode}")]
        public async Task<IActionResult> GetSentMessages(int fromPage, int limit, string langCode)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "CommunicationMessage");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                if (limit > 20)
                    limit = 20;

                var emailReadResultsResponse = Messages.SearchSentMessages(responseSession.UserId, fromPage, limit);

                var messageResponseModel = new MessageResponseModel
                {
                    TransactionValid = true,
                    SessionValid = true,
                    EmailMessageModelList = new List<EmailMessageModel>(),
                    TotalPages = (int)Math.Ceiling((double)emailReadResultsResponse.Total / limit)
                };

                foreach (var emailMessage in emailReadResultsResponse.EmailMessageModelList)
                {
                    messageResponseModel.EmailMessageModelList.Add(new EmailMessageModel
                    {
                        Id = emailMessage.Id,
                        ParentMessageId = emailMessage.ParentMessageId,
                        Subject = emailMessage.Subject,
                        DateCreated = emailMessage.DateCreated,
                        Read = true
                    });
                }

                return Ok(messageResponseModel);
            }
            catch
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// To retrieve the unread messages' quantity.
        /// </summary>
        /// <returns>Returns a HTTP Response Message with the unread messages total.</returns>
        [HttpGet("messages/unreads")]
        public async Task<IActionResult> GetUnreadMessages()
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "CommunicationMessage");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var messageResponseModel = new TotalUnreadResponseModel
                {
                    TransactionValid = true,
                    SessionValid = true,
                    TotalUnread = Messages.SearchUnreadMessagesTotal(responseSession.UserId)
                };

                return Ok(messageResponseModel);
            }
            catch
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// To mark a message as read.
        /// </summary>
        /// <param name="messageId">The message's id to mark as read.</param>
        /// <param name="langCode">The language code of the user's interface. This should be removed in the future and use the header's application's language.</param>
        /// <returns>A general response with transaction details.</returns>
        [HttpPost("messages/mark-read/{messageId}/{langCode}")]
        public async Task<IActionResult> MarkMessageAsRead(string messageId, string langCode)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "CommunicationMessage");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Run in background (non-blocking)
                _ = Task.Run(() =>
                {
                    Messages.MarkMessageAsRead(messageId, responseSession.UserId);
                });

                // Return 200 OK
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// The message's id to get the details.
        /// </summary>
        /// <param name="messageId">The message's id to get the details.</param>
        /// <returns>Includes the message details like sender, subject, body, attachment if any, etc.</returns>
        [HttpGet("messages/{messageId}")]
        public async Task<IActionResult> GetMessageDetails(string messageId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "CommunicationMessage");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var messageResponseModel = new MessageResponseModel
                {
                    TransactionValid = true,
                    SessionValid = true,
                    EmailMessageFullDetailModel = new MessagesAPI.MessageAPI.Domain.Models.General.EmailMessageFullDetailModel()
                };

                var emailFullMessage = Messages.GetMessageDetail(responseSession.UserId, messageId);

                if (emailFullMessage?.EmailMessageModel == null)
                {
                    return Unauthorized(); // Cannot read this message
                }

                messageResponseModel.EmailMessageFullDetailModel.EmailMessageModel = new EmailMessageModel
                {
                    DateCreated = emailFullMessage.EmailMessageModel.DateCreated,
                    Subject = emailFullMessage.EmailMessageModel.Subject,
                    ParentMessageId = emailFullMessage.EmailMessageModel.ParentMessageId,
                    Id = emailFullMessage.EmailMessageModel.Id,
                    Body = emailFullMessage.EmailMessageModel.Body,
                    AuthorId = emailFullMessage.EmailMessageModel.AuthorId
                };

                messageResponseModel.EmailMessageFullDetailModel.EmailAttachmentModelList = emailFullMessage.EmailAttachmentModelList;

                // Sign each attachment URL
                // TODO: Add the key path
                messageResponseModel.EmailMessageFullDetailModel.EmailAttachmentModelList
                    .ForEach(a => a.AttachmentUrl = AwsAssets.GetSignedUrlFronCloudFront(a.AttachmentUrl));

                var userSharedDetailsResponseModel = new UserAdvanceResponseModel();

                if (emailFullMessage.EmailMessageRecipientModelList?.Count > 20)
                {
                    try
                    {
                        userSharedDetailsResponseModel = GetUserDetails.GetUserFullNamesAPI(
                            RequestHandler.GetToken(HttpContext),
                            new List<string> { emailFullMessage.EmailMessageModel.AuthorId });

                        messageResponseModel.EmailMessageFullDetailModel.EmailRecipientModelList = new List<EmailMessageRecipientModel>
                {
                    new EmailMessageRecipientModel
                    {
                        RecipientName = "General Notification/Notificación General"
                    }
                };
                    }
                    catch
                    {
                        // Non-critical
                    }
                }
                else
                {
                    var usersToCall = emailFullMessage.EmailMessageRecipientModelList
                        .Select(x => x.RecipientId)
                        .ToList();

                    usersToCall.Add(emailFullMessage.EmailMessageModel.AuthorId);

                    try
                    {
                        userSharedDetailsResponseModel = GetUserDetails.GetUserFullNamesAPI(
                            RequestHandler.GetToken(HttpContext),
                            usersToCall);

                        messageResponseModel.EmailMessageFullDetailModel.EmailRecipientModelList = new List<EmailMessageRecipientModel>();

                        foreach (var recipient in emailFullMessage.EmailMessageRecipientModelList)
                        {
                            var user = userSharedDetailsResponseModel.UserInfo?.Find(x => x.UserId == recipient.RecipientId);
                            if (recipient.RecipientId != responseSession.UserId)
                            {
                                messageResponseModel.EmailMessageFullDetailModel.EmailRecipientModelList.Add(new EmailMessageRecipientModel
                                {
                                    RecipientId = recipient.RecipientId,
                                    DateRead = recipient.DateRead,
                                    IsRead = recipient.IsRead,
                                    MessageId = recipient.MessageId,
                                    RecipientName = user != null ? $"{user.GivenName} {user.Surname}" : null
                                });
                            }
                        }
                    }
                    catch
                    {
                        // Non-critical
                    }
                }

                // Set Author Full Name
                var author = userSharedDetailsResponseModel.UserInfo?.Find(x =>
                    x.UserId == messageResponseModel.EmailMessageFullDetailModel.EmailMessageModel.AuthorId);

                messageResponseModel.EmailMessageFullDetailModel.EmailMessageModel.UserFullName = author != null
                    ? $"{author.GivenName} {author.Surname}"
                    : "";

                return Ok(messageResponseModel);
            }
            catch
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// To do email testings.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="subject">Subject of message.</param>
        /// <param name="body">Body of the message.</param>
        /// <returns>Return a message transaction details.</returns>
        [HttpPost("email/test/{email}/{subject}/{body}")]
        public IActionResult SendEmail(string email, string subject, string body)
        {
            var message = EmailManager.SendEmailSimple(
                connectionStringRead,
                new List<string> { email },
                subject,
                body
            );

            var response = new MessagesAPI.MessageAPI.Domain.Models.General.ResponseModel
            {
                SessionValid = false,
                TransactionValid = true,
                Messages = new List<string> { message }
            };

            return Ok(response);
        }
    }
}
