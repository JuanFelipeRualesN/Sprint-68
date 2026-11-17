using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models.Filters
{
    public class Pagination
    {
        public bool ApplyPagination { get; set; }
        public int FromPage { get; set; }
        public int LimitRows { get; set; }
    }
}