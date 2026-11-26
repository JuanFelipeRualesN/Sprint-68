using Xunit;

namespace ReadingAPI.Tests
{
    public class DeprReadingLevelTests
    {
        // 100 words: the allowed errors per 100 words apply as they are.
        private static readonly string Passage = string.Join(" ", Enumerable.Repeat("palabra", 100));

        private static ReadingLevelResult Evaluate(double pronunciation, double accuracy, string? grade, string language = "es-MX", int errors = 0)
        {
            var result = new AssessmentResultDto
            {
                Overall = new OverallScoresDto { Pronunciation = pronunciation, Accuracy = accuracy, Fluency = 90, Completeness = 100 },
                Issues = Enumerable.Range(0, errors).Select(i => new WordIssueDto { Word = "palabra", ErrorType = "Mispronunciation" })
                    .Append(new WordIssueDto { Word = "bien", ErrorType = "None" })
                    .ToList(),
            };
            return new LanguageLevelEvaluator().DetermineLevel(result, language, Passage, grade);
        }

        // Boundaries of every Kinder tier (same thresholds as before): at the minimum and just below it.
        [Theory]
        [InlineData(90.0, 92.0, ReadingTier.Advanced, 4)]
        [InlineData(89.9, 92.0, ReadingTier.Proficient, 3)]
        [InlineData(90.0, 91.9, ReadingTier.Proficient, 3)]
        [InlineData(80.0, 85.0, ReadingTier.Proficient, 3)]
        [InlineData(79.9, 85.0, ReadingTier.Fluent, 3)]
        [InlineData(70.0, 75.0, ReadingTier.Fluent, 3)]
        [InlineData(69.9, 75.0, ReadingTier.Transitional, 2)]
        [InlineData(55.0, 60.0, ReadingTier.Transitional, 2)]
        [InlineData(54.9, 60.0, ReadingTier.Beginning, 1)]
        [InlineData(40.0, 45.0, ReadingTier.Beginning, 1)]
        [InlineData(39.9, 45.0, ReadingTier.Emergent, 1)]
        [InlineData(0.0, 0.0, ReadingTier.Emergent, 1)]
        public void Kinder_boundaries_give_the_DEPR_and_story_levels(double pronunciation, double accuracy, ReadingTier tier, int depr)
        {
            var result = Evaluate(pronunciation, accuracy, "Kinder");

            Assert.Equal(tier, result.Tier);
            Assert.Equal(depr, result.Level.Number);
            Assert.Equal(depr, result.StoryLevel); // story_levels has the same 4 levels
        }

        [Theory]
        [InlineData(96.0, 98.0, 4)]
        [InlineData(95.9, 98.0, 3)]
        [InlineData(92.0, 94.0, 3)]
        [InlineData(85.0, 88.0, 3)]
        [InlineData(84.9, 88.0, 2)]
        [InlineData(75.0, 80.0, 2)]
        [InlineData(74.9, 80.0, 1)]
        public void Fifth_grade_boundaries(double pronunciation, double accuracy, int depr)
        {
            Assert.Equal(depr, Evaluate(pronunciation, accuracy, "5th Grade").Level.Number);
        }

        [Fact]
        public void Too_many_errors_drop_the_tier_like_before()
        {
            // Kinder Advanced allows 3 errors per 100 words, Proficient 6.
            Assert.Equal(ReadingTier.Advanced, Evaluate(95, 95, "Kinder", errors: 3).Tier);
            Assert.Equal(ReadingTier.Proficient, Evaluate(95, 95, "Kinder", errors: 4).Tier);
            Assert.Equal(ReadingTier.Emergent, Evaluate(95, 95, "Kinder", errors: 41).Tier);
        }

        [Fact]
        public void Without_a_grade_the_CEFR_fallback_is_mapped_too()
        {
            var result = Evaluate(90, 92, null, "es-MX");
            Assert.Equal(DeprReadingLevels.Sobresaliente, result.Level);
            Assert.Equal(4, result.StoryLevel);

            Assert.Equal(2, Evaluate(65, 70, null, "en-US").Level.Number);
            Assert.Equal(1, Evaluate(10, 10, "Unknown grade", "en-US").Level.Number);
        }

        // Every former label (what the endpoint returned before) maps to the DEPR level and keeps its story level.
        [Theory]
        [InlineData("Emergent (Emergente)", 1)]
        [InlineData("Early / Beginning (Comenzando)", 1)]
        [InlineData("Developing / Transitional (Desarrollándose / Transicional)", 2)]
        [InlineData("Fluent (Fluido)", 3)]
        [InlineData("Proficient (Proficiente)", 3)]
        [InlineData("Advanced / Independent (Avanzado / Independiente)", 4)]
        [InlineData("fluido", 3)]
        [InlineData("Level 1 (A1) (Nivel 1 - A1)", 1)]
        [InlineData("Nivel 3 (B1)", 2)]
        [InlineData("Nivel 5 (C1+)", 4)]
        public void Former_labels_map_to_DEPR(string label, int depr)
        {
            var tier = DeprReadingLevels.TierFromLegacyLabel(label);

            Assert.NotNull(tier);
            Assert.Equal(depr, DeprReadingLevels.FromLegacyLabel(label)!.Number);
            Assert.Equal(depr, DeprReadingLevels.StoryLevel(tier!.Value));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Sobresaliente")]
        public void Unknown_labels_are_not_mapped(string? label)
        {
            Assert.Null(DeprReadingLevels.TierFromLegacyLabel(label));
        }

        [Theory]
        [InlineData("es-MX", 1, "Principiante", "El estudiante presenta dificultades importantes en la destreza y necesita apoyo inmediato y frecuente.")]
        [InlineData("es-MX", 2, "Aprendiz", "El estudiante demuestra avances, pero todavía necesita fortalecer y consolidar la destreza.")]
        [InlineData("es-MX", 3, "Proficiente", "El estudiante demuestra el dominio esperado para su grado.")]
        [InlineData("es-MX", 4, "Sobresaliente", "El estudiante supera el desempeño esperado para su grado y puede beneficiarse de actividades de enriquecimiento.")]
        [InlineData("en-US", 1, "Beginner", "The student shows significant difficulties with the skill and needs immediate and frequent support.")]
        [InlineData("en-US", 2, "Apprentice", "The student shows progress but still needs to strengthen and consolidate the skill.")]
        [InlineData("en-US", 3, "Proficient", "The student shows the mastery expected for their grade.")]
        [InlineData("en-US", 4, "Outstanding", "The student exceeds the performance expected for their grade and can benefit from enrichment activities.")]
        public void Labels_and_descriptions_follow_the_request_language(string language, int number, string label, string description)
        {
            var level = DeprReadingLevels.FromNumber(number)!;

            Assert.Equal(label, level.Label(language));
            Assert.Equal(description, level.Description(language));
        }
    }
}
