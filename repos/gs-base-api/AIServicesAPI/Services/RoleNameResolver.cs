namespace GSAIServicesAPI.Services
{
    /// <inheritdoc />
    public class RoleNameResolver : IRoleNameResolver
    {
        /// <summary>Role names keyed by id, read once from <c>AppSettings:role_names</c>.</summary>
        private readonly IReadOnlyDictionary<string, string> _namesById;

        public RoleNameResolver(IConfiguration config)
        {
            _namesById = config.GetSection("AppSettings:role_names")
                .GetChildren()
                .Where(child => !string.IsNullOrWhiteSpace(child.Value))
                .ToDictionary(child => child.Key, child => child.Value!, StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public string? Resolve(string? roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                return roleId;
            }

            return _namesById.TryGetValue(roleId, out var name) ? name : roleId;
        }
    }
}
