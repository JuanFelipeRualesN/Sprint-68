using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Emails;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Emails;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Emails
{
    /// <summary>
    /// To handle the email sending.
    /// </summary>
    public class EmailManager
    {
        /// <summary>
        /// To send an email.
        /// </summary>
        /// <param name="connectionString">The connection string to read.</param>
        /// <param name="clientEmailsList">The users' emails to send the email.</param>
        /// <param name="title">The email's subject.</param>
        /// <param name="body">The email's body.</param>
        /// <returns>Returns an OK if was sent.</returns>
        public static string SendEmailSimple(string connectionString,
            List<string> clientEmailsList,
            string title,
            string body)
        {
            EmailSettingModel emailSettingModel = new EmailSettingModel();
            emailSettingModel = new EmailSettingDAO().Read(connectionString, emailSettingModel, true, true)[0];

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
                    NetworkCredential Credentials = new NetworkCredential(emailSettingModel.EmailSender, emailSettingModel.SenderPassword);
                    smtpClient.Credentials = Credentials;

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
