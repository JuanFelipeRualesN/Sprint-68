using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.UserQuizzes
{
    /// <summary>
    /// Do the relation between User Quiz Result and User Quiz Responses
    /// </summary>
    public class UserQuizResponseModel
    {
        public UserQuizResultModel UserQuizResult { get; set; }
        public List<UserResponseModel> UserResponsesList { get; set; }
        public List<UserQuizFileModel> UserQuizFileModelList { get; set; }
    }
}
