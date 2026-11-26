using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    // [EnumMember] holds the exact MySQL ENUM value. Declaration order is the column's ENUM order.
    // The member whose numeric value is 0 must be the column's DB default (EF does not send CLR defaults
    // for properties that have a DB default, so 0 and the DB default must mean the same thing).

    /// <summary>custom_plan_client.status. Flow: draft → active / cancelled; active → paused / completed / cancelled; paused → active / cancelled.</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomPlanStatus
    {
        /// <summary>Being prepared; the only status in which the structure can be edited. DB default.</summary>
        [EnumMember(Value = "draft")] Draft = 0,

        /// <summary>Running; lesson progress and activity results can change. Needs at least one lesson.</summary>
        [EnumMember(Value = "active")] Active = 1,

        /// <summary>Temporarily stopped; can go back to active or be cancelled.</summary>
        [EnumMember(Value = "paused")] Paused = 2,

        /// <summary>Finished. Final.</summary>
        [EnumMember(Value = "completed")] Completed = 3,

        /// <summary>Cancelled. Final.</summary>
        [EnumMember(Value = "cancelled")] Cancelled = 4,
    }

    /// <summary>Status of a practice, exam or pre/post test (status column of the three activity tables).</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlanActivityStatus
    {
        /// <summary>Not started yet (<c>not_started</c>). DB default.</summary>
        [EnumMember(Value = "not_started")] NotStarted = 0,

        /// <summary>Started by the student (<c>in_progress</c>).</summary>
        [EnumMember(Value = "in_progress")] InProgress = 1,

        /// <summary>Done; completed_at is set.</summary>
        [EnumMember(Value = "completed")] Completed = 2,
    }

    /// <summary>custom_plan_client_lesson_tests.test_type. The numbers match Athenas IsExam (2 pre-test, 3 post-test) minus 2.</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlanTestType
    {
        /// <summary>Pre-test, Athenas IsExam 2 (<c>pre_test</c>).</summary>
        [EnumMember(Value = "pre_test")] PreTest = 0,

        /// <summary>Post-test, Athenas IsExam 3 (<c>post_test</c>).</summary>
        [EnumMember(Value = "post_test")] PostTest = 1,
    }

    /// <summary>custom_plan_evidences.evidence_type</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomPlanEvidenceType
    {
        /// <summary>Evidence of the student's work. DB default.</summary>
        [EnumMember(Value = "evidence")] Evidence = 0,

        /// <summary>A project.</summary>
        [EnumMember(Value = "project")] Project = 1,
    }

    /// <summary>custom_plan_client_lessons.lesson_status</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlanLessonStatus
    {
        /// <summary>Not started yet (<c>not_started</c>). DB default.</summary>
        [EnumMember(Value = "not_started")] NotStarted = 0,

        /// <summary>In progress (<c>in_progress</c>).</summary>
        [EnumMember(Value = "in_progress")] InProgress = 1,

        /// <summary>Completed; completed_at is set. Only accepted when the lesson's practices and exams are completed.</summary>
        [EnumMember(Value = "completed")] Completed = 2,

        /// <summary>Skipped.</summary>
        [EnumMember(Value = "skipped")] Skipped = 3,
    }
}
