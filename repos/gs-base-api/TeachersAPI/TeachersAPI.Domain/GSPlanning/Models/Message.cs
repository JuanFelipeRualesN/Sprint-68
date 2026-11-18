using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the messages that we will return to the users according to some transactions.
    /// </summary>
    public class Message
    {
        public int Id { get; set; }

        /// <summary>
        /// The message code to retrieve the message details.
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// The language code (es for Spanish, en for English).
        /// </summary>
        public string LangCode { get; set; }

        /// <summary>
        /// The message&apos;s explanation.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
