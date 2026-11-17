using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace UsersAPI.CustomClasses
{
    /// <summary>
    /// To handle text fixes.
    /// </summary>
    public static class TextFixer
    {
        /// <summary>
        /// Replates texts like áéíñ to aein.
        /// </summary>
        /// <param name="text">Text to replace.</param>
        /// <returns>Returns the string with replaced characters.</returns>
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Remove special characters.
        /// </summary>
        /// <param name="str">THe string to replace.</param>
        /// <returns>Returns the string replaced.</returns>
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        /// <summary>
        /// Replace CSV auto characters.
        /// </summary>
        /// <param name="str">String to replace.</param>
        /// <returns>Returns the string with replaced details.</returns>
        public static string ReplaceCSVWeirdCharacters(string str)
        {
            str = str.Replace("Ã", "A");
            str = str.Replace("Ã¡", "a");
            str = str.Replace("Ã‰", "E");
            str = str.Replace("Ã©", "e");
            str = str.Replace("Ã ", "I");
            str = str.Replace("Ã­", "i");
            str = str.Replace("Ã“", "O");
            str = str.Replace("Ã³", "o");
            str = str.Replace("Ãš", "U");
            str = str.Replace("Ãº", "u");
            str = str.Replace("Ã‘", "N");
            str = str.Replace("Ã±", "n");
            return str;
        }

        /// <summary>
        /// Remove special character, but leaves the spaces.
        /// </summary>
        /// <param name="str">String to replace.</param>
        /// <returns>Returns the string replaced.</returns>
        public static string RemoveSpecialCharacterExceptWhiteSpaces(string str)
        {
            return Regex.Replace(str, "[^0-9A-Za-z ,]", "", RegexOptions.Compiled);
        }

        /// <summary>
        /// Fix the sex string.
        /// </summary>
        /// <param name="str">String fixed.</param>
        /// <returns>Return the string fixed.</returns>
        public static string FixSex(string str)
        {
            str = str.Trim();
            str = str.ToUpper();

            if (str == "MALE" || str == "VARÓN" || str == "VARON" || str == "NINO" || str == "MASCULINO" || str == "NIÑO" || str == "HOMBRE" || str == "M")
            {
                str = "M";
            }
            else if (str == "FEMALE" || str == "FEMENINO" || str == "FEMENINA" || str == "MUJER" || str == "NIÑA" || str == "NINA" || str == "F")
            {
                str = "F";
            }
            else if (str == "OTHER" || str == "OTRO" || str == "O")
            {
                str = "O";
            }
            else
            {
                str = "U";
            }

            return str;
        }
    }
}