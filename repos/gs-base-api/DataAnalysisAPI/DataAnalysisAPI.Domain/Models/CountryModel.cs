using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string CountryIso { get; set; }
        public string Country { get; set; }

    }
}