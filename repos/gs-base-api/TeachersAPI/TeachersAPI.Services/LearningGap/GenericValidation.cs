using TeachersAPI.TeachersAPI.Services.Generals;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.LearningGap
{
    /// <summary>
    /// To handle generic validations.
    /// </summary>
    public class GenericValidation
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// To initialize the class.
        /// </summary>
        /// <param name="connectionStringRead">The connection string read.</param>
        /// <param name="langCode">The language code.</param>
        public GenericValidation(string connectionStringRead, string langCode)
        {
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode;
        }

        /// <summary>
        /// To validate a model.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="model">The model to validate.</param>
        /// <param name="requiredFields">The required fields.</param>
        /// <param name="customValidation">The custom validation.</param>
        /// <param name="invalidMessageKey">The invalid message key.</param>
        /// <returns>Returns the validated model.</returns>
        public T ValidateModel<T>(T model, Dictionary<string, string> requiredFields, Func<bool> customValidation, string invalidMessageKey) where T : class, new()
        {
            var messageManager = new MessageManager(this._connectionStringRead, this._langCode);

            if (model == null)
            {
                return CreateInvalidModel<T>(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_model_lack"]));
            }

            if (requiredFields.Any(field => string.IsNullOrWhiteSpace(field.Value)))
            {
                return CreateInvalidModel<T>(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:lgap_message_model_lack"]), model);
            }

            if (!customValidation())
            {
                return CreateInvalidModel<T>(messageManager.GetMessageDetails(AppConfiguration.Configuration[invalidMessageKey]), model);
            }

            SetModelValid(model, true);
            return model;
        }

        /// <summary>
        /// To create an invalid model.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="message">The message to show.</param>
        /// <param name="model">The model to invalidate.</param>
        /// <returns>Returns the invalidate model.</returns>
        public T CreateInvalidModel<T>(string message, T model = null) where T : class, new()
        {
            if (model == null)
            {
                model = new T();
            }
            SetModelValid(model, false);
            SetModelMessages(model, new List<string> { message });
            return model;
        }

        /// <summary>
        /// To set the model as valid.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="model">The model to set as valid.</param>
        /// <param name="isValid">If valid of not (bool).</param>
        public void SetModelValid<T>(T model, bool isValid)
        {
            var validProperty = model.GetType().GetProperty("Valid");
            validProperty?.SetValue(model, isValid);
        }

        /// <summary>
        /// To set messages to the model.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="model">The model to set the messages.</param>
        /// <param name="messages">The messages to add.</param>
        public void SetModelMessages<T>(T model, List<string> messages)
        {
            var messagesProperty = model.GetType().GetProperty("Messages");
            messagesProperty?.SetValue(model, messages);
        }

        /// <summary>
        /// To check if string is a valid decimal.
        /// </summary>
        /// <param name="value">The decimal value.</param>
        /// <returns>Returns true if valid, else false.</returns>
        public bool IsValidDecimal(string value) => decimal.TryParse(value, out _);

        /// <summary>
        /// To check if string is a valid integer.
        /// </summary>
        /// <param name="value">The integer value.</param>
        /// <returns>Returns true if valid, else false.</returns>
        public bool IsValidInt(string value) => int.TryParse(value, out _);
    }
}