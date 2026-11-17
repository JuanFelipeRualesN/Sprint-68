using System;
using System.Text.RegularExpressions;


namespace UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses
{
    /// <summary>
    /// To validate fields such lenghts, if seems valid, etc.
    /// </summary>
    class FieldValidator
    {

        /// <summary>
        /// Validate the name length.
        /// </summary>
        /// <param name="name">The name or surname to validate.</param>
        /// <returns>Returns true if name seems valid, else, false.</returns>
        public bool IsNameLengthValid(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
                return true;
            else
                return false;
        }
    }

}