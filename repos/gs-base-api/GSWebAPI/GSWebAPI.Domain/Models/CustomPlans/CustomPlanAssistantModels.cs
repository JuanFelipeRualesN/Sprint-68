namespace GSWebAPI.GSWebAPI.Domain.Models.CustomPlans
{
    /// <summary>
    /// One turn of the study-plan assistant (<c>POST gswebapi/custom-plans/assistant/messages</c>). Start with a null
    /// <see cref="ConversationId"/> (and no message) to get the welcome step; then send the returned id with the user's
    /// answer: a clicked option in <see cref="SelectedOption"/> (or <see cref="SelectedOptions"/> for multi-select
    /// steps) and/or free text in <see cref="Message"/>.
    /// </summary>
    public class CustomPlanAssistantRequestModel
    {
        /// <summary>Id returned by the previous turn. Null starts a new conversation.</summary>
        public string? ConversationId { get; set; }

        /// <summary>Free text written by the user (limited by CustomPlanAssistant:MaxMessageLength). Optional when an option is sent.</summary>
        public string? Message { get; set; }

        /// <summary>The <c>value</c> of the option the user clicked (single-select steps, and the confirm step).</summary>
        public string? SelectedOption { get; set; }

        /// <summary>The <c>value</c>s of the options chosen in a multi-select step (subjects).</summary>
        public List<string>? SelectedOptions { get; set; }

        /// <summary>Where the assistant was opened from. Mode <c>create</c> (default) builds a new plan; <c>adjust</c> is reserved.</summary>
        public CustomPlanAssistantContextModel? Context { get; set; }
    }

    /// <summary>Where the assistant was opened from.</summary>
    public class CustomPlanAssistantContextModel
    {
        /// <summary><c>create</c> (default) or <c>adjust</c> (phase 2: change an existing plan; not available yet).</summary>
        public string? Mode { get; set; }

        /// <summary>Plan to adjust (adjust mode only).</summary>
        public ulong? PlanId { get; set; }

        /// <summary>Screen section the user opened the assistant from (informative).</summary>
        public string? Section { get; set; }
    }

    /// <summary>What the frontend renders after each turn.</summary>
    public class CustomPlanAssistantResponseModel
    {
        /// <summary>Send it back on the next turn.</summary>
        public string ConversationId { get; set; } = null!;

        /// <summary>
        /// Current step: plan_type, custom_needs, student, level, subjects, framework, days, daily_time, duration, confirm,
        /// created (and adjust, reserved).
        /// </summary>
        public string Step { get; set; } = null!;

        /// <summary>Text to show, in the session language (es by default, en with Accept-Language: en).</summary>
        public string Message { get; set; } = null!;

        /// <summary>Buttons/chips. <c>value</c> is the code or id to send back; <c>label</c> is localized.</summary>
        public List<CustomPlanAssistantOptionModel> Options { get; set; } = new();

        /// <summary>Whether the user may answer by typing.</summary>
        public bool AllowFreeText { get; set; }

        /// <summary>Whether several options can be chosen (send them in SelectedOptions).</summary>
        public bool MultiSelect { get; set; }

        /// <summary>Everything answered so far, so the UI can show it and let the user change it.</summary>
        public CustomPlanAssistantAnswersModel Answers { get; set; } = new();

        /// <summary>Readable summary of the plan, from the confirm step on.</summary>
        public CustomPlanPreviewModel? PlanPreview { get; set; }

        /// <summary>The exact body that will be sent to <c>POST gswebapi/custom-plans</c> when the user confirms.</summary>
        public CreateCustomPlanRequestModel? PlanPayload { get; set; }

        /// <summary>The created plan (same body as the 201 of <c>POST gswebapi/custom-plans</c>), on the created step.</summary>
        public CustomPlanResponseModel? CreatedPlan { get; set; }

        /// <summary>Error codes when the plan could not be created (the conversation stays open to fix them).</summary>
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>A button or chip.</summary>
    public class CustomPlanAssistantOptionModel
    {
        /// <summary>Code or id to send back in SelectedOption(s).</summary>
        public string Value { get; set; } = null!;

        /// <summary>Text to show, localized.</summary>
        public string Label { get; set; } = null!;
    }

    /// <summary>Answers collected so far. Null = not answered yet.</summary>
    public class CustomPlanAssistantAnswersModel
    {
        /// <summary>homeschool, reinforcements, specific_task or customized.</summary>
        public string? PlanTypeCode { get; set; }

        /// <summary>custom_plan_types.id of the plan type.</summary>
        public byte? CustomPlanTypeId { get; set; }

        /// <summary>The skill or needs the user described (specific_task and customized).</summary>
        public string? CustomNeeds { get; set; }

        /// <summary>Selected child.</summary>
        public ulong? StudentId { get; set; }

        /// <summary>Name of the selected child.</summary>
        public string? StudentName { get; set; }

        /// <summary>Level (grade) code, e.g. 2.</summary>
        public string? LevelCode { get; set; }

        /// <summary>Athenas level id.</summary>
        public ulong? LevelId { get; set; }

        /// <summary>Level name.</summary>
        public string? LevelName { get; set; }

        /// <summary>Selected subjects.</summary>
        public List<CustomPlanAssistantSubjectModel> Subjects { get; set; } = new();

        /// <summary>True = aligned to DEPR (curriculum framework 1); false = no framework.</summary>
        public bool? UsesFramework { get; set; }

        /// <summary>Study days, ISO 8601 (1 = Monday ... 7 = Sunday).</summary>
        public List<byte> Days { get; set; } = new();

        /// <summary>Minutes per study session.</summary>
        public ushort? DurationMinutes { get; set; }

        /// <summary>Session start time (local), HH:mm:ss.</summary>
        public string? StartTime { get; set; }

        /// <summary>Plan length in weeks.</summary>
        public ushort? PlanDurationWeeks { get; set; }

        /// <summary>First day of the plan.</summary>
        public DateOnly? StartDate { get; set; }

        /// <summary>Time zone of the sessions and deadlines.</summary>
        public string? Timezone { get; set; }
    }

    /// <summary>A subject of the plan.</summary>
    public class CustomPlanAssistantSubjectModel
    {
        /// <summary>Athenas subject id.</summary>
        public ulong Id { get; set; }

        /// <summary>Subject code, e.g. mat-sp.</summary>
        public string Code { get; set; } = null!;

        /// <summary>Subject name.</summary>
        public string Name { get; set; } = null!;
    }

    /// <summary>Readable summary of the plan shown before it is created.</summary>
    public class CustomPlanPreviewModel
    {
        /// <summary>Plan type name.</summary>
        public string? PlanTypeName { get; set; }

        /// <summary>Child's name.</summary>
        public string? StudentName { get; set; }

        /// <summary>Level (grade) name.</summary>
        public string? LevelName { get; set; }

        /// <summary>Subjects and how many lessons each one got.</summary>
        public List<CustomPlanPreviewSubjectModel> Subjects { get; set; } = new();

        /// <summary>Whether it is aligned to DEPR.</summary>
        public bool UsesFramework { get; set; }

        /// <summary>Study days.</summary>
        public List<CustomPlanAssistantOptionModel> Days { get; set; } = new();

        /// <summary>Session start time, HH:mm.</summary>
        public string StartTime { get; set; } = null!;

        /// <summary>Minutes per session.</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>Time zone.</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>Plan length in weeks.</summary>
        public ushort PlanDurationWeeks { get; set; }

        /// <summary>First day.</summary>
        public DateOnly StartDate { get; set; }

        /// <summary>Last day (the API's default: StartDate + weeks).</summary>
        public DateOnly EndDate { get; set; }

        /// <summary>Study sessions in the plan.</summary>
        public int TotalSessions { get; set; }

        /// <summary>Sessions each lesson takes (CustomPlanAssistant:MinutesPerLesson / session minutes, rounded up).</summary>
        public int SessionsPerLesson { get; set; }

        /// <summary>Lessons in the plan.</summary>
        public int LessonCount { get; set; }

        /// <summary>Lessons by week.</summary>
        public List<CustomPlanPreviewWeekModel> Weeks { get; set; } = new();

        /// <summary>Notes for the user (e.g. fewer lessons available than time, no lessons matched the skill).</summary>
        public List<string> Notes { get; set; } = new();
    }

    /// <summary>A subject in the preview.</summary>
    public class CustomPlanPreviewSubjectModel
    {
        /// <summary>Athenas subject id.</summary>
        public ulong Id { get; set; }

        /// <summary>Subject name.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Lessons of this subject in the plan.</summary>
        public int LessonCount { get; set; }
    }

    /// <summary>One week of the preview.</summary>
    public class CustomPlanPreviewWeekModel
    {
        /// <summary>Week number, from 1.</summary>
        public int Week { get; set; }

        /// <summary>Monday (or plan start) of the week.</summary>
        public DateOnly StartDate { get; set; }

        /// <summary>Lessons with a session this week.</summary>
        public List<CustomPlanPreviewLessonModel> Lessons { get; set; } = new();
    }

    /// <summary>A lesson in the preview.</summary>
    public class CustomPlanPreviewLessonModel
    {
        /// <summary>Order in the plan.</summary>
        public ushort Sequence { get; set; }

        /// <summary>Athenas lesson id.</summary>
        public ulong LessonId { get; set; }

        /// <summary>Lesson title.</summary>
        public string? Title { get; set; }

        /// <summary>Subject name.</summary>
        public string? SubjectName { get; set; }

        /// <summary>Session dates of the lesson that fall in this week.</summary>
        public List<DateOnly> Dates { get; set; } = new();
    }
}
