namespace TeachersAPI.Domain.Models.Lessons
{
    public class TaxonomyLevelModel
    {
        private string _id;
        private string _tax_level_number;
        private string _tax_level_name;
        private string _tax_psych_to_tax_psych_id;
        private string _modified_by;
        private string _created_at;
        private string _updated_at;

        public string Id { get { return _id; } set { _id = value; } }
        public string TaxLevelNumber { get { return _tax_level_number; } set { _tax_level_number = value; } }
        public string TaxLevelName { get { return _tax_level_name; } set { _tax_level_name = value; } }
        public string TaxPsychToTaxPsychId { get { return _tax_psych_to_tax_psych_id; } set { _tax_psych_to_tax_psych_id = value; } }
        public string ModifiedBy { get { return _modified_by; } set { _modified_by = value; } }
        public string CreatedAt { get { return _created_at; } set { _created_at = value; } }
        public string UpdatedAt { get { return _updated_at; } set { _updated_at = value; } }

    }
}