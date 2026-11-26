namespace TeachersAPI.Domain.Models.Events
{
    /// <summary>
    /// Known RoleId values used by the events module.
    /// Same values as the corresponding role constants in the frontend and
    /// the RoleId = "3" already hardcoded for students in GlobalManager.cs/TeacherGroupController.cs —
    /// these are fixed system role ids, not data that needs to be resolved from UsersAPI.
    /// </summary>
    public static class EventAudienceRoles
    {
        public const string Director = "6";
        public const string Teacher = "5";
        public const string Student = "3";

        /// <summary>
        /// Used to grant central administration access to every calendar event.
        /// It is not a selectable event audience role.
        /// </summary>
        public const string Admin = "2";

        /// <summary>
        /// Used to apply regional administration scope. It is not a selectable event audience role.
        /// </summary>
        public const string AdminOre = "8";
    }
}
