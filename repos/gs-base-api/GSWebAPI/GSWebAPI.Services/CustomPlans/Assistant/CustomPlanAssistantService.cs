using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans.Assistant
{
    /// <summary>The study-plan assistant: one call per turn of the conversation.</summary>
    public interface ICustomPlanAssistantService
    {
        /// <summary>
        /// Answers one turn: applies the user's answer (clicked option or free text) to the current step, then returns the
        /// next step with its message and options, the answers so far and, from the confirm step on, the plan preview and
        /// payload. The plan is created only when the user confirms the preview returned in the previous turn.
        /// </summary>
        /// <param name="user">The client in session (its token is used for the downstream calls and never stored).</param>
        /// <param name="request">The turn.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The turn's response. Fails with Forbidden (not a client), NotFound (conversation unknown, expired or of another
        /// user), Validation (message too long, adjust mode) or Conflict (another turn in progress, too many turns).
        /// </returns>
        Task<ServiceResult<CustomPlanAssistantResponseModel>> SendAsync(RequestUser user, CustomPlanAssistantRequestModel request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// The code drives the conversation: it decides the steps, builds the options from real data (children, active
    /// subscriptions, Athenas catalog), validates every answer and builds the plan with <see cref="CustomPlanBuilder"/>.
    /// The agent (<see cref="IAssistantLanguageModel"/>) is only asked to read free text that does not match an option
    /// directly, and to pick the lessons that fit a described skill among real catalog lessons.
    /// </summary>
    public sealed class CustomPlanAssistantService : ICustomPlanAssistantService
    {
        private const string Homeschool = "homeschool";
        private const string Reinforcements = "reinforcements";
        private const string SpecificTask = "specific_task";
        private const string Customized = "customized";
        private const string OptionYes = "yes";
        private const string OptionNo = "no";
        private const string OptionConfirm = "confirm";
        private const string OptionChange = "change";
        private const string OptionChangeStudent = "change_student";
        private const int MaxReplyLength = 600;
        private const int CustomNeedsMaxLength = 300;
        private const int MinSessionMinutes = 15;
        private const int MaxSessionMinutes = 480;
        private const int MaxWeeks = 52;

        private static readonly string[] DayOptions = { "weekdays", "3", "2", "weekend" };
        private static readonly string[] TimeOptions = { "30", "45", "60", "120", "180" };
        private static readonly string[] DurationOptions = { "month", "trimester", "semester", "school_year" };

        private static readonly Dictionary<string, byte> DayNames = new(StringComparer.Ordinal)
        {
            ["lunes"] = 1, ["monday"] = 1, ["martes"] = 2, ["tuesday"] = 2, ["miercoles"] = 3, ["wednesday"] = 3,
            ["jueves"] = 4, ["thursday"] = 4, ["viernes"] = 5, ["friday"] = 5, ["sabado"] = 6, ["saturday"] = 6,
            ["domingo"] = 7, ["sunday"] = 7,
        };

        private static readonly HashSet<string> YesWords = new(StringComparer.Ordinal) { "si", "yes", "ok", "okay", "claro", "dale", "correcto", "confirmo", "confirmar", "crear", "crealo", "acepto", "sure", "yep", "create" };
        private static readonly HashSet<string> NoWords = new(StringComparer.Ordinal) { "no", "nop", "nope" };

        private readonly GSWebDbContext _db;
        private readonly ICustomPlanService _plans;
        private readonly ICustomPlanOverviewService _overview;
        private readonly IUserDirectoryClient _users;
        private readonly IClientSubscriptionsClient _subscriptions;
        private readonly IAthenasCatalogClient _athenas;
        private readonly IAthenasLessonSearchClient _lessons;
        private readonly IAssistantLanguageModel _model;
        private readonly IAssistantConversationStore _store;
        private readonly CustomPlanAssistantSettings _settings;
        private readonly GSWebModuleSettings _moduleSettings;
        private readonly TimeProvider _time;
        private readonly ILogger<CustomPlanAssistantService> _logger;

        /// <summary>Creates the service.</summary>
        public CustomPlanAssistantService(
            GSWebDbContext db,
            ICustomPlanService plans,
            ICustomPlanOverviewService overview,
            IUserDirectoryClient users,
            IClientSubscriptionsClient subscriptions,
            IAthenasCatalogClient athenas,
            IAthenasLessonSearchClient lessons,
            IAssistantLanguageModel model,
            IAssistantConversationStore store,
            CustomPlanAssistantSettings settings,
            GSWebModuleSettings moduleSettings,
            TimeProvider time,
            ILogger<CustomPlanAssistantService> logger)
        {
            _db = db;
            _plans = plans;
            _overview = overview;
            _users = users;
            _subscriptions = subscriptions;
            _athenas = athenas;
            _lessons = lessons;
            _model = model;
            _store = store;
            _settings = settings;
            _moduleSettings = moduleSettings;
            _time = time;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanAssistantResponseModel>> SendAsync(RequestUser user, CustomPlanAssistantRequestModel request, CancellationToken cancellationToken = default)
        {
            if (string.Equals(request.Context?.Mode?.Trim(), "adjust", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<CustomPlanAssistantResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.AssistantAdjustNotAvailable);
            }

            if (user.RoleId != _moduleSettings.ClientRoleId)
            {
                return ServiceResult<CustomPlanAssistantResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.PlanOnlyClients);
            }

            var message = request.Message?.Trim();
            if (message?.Length > _settings.MaxMessageLength)
            {
                return ServiceResult<CustomPlanAssistantResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.AssistantMessageTooLong);
            }

            AssistantConversation? conversation;
            if (string.IsNullOrWhiteSpace(request.ConversationId))
            {
                conversation = _store.Create(user.UserId);
            }
            else
            {
                conversation = _store.Get(request.ConversationId.Trim(), user.UserId);
                if (conversation == null)
                {
                    return ServiceResult<CustomPlanAssistantResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.AssistantConversationNotFound);
                }
            }

            if (!await conversation.Lock.WaitAsync(0, cancellationToken))
            {
                return ServiceResult<CustomPlanAssistantResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.AssistantBusy);
            }

            try
            {
                if (conversation.Turns >= _settings.MaxTurns)
                {
                    return ServiceResult<CustomPlanAssistantResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.AssistantTooManyTurns);
                }

                conversation.Turns++;
                var turn = new Turn(user, CustomPlanAssistantTexts.Language(user.LangCode), message, Selected(request), cancellationToken);

                if (conversation.Step == AssistantSteps.Created && conversation.LastResponse is CustomPlanAssistantResponseModel done)
                {
                    done.Message = CustomPlanAssistantTexts.Get("already_created", turn.Lang);
                    return ServiceResult<CustomPlanAssistantResponseModel>.Ok(done);
                }

                var context = await LoadContextAsync(turn);
                var outcome = turn.HasInput ? await ApplyAsync(conversation, turn, context) : new Outcome();
                var response = await RenderAsync(conversation, turn, context, outcome);
                conversation.LastResponse = response;
                return ServiceResult<CustomPlanAssistantResponseModel>.Ok(response);
            }
            finally
            {
                _store.Touch(conversation);
                conversation.Lock.Release();
            }
        }

        // ----- Applying the user's answer -----

        /// <summary>Applies the turn's input to the current step; returns what the next render should say or do.</summary>
        private async Task<Outcome> ApplyAsync(AssistantConversation c, Turn turn, PlanContext context)
        {
            var step = NextStep(c);
            var options = await OptionsAsync(c, step, turn, context);

            // Clicked options are applied as they are, after checking they are valid values of the step.
            if (turn.Selected.Count > 0 && (step != AssistantSteps.CustomNeeds))
            {
                var valid = turn.Selected.Where(v => options.Any(o => o.Value == v)).ToList();
                if (valid.Count == 0)
                {
                    return Outcome.NotUnderstood;
                }

                return await ApplyValuesAsync(c, step, valid, null, null, turn, context);
            }

            if (string.IsNullOrEmpty(turn.Message))
            {
                return Outcome.NotUnderstood;
            }

            if (step == AssistantSteps.CustomNeeds)
            {
                c.CustomNeeds = turn.Message.Length > CustomNeedsMaxLength ? turn.Message[..CustomNeedsMaxLength] : turn.Message;
                c.MatchedLessons = null;
                Invalidate(c);
                return new Outcome();
            }

            // Simple answers are matched in code (no model call): an option label, yes/no, a number, day names.
            var direct = MatchDirectly(step, options, turn.Message);
            if (direct != null)
            {
                return await ApplyValuesAsync(c, step, direct.Values ?? new List<string>(), direct.Number, direct.Days, turn, context);
            }

            var interpretation = await InterpretAsync(c, step, options, turn, context);
            if (interpretation == null)
            {
                return Outcome.NotUnderstood;
            }

            var outcome = new Outcome();
            var changed = interpretation.Changes != null && ApplyChanges(c, interpretation.Changes, context);
            switch (interpretation.Intent?.Trim().ToLowerInvariant())
            {
                case "question":
                    var reply = interpretation.Reply?.Trim();
                    outcome.Prefix = string.IsNullOrEmpty(reply) ? null : (reply.Length > MaxReplyLength ? reply[..MaxReplyLength] : reply);
                    return outcome;

                case "confirm" when step == AssistantSteps.Confirm && !changed:
                    return new Outcome { Submit = true };

                case "reject" when step == AssistantSteps.Confirm:
                    c.AwaitingChange = true;
                    return outcome;

                case "answer" when step != AssistantSteps.Confirm:
                    var values = (interpretation.Values ?? new List<string>()).Where(v => options.Any(o => o.Value == v)).ToList();
                    if (values.Count == 0 && interpretation.Number == null && interpretation.Days is not { Count: > 0 })
                    {
                        return changed ? outcome : Outcome.NotUnderstood;
                    }

                    return await ApplyValuesAsync(c, step, values, interpretation.Number, interpretation.Days, turn, context);
            }

            if (changed)
            {
                c.AwaitingChange = false;
                outcome.Prefix = CustomPlanAssistantTexts.Get("confirm_updated", turn.Lang);
                return outcome;
            }

            return Outcome.NotUnderstood;
        }

        /// <summary>Applies valid values (or a number / named days) to the step. Values were already checked against the options.</summary>
        private async Task<Outcome> ApplyValuesAsync(AssistantConversation c, string step, List<string> values, int? number, List<int>? days, Turn turn, PlanContext context)
        {
            var value = values.FirstOrDefault();
            switch (step)
            {
                case AssistantSteps.PlanType when value != null:
                    if (c.PlanTypeCode != value)
                    {
                        c.PlanTypeCode = value;
                        c.CustomNeeds = value is SpecificTask or Customized ? c.CustomNeeds : null;
                        c.MatchedLessons = null;
                    }

                    break;

                case AssistantSteps.Student when value != null && ulong.TryParse(value, out var studentId):
                    var student = c.Children?.FirstOrDefault(s => s.UserId == studentId);
                    if (student == null)
                    {
                        return Outcome.NotUnderstood;
                    }

                    if (c.Student?.UserId != student.UserId)
                    {
                        c.Student = student;
                        c.Levels = null;
                        c.Level = null;
                        c.AvailableSubjects = null;
                        c.Subjects.Clear();
                        c.MatchedLessons = null;
                    }

                    break;

                case AssistantSteps.Level when value == OptionChangeStudent:
                    c.Student = null;
                    c.Levels = null;
                    break;

                case AssistantSteps.Level when value != null:
                    var level = c.Levels?.FirstOrDefault(l => l.Code == value);
                    if (level == null)
                    {
                        return Outcome.NotUnderstood;
                    }

                    if (c.Level?.Code != level.Code)
                    {
                        c.Level = level;
                        c.AvailableSubjects = null;
                        c.Subjects.Clear();
                        c.MatchedLessons = null;
                    }

                    break;

                case AssistantSteps.Subjects when values.Count > 0:
                    var chosen = (c.AvailableSubjects ?? new List<AssistantSubject>()).Where(s => values.Contains(s.Id.ToString(CultureInfo.InvariantCulture))).ToList();
                    if (chosen.Count == 0)
                    {
                        return Outcome.NotUnderstood;
                    }

                    c.Subjects = chosen;
                    c.MatchedLessons = null;
                    break;

                case AssistantSteps.Framework when value is OptionYes or OptionNo:
                    c.UsesFramework = value == OptionYes;
                    break;

                case AssistantSteps.Days:
                    var picked = DaysFrom(value, number, days);
                    if (picked.Count == 0)
                    {
                        return Outcome.NotUnderstood;
                    }

                    c.Days = picked;
                    break;

                case AssistantSteps.DailyTime:
                    var minutes = value != null && int.TryParse(value, out var m) ? m : number;
                    if (minutes is not (>= MinSessionMinutes and <= MaxSessionMinutes))
                    {
                        return Outcome.NotUnderstood;
                    }

                    c.DurationMinutes = (ushort)minutes.Value;
                    break;

                case AssistantSteps.Duration:
                    var weeks = value != null && _settings.DurationWeeks.TryGetValue(value, out var w) ? w : number;
                    if (weeks is not (>= 1 and <= MaxWeeks))
                    {
                        return Outcome.NotUnderstood;
                    }

                    c.Weeks = (ushort)weeks.Value;
                    break;

                case AssistantSteps.Confirm when value == OptionConfirm:
                    return new Outcome { Submit = true };

                case AssistantSteps.Confirm when value == OptionChange:
                    c.AwaitingChange = true;
                    return new Outcome();

                default:
                    return Outcome.NotUnderstood;
            }

            Invalidate(c);
            await Task.CompletedTask;
            return new Outcome();
        }

        /// <summary>Applies the changes the model read, each one checked like the step answers. True if any was applied.</summary>
        private bool ApplyChanges(AssistantConversation c, AssistantChanges changes, PlanContext context)
        {
            var applied = false;
            if (changes.PlanType is { } type && context.Types.ContainsKey(type) && c.PlanTypeCode != type)
            {
                c.PlanTypeCode = type;
                c.CustomNeeds = type is SpecificTask or Customized ? c.CustomNeeds : null;
                c.MatchedLessons = null;
                applied = true;
            }

            var available = c.AvailableSubjects ?? new List<AssistantSubject>();
            var add = available.Where(s => changes.SubjectsAdd?.Contains(s.Id.ToString(CultureInfo.InvariantCulture)) == true && c.Subjects.All(x => x.Id != s.Id)).ToList();
            var remove = c.Subjects.Where(s => changes.SubjectsRemove?.Contains(s.Id.ToString(CultureInfo.InvariantCulture)) == true).ToList();
            if (add.Count > 0 || (remove.Count > 0 && remove.Count < c.Subjects.Count + add.Count))
            {
                c.Subjects = c.Subjects.Except(remove).Concat(add).ToList();
                c.MatchedLessons = null;
                applied = true;
            }

            var days = DaysFrom(null, changes.DaysPerWeek, changes.Days);
            if (days.Count > 0)
            {
                c.Days = days;
                applied = true;
            }

            if (changes.Minutes is >= MinSessionMinutes and <= MaxSessionMinutes)
            {
                c.DurationMinutes = (ushort)changes.Minutes.Value;
                applied = true;
            }

            if (changes.Weeks is >= 1 and <= MaxWeeks)
            {
                c.Weeks = (ushort)changes.Weeks.Value;
                applied = true;
            }

            if (TimeOnly.TryParseExact(changes.StartTime, new[] { "HH:mm", "H:mm", "HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
            {
                c.StartTime = time;
                applied = true;
            }

            if (DateOnly.TryParseExact(changes.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) && date >= context.Today)
            {
                c.StartDate = date;
                applied = true;
            }

            if (changes.Framework is { } framework && c.UsesFramework != framework)
            {
                c.UsesFramework = framework;
                applied = true;
            }

            if (applied)
            {
                Invalidate(c);
            }

            return applied;
        }

        /// <summary>The days from an option value, a number of days a week or named ISO days.</summary>
        private static List<byte> DaysFrom(string? value, int? number, List<int>? days)
        {
            var fromOption = value switch
            {
                "weekdays" => CustomPlanBuilder.DefaultDays(5),
                "3" => CustomPlanBuilder.DefaultDays(3),
                "2" => CustomPlanBuilder.DefaultDays(2),
                "weekend" => new byte[] { 6, 7 },
                _ => Array.Empty<byte>(),
            };
            if (fromOption.Count > 0)
            {
                return fromOption.ToList();
            }

            var named = (days ?? new List<int>()).Where(d => d is >= 1 and <= 7).Select(d => (byte)d).Distinct().OrderBy(d => d).ToList();
            if (named.Count > 0)
            {
                return named;
            }

            return number is >= 1 and <= 7 ? CustomPlanBuilder.DefaultDays(number.Value).ToList() : new List<byte>();
        }

        /// <summary>Answers recognized without the model: an option label or value, yes/no, numbers with units, day names.</summary>
        private static AssistantInterpretation? MatchDirectly(string step, IReadOnlyList<CustomPlanAssistantOptionModel> options, string message)
        {
            var text = Normalize(message);
            var tokens = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var exact = options.Where(o => Normalize(o.Label) == text || Normalize(o.Value) == text).Select(o => o.Value).ToList();
            if (exact.Count == 1)
            {
                return new AssistantInterpretation { Values = exact };
            }

            if (step == AssistantSteps.Subjects)
            {
                var named = options.Where(o => Normalize(o.Label).Length > 0 && ContainsPhrase(text, Normalize(o.Label))).Select(o => o.Value).ToList();
                return named.Count > 0 ? new AssistantInterpretation { Values = named } : null;
            }

            if (step is AssistantSteps.Framework or AssistantSteps.Confirm && tokens.Length is > 0 and <= 3)
            {
                if (tokens.Any(YesWords.Contains) && !tokens.Any(NoWords.Contains))
                {
                    return new AssistantInterpretation { Values = new List<string> { step == AssistantSteps.Framework ? OptionYes : OptionConfirm } };
                }

                if (tokens.All(NoWords.Contains))
                {
                    return new AssistantInterpretation { Values = new List<string> { step == AssistantSteps.Framework ? OptionNo : OptionChange } };
                }
            }

            var numberMatch = Regex.Match(text, @"\b(\d{1,3})\b");
            int? number = numberMatch.Success ? int.Parse(numberMatch.Groups[1].Value, CultureInfo.InvariantCulture) : null;
            switch (step)
            {
                case AssistantSteps.Days:
                    var days = tokens.Where(DayNames.ContainsKey).Select(t => (int)DayNames[t]).Distinct().ToList();
                    if (days.Count > 0)
                    {
                        return new AssistantInterpretation { Days = days };
                    }

                    return number != null && tokens.Length <= 3 ? new AssistantInterpretation { Number = number } : null;

                case AssistantSteps.DailyTime when number != null && tokens.Length <= 3:
                    var hours = tokens.Any(t => t.StartsWith("hora", StringComparison.Ordinal) || t.StartsWith("hour", StringComparison.Ordinal) || t == "h");
                    return new AssistantInterpretation { Number = hours ? number * 60 : number };

                case AssistantSteps.Duration when number != null && tokens.Length <= 3:
                    var months = tokens.Any(t => t.StartsWith("mes", StringComparison.Ordinal) || t.StartsWith("month", StringComparison.Ordinal));
                    return new AssistantInterpretation { Number = months ? number * 4 : number };
            }

            return null;
        }

        /// <summary>Asks the agent to read the message; null when it is not available or answered something unreadable.</summary>
        private async Task<AssistantInterpretation?> InterpretAsync(AssistantConversation c, string step, IReadOnlyList<CustomPlanAssistantOptionModel> options, Turn turn, PlanContext context)
        {
            var subjects = step == AssistantSteps.Confirm ? c.AvailableSubjects ?? new List<AssistantSubject>() : new List<AssistantSubject>();
            var (instructions, input) = AssistantInterpreter.AnswerPrompt(step, Question(c, step, turn.Lang), options, subjects, Summary(c), context.Today, turn.Lang, turn.Message!);
            return AssistantInterpreter.ParseAnswer(await _model.CompleteAsync(instructions, input, turn.CancellationToken));
        }

        // ----- Rendering the next step -----

        private async Task<CustomPlanAssistantResponseModel> RenderAsync(AssistantConversation c, Turn turn, PlanContext context, Outcome outcome)
        {
            var response = new CustomPlanAssistantResponseModel { ConversationId = c.Id };
            var lang = turn.Lang;

            if (outcome.Submit)
            {
                if (await SubmitAsync(c, turn, context, response))
                {
                    return response;
                }
            }

            var step = NextStep(c);
            response.Step = step;
            response.Options = await OptionsAsync(c, step, turn, context);
            response.AllowFreeText = true;
            response.MultiSelect = step == AssistantSteps.Subjects;
            var question = Question(c, step, lang);

            // Steps whose data is missing: say why, and offer what can be done.
            if (step == AssistantSteps.Student && c.Children is { Count: 0 })
            {
                question = CustomPlanAssistantTexts.Get("no_children", lang);
                response.AllowFreeText = false;
            }
            else if (step == AssistantSteps.Level && c.Levels is { Count: 0 })
            {
                question = CustomPlanAssistantTexts.Get("no_subscription", lang, c.Student?.FullName);
                response.AllowFreeText = false;
            }
            else if (step == AssistantSteps.Subjects && c.AvailableSubjects is { Count: 0 })
            {
                question = CustomPlanAssistantTexts.Get("no_subjects", lang, c.Student?.FullName);
                c.Level = null;
                response.Step = AssistantSteps.Level;
                response.Options = await OptionsAsync(c, AssistantSteps.Level, turn, context);
                response.MultiSelect = false;
            }
            else if (step == AssistantSteps.Confirm)
            {
                var plan = await BuildPlanAsync(c, turn, context);
                question = plan.Message;
                response.PlanPreview = plan.Preview;
                response.PlanPayload = plan.Payload;
                c.ShownPreviewHash = plan.Payload != null ? Hash(plan.Payload) : null;
                if (plan.Payload == null)
                {
                    response.Options = response.Options.Where(o => o.Value == OptionChange).ToList();
                }
                else if (outcome.ConfirmAgain)
                {
                    question = CustomPlanAssistantTexts.Get("confirm_again", lang) + " " + question;
                }

                if (c.AwaitingChange)
                {
                    question = CustomPlanAssistantTexts.Get("ask_change", lang);
                }
            }

            if (outcome.Errors.Count > 0)
            {
                response.Errors = outcome.Errors;
            }

            var prefix = outcome.NotUnderstoodFlag ? CustomPlanAssistantTexts.Get("not_understood", lang) : outcome.Prefix;
            response.Message = string.IsNullOrEmpty(prefix) ? question : prefix + "\n\n" + question;
            response.Answers = Answers(c, context);
            return response;
        }

        /// <summary>
        /// Creates the plan, only when the preview returned last is still what would be sent (a different plan is shown
        /// again for confirmation). Fills the response and returns true when the conversation ends.
        /// </summary>
        private async Task<bool> SubmitAsync(AssistantConversation c, Turn turn, PlanContext context, CustomPlanAssistantResponseModel response)
        {
            if (NextStep(c) != AssistantSteps.Confirm || c.ShownPreviewHash == null)
            {
                return false;
            }

            var plan = await BuildPlanAsync(c, turn, context);
            if (plan.Payload == null)
            {
                return false;
            }

            if (Hash(plan.Payload) != c.ShownPreviewHash)
            {
                // Shown again by RenderAsync with the new preview.
                c.ShownPreviewHash = null;
                response.Message = CustomPlanAssistantTexts.Get("confirm_again", turn.Lang);
                return false;
            }

            var result = await _plans.CreateAsync(turn.User, plan.Payload, turn.CancellationToken);
            c.ShownPreviewHash = null;
            if (!result.Valid || result.Data == null)
            {
                _logger.LogWarning("The assistant could not create the plan: {Codes}.", string.Join(", ", result.Messages));
                var reason = result.ErrorKind == ServiceErrorKind.Unavailable
                    ? CustomPlanAssistantTexts.Get("create_unavailable", turn.Lang)
                    : CustomPlanAssistantTexts.Get("create_failed", turn.Lang, FriendlyReason(result.Messages, turn.Lang));
                response.Errors = result.Messages.ToList();
                response.Message = reason;
                return FinishFailed(c, turn, context, response, plan, reason);
            }

            c.Step = AssistantSteps.Created;
            c.CreatedPlanId = result.Data.Id;
            response.Step = AssistantSteps.Created;
            response.CreatedPlan = result.Data;
            response.PlanPreview = plan.Preview;
            response.PlanPayload = plan.Payload;
            response.Answers = Answers(c, context);
            var end = result.Data.EndDate ?? CustomPlanBuilder.EndDate(result.Data.StartDate, result.Data.PlanDurationWeeks);
            response.Message = CustomPlanAssistantTexts.Get("created", turn.Lang,
                CustomPlanAssistantTexts.Date(result.Data.StartDate, turn.Lang), CustomPlanAssistantTexts.Date(end, turn.Lang), result.Data.Lessons.Count);
            return true;
        }

        /// <summary>A failed creation keeps the conversation on the confirm step with the preview, so the user can change something.</summary>
        private bool FinishFailed(AssistantConversation c, Turn turn, PlanContext context, CustomPlanAssistantResponseModel response, BuiltPlan plan, string reason)
        {
            response.Step = AssistantSteps.Confirm;
            response.PlanPreview = plan.Preview;
            response.PlanPayload = plan.Payload;
            response.AllowFreeText = true;
            response.Options = ConfirmOptions(turn.Lang);
            response.Answers = Answers(c, context);
            response.Message = reason;
            c.ShownPreviewHash = Hash(plan.Payload!);
            return true;
        }

        private static string FriendlyReason(IEnumerable<string> codes, string lang)
        {
            var list = codes.ToList();
            var key = list.Any(c => c.Contains("only_clients", StringComparison.Ordinal)) ? "error.only_clients"
                : list.Any(c => c.Contains("student", StringComparison.Ordinal)) ? "error.student"
                : list.Any(c => c.Contains("not_in_lesson", StringComparison.Ordinal)) ? "error.activities"
                : list.Any(c => c.Contains("outside", StringComparison.Ordinal) || c.Contains("before", StringComparison.Ordinal) || c.Contains("due", StringComparison.Ordinal)) ? "error.dates"
                : "error.default";
            return CustomPlanAssistantTexts.Get(key, lang);
        }

        /// <summary>The first step without an answer.</summary>
        private static string NextStep(AssistantConversation c)
        {
            if (c.Step == AssistantSteps.Created)
            {
                return AssistantSteps.Created;
            }

            if (c.PlanTypeCode == null)
            {
                return AssistantSteps.PlanType;
            }

            if (c.PlanTypeCode is SpecificTask or Customized && string.IsNullOrWhiteSpace(c.CustomNeeds))
            {
                return AssistantSteps.CustomNeeds;
            }

            if (c.Student == null)
            {
                return AssistantSteps.Student;
            }

            if (c.Level == null)
            {
                return AssistantSteps.Level;
            }

            if (c.Subjects.Count == 0)
            {
                return AssistantSteps.Subjects;
            }

            if (c.UsesFramework == null)
            {
                return AssistantSteps.Framework;
            }

            if (c.Days.Count == 0)
            {
                return AssistantSteps.Days;
            }

            if (c.DurationMinutes == null)
            {
                return AssistantSteps.DailyTime;
            }

            return c.Weeks == null ? AssistantSteps.Duration : AssistantSteps.Confirm;
        }

        /// <summary>The options of a step, loading (once per conversation) the data they come from.</summary>
        private async Task<List<CustomPlanAssistantOptionModel>> OptionsAsync(AssistantConversation c, string step, Turn turn, PlanContext context)
        {
            var lang = turn.Lang;
            switch (step)
            {
                case AssistantSteps.PlanType:
                    return context.Types.Values.OrderBy(t => t.Id)
                        .Select(t => Option(t.Code, Label($"type.{t.Code}", lang, t.Name)))
                        .ToList();

                case AssistantSteps.Student:
                    c.Children ??= (await _users.GetStudentsOfClientAsync(turn.User, turn.CancellationToken))?.ToList();
                    return (c.Children ?? new List<StudentProfile>()).Select(s => Option(s.UserId.ToString(CultureInfo.InvariantCulture), s.FullName)).ToList();

                case AssistantSteps.Level:
                    if (c.Levels == null && c.Student != null)
                    {
                        var subscriptions = await _subscriptions.GetSubscriptionsAsync(turn.User, turn.CancellationToken);
                        if (subscriptions != null)
                        {
                            var levels = await _athenas.GetLevelsAsync(turn.User.LangCode, turn.CancellationToken);
                            c.Levels = AssistantCatalogMapper.LevelsFor(subscriptions, c.Student.UserId, levels);
                        }
                    }

                    var levelOptions = (c.Levels ?? new List<AssistantLevel>()).Select(l => Option(l.Code, l.Name)).ToList();
                    if (c.Levels is { Count: 0 } && c.Children is { Count: > 1 })
                    {
                        levelOptions.Add(Option(OptionChangeStudent, CustomPlanAssistantTexts.Get("option.change_student", lang)));
                    }

                    return levelOptions;

                case AssistantSteps.Subjects:
                    if (c.AvailableSubjects == null && c.Student != null && c.Level != null)
                    {
                        var subscriptions = await _subscriptions.GetSubscriptionsAsync(turn.User, turn.CancellationToken);
                        if (subscriptions != null)
                        {
                            var subjects = await _athenas.GetSubjectsAsync(turn.User.LangCode, turn.CancellationToken);
                            c.AvailableSubjects = AssistantCatalogMapper.SubjectsFor(subscriptions, c.Student.UserId, c.Level.Code, subjects);
                        }
                    }

                    return (c.AvailableSubjects ?? new List<AssistantSubject>()).Select(s => Option(s.Id.ToString(CultureInfo.InvariantCulture), s.Name)).ToList();

                case AssistantSteps.Framework:
                    return new List<CustomPlanAssistantOptionModel>
                    {
                        Option(OptionYes, CustomPlanAssistantTexts.Get("option.yes", lang)),
                        Option(OptionNo, CustomPlanAssistantTexts.Get("option.no", lang)),
                    };

                case AssistantSteps.Days:
                    return DayOptions.Select(v => Option(v, CustomPlanAssistantTexts.Get($"days.{v}", lang))).ToList();

                case AssistantSteps.DailyTime:
                    return TimeOptions.Select(v => Option(v, CustomPlanAssistantTexts.Get($"time.{v}", lang))).ToList();

                case AssistantSteps.Duration:
                    return DurationOptions.Select(v => Option(v, CustomPlanAssistantTexts.Get($"duration.{v}", lang))).ToList();

                case AssistantSteps.Confirm:
                    return ConfirmOptions(lang);

                default:
                    return new List<CustomPlanAssistantOptionModel>();
            }
        }

        private static List<CustomPlanAssistantOptionModel> ConfirmOptions(string lang) => new()
        {
            Option(OptionConfirm, CustomPlanAssistantTexts.Get("option.confirm", lang)),
            Option(OptionChange, CustomPlanAssistantTexts.Get("option.change", lang)),
        };

        private static string Question(AssistantConversation c, string step, string lang) => step switch
        {
            AssistantSteps.PlanType => CustomPlanAssistantTexts.Get("welcome", lang),
            AssistantSteps.CustomNeeds => CustomPlanAssistantTexts.Get(c.PlanTypeCode == SpecificTask ? "ask_skill" : "ask_needs", lang),
            AssistantSteps.Student when c.Children is { Count: 1 } => CustomPlanAssistantTexts.Get("ask_student_one", lang, c.Children[0].FullName),
            AssistantSteps.Student => CustomPlanAssistantTexts.Get("ask_student", lang),
            AssistantSteps.Level => CustomPlanAssistantTexts.Get("ask_level", lang, c.Student?.FullName),
            AssistantSteps.Subjects => CustomPlanAssistantTexts.Get("ask_subjects", lang),
            AssistantSteps.Framework => CustomPlanAssistantTexts.Get("ask_framework", lang),
            AssistantSteps.Days => CustomPlanAssistantTexts.Get("ask_days", lang),
            AssistantSteps.DailyTime => CustomPlanAssistantTexts.Get("ask_daily_time", lang),
            AssistantSteps.Duration => CustomPlanAssistantTexts.Get("ask_duration", lang),
            _ => string.Empty,
        };

        // ----- Building the plan -----

        /// <summary>
        /// Builds the payload and the preview from the answers. Lessons come from the Athenas catalog of each subject and
        /// level (only lessons with a standard when DEPR is chosen); reinforcements put first the lessons with low scores or
        /// overdue work; specific_task / customized put first the lessons matched to the described need.
        /// </summary>
        private async Task<BuiltPlan> BuildPlanAsync(AssistantConversation c, Turn turn, PlanContext context)
        {
            var lang = turn.Lang;
            var ct = turn.CancellationToken;
            var schedule = new PlanBuilderSchedule
            {
                StartDate = c.StartDate ?? CustomPlanBuilder.NextMonday(context.Today),
                Weeks = c.Weeks!.Value,
                Days = c.Days,
                StartTime = c.StartTime ?? _settings.DefaultStartTime,
                DurationMinutes = c.DurationMinutes!.Value,
                Timezone = _settings.DefaultTimezone,
            };
            var notes = new List<string>();

            // 1. Catalog of every subject (Athenas fails => the plan cannot be built now).
            var catalogs = new Dictionary<ulong, IReadOnlyList<AthenasCatalogLesson>>();
            foreach (var subject in c.Subjects)
            {
                var catalog = await _lessons.GetPublishedLessonsAsync(subject.Code, c.Level!.Code, turn.User.LangCode, ct);
                if (catalog == null)
                {
                    return BuiltPlan.Failed(CustomPlanAssistantTexts.Get("create_unavailable", lang));
                }

                catalogs[subject.Id] = catalog;
            }

            // 2. Standards (DEPR): codes of the lessons that are standards in the Athenas catalog.
            IReadOnlySet<ulong>? standards = null;
            if (c.UsesFramework == true)
            {
                var codeIds = catalogs.Values.SelectMany(l => l).SelectMany(l => l.CodeIds).Distinct();
                standards = (await _athenas.GetStandardsAsync(codeIds, turn.User.LangCode, ct)).Keys.ToHashSet();
            }

            // 3. Reinforcement: lessons (and their standards) with low scores or overdue work in the child's plans.
            var weakLessons = new HashSet<ulong>();
            var weakStandards = new HashSet<ulong>();
            if (c.PlanTypeCode == Reinforcements)
            {
                await LoadWeakLessonsAsync(c, turn, weakLessons, weakStandards);
            }

            // 4. Skill / needs: lessons matched among real catalog lessons.
            if (c.PlanTypeCode is SpecificTask or Customized && !string.IsNullOrWhiteSpace(c.CustomNeeds) && c.MatchedLessons == null)
            {
                c.MatchedLessons = await MatchLessonsAsync(c, catalogs, ct);
            }

            // 5. Order each subject, take round robin what fits, read the lesson quizzes and schedule.
            var queues = c.Subjects.Select(subject =>
            {
                var matched = c.MatchedLessons?.GetValueOrDefault(subject.Id);
                var ordered = CustomPlanBuilder.OrderCandidates(catalogs[subject.Id], standards, weakLessons, weakStandards, matched, onlyMatched: c.PlanTypeCode == SpecificTask);
                return (IReadOnlyList<(AssistantSubject Subject, PlanCandidateLesson Lesson)>)ordered.Select(l => (subject, l)).ToList();
            }).ToList();

            var sessions = CustomPlanBuilder.SessionDates(schedule.StartDate, schedule.Weeks, schedule.Days);
            var perLesson = CustomPlanBuilder.SessionsPerLesson(_settings.MinutesPerLesson, schedule.DurationMinutes);
            var capacity = CustomPlanBuilder.Capacity(sessions.Count, perLesson);
            var selected = CustomPlanBuilder.Interleave(queues, capacity);
            if (selected.Count == 0)
            {
                return BuiltPlan.Failed(CustomPlanAssistantTexts.Get("no_lessons", lang));
            }

            var quizzes = await _athenas.TryGetLessonQuizzesAsync(selected.Select(s => s.Lesson.LessonId), ct);
            var lessons = selected.Select(s =>
            {
                var lessonQuizzes = quizzes.GetValueOrDefault(s.Lesson.LessonId) ?? new Dictionary<ulong, AthenasQuizInfo>();
                List<ulong> Of(string kind) => lessonQuizzes.Where(q => q.Value.IsExam == kind).Select(q => q.Key).OrderBy(id => id).ToList();
                return new PlanBuilderLesson(
                    s.Lesson.LessonId, s.Subject.Id, s.Lesson.Title, s.Lesson.StandardId,
                    Of(AthenasQuizInfo.Practice), Of(AthenasQuizInfo.Exam),
                    Of(AthenasQuizInfo.PreTest).Select(id => (ulong?)id).FirstOrDefault(),
                    Of(AthenasQuizInfo.PostTest).Select(id => (ulong?)id).FirstOrDefault());
            }).ToList();

            var built = CustomPlanBuilder.Build(schedule, c.Subjects.Select(s => s.Id).ToList(), lessons, _settings);

            // Notes for the user.
            if (selected.Count < capacity)
            {
                notes.Add(CustomPlanAssistantTexts.Get("note.fewer_lessons", lang, capacity, selected.Count));
            }

            if (capacity == 1 && sessions.Count < perLesson)
            {
                notes.Add(CustomPlanAssistantTexts.Get("note.one_lesson_short", lang, _settings.MinutesPerLesson));
            }

            if (c.MatchedLessons != null && c.MatchedLessons.Values.All(set => set.Count == 0))
            {
                notes.Add(CustomPlanAssistantTexts.Get("note.no_match", lang, c.CustomNeeds));
            }

            var reinforced = selected.Count(s => weakLessons.Contains(s.Lesson.LessonId));
            if (reinforced > 0)
            {
                notes.Add(CustomPlanAssistantTexts.Get("note.reinforced", lang, reinforced));
            }

            var payload = new CreateCustomPlanRequestModel
            {
                CustomPlanTypeId = context.Types[c.PlanTypeCode!].Id,
                StudentId = c.Student!.UserId,
                LevelId = c.Level!.Id,
                CurriculumFrameworkId = c.UsesFramework == true ? context.DeprFrameworkId : null,
                PlanDurationWeeks = schedule.Weeks,
                StartDate = schedule.StartDate,
                EndDate = null,
                Structure = built.Structure,
            };

            var preview = Preview(c, context, schedule, built, lessons, notes, lang);
            var message = CustomPlanAssistantTexts.Get("confirm", lang, built.Structure.Lessons.Count,
                CustomPlanAssistantTexts.Date(schedule.StartDate, lang), CustomPlanAssistantTexts.Date(built.EndDate, lang),
                CustomPlanAssistantTexts.Time(schedule.StartTime, lang));
            return new BuiltPlan(payload, preview, message);
        }

        private async Task LoadWeakLessonsAsync(AssistantConversation c, Turn turn, HashSet<ulong> weakLessons, HashSet<ulong> weakStandards)
        {
            var overview = await _overview.GetOverviewAsync(turn.User, new CustomPlanOverviewFilterModel { StudentId = c.Student!.UserId }, turn.CancellationToken);
            if (!overview.Valid || overview.Data == null)
            {
                return;
            }

            var threshold = _settings.ReinforcementScoreThreshold;
            foreach (var lesson in overview.Data.SelectMany(s => s.Plans).SelectMany(p => p.Subjects).SelectMany(s => s.Lessons))
            {
                var weak = lesson.Score < threshold
                    || lesson.Learning.IsOverdue
                    || lesson.Practices.Overdue > 0
                    || lesson.Exams.Any(e => e.IsOverdue)
                    || lesson.Tests.Any(t => t.IsOverdue);
                if (weak)
                {
                    weakLessons.Add(lesson.LessonId);
                    if (lesson.Standard != null)
                    {
                        weakStandards.Add(lesson.Standard.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Per subject: narrows the catalog by keywords, then lets the agent pick among those real lessons. Without the
        /// agent, the keyword matches are used. The agent's ids are kept only when they are among the candidates.
        /// </summary>
        private async Task<Dictionary<ulong, HashSet<ulong>>> MatchLessonsAsync(AssistantConversation c, Dictionary<ulong, IReadOnlyList<AthenasCatalogLesson>> catalogs, CancellationToken ct)
        {
            var result = new Dictionary<ulong, HashSet<ulong>>();
            foreach (var (subjectId, catalog) in catalogs)
            {
                var byKeywords = CustomPlanBuilder.MatchByKeywords(catalog, c.CustomNeeds, _settings.MaxLessonCandidates);
                var candidates = byKeywords.Count > 0 ? byKeywords : catalog.Take(_settings.MaxLessonCandidates).ToList();
                if (candidates.Count == 0)
                {
                    result[subjectId] = new HashSet<ulong>();
                    continue;
                }

                var (instructions, input) = AssistantInterpreter.LessonMatchPrompt(candidates, c.CustomNeeds!);
                var picked = AssistantInterpreter.ParseLessonIds(await _model.CompleteAsync(instructions, input, ct), candidates.Select(l => l.LessonId));
                result[subjectId] = picked ?? byKeywords.Select(l => l.LessonId).ToHashSet();
            }

            return result;
        }

        private static CustomPlanPreviewModel Preview(AssistantConversation c, PlanContext context, PlanBuilderSchedule schedule, PlanBuildResult built,
            IReadOnlyList<PlanBuilderLesson> lessons, List<string> notes, string lang)
        {
            var titles = lessons.ToDictionary(l => l.LessonId, l => l.Title);
            var subjectNames = c.Subjects.ToDictionary(s => s.Id, s => s.Name);
            var preview = new CustomPlanPreviewModel
            {
                PlanTypeName = Label($"type.{c.PlanTypeCode}", lang, context.Types[c.PlanTypeCode!].Name),
                StudentName = c.Student?.FullName,
                LevelName = c.Level?.Name,
                Subjects = c.Subjects.Select(s => new CustomPlanPreviewSubjectModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    LessonCount = built.Structure.Lessons.Count(l => l.SubjectId == s.Id),
                }).ToList(),
                UsesFramework = c.UsesFramework == true,
                Days = schedule.Days.OrderBy(d => d).Select(d => Option(d.ToString(CultureInfo.InvariantCulture), CustomPlanAssistantTexts.Get($"day.{d}", lang))).ToList(),
                StartTime = schedule.StartTime.ToString("HH:mm", CultureInfo.InvariantCulture),
                DurationMinutes = schedule.DurationMinutes,
                Timezone = schedule.Timezone,
                PlanDurationWeeks = schedule.Weeks,
                StartDate = schedule.StartDate,
                EndDate = built.EndDate,
                TotalSessions = built.Sessions.Count,
                SessionsPerLesson = built.SessionsPerLesson,
                LessonCount = built.Structure.Lessons.Count,
                Notes = notes,
            };

            for (var week = 1; week <= schedule.Weeks; week++)
            {
                var from = schedule.StartDate.AddDays(7 * (week - 1));
                var to = from.AddDays(6);
                var weekModel = new CustomPlanPreviewWeekModel { Week = week, StartDate = from };
                foreach (var lesson in built.Structure.Lessons)
                {
                    var dates = lesson.AssignedDates.Where(d => d >= from && d <= to).ToList();
                    if (dates.Count > 0)
                    {
                        weekModel.Lessons.Add(new CustomPlanPreviewLessonModel
                        {
                            Sequence = lesson.Sequence,
                            LessonId = lesson.LessonId,
                            Title = titles.GetValueOrDefault(lesson.LessonId),
                            SubjectName = subjectNames.GetValueOrDefault(lesson.SubjectId),
                            Dates = dates,
                        });
                    }
                }

                preview.Weeks.Add(weekModel);
            }

            return preview;
        }

        // ----- Helpers -----

        /// <summary>Plan types (by code) and the DEPR framework id, read once per turn.</summary>
        private async Task<PlanContext> LoadContextAsync(Turn turn)
        {
            var types = await _plans.ListTypesAsync(turn.User.LangCode, turn.CancellationToken);
            var depr = await _db.CurriculumFrameworks.AsNoTracking()
                .Where(f => f.Code == CurriculumFramework.DeprCode && f.IsActive)
                .Select(f => (ushort?)f.Id)
                .FirstOrDefaultAsync(turn.CancellationToken);
            var zone = CustomPlanBuilder.Zone(_settings.DefaultTimezone);
            var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(_time.GetUtcNow().UtcDateTime, zone));
            return new PlanContext(
                (types.Data ?? new List<CustomPlanTypeResponseModel>()).ToDictionary(t => t.Code, t => t, StringComparer.Ordinal),
                depr,
                today);
        }

        private static CustomPlanAssistantAnswersModel Answers(AssistantConversation c, PlanContext context) => new()
        {
            PlanTypeCode = c.PlanTypeCode,
            CustomPlanTypeId = c.PlanTypeCode != null && context.Types.TryGetValue(c.PlanTypeCode, out var type) ? type.Id : null,
            CustomNeeds = c.CustomNeeds,
            StudentId = c.Student?.UserId,
            StudentName = c.Student?.FullName,
            LevelCode = c.Level?.Code,
            LevelId = c.Level?.Id,
            LevelName = c.Level?.Name,
            Subjects = c.Subjects.Select(s => new CustomPlanAssistantSubjectModel { Id = s.Id, Code = s.Code, Name = s.Name }).ToList(),
            UsesFramework = c.UsesFramework,
            Days = c.Days.ToList(),
            DurationMinutes = c.DurationMinutes,
            StartTime = (c.StartTime ?? DefaultStartTimeOf(c)).ToString("HH:mm:ss", CultureInfo.InvariantCulture),
            PlanDurationWeeks = c.Weeks,
            StartDate = c.StartDate ?? CustomPlanBuilder.NextMonday(context.Today),
            Timezone = null,
        };

        private static TimeOnly DefaultStartTimeOf(AssistantConversation c) => c.StartTime ?? new TimeOnly(9, 0);

        private static string Summary(AssistantConversation c) =>
            JsonConvert.SerializeObject(new
            {
                planType = c.PlanTypeCode,
                student = c.Student?.FullName,
                level = c.Level?.Name,
                subjects = c.Subjects.Select(s => new { value = s.Id, name = s.Name }),
                framework = c.UsesFramework,
                days = c.Days,
                minutes = c.DurationMinutes,
                weeks = c.Weeks,
                startTime = c.StartTime?.ToString("HH:mm", CultureInfo.InvariantCulture),
                startDate = c.StartDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            });

        /// <summary>Any change drops the confirmation of the preview shown before.</summary>
        private static void Invalidate(AssistantConversation c)
        {
            c.ShownPreviewHash = null;
            c.AwaitingChange = false;
        }

        private static string Hash(CreateCustomPlanRequestModel payload) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload))));

        private static List<string> Selected(CustomPlanAssistantRequestModel request)
        {
            var values = new List<string>();
            if (!string.IsNullOrWhiteSpace(request.SelectedOption))
            {
                values.Add(request.SelectedOption.Trim());
            }

            values.AddRange((request.SelectedOptions ?? new List<string>()).Where(v => !string.IsNullOrWhiteSpace(v)).Select(v => v.Trim()));
            return values.Distinct().Take(50).ToList();
        }

        private static CustomPlanAssistantOptionModel Option(string value, string label) => new() { Value = value, Label = label };

        private static string Label(string key, string lang, string fallback)
        {
            var text = CustomPlanAssistantTexts.Get(key, lang);
            return text == key ? fallback : text;
        }

        /// <summary>Lower case, without accents or punctuation, single spaces.</summary>
        private static string Normalize(string? text)
        {
            var decomposed = (text ?? string.Empty).Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(decomposed.Length);
            foreach (var ch in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) == UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }

                builder.Append(char.IsLetterOrDigit(ch) ? char.ToLowerInvariant(ch) : ' ');
            }

            return string.Join(' ', builder.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        private static bool ContainsPhrase(string text, string phrase) => (" " + text + " ").Contains(" " + phrase + " ", StringComparison.Ordinal);

        /// <summary>One turn's inputs.</summary>
        private sealed record Turn(RequestUser User, string Lang, string? Message, List<string> Selected, CancellationToken CancellationToken)
        {
            public bool HasInput => Selected.Count > 0 || !string.IsNullOrEmpty(Message);
        }

        /// <summary>Data read once per turn.</summary>
        private sealed record PlanContext(Dictionary<string, CustomPlanTypeResponseModel> Types, ushort? DeprFrameworkId, DateOnly Today);

        /// <summary>What applying the input produced.</summary>
        private sealed class Outcome
        {
            public static Outcome NotUnderstood => new() { NotUnderstoodFlag = true };

            public bool NotUnderstoodFlag { get; init; }

            public bool Submit { get; init; }

            public bool ConfirmAgain { get; init; }

            public string? Prefix { get; set; }

            public List<string> Errors { get; } = new();
        }

        /// <summary>A built plan, or the message of why it could not be built.</summary>
        private sealed record BuiltPlan(CreateCustomPlanRequestModel? Payload, CustomPlanPreviewModel? Preview, string Message)
        {
            public static BuiltPlan Failed(string message) => new(null, null, message);
        }
    }
}
