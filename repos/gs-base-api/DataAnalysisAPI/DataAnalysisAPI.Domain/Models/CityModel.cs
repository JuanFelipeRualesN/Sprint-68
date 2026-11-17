using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models
{
    public class CityModel
    {
        public string City { get; set; }
        public int? OreId { get; set; }
        public string StateIsoCode { get; set; }
        public string CountryIso { get; set; }
        public int Id { get; set; }

    }
}