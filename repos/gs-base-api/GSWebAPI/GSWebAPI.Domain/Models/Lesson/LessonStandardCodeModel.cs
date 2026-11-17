using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class LessonStandardCodeModel
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string StandardId { get; set; }
        public string CodeMainId { get; set; }
        public string SubmainCodeId { get; set; }
        public string SubmainSubCodeId { get; set; }
        public string StandardCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }

    /// <summary>
    /// Gets or sets a List Constructed Codes  
    /// </summary>
    internal class ConstructedCodesListModel
    {
        /// <summary>
        /// Gets or sets a Constructed Code Id
        /// </summary>
        public int ConstructedCodeId { get; set; }
        /// <summary>
        /// Gets or sets main code status ( 1 = yes, 0 =no) 
        /// </summary>
        public int MainCode { get; set; }
        /// <summary>
        /// Gets or sets a code
        /// </summary>
        public string Codes { get; set; }
    }

    /// <summary>
    /// Gets a list of Constructed Code Codes by LessonId
    /// </summary>
    internal class ConstructedCodeCodesbyLessonIdListModel
    {
        /// <summary>
        /// Gets a id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets main code
        /// </summary>
        public int MainCode { get; set; }
    }

    /// <summary>
    /// Location vCode
    /// </summary>
    internal class vCode
    {
        public List<MlsCodes> LsCodes { get; set; }
        public List<MlsConstructedCode> LsConstructedCode { get; set; }
    }
    /// <summary>
    /// Location mls_constructed_code
    /// </summary>
    internal class MlsConstructedCode
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int MainCode { get; set; }
    }
    /// <summary>
    /// Location mls_codes
    /// </summary>
    internal class MlsCodes
    {
        public int LessonConstructedCodeId { get; set; }
        public int CodeId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int DotAfter { get; set; }

        public int TypeId { get; set; }

    }
}