using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans
{
    /// <summary>Checks that the standards of a plan belong to the plan's curriculum framework.</summary>
    public interface IStandardFrameworkValidator
    {
        /// <summary>Checks whether every given standard belongs to the framework.</summary>
        /// <param name="framework">Curriculum framework of the plan.</param>
        /// <param name="standardIds">Athenas standard ids chosen for the plan.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True when the list is empty or all standards belong to the framework.</returns>
        Task<bool> AreStandardsInFrameworkAsync(CurriculumFramework framework, IReadOnlyCollection<ulong> standardIds, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Standards come from Athenas lesson-standard codes, and Athenas does not expose a standard-to-framework link yet.
    /// Today every standard belongs to the Puerto Rico framework (DEPR), so DEPR is accepted and any other framework
    /// is rejected.
    /// TODO: replace with a real lookup when Athenas exposes which framework a standard belongs to.
    /// </summary>
    public sealed class StandardFrameworkValidator : IStandardFrameworkValidator
    {
        /// <inheritdoc/>
        public Task<bool> AreStandardsInFrameworkAsync(CurriculumFramework framework, IReadOnlyCollection<ulong> standardIds, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(standardIds.Count == 0 || framework.Code == CurriculumFramework.DeprCode);
        }
    }
}
