using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TeachersAPI.Domain.Models.Teachers;
using CacheHandlerLibrary;

namespace TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses
{
    /// <summary>
    /// To handle validations.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validate if the name is not empty.
        /// </summary>
        /// <param name="name">The name string.</param>
        /// <returns>Returns a ResponseModel</returns>
        public static ResponseModel ValidateName(string name)
        {
            ResponseModel responseModel = new ResponseModel()
            {
                Valid = true,
                MessageCodes = new List<string>(),
                Messages = new List<string>()
            };

            if (string.IsNullOrWhiteSpace(name))
            {
                responseModel.Valid = false;
                responseModel.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:tm_message_code_error_name_empty"]);
            }
            return responseModel;
        }

    }
}
