namespace GSAIServicesAPI.Services
{
    /// <summary>
    /// Turns the numeric role id the auth service returns (e.g. <c>"3"</c>) into its human-readable
    /// name (e.g. <c>"student"</c>), so the audit record stores a name and the classification gate can
    /// compare against a name instead of a magic number. Backed by a config map, so the catalog lives
    /// in <c>appsettings.json</c> and never in code.
    /// </summary>
    public interface IRoleNameResolver
    {
        /// <summary>
        /// Returns the role name mapped to <paramref name="roleId"/>. An id with no mapping is returned
        /// unchanged, so an unknown role degrades to its raw id rather than being lost.
        /// </summary>
        string? Resolve(string? roleId);
    }
}
