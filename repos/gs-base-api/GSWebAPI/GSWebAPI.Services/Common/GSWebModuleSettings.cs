namespace GSWebAPI.GSWebAPI.Services.Common
{
    /// <summary>Settings for the Custom Plans and Community modules, read from AppSettings.</summary>
    public sealed class GSWebModuleSettings
    {
        /// <summary>AppSettings:ur_role_client. The paying account holder.</summary>
        public string ClientRoleId { get; init; } = "1";

        /// <summary>AppSettings:ur_role_client_child. The student profile of a client.</summary>
        public string StudentRoleId { get; init; } = "3";
    }
}
