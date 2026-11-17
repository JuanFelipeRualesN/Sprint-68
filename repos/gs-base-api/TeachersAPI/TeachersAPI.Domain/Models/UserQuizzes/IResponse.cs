using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.UserQuizzes
{
    public interface IResponse
    {
        List<string> Messages { get; set; }
        bool Valid { get; set; }
    }
}
