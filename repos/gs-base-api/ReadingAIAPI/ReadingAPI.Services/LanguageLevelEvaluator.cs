using System;
using System.Collections.Generic;
using System.Linq;

public class LanguageLevelEvaluator
{
    private record LevelCriteria(double MinOverall, double MinAccuracy, int MaxErrorsPer100);

    // Dictionary mapping Grade/CEFR key to a list of score thresholds (sorted lowest to highest)
    private static readonly Dictionary<string, List<(ReadingTier Tier, LevelCriteria Criteria)>> ThresholdMaps = new()
{
    // === GRADE-LEVEL PROFILES mapped to General Cognitive Reading Levels ===
    // Notes:
    // - Scores assumed 0–100. If your source is 0–1, scale before calling.
    // - MaxErrorsPer100 tightens with grade progression.
    // - Adjust after field calibration (teacher judgments, ROC analysis).

    { "Kinder", new List<(ReadingTier, LevelCriteria)>
        {
            (ReadingTier.Emergent,                     new LevelCriteria( 0.0,  0.0, 40)),
            (ReadingTier.Beginning,           new LevelCriteria(40.0, 45.0, 28)),
            (ReadingTier.Transitional, new LevelCriteria(55.0, 60.0, 18)),
            (ReadingTier.Fluent,                          new LevelCriteria(70.0, 75.0, 10)),
            (ReadingTier.Proficient,                 new LevelCriteria(80.0, 85.0,  6)),
            (ReadingTier.Advanced, new LevelCriteria(90.0, 92.0,  3))
        }
    },

    { "1st Grade", new List<(ReadingTier, LevelCriteria)>
        {
            (ReadingTier.Emergent,                     new LevelCriteria( 0.0,  0.0, 35)),
            (ReadingTier.Beginning,           new LevelCriteria(45.0, 50.0, 24)),
            (ReadingTier.Transitional, new LevelCriteria(60.0, 65.0, 15)),
            (ReadingTier.Fluent,                          new LevelCriteria(75.0, 80.0,  9)),
            (ReadingTier.Proficient,                 new LevelCriteria(85.0, 88.0,  5)),
            (ReadingTier.Advanced, new LevelCriteria(92.0, 94.0,  3))
        }
    },

    { "2nd Grade", new List<(ReadingTier, LevelCriteria)>
        {
            (ReadingTier.Emergent,                     new LevelCriteria( 0.0,  0.0, 30)),
            (ReadingTier.Beginning,           new LevelCriteria(50.0, 55.0, 20)),
            (ReadingTier.Transitional, new LevelCriteria(65.0, 70.0, 12)),
            (ReadingTier.Fluent,                          new LevelCriteria(78.0, 83.0,  8)),
            (ReadingTier.Proficient,                 new LevelCriteria(88.0, 90.0,  4)),
            (ReadingTier.Advanced, new LevelCriteria(93.0, 95.0,  2))
        }
    },

    { "3rd Grade", new List<(ReadingTier, LevelCriteria)>
        {
            (ReadingTier.Emergent,                     new LevelCriteria( 0.0,  0.0, 26)),
            (ReadingTier.Beginning,           new LevelCriteria(55.0, 60.0, 18)),
            (ReadingTier.Transitional, new LevelCriteria(70.0, 75.0, 10)),
            (ReadingTier.Fluent,                          new LevelCriteria(80.0, 85.0,  6)),
            (ReadingTier.Proficient,                 new LevelCriteria(90.0, 92.0,  3)),
            (ReadingTier.Advanced, new LevelCriteria(94.0, 96.0,  2))
        }
    },

    { "4th Grade", new List<(ReadingTier, LevelCriteria)>
        {
            (ReadingTier.Emergent,                     new LevelCriteria( 0.0,  0.0, 22)),
            (ReadingTier.Beginning,           new LevelCriteria(60.0, 65.0, 16)),
            (ReadingTier.Transitional, new LevelCriteria(72.0, 77.0,  9)),
            (ReadingTier.Fluent,                          new LevelCriteria(82.0, 87.0,  5)),
            (ReadingTier.Proficient,                 new LevelCriteria(91.0, 93.0,  3)),
            (ReadingTier.Advanced, new LevelCriteria(95.0, 97.0,  1))
        }
    },

    { "5th Grade", new List<(ReadingTier, LevelCriteria)>
        {
            (ReadingTier.Emergent,                     new LevelCriteria( 0.0,  0.0, 20)),
            (ReadingTier.Beginning,           new LevelCriteria(62.0, 67.0, 14)),
            (ReadingTier.Transitional, new LevelCriteria(75.0, 80.0,  8)),
            (ReadingTier.Fluent,                          new LevelCriteria(85.0, 88.0,  4)),
            (ReadingTier.Proficient,                 new LevelCriteria(92.0, 94.0,  2)),
            (ReadingTier.Advanced, new LevelCriteria(96.0, 98.0,  1))
        }
    },

    // === DEFAULT CEFR PROFILES (Fallback for general or higher level use) ===
    { "CEFR-en", new List<(ReadingTier, LevelCriteria)>
        {
            (ReadingTier.Emergent,  new LevelCriteria(0.0,  0.0, 15)),
            (ReadingTier.Beginning,  new LevelCriteria(50.0, 55.0, 10)),
            (ReadingTier.Transitional,  new LevelCriteria(65.0, 70.0,  5)),
            (ReadingTier.Fluent,  new LevelCriteria(80.0, 85.0,  2)),
            (ReadingTier.Advanced, new LevelCriteria(90.0, 92.0,  1))
        }
    },
    { "CEFR-es", new List<(ReadingTier, LevelCriteria)>
        {
            (ReadingTier.Emergent,  new LevelCriteria(0.0,  0.0, 18)),
            (ReadingTier.Beginning,  new LevelCriteria(50.0, 55.0, 12)),
            (ReadingTier.Transitional,  new LevelCriteria(65.0, 70.0,  6)),
            (ReadingTier.Fluent,  new LevelCriteria(80.0, 85.0,  3)),
            (ReadingTier.Advanced, new LevelCriteria(90.0, 92.0,  1))
        }
    }
};


    /// <summary>
    /// The thresholds of a grade ("Kinder", "1st Grade" ... "5th Grade") or of the CEFR fallback ("CEFR-es" / "CEFR-en"),
    /// lowest tier first. Unknown keys use CEFR-en, as before. Exposed for tests.
    /// </summary>
    public static IReadOnlyList<(ReadingTier Tier, double MinOverall, double MinAccuracy, int MaxErrorsPer100)> Thresholds(string? studentGrade, string languageCode)
    {
        var key = studentGrade ?? (languageCode.StartsWith("es", StringComparison.OrdinalIgnoreCase) ? "CEFR-es" : "CEFR-en");
        if (!ThresholdMaps.TryGetValue(key, out var thresholds))
        {
            thresholds = ThresholdMaps["CEFR-en"];
        }

        return thresholds.Select(t => (t.Tier, t.Criteria.MinOverall, t.Criteria.MinAccuracy, t.Criteria.MaxErrorsPer100)).ToList();
    }

    /// <summary>
    /// Calculates the highest reading tier achieved based on the Azure assessment scores, using grade-specific thresholds
    /// if available (the scoring is the same as before the DEPR scale), and maps it to the DEPR level and the story level.
    /// </summary>
    /// <param name="azureResult">Azure pronunciation assessment summary.</param>
    /// <param name="languageCode">Request language (es-MX, en-US...): picks the CEFR fallback when there is no grade.</param>
    /// <param name="referenceText">The passage the student read (its word count scales the allowed errors).</param>
    /// <param name="studentGrade">The student's grade (e.g., "Kinder", "5th Grade") or null.</param>
    /// <returns>The tier, its DEPR level, its story level and the score details.</returns>
    public ReadingLevelResult DetermineLevel(
        AssessmentResultDto azureResult,
        string languageCode,
        string referenceText,
        string? studentGrade)
    {
        // 1. Determine which threshold map to use
        var thresholds = Thresholds(studentGrade, languageCode);

        // 2. Prepare scores and counts
        var wordCount = referenceText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        // Mispronounced words are those explicitly marked by Azure as an error (not just low score)
        var mispronouncedWordCount = azureResult.Issues.Count(i => i.ErrorType != null && !i.ErrorType.Equals("None", StringComparison.OrdinalIgnoreCase));

        var overallScore = azureResult.Overall.Pronunciation;
        var accuracyScore = azureResult.Overall.Accuracy;

        // 3. From the highest tier down: the first whose criteria are all met
        foreach (var (tier, minOverall, minAccuracy, maxErrorsPer100) in thresholds.Reverse())
        {
            var actualMaxErrors = (int)Math.Ceiling(maxErrorsPer100 * (wordCount / 100.0));

            if (overallScore >= minOverall &&
                accuracyScore >= minAccuracy &&
                mispronouncedWordCount <= actualMaxErrors)
            {
                return ReadingLevelResult.Of(tier, $"Scores: Overall={overallScore:F1}, Accuracy={accuracyScore:F1}. Errors: {mispronouncedWordCount} (Max Allowed: {actualMaxErrors})");
            }
        }

        // 4. Return the lowest tier if no criteria were met
        return ReadingLevelResult.Of(thresholds[0].Tier, "Did not meet minimum criteria for any defined level.");
    }
}

/// <summary>Result of the reading level evaluation.</summary>
/// <param name="Tier">Tier reached on the calibrated scale.</param>
/// <param name="Level">DEPR reading level of that tier.</param>
/// <param name="StoryLevel">Starting story difficulty (story_levels id, 1-4): the DEPR level number.</param>
/// <param name="ScoreDetails">Scores and errors behind the result (diagnostics).</param>
public sealed record ReadingLevelResult(ReadingTier Tier, DeprReadingLevel Level, int StoryLevel, string ScoreDetails)
{
    /// <summary>Builds the result of a tier.</summary>
    public static ReadingLevelResult Of(ReadingTier tier, string details) =>
        new(tier, DeprReadingLevels.FromTier(tier), DeprReadingLevels.StoryLevel(tier), details);
}
