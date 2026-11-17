using System.Net;
using System.Net.Mail;
using MessagesAPI.MessageAPI.Domain.Models.Email;
using MessagesAPI.MessageAPI.Infrastructure.Repositories.Emails;

namespace MessagesAPI.MessageAPI.Services.Emails
{
    /// <summary>
    /// To handle the emails sending.
    /// </summary>
    public class EmailManager
    {
        private readonly static string configLimitEmailsPerSecond = AppConfiguration.Configuration["AppSettings:limit_emails_per_second"];

        /// <summary>
        /// To send a basic email without a Task factory.
        /// </summary>
        /// <param name="connectionString">The connection string to read the email settings.</param>
        /// <param name="clientEmailsList">The email list.</param>
        /// <param name="title">The email's subject.</param>
        /// <param name="body">The email's body.</param>
        /// <returns>Returns an OK if the email was sent.</returns>
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
                From = new MailAddress(emailSettingModel.EmailSender) //IMPORTANT: This must be the same as your SMTP authentication address.
            };

            //set the content 
            mail.Subject = title;
            mail.Body = body;
            mail.IsBodyHtml = true;

            try
            {
                using (var smtpClient = new SmtpClient(emailSettingModel.MailDomainCom))
                {
                    smtpClient.Port = int.Parse(emailSettingModel.EmailPort);
                    smtpClient.UseDefaultCredentials = emailSettingModel.DefaultCredential == "1";
                    NetworkCredential credentials = new NetworkCredential(emailSettingModel.SenderUser, emailSettingModel.SenderPassword);
                    smtpClient.Credentials = credentials;
                    smtpClient.EnableSsl = emailSettingModel.EnableSsl == "1";

                    // Determine the batch size dynamically from configuration
                    int batchSize = int.Parse(configLimitEmailsPerSecond);

                    // Send emails without batching if batchSize is 0 or negative
                    if (batchSize <= 0)
                    {
                        foreach (var email in clientEmailsList)
                        {
                            mail.To.Add(email);
                        }
                        smtpClient.Send(mail);
                    }
                    else
                    {
                        // Send emails in batches
                        for (int i = 0; i < clientEmailsList.Count; i += batchSize)
                        {
                            // Extract a batch of emails
                            List<string> currentBatch = clientEmailsList.Skip(i).Take(batchSize).ToList();

                            // Clear existing recipients and add recipients for the current batch
                            mail.To.Clear();
                            foreach (string recipient in currentBatch)
                            {
                                mail.To.Add(recipient);
                            }

                            // Send the batch of emails
                            smtpClient.Send(mail);

                            // Pause for a short duration to stay within rate limits (adjust as needed)
                            Thread.Sleep(1000 / batchSize); // Sleep for 1 second per batchSize emails
                        }
                    }

                    return "OK";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
