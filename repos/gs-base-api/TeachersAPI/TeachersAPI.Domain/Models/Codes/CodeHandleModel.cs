using System;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.Domain.Models.Codes
{
    public class CodeHandleModel : CodeModel
    {
        public string CodeTypeName { get; set; }

        public List<CodeSubjectModel> CodeSubjectModels { get; set; }

        public List<CodeLevelModel> CodeLevelModels { get; set; }
    }
}