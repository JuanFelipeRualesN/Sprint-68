namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class LessonStatusModel
    {
        private string _id;
        private string _lessonId;
        private string _statusId;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string LessonId { get { return _lessonId; } set { _lessonId = value; } }
        public string StatusId { get { return _statusId; } set { _statusId = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}