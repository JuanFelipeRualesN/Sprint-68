namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class LessonCurricularMapModel
    {
        private string _id;
        private string _lesson_id;
        private string _unity_id;
        private string _modified_by;
        private string _created_at;
        private string _updated_at;

        public string Id { get { return _id; } set { _id = value; } }
        public string LessonId { get { return _lesson_id; } set { _lesson_id = value; } }
        public string UnityId { get { return _unity_id; } set { _unity_id = value; } }
        public string ModifiedBy { get { return _modified_by; } set { _modified_by = value; } }
        public string CreatedAt { get { return _created_at; } set { _created_at = value; } }
        public string UpdatedAt { get { return _updated_at; } set { _updated_at = value; } }

    }
}