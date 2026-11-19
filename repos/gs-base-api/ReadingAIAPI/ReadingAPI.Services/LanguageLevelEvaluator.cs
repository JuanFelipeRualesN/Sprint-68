using System;
using System.Collections.Generic;
using System.Linq;

public class LanguageLevelEvaluator
{
    private record LevelCriteria(double MinOverall, double MinAccuracy, int MaxErrorsPer100);

    // Dictionary mapping Grade/CEFR key to a list of score thresholds (sorted lowest to highest)
    private static readonly Dictionary<string, List<(string LevelName, LevelCriteria Criteria)>> ThresholdMaps = new()
{
    // === GRADE-LEVEL PROFILES mapped to General Cognitive Reading Levels ===
    // Notes:
    // - Scores assumed 0–100. If your source is 0–1, scale before calling.
    // - MaxErrorsPer100 tightens with grade progression.
    // - Adjust after field calibration (teacher judgments, ROC analysis).

    { "Kinder", new List<(string, LevelCriteria)>
        {
            ("Emergent (Emergente)",                     new LevelCriteria( 0.0,  0.0, 40)),
            ("Early / Beginning (Comenzando)",           new LevelCriteria(40.0, 45.0, 28)),
            ("Developing / Transitional (Desarrollándose / Transicional)", new LevelCriteria(55.0, 60.0, 18)),
            ("Fluent (Fluido)",                          new LevelCriteria(70.0, 75.0, 10)),
            ("Proficient (Proficiente)",                 new LevelCriteria(80.0, 85.0,  6)),
            ("Advanced / Independent (Avanzado / Independiente)", new LevelCriteria(90.0, 92.0,  3))
        }
    },

    { "1st Grade", new List<(string, LevelCriteria)>
        {
            ("Emergent (Emergente)",                     new LevelCriteria( 0.0,  0.0, 35)),
            ("Early / Beginning (Comenzando)",           new LevelCriteria(45.0, 50.0, 24)),
            ("Developing / Transitional (Desarrollándose / Transicional)", new LevelCriteria(60.0, 65.0, 15)),
            ("Fluent (Fluido)",                          new LevelCriteria(75.0, 80.0,  9)),
            ("Proficient (Proficiente)",                 new LevelCriteria(85.0, 88.0,  5)),
            ("Advanced / Independent (Avanzado / Independiente)", new LevelCriteria(92.0, 94.0,  3))
        }
    },

    { "2nd Grade", new List<(string, LevelCriteria)>
        {
            ("Emergent (Emergente)",                     new LevelCriteria( 0.0,  0.0, 30)),
            ("Early / Beginning (Comenzando)",           new LevelCriteria(50.0, 55.0, 20)),
            ("Developing / Transitional (Desarrollándose / Transicional)", new LevelCriteria(65.0, 70.0, 12)),
            ("Fluent (Fluido)",                          new LevelCriteria(78.0, 83.0,  8)),
            ("Proficient (Proficiente)",                 new LevelCriteria(88.0, 90.0,  4)),
            ("Advanced / Independent (Avanzado / Independiente)", new LevelCriteria(93.0, 95.0,  2))
        }
    },

    { "3rd Grade", new List<(string, LevelCriteria)>
        {
            ("Emergent (Emergente)",                     new LevelCriteria( 0.0,  0.0, 26)),
            ("Early / Beginning (Comenzando)",           new LevelCriteria(55.0, 60.0, 18)),
            ("Developing / Transitional (Desarrollándose / Transicional)", new LevelCriteria(70.0, 75.0, 10)),
            ("Fluent (Fluido)",                          new LevelCriteria(80.0, 85.0,  6)),
            ("Proficient (Proficiente)",                 new LevelCriteria(90.0, 92.0,  3)),
            ("Advanced / Independent (Avanzado / Independiente)", new LevelCriteria(94.0, 96.0,  2))
        }
    },

    { "4th Grade", new List<(string, LevelCriteria)>
        {
            ("Emergent (Emergente)",                     new LevelCriteria( 0.0,  0.0, 22)),
            ("Early / Beginning (Comenzando)",           new LevelCriteria(60.0, 65.0, 16)),
            ("Developing / Transitional (Desarrollándose / Transicional)", new LevelCriteria(72.0, 77.0,  9)),
            ("Fluent (Fluido)",                          new LevelCriteria(82.0, 87.0,  5)),
            ("Proficient (Proficiente)",                 new LevelCriteria(91.0, 93.0,  3)),
            ("Advanced / Independent (Avanzado / Independiente)", new LevelCriteria(95.0, 97.0,  1))
        }
    },

    { "5th Grade", new List<(string, LevelCriteria)>
        {
            ("Emergent (Emergente)",                     new LevelCriteria( 0.0,  0.0, 20)),
            ("Early / Beginning (Comenzando)",           new LevelCriteria(62.0, 67.0, 14)),
            ("Developing / Transitional (Desarrollándose / Transicional)", new LevelCriteria(75.0, 80.0,  8)),
            ("Fluent (Fluido)",                          new LevelCriteria(85.0, 88.0,  4)),
            ("Proficient (Proficiente)",                 new LevelCriteria(92.0, 94.0,  2)),
            ("Advanced / Independent (Avanzado / Independiente)", new LevelCriteria(96.0, 98.0,  1))
        }
    },

    // === DEFAULT CEFR PROFILES (Fallback for general or higher level use) ===
    { "CEFR-en", new List<(string, LevelCriteria)>
        {
            ("Level 1 (A1) (Nivel 1 - A1)",  new LevelCriteria(0.0,  0.0, 15)),
            ("Level 2 (A2) (Nivel 2 - A2)",  new LevelCriteria(50.0, 55.0, 10)),
            ("Level 3 (B1) (Nivel 3 - B1)",  new LevelCriteria(65.0, 70.0,  5)),
            ("Level 4 (B2) (Nivel 4 - B2)",  new LevelCriteria(80.0, 85.0,  2)),
            ("Level 5 (C1+) (Nivel 5 - C1+)", new LevelCriteria(90.0, 92.0,  1))
        }
    },
    { "CEFR-es", new List<(string, LevelCriteria)>
        {
            ("Nivel 1 (A1)",  new LevelCriteria(0.0,  0.0, 18)),
            ("Nivel 2 (A2)",  new LevelCriteria(50.0, 55.0, 12)),
            ("Nivel 3 (B1)",  new LevelCriteria(65.0, 70.0,  6)),
            ("Nivel 4 (B2)",  new LevelCriteria(80.0, 85.0,  3)),
            ("Nivel 5 (C1+)", new LevelCriteria(90.0, 92.0,  1))
        }
    }
};


    /// <summary>
    /// Calculates the highest custom language level achieved based on Azure Assessment scores, 
    /// using grade-specific thresholds if available.
    /// </summary>
    /// <param name="studentGrade">The student's grade (e.g., "Kinder", "5th Grade") or null.</param>
    public (string Level, string Description) DetermineLevel(
        AssessmentResultDto azureResult,
        string languageCode,
        string referenceText,
        string? studentGrade)
    {
        // 1. Determine which threshold map to use
        var thresholdKey = studentGrade ?? (languageCode.StartsWith("es", StringComparison.OrdinalIgnoreCase) ? "CEFR-es" : "CEFR-en");

        // Fallback to default English if the specific grade/key is not found
        if (!ThresholdMaps.TryGetValue(thresholdKey, out var thresholds))
        {
            thresholds = ThresholdMaps["CEFR-en"];
        }

        // 2. Prepare scores and counts
        var wordCount = referenceText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        // Mispronounced words are those explicitly marked by Azure as an error (not just low score)
        var mispronouncedWordCount = azureResult.Issues.Count(i => i.ErrorType != null && !i.ErrorType.Equals("None", StringComparison.OrdinalIgnoreCase));

        var overallScore = azureResult.Overall.Pronunciation;
        var accuracyScore = azureResult.Overall.Accuracy;

        // 3. Iterate backward (from highest level down)
        // Use a reverse copy or reverse iteration on the list
        foreach (var (level, criteria) in thresholds.Reverse<ValueTuple<string, LevelCriteria>>())
        {
            var actualMaxErrors = (int)Math.Ceiling(criteria.MaxErrorsPer100 * (wordCount / 100.0));

            if (overallScore >= criteria.MinOverall &&
                accuracyScore >= criteria.MinAccuracy &&
                mispronouncedWordCount <= actualMaxErrors)
            {
                return (level, $"Scores: Overall={overallScore:F1}, Accuracy={accuracyScore:F1}. Errors: {mispronouncedWordCount} (Max Allowed: {actualMaxErrors})");
            }
        }

        // 4. Return the lowest level if no criteria were met
        return (thresholds.First().LevelName, "Did not meet minimum criteria for any defined level.");
    }
}