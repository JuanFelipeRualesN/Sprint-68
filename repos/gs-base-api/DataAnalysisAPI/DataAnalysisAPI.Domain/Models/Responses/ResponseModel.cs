using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses
{
    public class ResponseModel<T>
    {
        public int Count { get; set; }

        public List<T> Results { get; set; }

    }
}