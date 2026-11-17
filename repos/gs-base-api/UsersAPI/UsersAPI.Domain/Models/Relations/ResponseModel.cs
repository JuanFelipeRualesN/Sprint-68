using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.UsersAPI.Domain.Models.Relations
{
    public class ResponseModel
    {
        public bool Valid { get; set; }
        public List<string> Messages { get; set; }
    }
}
