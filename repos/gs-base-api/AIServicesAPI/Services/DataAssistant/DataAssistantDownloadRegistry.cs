using System.Collections.Concurrent;
using GSAIServicesAPI.Models.DataAssistant;

namespace GSAIServicesAPI.Services.DataAssistant
{
    /// <summary>
    /// Registers the downloads (Excel files and charts) of the answers: an opaque id tied to the query and to the person who asked.
    /// Only the query is kept, never data; the file is rebuilt with the token of whoever asks for it. It is an interface so the registry can move
    /// to a store shared by several instances of the monolith.
    /// </summary>
    public interface IDataAssistantDownloadRegistry
    {
        /// <summary>
        /// Registers a download.
        /// </summary>
        /// <param name="queryName">Route name of the query.</param>
        /// <param name="queryString">Query string of the export.</param>
        /// <param name="userId">The user who asked.</param>
        /// <returns>The opaque id of the download.</returns>
        string Register(string queryName, string queryString, string userId);

        /// <summary>
        /// Finds a download. It is null when the id does not exist, expired or belongs to ANOTHER person (same result: the id is not revealed).
        /// </summary>
        /// <param name="id">The id of the download.</param>
        /// <param name="userId">The user asking for it.</param>
        /// <returns>The registration, or null.</returns>
        DownloadRegistration? Find(string id, string userId);
    }

    /// <summary>
    /// In-memory download registry: ids of 128 bits, two hours of life and at most 1000 entries (the oldest go first).
    /// </summary>
    public sealed class DataAssistantDownloadRegistry : IDataAssistantDownloadRegistry
    {
        /// <summary>
        /// Maximum number of registered downloads.
        /// </summary>
        public const int MaxEntries = 1000;

        /// <summary>
        /// Life time of a registration.
        /// </summary>
        public static readonly TimeSpan Lifetime = TimeSpan.FromHours(2);

        private readonly ConcurrentDictionary<string, DownloadRegistration> _entries = new();
        private readonly TimeProvider _timeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAssistantDownloadRegistry"/> class.
        /// </summary>
        /// <param name="timeProvider">The clock.</param>
        public DataAssistantDownloadRegistry(TimeProvider? timeProvider = null)
        {
            _timeProvider = timeProvider ?? TimeProvider.System;
        }

        /// <inheritdoc />
        public string Register(string queryName, string queryString, string userId)
        {
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            foreach (var key in _entries.Where(entry => entry.Value.ExpiresAtUtc <= now).Select(entry => entry.Key).ToList())
            {
                _entries.TryRemove(key, out _);
            }

            while (_entries.Count >= MaxEntries && _entries.OrderBy(entry => entry.Value.ExpiresAtUtc).Select(entry => entry.Key).FirstOrDefault() is { } oldest)
            {
                _entries.TryRemove(oldest, out _);
            }

            var id = "d_" + Guid.NewGuid().ToString("N");
            _entries[id] = new DownloadRegistration(queryName, queryString, userId, now + Lifetime);
            return id;
        }

        /// <inheritdoc />
        public DownloadRegistration? Find(string id, string userId) =>
            _entries.TryGetValue(id, out var entry) && entry.ExpiresAtUtc > _timeProvider.GetUtcNow().UtcDateTime && entry.UserId == userId ? entry : null;
    }
}
