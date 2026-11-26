using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models
{
    public class PowerBIModel
    {
        public string embedToken { get; set; }
        public string reportId { get; set; }
        public string embedUrl { get; set; }
    }
}