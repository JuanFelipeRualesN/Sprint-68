namespace GSWebAPI.GSWebAPI.Services.Common
{
    /// <summary>
    /// Error codes returned in <see cref="ServiceResult{T}.Messages"/>.
    /// TODO: add localized rows to the messages table and translate them with MessageManager, like the existing services.
    /// </summary>
    /// <remarks>
    /// Codes are grouped by module: general codes, custom plans (prefix <c>custom_plan.</c>) and community (prefix <c>community.</c>).
    /// Validation codes may be returned several at a time; the other kinds return a single code.
    /// </remarks>
    public static class ServiceErrorCodes
    {
        /// <summary>The Users API could not be reached or answered with an error while checking that the student belongs to the client on plan creation (Unavailable).</summary>
        public const string UsersApiUnavailable = "users_api_unavailable";

        /// <summary>Reserved for a write that lost an optimistic-concurrency race. Not returned by any service yet.</summary>
        public const string ConcurrentChange = "concurrent_change";

        // Custom plans

        /// <summary>The plan does not exist or the caller cannot access it (not its client; for reads, also not staff) (NotFound).</summary>
        public const string PlanNotFound = "custom_plan.not_found";

        /// <summary>Only users with the client role can create a plan (Forbidden).</summary>
        public const string PlanOnlyClients = "custom_plan.only_clients";

        /// <summary>The client tried to create a plan for themselves as the student (Validation).</summary>
        public const string PlanClientIsStudent = "custom_plan.client_is_student";

        /// <summary>The student id is missing (0) or the Users API says the student is not one of the client's students (Validation).</summary>
        public const string PlanStudentNotOfClient = "custom_plan.student_not_of_client";

        /// <summary>The plan type id does not exist in the plan type catalog (Validation).</summary>
        public const string PlanTypeInvalid = "custom_plan.type_invalid";

        /// <summary>The curriculum framework does not exist or is inactive (Validation).</summary>
        public const string PlanFrameworkInvalid = "custom_plan.framework_invalid";

        /// <summary>The plan duration in weeks is 0 (Validation).</summary>
        public const string PlanDurationInvalid = "custom_plan.duration_invalid";

        /// <summary>The plan end date is before its start date (Validation).</summary>
        public const string PlanEndBeforeStart = "custom_plan.end_before_start";

        /// <summary>The plan level id is missing (0) (Validation).</summary>
        public const string PlanLevelRequired = "custom_plan.level_required";

        /// <summary>The plan structure can only be replaced while the plan is in Draft status (Conflict).</summary>
        public const string PlanNotDraft = "custom_plan.not_draft";

        /// <summary>Progress (lesson or activity status, results) can only be recorded while the plan is Active (Conflict).</summary>
        public const string PlanNotActive = "custom_plan.not_active";

        /// <summary>
        /// The requested status change is not allowed: Draft to Active/Cancelled, Active to Paused/Completed/Cancelled,
        /// Paused to Active/Cancelled; Completed and Cancelled are final (Conflict).
        /// </summary>
        public const string PlanStatusTransitionInvalid = "custom_plan.status_transition_invalid";

        /// <summary>A Draft plan cannot be activated without at least one lesson (Conflict).</summary>
        public const string PlanNeedsLessons = "custom_plan.needs_lessons";

        /// <summary>The same subject id appears more than once in the plan structure (Validation).</summary>
        public const string PlanSubjectDuplicated = "custom_plan.subject_duplicated";

        /// <summary>The subject id to add is 0 (Validation).</summary>
        public const string PlanSubjectInvalid = "custom_plan.subject_invalid";

        /// <summary>The subject to remove is not in the plan (NotFound).</summary>
        public const string PlanSubjectNotFound = "custom_plan.subject_not_found";

        /// <summary>The subject to remove still has lessons in the plan; send removeLessons=true to remove them too (Conflict).</summary>
        public const string PlanSubjectHasLessons = "custom_plan.subject_has_lessons";

        /// <summary>
        /// The subject's lessons cannot be removed because one has progress (status, a completed area or activity, an
        /// activity started, or an evidence attached) (Conflict).
        /// </summary>
        public const string PlanSubjectHasProgress = "custom_plan.subject_has_progress";

        /// <summary>The study time to add is 0 or more than 600 minutes (Validation).</summary>
        public const string PlanStudyMinutesInvalid = "custom_plan.study_minutes_invalid";

        /// <summary>A schedule day of week is outside 1..7 (Validation).</summary>
        public const string PlanDayInvalid = "custom_plan.day_invalid";

        /// <summary>The same day of week appears more than once in the schedule (Validation).</summary>
        public const string PlanDayDuplicated = "custom_plan.day_duplicated";

        /// <summary>Two time slots of the same day have the same start time (Validation).</summary>
        public const string PlanTimeDuplicated = "custom_plan.time_duplicated";

        /// <summary>A schedule time slot has a duration of 0 minutes (Validation).</summary>
        public const string PlanTimeDurationInvalid = "custom_plan.time_duration_invalid";

        /// <summary>A time zone (schedule slot, lesson learning window or activity) is missing, longer than 64 characters or not a time zone id known to the server (Validation).</summary>
        public const string PlanTimezoneInvalid = "custom_plan.timezone_invalid";

        /// <summary>The same lesson id appears more than once in the plan (Validation).</summary>
        public const string PlanLessonDuplicated = "custom_plan.lesson_duplicated";

        /// <summary>A lesson lists the same assigned date more than once (Validation).</summary>
        public const string PlanLessonDateDuplicated = "custom_plan.lesson_date_duplicated";

        /// <summary>A lesson id is missing (0) (Validation).</summary>
        public const string PlanLessonInvalid = "custom_plan.lesson_invalid";

        /// <summary>Two lessons of the plan have the same sequence number (Validation).</summary>
        public const string PlanLessonSequenceDuplicated = "custom_plan.lesson_sequence_duplicated";

        /// <summary>A lesson is missing its learning assigned date or learning due date (Validation).</summary>
        public const string PlanLearningDatesRequired = "custom_plan.learning_dates_required";

        /// <summary>A lesson's learning due date is before its learning assigned date (Validation).</summary>
        public const string PlanLearningDueBeforeAssigned = "custom_plan.learning_due_before_assigned";

        /// <summary>A lesson's learning window starts before the plan start or ends after the plan end (Validation).</summary>
        public const string PlanLearningOutsidePlan = "custom_plan.learning_outside_plan";

        /// <summary>A lesson assigned date falls outside the lesson's learning window (Validation).</summary>
        public const string PlanLessonDateOutsideLearning = "custom_plan.lesson_date_outside_learning";

        /// <summary>An exam id is missing (0) (Validation).</summary>
        public const string PlanExamInvalid = "custom_plan.exam_invalid";

        /// <summary>The same exam is scheduled twice on the same assigned date in a lesson (Validation).</summary>
        public const string PlanExamDuplicated = "custom_plan.exam_duplicated";

        /// <summary>An exam has no due date-time (Validation).</summary>
        public const string PlanExamDueRequired = "custom_plan.exam_due_required";

        /// <summary>An exam's due date, in the exam's time zone, is before its assigned date (Validation).</summary>
        public const string PlanExamDueBeforeAssigned = "custom_plan.exam_due_before_assigned";

        /// <summary>An exam's assigned date or due date (in its time zone) falls outside the plan dates (Validation).</summary>
        public const string PlanExamOutsidePlan = "custom_plan.exam_outside_plan";

        /// <summary>An exam opens before the lesson's learning assigned date (Validation).</summary>
        public const string PlanExamBeforeLearning = "custom_plan.exam_before_learning";

        /// <summary>An exam duration is 0 or longer than 480 minutes (Validation).</summary>
        public const string PlanExamDurationInvalid = "custom_plan.exam_duration_invalid";

        /// <summary>A lesson's subject is not one of the plan's subjects (Validation).</summary>
        public const string PlanLessonSubjectNotInPlan = "custom_plan.lesson_subject_not_in_plan";

        /// <summary>A practice id or sequence is missing (0) (Validation).</summary>
        public const string PlanPracticeInvalid = "custom_plan.practice_invalid";

        /// <summary>The same practice id appears more than once in a lesson (Validation).</summary>
        public const string PlanPracticeDuplicated = "custom_plan.practice_duplicated";

        /// <summary>Two practices of a lesson have the same sequence number (Validation).</summary>
        public const string PlanPracticeSequenceDuplicated = "custom_plan.practice_sequence_duplicated";

        /// <summary>A practice has no due date-time (Validation).</summary>
        public const string PlanPracticeDueRequired = "custom_plan.practice_due_required";

        /// <summary>A practice's due date, in the practice's time zone, is before its assigned date (Validation).</summary>
        public const string PlanPracticeDueBeforeAssigned = "custom_plan.practice_due_before_assigned";

        /// <summary>A practice's assigned date or due date (in its time zone) falls outside the plan dates (Validation).</summary>
        public const string PlanPracticeOutsidePlan = "custom_plan.practice_outside_plan";

        /// <summary>A practice opens before the lesson's learning assigned date (Validation).</summary>
        public const string PlanPracticeBeforeLearning = "custom_plan.practice_before_learning";

        /// <summary>The scheduled practice does not exist in the plan (NotFound).</summary>
        public const string PlanPracticeNotFound = "custom_plan.practice_not_found";

        /// <summary>A practice score is outside 0..100, or the correct answers exceed the total questions (Validation).</summary>
        public const string PlanPracticeScoreInvalid = "custom_plan.practice_score_invalid";

        /// <summary>According to Athenas, the quiz is not a practice of the lesson (Validation).</summary>
        public const string PlanPracticeNotInLesson = "custom_plan.practice_not_in_lesson";

        /// <summary>According to Athenas, the quiz is not an exam of the lesson (Validation).</summary>
        public const string PlanExamNotInLesson = "custom_plan.exam_not_in_lesson";

        /// <summary>A pre/post-test id is missing (0) or its test type is not defined (Validation).</summary>
        public const string PlanTestInvalid = "custom_plan.test_invalid";

        /// <summary>The same test (type and id) appears more than once in a lesson (Validation).</summary>
        public const string PlanTestDuplicated = "custom_plan.test_duplicated";

        /// <summary>A pre/post-test has no due date-time (Validation).</summary>
        public const string PlanTestDueRequired = "custom_plan.test_due_required";

        /// <summary>A pre/post-test due date, in the test's time zone, is before its assigned date (Validation).</summary>
        public const string PlanTestDueBeforeAssigned = "custom_plan.test_due_before_assigned";

        /// <summary>A pre/post-test assigned date or due date (in its time zone) falls outside the plan dates (Validation).</summary>
        public const string PlanTestOutsidePlan = "custom_plan.test_outside_plan";

        /// <summary>A pre/post-test duration is 0 or longer than 480 minutes (Validation).</summary>
        public const string PlanTestDurationInvalid = "custom_plan.test_duration_invalid";

        /// <summary>According to Athenas, the quiz is not a pre-test/post-test (matching the requested type) of the lesson (Validation).</summary>
        public const string PlanTestNotInLesson = "custom_plan.test_not_in_lesson";

        /// <summary>The scheduled pre/post-test does not exist in the plan (NotFound).</summary>
        public const string PlanTestNotFound = "custom_plan.test_not_found";

        /// <summary>The quiz result to link is not an attempt of the plan's student at that pre/post-test (Validation).</summary>
        public const string PlanTestResultInvalid = "custom_plan.test_result_invalid";

        /// <summary>A lesson cannot be completed while a scheduled practice, or a practice the lesson has in Athenas, is not completed in the plan (Conflict).</summary>
        public const string PlanLessonPracticesPending = "custom_plan.lesson_practices_pending";

        /// <summary>A lesson cannot be completed while a scheduled exam, or an exam the lesson has in Athenas, is not completed in the plan (Conflict).</summary>
        public const string PlanLessonExamsPending = "custom_plan.lesson_exams_pending";

        /// <summary>The Athenas content service could not be reached to validate lessons and quizzes (Unavailable).</summary>
        public const string AthenasUnavailable = "athenas_unavailable";

        /// <summary>Activities cannot be added to a Completed or Cancelled plan (Conflict).</summary>
        public const string PlanClosed = "custom_plan.plan_closed";

        /// <summary>A request to add activities to a lesson contains no practices, exams or tests (Validation).</summary>
        public const string PlanActivitiesRequired = "custom_plan.activities_required";

        /// <summary>The lesson's practices-completed flag cannot be set by hand when the lesson has scheduled practices; it is derived from them (Validation).</summary>
        public const string PlanPracticesFlagFollowsPractices = "custom_plan.practices_flag_follows_practices";

        /// <summary>The overview filter's end date is before its start date (Validation).</summary>
        public const string PlanOverviewDatesInvalid = "custom_plan.overview_dates_invalid";

        /// <summary>The overview filter's plan end date (PlanDateTo) is before its plan start date (PlanDateFrom) (Validation).</summary>
        public const string PlanOverviewPlanDatesInvalid = "custom_plan.overview_plan_dates_invalid";

        /// <summary>The overview Month is not yyyy-MM, or it was sent together with DateFrom/DateTo (Validation).</summary>
        public const string PlanOverviewMonthInvalid = "custom_plan.overview_month_invalid";

        /// <summary>An overview score filter is outside 0..100, or the maximum is below the minimum (Validation).</summary>
        public const string PlanOverviewScoresInvalid = "custom_plan.overview_scores_invalid";

        /// <summary>Staff must pass at least one student id to the overview (Validation).</summary>
        public const string PlanOverviewStudentsRequired = "custom_plan.overview_students_required";

        /// <summary>The evidence does not exist in the plan or the caller is not the plan's client (NotFound).</summary>
        public const string EvidenceNotFound = "custom_plan.evidence_not_found";

        /// <summary>The evidence title is empty or longer than 150 characters (Validation).</summary>
        public const string EvidenceTitleRequired = "custom_plan.evidence_title_required";

        /// <summary>The evidence description is longer than 5,000 characters (Validation).</summary>
        public const string EvidenceDescriptionTooLong = "custom_plan.evidence_description_too_long";

        /// <summary>The evidence has neither a file nor an external URL (Validation).</summary>
        public const string EvidenceSourceRequired = "custom_plan.evidence_source_required";

        /// <summary>The evidence external URL is longer than 1,000 characters or is not an absolute http/https URL (Validation).</summary>
        public const string EvidenceUrlInvalid = "custom_plan.evidence_url_invalid";

        /// <summary>The evidence file is empty or larger than 25 MB (Validation).</summary>
        public const string EvidenceFileTooLarge = "custom_plan.evidence_file_too_large";

        /// <summary>The evidence file extension is missing or not in the allowed list (Validation).</summary>
        public const string EvidenceFileTypeNotAllowed = "custom_plan.evidence_file_type_not_allowed";

        /// <summary>The lesson the evidence points to does not belong to the plan (Validation).</summary>
        public const string EvidenceLessonNotInPlan = "custom_plan.evidence_lesson_not_in_plan";

        /// <summary>Evidence cannot be added to a Cancelled plan (Conflict).</summary>
        public const string EvidencePlanClosed = "custom_plan.evidence_plan_closed";

        /// <summary>The evidence file could not be uploaded to blob storage (Unavailable).</summary>
        public const string StorageUnavailable = "custom_plan.storage_unavailable";

        /// <summary>A lesson references a standard but the plan has no valid curriculum framework (Validation).</summary>
        public const string PlanStandardWithoutFramework = "custom_plan.standard_without_framework";

        /// <summary>A referenced standard does not belong to the plan's curriculum framework (Validation).</summary>
        public const string PlanStandardNotInFramework = "custom_plan.standard_not_in_framework";

        /// <summary>The lesson does not exist in the plan (NotFound).</summary>
        public const string PlanLessonNotFound = "custom_plan.lesson_not_found";

        /// <summary>The scheduled exam date does not exist in the plan (NotFound).</summary>
        public const string PlanExamNotFound = "custom_plan.exam_not_found";

        /// <summary>The quiz result to link is not an attempt of the plan's student at that exam (Validation).</summary>
        public const string PlanExamResultInvalid = "custom_plan.exam_result_invalid";

        // Community

        /// <summary>The community group does not exist (NotFound).</summary>
        public const string GroupNotFound = "community.group_not_found";

        /// <summary>The group name is empty or longer than 120 characters, or the location is longer than 255 characters (Validation).</summary>
        public const string GroupNameRequired = "community.group_name_required";

        /// <summary>The caller is not an active member of the group (Forbidden).</summary>
        public const string NotActiveMember = "community.not_active_member";

        /// <summary>The action requires an active owner or moderator of the group (Forbidden).</summary>
        public const string NotModerator = "community.not_owner_or_moderator";

        /// <summary>The action requires an active owner of the group (Forbidden).</summary>
        public const string NotOwner = "community.not_owner";

        /// <summary>The caller is already a member of the group, including a join that raced with another (Conflict).</summary>
        public const string AlreadyMember = "community.already_member";

        /// <summary>The caller already has a pending join request for the group (Conflict).</summary>
        public const string AlreadyPending = "community.already_pending";

        /// <summary>The caller is banned from the group and cannot join or leave it (Forbidden).</summary>
        public const string MemberBanned = "community.member_banned";

        /// <summary>The membership (of the caller or of the target user) does not exist in the group (NotFound).</summary>
        public const string MemberNotFound = "community.member_not_found";

        /// <summary>Only a pending membership can be approved or rejected (Conflict).</summary>
        public const string MemberNotPending = "community.member_not_pending";

        /// <summary>The group's last owner cannot leave or be demoted (Conflict).</summary>
        public const string LastOwner = "community.last_owner";

        /// <summary>An owner, or the caller themselves, cannot be banned (Forbidden).</summary>
        public const string CannotTargetOwner = "community.cannot_target_owner";

        /// <summary>The event does not exist (NotFound).</summary>
        public const string EventNotFound = "community.event_not_found";

        /// <summary>The event title is empty or longer than 150 characters, or the location is longer than 255 characters (Validation).</summary>
        public const string EventTitleRequired = "community.event_title_required";

        /// <summary>The event end date-time is not after its start date-time (Validation).</summary>
        public const string EventEndBeforeStart = "community.event_end_before_start";

        /// <summary>The event time zone is missing or unknown (Validation).</summary>
        public const string EventTimezoneInvalid = "community.event_timezone_invalid";

        /// <summary>The meeting format id does not exist in the catalog (Validation).</summary>
        public const string EventMeetingFormatInvalid = "community.event_meeting_format_invalid";

        /// <summary>
        /// The event capacity is 0 (Validation), or an update lowers it below the people already going (Conflict).
        /// </summary>
        public const string EventCapacityInvalid = "community.event_capacity_invalid";

        /// <summary>The event is not open for registrations; only a "not going" answer is accepted (Conflict).</summary>
        public const string EventNotOpen = "community.event_not_open";

        /// <summary>The registration would exceed the event capacity (Conflict).</summary>
        public const string EventFull = "community.event_full";

        /// <summary>The number of persons attending is less than 1 (Validation).</summary>
        public const string EventQuantityInvalid = "community.event_quantity_invalid";

        /// <summary>Only the event organizer (an active member) or a group owner/moderator can manage the event (Forbidden).</summary>
        public const string EventNotOrganizer = "community.event_not_organizer";

        /// <summary>The caller has no registration for the event (NotFound).</summary>
        public const string RegistrationNotFound = "community.registration_not_found";

        /// <summary>The resource does not exist or is not visible to the caller (NotFound).</summary>
        public const string ResourceNotFound = "community.resource_not_found";

        /// <summary>The resource title is empty or longer than 150 characters (Validation).</summary>
        public const string ResourceTitleRequired = "community.resource_title_required";

        /// <summary>The resource link is empty or longer than 500 characters (Validation).</summary>
        public const string ResourceLinkRequired = "community.resource_link_required";

        /// <summary>The resource link is not an absolute http/https URL (Validation).</summary>
        public const string ResourceLinkInvalid = "community.resource_link_invalid";

        /// <summary>The uploaded resource file is empty or larger than the limit (100 MB) (Validation).</summary>
        public const string ResourceFileTooLarge = "community.resource_file_too_large";

        /// <summary>
        /// The uploaded resource file is not an allowed type: its extension is not in the list, or its content does not match
        /// the extension (for example, a renamed executable) (Validation).
        /// </summary>
        public const string ResourceFileTypeNotAllowed = "community.resource_file_type_not_allowed";

        /// <summary>The resource file could not be uploaded to Blob Storage (Unavailable).</summary>
        public const string ResourceStorageUnavailable = "community.resource_storage_unavailable";

        /// <summary>
        /// Only the author can update the resource; deleting also allows a group owner/moderator, or staff for platform
        /// resources (Forbidden).
        /// </summary>
        public const string ResourceNotAuthor = "community.resource_not_author";

        /// <summary>The forum title is empty or longer than 150 characters (Validation).</summary>
        public const string ForumTitleRequired = "community.forum_title_required";

        /// <summary>The forum description is longer than 10,000 characters (Validation).</summary>
        public const string ForumDescriptionTooLong = "community.forum_description_too_long";

        /// <summary>The forum does not exist (NotFound).</summary>
        public const string ForumNotFound = "community.forum_not_found";

        /// <summary>The forum category id does not exist (Validation).</summary>
        public const string ForumCategoryInvalid = "community.forum_category_invalid";

        /// <summary>The forum category is reserved for group owners/moderators (Forbidden).</summary>
        public const string ForumCategoryModeratorsOnly = "community.forum_category_moderators_only";

        /// <summary>Only the forum author or a group owner/moderator can change or delete the forum (Forbidden).</summary>
        public const string ForumNotAuthor = "community.forum_not_author";

        /// <summary>The forum is locked; only owners/moderators can edit it or post in it (Conflict).</summary>
        public const string ForumLocked = "community.forum_locked";

        /// <summary>The post does not exist (NotFound).</summary>
        public const string PostNotFound = "community.post_not_found";

        /// <summary>The post text is empty or longer than 10,000 characters (Validation).</summary>
        public const string PostTextRequired = "community.post_text_required";

        /// <summary>The parent post does not exist in the same forum (Validation).</summary>
        public const string PostParentInvalid = "community.post_parent_invalid";

        /// <summary>Only the post author can edit it; deleting also allows a group owner/moderator (Forbidden).</summary>
        public const string PostNotAuthor = "community.post_not_author";

        /// <summary>The forum title, description or post text contains blocked words (Validation).</summary>
        public const string ForumContentNotAllowed = "community.forum_content_not_allowed";

        /// <summary>The assistant conversation does not exist, expired or belongs to another user (NotFound).</summary>
        public const string AssistantConversationNotFound = "custom_plan_assistant.conversation_not_found";

        /// <summary>The message to the assistant is longer than the configured limit (Validation).</summary>
        public const string AssistantMessageTooLong = "custom_plan_assistant.message_too_long";

        /// <summary>The conversation is answering another message of the same user (Conflict).</summary>
        public const string AssistantBusy = "custom_plan_assistant.busy";

        /// <summary>The conversation reached the configured maximum number of turns; start a new one (Conflict).</summary>
        public const string AssistantTooManyTurns = "custom_plan_assistant.too_many_turns";

        /// <summary>Adjust mode (changing an existing plan with the assistant) is not available yet (Validation).</summary>
        public const string AssistantAdjustNotAvailable = "custom_plan_assistant.adjust_not_available";
    }
}
