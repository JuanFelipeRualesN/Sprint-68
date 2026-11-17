using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using UsersAPI.UsersAPI.Domain.Models.Emails;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Emails;


namespace UsersAPI.UsersAPI.Services.Emails
{
    /// <summary>
    /// To handle emails details (to send emails to the user).
    /// </summary>
    public class EmailManager
    {
        /// <summary>
        /// To send an email.
        /// </summary>
        /// <param name="connectionString">The connection string (read).</param>
        /// <param name="clientEmailsList">The clients that will receive the email.</param>
        /// <param name="title">The subject of the email.</param>
        /// <param name="body">The body of the email.</param>
        /// <returns>Returns OK if sent.</returns>
        public static string SendEmailSimple(string connectionString,
            List<string> clientEmailsList,
            string title,
            string body)
        {
            EmailSettingModel emailSettingModel = new EmailSettingModel();
            EmailSettingDAO emailSettingDAO = new EmailSettingDAO();
            emailSettingModel = emailSettingDAO.Read(connectionString, emailSettingModel, true, true)[0];

            //create the mail message 
            MailMessage mail = new MailMessage
            {
                //set the addresses 
                From = new MailAddress(emailSettingModel.EmailSender) //IMPORTANT: This must be same as your smtp authentication address.
            };

            // Receivers
            for (int i = 0; i < clientEmailsList.Count; i++)
            {
                mail.To.Add(clientEmailsList[i]);
            }

            //set the content 
            mail.Subject = title;

            mail.Body = body;
            mail.IsBodyHtml = true;


            try
            {
                //send the message 
                using (var smtpClient = new SmtpClient(emailSettingModel.MailDomainCom))
                {

                    //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                    smtpClient.Port = int.Parse(emailSettingModel.EmailPort);
                    smtpClient.UseDefaultCredentials = emailSettingModel.DefaultCredential == "1";
                    NetworkCredential Credentials = new NetworkCredential(emailSettingModel.SenderUser, emailSettingModel.SenderPassword);
                    smtpClient.Credentials = Credentials;
                    smtpClient.EnableSsl = emailSettingModel.EnableSsl == "1";
                    smtpClient.Send(mail);
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}
