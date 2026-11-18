namespace GSBaseAPI.GPTModule.Models
{
    public class AiConsultingLessonModel
    {
        public long MyRowId { get; set; }
        public string LessonId { get; set; }
        public string LessonTitle { get; set; }
        public string SubjectName { get; set; }
        public string LessonLevel { get; set; }

        public string LessonDescription { get; set; }
        public string Concept { get; set; }
        public string AllExamples { get; set; }

        public string StandardCodes { get; set; }
        public string StandardDescriptions { get; set; }

        public string LevelCodes { get; set; }
        public string LevelDescriptions { get; set; }

        public string ExpectationCodes { get; set; }
        public string ExpectationDescriptions { get; set; }

        public string OtherCodes { get; set; }
        public string OtherDescriptions { get; set; }

        public string AllConstructedFullCodes { get; set; }

        public string Units { get; set; }
        public string UnitsDescriptions { get; set; }

        public string TransversalThemes { get; set; }
        public string Strategies { get; set; }

        public string Objectives { get; set; }
    }
}
