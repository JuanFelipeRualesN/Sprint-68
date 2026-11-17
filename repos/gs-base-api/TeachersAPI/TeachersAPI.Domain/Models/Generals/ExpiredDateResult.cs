using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals
{
    public class ExpiredDateResult
    {
        public string Section { get; set; }
        public bool DateExpired { get; set; }
    }
}