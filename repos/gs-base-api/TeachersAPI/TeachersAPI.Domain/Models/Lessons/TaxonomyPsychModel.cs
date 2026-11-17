namespace TeachersAPI.Domain.Models.Lessons
{
    public class TaxonomyPsychModel
    {
        private string _id;
        private string _psych_name;
        private string _modified_by;
        private string _created_at;
        private string _updated_at;

        public string Id { get { return _id; } set { _id = value; } }
        public string PsychName { get { return _psych_name; } set { _psych_name = value; } }
        public string ModifiedBy { get { return _modified_by; } set { _modified_by = value; } }
        public string CreatedAt { get { return _created_at; } set { _created_at = value; } }
        public string UpdatedAt { get { return _updated_at; } set { _updated_at = value; } }

    }
}