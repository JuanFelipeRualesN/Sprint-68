using System;
using System.Collections.Generic;
using System.Configuration;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Emails;


namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle emails to send.
    /// </summary>
    class EmailSenderManager 
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Class to send emails about the account validation.
        /// </summary>
        /// <param name="connectionString">The connection string to write.</param>
        /// <param name="connectionStringRead">The connection string to read.</param>
        /// <param name="langCode">The language's code.</param>
        public EmailSenderManager(string connectionString, string connectionStringRead, string langCode)
        {
            this._connectionString = connectionString;
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// Method to send the email about email validation.
        /// </summary>
        /// <param name="userEmail">The user's email.</param>
        /// <param name="token">The email's token.</param>
        public void SendEmailValidation(string userEmail, string token)
        {
            this.SendEmail(userEmail, AppConfiguration.Configuration["AppSettings:token_type_email_confirmation_code"], token); 
        }

        /// <summary>
        /// Method to send an email about reseting the password.
        /// </summary>
        /// <param name="userEmail">The user's Email.</param>
        /// <param name="token">The token to reset the password.</param>
        public void SendPasswordReset(string userEmail, string token)
        {
            this.SendEmail(userEmail, AppConfiguration.Configuration["AppSettings:token_type_password_reset_code"], token);
        }

        /// <summary>
        /// Method to send the email.
        /// </summary>
        /// <param name="useremail">The user's email/</param>
        /// <param name="typeCode">The type code (if password reset code, if email validation, etc). </param>
        /// <param name="token">The token related to the email.</param>
        private void SendEmail(string useremail, string typeCode, string token)
        {
            // Get the ID of the token Type 
            EmailTokenTypeModel emailTokenTypeModel = new EmailTokenTypeModel
            {
                Code = typeCode
            };
            EmailTokenTypeDAO emailTokenTypeDAO = new EmailTokenTypeDAO();
            emailTokenTypeModel = emailTokenTypeDAO.Read(this._connectionStringRead, emailTokenTypeModel, true, true)[0];

            // Get the email properties 
            EmailValidationDetailModel emailValidationDetailModel = new EmailValidationDetailModel
            {
                LangCode = this._langCode,
                EmailTokenTypeId = emailTokenTypeModel.Id
            };
            EmailValidationDetailDAO emailValidationDetailDAO = new EmailValidationDetailDAO();
            emailValidationDetailModel = emailValidationDetailDAO.Read(this._connectionStringRead, emailValidationDetailModel, true, true)[0];

            // Execute in task because this takes some time to send 
            List<string> clientEmails = new List<string>
            {
                useremail
            };

            EmailManager.SendEmailSimple(this._connectionString, clientEmails, emailValidationDetailModel.Subject, 
                String.Concat(emailValidationDetailModel.Header, emailValidationDetailModel.Body,
                        emailValidationDetailModel.Url, token, emailValidationDetailModel.Footer));
               
            
        }
    }
}
