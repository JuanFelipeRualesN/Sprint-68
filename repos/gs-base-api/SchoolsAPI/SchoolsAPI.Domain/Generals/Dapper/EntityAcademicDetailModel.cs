namespace SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Dapper
{
    public class EntityAcademicDetailModel
    {
        public string Id { get; set; }
        public string EntityId { get; set; }
        public string TermId { get; set; }
        public string PeriodQuantity { get; set; }
        public string DaysToClosePeriod { get; set; }
        public string FullTermStartDate { get; set; }
        public string FullTermEndDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }

}