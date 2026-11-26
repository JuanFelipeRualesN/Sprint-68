using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans.Assistant
{
    /// <summary>
    /// Settings of the study-plan assistant, section <c>CustomPlanAssistant</c> of the app settings. Every value has a
    /// default, so the section is optional. The Foundry project is the one GenerateController uses
    /// (<c>Foundry:ProjectEndpoint</c>, registered in GSBaseAPI), and the agent is <c>CustomPlanAssistant:AgentName</c>
    /// or, when it is empty, <c>Foundry:AgentName</c>.
    /// </summary>
    public sealed class CustomPlanAssistantSettings
    {
        /// <summary>Foundry agent that interprets the free text. Default: Foundry:AgentName.</summary>
        public string? AgentName { get; init; }

        /// <summary>Weeks of each duration option (month, trimester, semester, school_year).</summary>
        public IReadOnlyDictionary<string, ushort> DurationWeeks { get; init; } = new Dictionary<string, ushort>
        {
            ["month"] = 4,
            ["trimester"] = 12,
            ["semester"] = 18,
            ["school_year"] = 36,
        };

        /// <summary>Session start time when the user does not give one.</summary>
        public TimeOnly DefaultStartTime { get; init; } = new(9, 0);

        /// <summary>Time zone of the sessions and deadlines (IANA or Windows id; both are accepted).</summary>
        public string DefaultTimezone { get; init; } = "America/Puerto_Rico";

        /// <summary>Estimated study minutes of one lesson; a lesson takes ceil(MinutesPerLesson / session minutes) sessions.</summary>
        public int MinutesPerLesson { get; init; } = 180;

        /// <summary>Minutes a conversation lives without activity.</summary>
        public int ConversationMinutes { get; init; } = 60;

        /// <summary>Longest message the user can send.</summary>
        public int MaxMessageLength { get; init; } = 500;

        /// <summary>Most turns in one conversation.</summary>
        public int MaxTurns { get; init; } = 60;

        /// <summary>Seconds to wait for the agent before falling back to asking again with the options.</summary>
        public int AgentTimeoutSeconds { get; init; } = 30;

        /// <summary>Lessons with a score under this value (0-100) are reinforced first.</summary>
        public double ReinforcementScoreThreshold { get; init; } = 70;

        /// <summary>Minutes allowed for the pre-test and the post-test.</summary>
        public ushort TestDurationMinutes { get; init; } = 30;

        /// <summary>Minutes allowed for an exam.</summary>
        public ushort ExamDurationMinutes { get; init; } = 60;

        /// <summary>Deadline of the pre-test (on the lesson's first session).</summary>
        public TimeOnly PreTestDueTime { get; init; } = new(23, 0);

        /// <summary>Deadline of a practice (the day after it opens, never after the plan ends).</summary>
        public TimeOnly PracticeDueTime { get; init; } = new(20, 0);

        /// <summary>Deadline of the exams (on the lesson's last session).</summary>
        public TimeOnly ExamDueTime { get; init; } = new(23, 0);

        /// <summary>Deadline of the post-test (on the lesson's last session).</summary>
        public TimeOnly PostTestDueTime { get; init; } = new(22, 0);

        /// <summary>Most lessons offered to the agent when matching a skill or need.</summary>
        public int MaxLessonCandidates { get; init; } = 30;

        /// <summary>Reads the section; missing or invalid values keep their default.</summary>
        /// <param name="configuration">App configuration.</param>
        /// <returns>The settings.</returns>
        public static CustomPlanAssistantSettings FromConfiguration(IConfiguration configuration)
        {
            var section = configuration.GetSection("CustomPlanAssistant");
            var defaults = new CustomPlanAssistantSettings();

            var weeks = new Dictionary<string, ushort>(defaults.DurationWeeks);
            foreach (var key in defaults.DurationWeeks.Keys)
            {
                if (ushort.TryParse(section[$"DurationWeeks:{key}"], out var value) && value > 0)
                {
                    weeks[key] = value;
                }
            }

            return new CustomPlanAssistantSettings
            {
                AgentName = Text(section["AgentName"]) ?? Text(configuration["Foundry:AgentName"]),
                DurationWeeks = weeks,
                DefaultStartTime = Time(section["DefaultStartTime"]) ?? defaults.DefaultStartTime,
                DefaultTimezone = Text(section["DefaultTimezone"]) ?? defaults.DefaultTimezone,
                MinutesPerLesson = Int(section["MinutesPerLesson"], 1) ?? defaults.MinutesPerLesson,
                ConversationMinutes = Int(section["ConversationMinutes"], 1) ?? defaults.ConversationMinutes,
                MaxMessageLength = Int(section["MaxMessageLength"], 1) ?? defaults.MaxMessageLength,
                MaxTurns = Int(section["MaxTurns"], 1) ?? defaults.MaxTurns,
                AgentTimeoutSeconds = Int(section["AgentTimeoutSeconds"], 1) ?? defaults.AgentTimeoutSeconds,
                ReinforcementScoreThreshold = double.TryParse(section["ReinforcementScoreThreshold"], NumberStyles.Float, CultureInfo.InvariantCulture, out var threshold)
                    ? threshold
                    : defaults.ReinforcementScoreThreshold,
                TestDurationMinutes = (ushort)(Int(section["TestDurationMinutes"], 1) ?? defaults.TestDurationMinutes),
                ExamDurationMinutes = (ushort)(Int(section["ExamDurationMinutes"], 1) ?? defaults.ExamDurationMinutes),
                PreTestDueTime = Time(section["PreTestDueTime"]) ?? defaults.PreTestDueTime,
                PracticeDueTime = Time(section["PracticeDueTime"]) ?? defaults.PracticeDueTime,
                ExamDueTime = Time(section["ExamDueTime"]) ?? defaults.ExamDueTime,
                PostTestDueTime = Time(section["PostTestDueTime"]) ?? defaults.PostTestDueTime,
                MaxLessonCandidates = Int(section["MaxLessonCandidates"], 1) ?? defaults.MaxLessonCandidates,
            };
        }

        private static string? Text(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static int? Int(string? value, int min) => int.TryParse(value, out var parsed) && parsed >= min ? parsed : null;

        private static TimeOnly? Time(string? value) =>
            TimeOnly.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed) ? parsed : null;
    }
}
