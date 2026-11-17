using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Lessons
{
	public class QuizTypeModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string AutoCorrect { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
    }