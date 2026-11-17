using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses
{
    public class TextTranslationModel
    {
        public string TextCode { get; set; }
        public string Text { get; set; }
        public string LangCode { get; set; }  
    }
}