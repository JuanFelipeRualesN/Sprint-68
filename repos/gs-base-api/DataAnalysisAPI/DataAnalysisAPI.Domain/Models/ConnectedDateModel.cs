using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models
{
    public class ConnectedDateModel : UserModel
    {
        public DateTime ConnectedDate { get; set; }
        public int Id { get; set; }

    }
}