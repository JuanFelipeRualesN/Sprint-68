namespace GSAIServicesAPI.Services.PromptMonitoring
{
    /// <summary>
    /// Resolves the set of students a viewer is related to, so an alert count can be scoped to them.
    /// The monitoring database stores prompts, not enrollments, so this relationship is owned by the
    /// users API and read from there. Used for roles whose reach is bounded (a teacher's or director's
    /// students); a platform-wide administrator does not need it, since their scope is every student.
    /// </summary>
    public interface IViewerStudentsProvider
    {
        /// <summary>
        /// Returns the ids of the students the holder of <paramref name="viewerToken"/> can see. The
        /// users API scopes the result to that viewer, so the token is what defines which students
        /// come back.
        /// </summary>
        /// <param name="viewerToken">The viewer's session token, forwarded to the users API.</param>
        Task<IReadOnlyList<string>> GetStudentIdsAsync(
            string viewerToken,
            CancellationToken cancellationToken = default);
    }
}
