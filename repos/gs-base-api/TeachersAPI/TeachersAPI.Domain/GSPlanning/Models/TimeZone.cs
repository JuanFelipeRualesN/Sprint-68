namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the time zones that the teacher can use to specify the time of an exam.
    /// </summary>
    public class TimeZone
    {
        /// <summary>
        /// The time zone code as TimeZone IDs from several libraries or classes. Example: GMT Standard Time
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The time zone description, which should be friendly to the User UI.
        /// </summary>
        public string Desc { get; set; }
    }
}
