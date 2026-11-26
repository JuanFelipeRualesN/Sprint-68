using GSPlanning.Domain;

namespace GSPlanning.Services
{
    /// <summary>
    /// Defines the operations to be used to manage messages and its related data.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Retrieves the message related to the provided code.
        /// </summary>
        /// <param name="code">The code used as an identifier for the message.</param>
        /// <param name="langCode">The language code in which the message is written.</param>
        /// <returns>The desired message.</returns>
        Message GetMessage(string code, string langCode = "en");
    }
}