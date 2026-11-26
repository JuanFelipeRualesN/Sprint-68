using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// One of the four official reading levels of the Departamento de Educación de Puerto Rico (DEPR).
/// </summary>
/// <param name="Number">1 Principiante, 2 Aprendiz, 3 Proficiente, 4 Sobresaliente.</param>
/// <param name="LabelEs">Spanish label.</param>
/// <param name="LabelEn">English label.</param>
/// <param name="DescriptionEs">Spanish description (official text, third person).</param>
/// <param name="DescriptionEn">English description (official text, third person).</param>
public sealed record DeprReadingLevel(int Number, string LabelEs, string LabelEn, string DescriptionEs, string DescriptionEn)
{
    /// <summary>The label in the request language: Spanish for es-*, English otherwise.</summary>
    public string Label(string? languageCode) => DeprReadingLevels.IsSpanish(languageCode) ? LabelEs : LabelEn;

    /// <summary>The description in the request language: Spanish for es-*, English otherwise.</summary>
    public string Description(string? languageCode) => DeprReadingLevels.IsSpanish(languageCode) ? DescriptionEs : DescriptionEn;
}

/// <summary>
/// The reading tiers the scoring thresholds were calibrated on (the former 6-level scale). They are kept internally only
/// so the scoring does not change; every result is reported as one of the 4 DEPR levels.
/// </summary>
public enum ReadingTier
{
    /// <summary>Emergent (Emergente).</summary>
    Emergent = 0,

    /// <summary>Early / Beginning (Comenzando).</summary>
    Beginning = 1,

    /// <summary>Developing / Transitional (Desarrollándose / Transicional).</summary>
    Transitional = 2,

    /// <summary>Fluent (Fluido).</summary>
    Fluent = 3,

    /// <summary>Proficient (Proficiente).</summary>
    Proficient = 4,

    /// <summary>Advanced / Independent (Avanzado / Independiente).</summary>
    Advanced = 5,
}

/// <summary>
/// Single source of truth of the DEPR reading levels and of how the former 6-level scale maps to them.
/// </summary>
public static class DeprReadingLevels
{
    /// <summary>1. Principiante / Beginner.</summary>
    public static readonly DeprReadingLevel Principiante = new(
        1,
        "Principiante",
        "Beginner",
        "El estudiante presenta dificultades importantes en la destreza y necesita apoyo inmediato y frecuente.",
        "The student shows significant difficulties with the skill and needs immediate and frequent support.");

    /// <summary>2. Aprendiz / Apprentice.</summary>
    public static readonly DeprReadingLevel Aprendiz = new(
        2,
        "Aprendiz",
        "Apprentice",
        "El estudiante demuestra avances, pero todavía necesita fortalecer y consolidar la destreza.",
        "The student shows progress but still needs to strengthen and consolidate the skill.");

    /// <summary>3. Proficiente / Proficient.</summary>
    public static readonly DeprReadingLevel Proficiente = new(
        3,
        "Proficiente",
        "Proficient",
        "El estudiante demuestra el dominio esperado para su grado.",
        "The student shows the mastery expected for their grade.");

    /// <summary>4. Sobresaliente / Outstanding.</summary>
    public static readonly DeprReadingLevel Sobresaliente = new(
        4,
        "Sobresaliente",
        "Outstanding",
        "El estudiante supera el desempeño esperado para su grado y puede beneficiarse de actividades de enriquecimiento.",
        "The student exceeds the performance expected for their grade and can benefit from enrichment activities.");

    /// <summary>The four levels, in order.</summary>
    public static readonly IReadOnlyList<DeprReadingLevel> All = new[] { Principiante, Aprendiz, Proficiente, Sobresaliente };

    /// <summary>The level by number (1-4); null for any other number.</summary>
    public static DeprReadingLevel? FromNumber(int number) => All.FirstOrDefault(l => l.Number == number);

    /// <summary>Emergent and Beginning = 1, Transitional = 2, Fluent and Proficient = 3, Advanced = 4.</summary>
    public static DeprReadingLevel FromTier(ReadingTier tier) => tier switch
    {
        ReadingTier.Emergent or ReadingTier.Beginning => Principiante,
        ReadingTier.Transitional => Aprendiz,
        ReadingTier.Fluent or ReadingTier.Proficient => Proficiente,
        _ => Sobresaliente,
    };

    /// <summary>
    /// Starting story difficulty: the id in <c>story_levels</c>, which has the same 4 levels as DEPR (1 Principiante ...
    /// 4 Sobresaliente), so it is the DEPR level number.
    /// </summary>
    public static int StoryLevel(ReadingTier tier) => FromTier(tier).Number;

    /// <summary>
    /// Read-time mapping for results saved before the DEPR scale: the tier of a former label, e.g.
    /// "Fluent (Fluido)" or "Early / Beginning (Comenzando)" (English or Spanish part, any case), and of the former CEFR
    /// labels ("Level 3 (B1) (Nivel 3 - B1)", "Nivel 3 (B1)"). Null when the text is not a former label.
    /// </summary>
    public static ReadingTier? TierFromLegacyLabel(string? label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            return null;
        }

        var text = label.Trim().ToLowerInvariant();

        // Former 6-level scale. "Proficiente" is also checked before "Fluent" so it is not confused.
        if (text.Contains("advanced") || text.Contains("avanzado") || text.Contains("independ"))
        {
            return ReadingTier.Advanced;
        }

        if (text.Contains("proficient"))
        {
            return ReadingTier.Proficient;
        }

        if (text.Contains("fluent") || text.Contains("fluido"))
        {
            return ReadingTier.Fluent;
        }

        if (text.Contains("transitional") || text.Contains("transicional") || text.Contains("developing") || text.Contains("desarroll"))
        {
            return ReadingTier.Transitional;
        }

        if (text.Contains("beginning") || text.Contains("comenzando") || text.Contains("early"))
        {
            return ReadingTier.Beginning;
        }

        if (text.Contains("emergent"))
        {
            return ReadingTier.Emergent;
        }

        // Former CEFR fallback scale (5 levels).
        foreach (var (code, tier) in CefrTiers)
        {
            if (text.Contains("(" + code.ToLowerInvariant()) || text.Contains("- " + code.ToLowerInvariant()))
            {
                return tier;
            }
        }

        return null;
    }

    /// <summary>The DEPR level of a former label (see <see cref="TierFromLegacyLabel"/>); null when it is not one.</summary>
    public static DeprReadingLevel? FromLegacyLabel(string? label) =>
        TierFromLegacyLabel(label) is { } tier ? FromTier(tier) : null;

    /// <summary>
    /// Tier of each CEFR level (fallback scale when the grade is unknown): A1 Emergent, A2 Beginning, B1 Transitional,
    /// B2 Fluent, C1+ Advanced. Gives DEPR (and story) levels 1, 1, 2, 3, 4.
    /// </summary>
    public static readonly IReadOnlyList<(string Code, ReadingTier Tier)> CefrTiers = new[]
    {
        ("C1", ReadingTier.Advanced),
        ("B2", ReadingTier.Fluent),
        ("B1", ReadingTier.Transitional),
        ("A2", ReadingTier.Beginning),
        ("A1", ReadingTier.Emergent),
    };

    /// <summary>True for Spanish language codes (es, es-MX, es-PR...).</summary>
    public static bool IsSpanish(string? languageCode) =>
        !string.IsNullOrWhiteSpace(languageCode) && languageCode.Trim().StartsWith("es", StringComparison.OrdinalIgnoreCase);
}
