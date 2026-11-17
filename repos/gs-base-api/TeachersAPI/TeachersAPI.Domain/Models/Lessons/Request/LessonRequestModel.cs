using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Lessons.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class LessonRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string LessonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsForOutsideSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool AddQuizDetails { get; set; }
    }
}