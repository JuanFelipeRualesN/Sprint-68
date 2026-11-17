using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;


namespace UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses
{
    /// <summary>
    /// TO handle validations related to personal details.
    /// </summary>
    public static class ValidationManager
    {

        /// <summary>
        /// To validate the user's gender.
        /// </summary>
        /// <param name="gender">The gender to validate (should be F, M, O, U).</param>
        /// <returns>Returns the response details if valid or not.</returns>
        public static ResponseModel ValidateGender(string gender)
        {
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                ErrorCodes = new List<string>()
            };

            string[] availableGenders = { "F", "M", "O", "U"};
            if (String.IsNullOrWhiteSpace(gender) || !availableGenders.Contains(gender))
            {
                responseModel.Valid = false;
                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:upd_message_code_gender_invalid"]);
                return responseModel;
            }
            else
                return responseModel;
             
        }

        /// <summary>
        /// Validates the date of birth of the user.
        /// </summary>
        /// <param name="dob">The user's date of birth.</param>
        /// <returns>Returns the transaction details whether valid or not.</returns>
        public static ResponseModel ValidateDOB(string dob)
        {
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                ErrorCodes = new List<string>()
            };

            if (String.IsNullOrWhiteSpace(dob))
            {
                responseModel.Valid = false;
                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:upd_message_code_user_dob_required"]);
                return responseModel;
            }

            DateTime dateDOB; 
            try
            {
                dateDOB = DateTime.Parse(dob); 
            }
            catch(Exception)
            {
                responseModel.Valid = false;
                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:upd_message_code_user_dob_invalid"]);
                return responseModel; 
            }
            return responseModel; 
        }

        /// <summary>
        /// Validates the surname.
        /// </summary>
        /// <param name="surname">The suername to validate.</param>
        /// <returns>Returns the transaction details whether valid or not.</returns>
        public static ResponseModel ValidateSurname(string surname)
        {
            ResponseModel responseModel = new ResponseModel
            {
                Valid = true,
                ErrorCodes = new List<string>()
            };

            // Validate 
            FieldValidator fieldValidator = new FieldValidator();
            if (!fieldValidator.IsNameLengthValid(surname))
            {
                responseModel.Valid = false;
                responseModel.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_last_name_short_error"]);
            }
            return responseModel; 
        }

        /// <summary>
        /// Validates the user's first name.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        /// <returns>Returns the transaction details whether valid or not.</returns>
        public static ResponseModel ValidateName(string name)
        {
            ResponseModel response = new ResponseModel
            {
                Valid = true,
                ErrorCodes = new List<string>()
            };

            // Validate 
            FieldValidator fieldValidator = new FieldValidator();
            if (!fieldValidator.IsNameLengthValid(name))
            {
                response.Valid = false;
                response.ErrorCodes.Add(AppConfiguration.Configuration["AppSettings:message_code_first_name_short_error"]);
            }

            return response; 
        }
    }
}
