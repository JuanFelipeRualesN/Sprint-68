using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using Dapper;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;

namespace MessagesAPI.MessageAPI.Services.EmailSystems
{
    /// <summary>
    /// To handle the attachments.
    /// </summary>
    public static class Files
    {
        private readonly static string connectionStringRead = AppConfiguration.Configuration["ConnectionStrings:EmailSystemManagerConnectionStringReader"];
        /// <summary>
        /// To get the email attachments' rules.
        /// </summary>
        /// <returns>Return a list of Attachment Rule Model with attachment rule details.</returns>
        public static List<AttachmentRuleModel> GetEmailAttachmentRules()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var query = @"SELECT * FROM gs_communication_db_prod.cm_attachment_rules;";

            IEnumerable<AttachmentRuleModel> items = null;
            var connection = new MySqlConnection
            {
                ConnectionString = connectionStringRead
            };
            connection.Open();
            try
            {
                items = connection.Query<AttachmentRuleModel>(query);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<AttachmentRuleModel>();
        }

    }
}