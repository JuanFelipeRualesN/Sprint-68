namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Links a lesson to a position within a curricular map (unit/module).
    /// </summary>
    public class LessonCurricularMapModel
    {
        /// <summary>Unique mapping record identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the mapped lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Identifier of the curricular unit this lesson belongs to.</summary>
        public string UnityId { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// An instructional strategy recommended for delivering this lesson.
    /// </summary>
    public class LessonStrategyModel
    {
        /// <summary>Unique strategy identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Description of the instructional strategy.</summary>
        public string Desc { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Associates a lesson with an "address" (e.g. a learning standard or competency reference).
    /// </summary>
    public class AddressToLessonModel
    {
        /// <summary>Unique association record identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the associated lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Foreign key referencing the address/standard lookup table.</summary>
        public string AddressId { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Maps a lesson to a display/rendering template.
    /// </summary>
    public class LessonTemplateModel
    {
        /// <summary>Unique lesson-template mapping identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the associated lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Foreign key referencing the template definition.</summary>
        public string TemplateId { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Defines a visual/rendering template including character animations, backgrounds, and audio assets.
    /// </summary>
    public class TemplateModel
    {
        /// <summary>Unique template identifier.</summary>
        public string Id { get; set; }

        /// <summary>Display name of the template.</summary>
        public string Name { get; set; }

        /// <summary>Description of the template and its intended use.</summary>
        public string Desc { get; set; }

        /// <summary>Template category type (e.g. "quiz", "lesson").</summary>
        public string Type { get; set; }

        /// <summary>URL or path to the template's preview image.</summary>
        public string ImgTemplate { get; set; }

        /// <summary>Foreign key referencing the template's code definition.</summary>
        public string TemplateCodeId { get; set; }

        // ── Character animation ──────────────────────────────────────────────

        /// <summary>Sprite sheet image for the character animation.</summary>
        public string CharacterAnimationImg { get; set; }

        /// <summary>Frame data (count or JSON) for the character animation.</summary>
        public string CharacterAnimationFrames { get; set; }

        /// <summary>MP3 audio file for the character animation.</summary>
        public string CharacterAnimationAudio { get; set; }

        /// <summary>OGG audio file for the character animation (fallback).</summary>
        public string CharacterAnimationAudioOgg { get; set; }

        /// <summary>Indicates whether the character animation should loop indefinitely.</summary>
        public string CharacterInfiniteLoop { get; set; }

        // ── Touch object ─────────────────────────────────────────────────────

        /// <summary>Sprite sheet image for the touchable object.</summary>
        public string ObjectTouchImg { get; set; }

        /// <summary>Frame data for the touch object animation.</summary>
        public string ObjectTouchImgFrames { get; set; }

        /// <summary>MP3 audio played when the object is touched.</summary>
        public string ObjectTouchAudio { get; set; }

        /// <summary>OGG audio played when the object is touched (fallback).</summary>
        public string ObjectTouchAudioOgg { get; set; }

        /// <summary>Indicates whether the touch object animation loops indefinitely.</summary>
        public string ObjectTouchInfiniteLoop { get; set; }

        // ── Drop object ──────────────────────────────────────────────────────

        /// <summary>Sprite sheet image for the droppable object.</summary>
        public string ObjectDropImg { get; set; }

        /// <summary>Frame data for the drop object animation.</summary>
        public string ObjectDropImgFrames { get; set; }

        /// <summary>MP3 audio played when the object is dropped.</summary>
        public string ObjectDropAudio { get; set; }

        /// <summary>OGG audio played when the object is dropped (fallback).</summary>
        public string ObjectDropAudioOgg { get; set; }

        /// <summary>Indicates whether the drop object animation loops indefinitely.</summary>
        public string ObjectDropInfiniteLoop { get; set; }

        // ── Background ───────────────────────────────────────────────────────

        /// <summary>Background image asset.</summary>
        public string BackgroundImg { get; set; }

        /// <summary>Indicates whether the background image scrolls infinitely.</summary>
        public string BackgroundImgInfiniteScroll { get; set; }

        /// <summary>Frame data for an animated background.</summary>
        public string BackgroundFrames { get; set; }

        /// <summary>MP3 background music track.</summary>
        public string BackgroundMusic { get; set; }

        /// <summary>OGG background music track (fallback).</summary>
        public string BackgroundMusicOgg { get; set; }

        // ────────────────────────────────────────────────────────────────────

        /// <summary>Freeform annotation or note about the template.</summary>
        public string Annotation { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Represents a workflow/publishing status in the lesson lifecycle.
    /// </summary>
    public class StatusModel
    {
        /// <summary>Unique status identifier.</summary>
        public string Id { get; set; }

        /// <summary>Display name of the status (e.g. "Draft", "Published").</summary>
        public string Name { get; set; }

        /// <summary>Human-readable description of what this status means.</summary>
        public string Desc { get; set; }

        /// <summary>Indicates whether content in this status is publicly accessible.</summary>
        public string PublicAvailable { get; set; }

        /// <summary>Indicates whether this is a terminal/final status in the workflow.</summary>
        public string IsFinal { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
