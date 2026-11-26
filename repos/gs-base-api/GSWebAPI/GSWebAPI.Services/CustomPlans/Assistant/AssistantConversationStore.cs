using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.Extensions.Caching.Memory;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans.Assistant
{
    /// <summary>Step names, as the frontend receives them.</summary>
    public static class AssistantSteps
    {
        /// <summary>Choose the plan type.</summary>
        public const string PlanType = "plan_type";

        /// <summary>Describe the skill (specific_task) or the needs (customized).</summary>
        public const string CustomNeeds = "custom_needs";

        /// <summary>Choose the child.</summary>
        public const string Student = "student";

        /// <summary>Choose the grade.</summary>
        public const string Level = "level";

        /// <summary>Choose the subjects (multi-select).</summary>
        public const string Subjects = "subjects";

        /// <summary>DEPR alignment yes/no.</summary>
        public const string Framework = "framework";

        /// <summary>Study days.</summary>
        public const string Days = "days";

        /// <summary>Minutes per day.</summary>
        public const string DailyTime = "daily_time";

        /// <summary>Plan length.</summary>
        public const string Duration = "duration";

        /// <summary>Preview and confirmation.</summary>
        public const string Confirm = "confirm";

        /// <summary>The plan was created.</summary>
        public const string Created = "created";

        /// <summary>Reserved for adjust mode (phase 2).</summary>
        public const string Adjust = "adjust";
    }

    /// <summary>
    /// Structured state of one conversation: what was answered, the data the options came from (so answers are checked
    /// against them), and the preview the user is confirming. Never holds the user's token.
    /// </summary>
    public sealed class AssistantConversation
    {
        /// <summary>Conversation id (random).</summary>
        public string Id { get; init; } = null!;

        /// <summary>Owner: only this user can continue it.</summary>
        public ulong UserId { get; init; }

        /// <summary>One turn at a time.</summary>
        public SemaphoreSlim Lock { get; } = new(1, 1);

        /// <summary>Turns answered.</summary>
        public int Turns { get; set; }

        /// <summary>Current step.</summary>
        public string Step { get; set; } = AssistantSteps.PlanType;

        /// <summary>Plan type code.</summary>
        public string? PlanTypeCode { get; set; }

        /// <summary>Skill or needs text (data, never instructions).</summary>
        public string? CustomNeeds { get; set; }

        /// <summary>The client's children, as read from the Users API.</summary>
        public List<StudentProfile>? Children { get; set; }

        /// <summary>Selected child.</summary>
        public StudentProfile? Student { get; set; }

        /// <summary>Grades of the child's active subscriptions.</summary>
        public List<AssistantLevel>? Levels { get; set; }

        /// <summary>Selected grade.</summary>
        public AssistantLevel? Level { get; set; }

        /// <summary>Subjects of the selected grade in the child's active subscriptions.</summary>
        public List<AssistantSubject>? AvailableSubjects { get; set; }

        /// <summary>Selected subjects.</summary>
        public List<AssistantSubject> Subjects { get; set; } = new();

        /// <summary>DEPR alignment.</summary>
        public bool? UsesFramework { get; set; }

        /// <summary>Study days (ISO).</summary>
        public List<byte> Days { get; set; } = new();

        /// <summary>Minutes per session.</summary>
        public ushort? DurationMinutes { get; set; }

        /// <summary>Plan weeks.</summary>
        public ushort? Weeks { get; set; }

        /// <summary>Start time; null = settings default.</summary>
        public TimeOnly? StartTime { get; set; }

        /// <summary>Start date; null = next Monday.</summary>
        public DateOnly? StartDate { get; set; }

        /// <summary>Lesson ids the model matched to the skill or needs (only real catalog ids), per subject id.</summary>
        public Dictionary<ulong, HashSet<ulong>>? MatchedLessons { get; set; }

        /// <summary>Fingerprint of the preview returned last; confirming is only accepted while it is set and unchanged.</summary>
        public string? ShownPreviewHash { get; set; }

        /// <summary>True after the user chose "change something" on the confirm step.</summary>
        public bool AwaitingChange { get; set; }

        /// <summary>Id of the created plan.</summary>
        public ulong? CreatedPlanId { get; set; }

        /// <summary>Last response (returned again when the plan is already created).</summary>
        public object? LastResponse { get; set; }
    }

    /// <summary>A grade the child can study.</summary>
    /// <param name="Code">Level code, e.g. 2.</param>
    /// <param name="Id">Athenas level id.</param>
    /// <param name="Name">Level name.</param>
    public sealed record AssistantLevel(string Code, ulong Id, string Name);

    /// <summary>A subject the child can study.</summary>
    /// <param name="Id">Athenas subject id.</param>
    /// <param name="Code">Subject code, e.g. mat-sp.</param>
    /// <param name="Name">Subject name.</param>
    public sealed record AssistantSubject(ulong Id, string Code, string Name);

    /// <summary>Conversations in memory (single instance), expiring after CustomPlanAssistant:ConversationMinutes without activity.</summary>
    public interface IAssistantConversationStore
    {
        /// <summary>Starts a conversation for the user.</summary>
        AssistantConversation Create(ulong userId);

        /// <summary>The conversation if it exists and belongs to the user; null otherwise.</summary>
        AssistantConversation? Get(string id, ulong userId);

        /// <summary>Keeps the conversation alive for another expiration period.</summary>
        void Touch(AssistantConversation conversation);
    }

    /// <inheritdoc />
    public sealed class AssistantConversationStore : IAssistantConversationStore
    {
        private readonly IMemoryCache _cache;
        private readonly CustomPlanAssistantSettings _settings;

        /// <summary>Creates the store.</summary>
        /// <param name="cache">Memory cache.</param>
        /// <param name="settings">Expiration.</param>
        public AssistantConversationStore(IMemoryCache cache, CustomPlanAssistantSettings settings)
        {
            _cache = cache;
            _settings = settings;
        }

        /// <inheritdoc />
        public AssistantConversation Create(ulong userId)
        {
            var conversation = new AssistantConversation { Id = Guid.NewGuid().ToString("N"), UserId = userId };
            Touch(conversation);
            return conversation;
        }

        /// <inheritdoc />
        public AssistantConversation? Get(string id, ulong userId) =>
            _cache.TryGetValue(Key(id), out AssistantConversation? conversation) && conversation != null && conversation.UserId == userId
                ? conversation
                : null;

        /// <inheritdoc />
        public void Touch(AssistantConversation conversation) =>
            _cache.Set(Key(conversation.Id), conversation, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(_settings.ConversationMinutes) });

        private static string Key(string id) => $"custom-plan-assistant:{id}";
    }
}
