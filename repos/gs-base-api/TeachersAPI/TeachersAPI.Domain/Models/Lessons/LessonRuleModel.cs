namespace TeachersAPI.Domain.Models.Lessons
{
    public class LessonRuleModel
    {
        public string Id { get; set; }
        public string AddressedId { get; set; }
        public string TitleMaxLength { get; set; }
        public string SimpleText { get; set; }
        public string UnityRequired { get; set; }
        public string DescriptionRequired { get; set; }
        public string ObjectiveRequired { get; set; }
        public string PerformanceTaskRequired { get; set; }
        public string StrategyRequired { get; set; }
        public string TransversalThemeRequired { get; set; }
        public string StandardCodeRequired { get; set; }
        public string QuizRequired { get; set; }
        public string ConceptMaxLength { get; set; }
        public string DefinitionMaxLength { get; set; }
        public string ExampleMaxLength { get; set; }
        public string LessonNoRequired { get; set; }
        public string TaxonomyRequired { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}