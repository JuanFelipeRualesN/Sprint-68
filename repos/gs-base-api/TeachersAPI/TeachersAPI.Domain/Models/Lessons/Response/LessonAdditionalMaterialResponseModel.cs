using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Lessons.Response
{
    public class LessonAdditionalMaterialResponseModel : LessonAdditionalMaterialModel
    {
        public AdditionalMaterialModel AdditionalMaterialModel { get; set; }

    }
}
