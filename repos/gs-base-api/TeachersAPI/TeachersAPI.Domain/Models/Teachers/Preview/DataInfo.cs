using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Teachers.Preview
{
    /// <summary>
    /// 
    /// </summary>
    public class DataInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public const string GenialLessonType = "Lección genial";
        /// <summary>
        /// 
        /// </summary>
        public const string CustomLessonType = "Lección libre";
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecundaryDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; } = GenialLessonType;
    }
}