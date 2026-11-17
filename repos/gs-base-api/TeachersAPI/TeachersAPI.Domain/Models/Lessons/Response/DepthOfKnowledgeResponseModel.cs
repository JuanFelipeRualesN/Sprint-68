using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Lessons.Response
{
    public class DepthOfKnowledgeResponseModel
    {
        public string PsychName { get; set; }
        public string LevelOfKnowledgeNumber { get; set; }
        public string LevelOfKnowledgeDescription { get; set; }
        public string LevelOfKnowledgeId { get; set; }
    }
}
