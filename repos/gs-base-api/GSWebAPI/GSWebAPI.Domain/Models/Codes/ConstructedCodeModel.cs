using GSWebAPI.GSWebAPI.Domain.Models.General;

namespace GSWebAPI.GSWebAPI.Domain.Models.Codes
{
    public class ConstructedCodeModel: IResponse
    {
        public bool Valid { get; set; }
        public List<string> Messages { get; set; }
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string MainCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string ConstructedFullCode { get; set; }
    }
}
