using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class ResponseModel
    {
        /// <summary>
        /// Gets or sets whether transaction is valid or not.
        /// </summary>
        public bool TransactionValid { get; set; }

        /// <summary>
        /// Gets or sets whether the session was valid or not.
        /// </summary>
        public bool SessionValid { get; set; }

        /// <summary>
        /// Gets or sets whether the transaction was valid or not.
        /// </summary>
        public bool Valid { get; set; }
        /// <summary>
        /// Gets or sets the list of messages of transaction.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// The message codes about the errors
        /// </summary>
        public List<string> MessageCodes { get; set; }
        public string LessonId { get; set; }
        public string QuizId { get; set; }
    }
}
