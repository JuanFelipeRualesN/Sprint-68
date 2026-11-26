using Microsoft.Extensions.Caching.Memory;

namespace GSAIServicesAPI.Services.DataAssistant
{
    /// <summary>
    /// The state of one conversation: the messages already exchanged with the model (without the system message) and the figures already seen,
    /// so follow-up questions can use them.
    /// </summary>
    /// <param name="UserId">The user who owns the conversation.</param>
    /// <param name="Messages">Messages exchanged so far.</param>
    /// <param name="Figures">Normalized figures of the previous turns.</param>
    public sealed record DataAssistantConversation(string UserId, IReadOnlyList<DataAssistantChatMessage> Messages, IReadOnlySet<string> Figures);

    /// <summary>
    /// Keeps the conversations of the data assistant. It is an interface so the state can move to a store shared by several instances
    /// of the monolith (the in-memory implementation only works with one instance or session affinity).
    /// </summary>
    public interface IDataAssistantConversationStore
    {
        /// <summary>
        /// Gets a conversation. A conversation belongs to the person who started it: for anybody else the result is the same as for an unknown id.
        /// </summary>
        /// <param name="conversationId">Id of the conversation.</param>
        /// <param name="userId">The user asking for it.</param>
        /// <returns>The conversation, or null when it does not exist, expired or is somebody else's.</returns>
        DataAssistantConversation? Get(string conversationId, string userId);

        /// <summary>
        /// Saves a conversation and renews its life time.
        /// </summary>
        /// <param name="conversationId">Id of the conversation.</param>
        /// <param name="conversation">The conversation.</param>
        void Save(string conversationId, DataAssistantConversation conversation);
    }

    /// <summary>
    /// In-memory conversation store. A conversation lives two hours since its last message.
    /// </summary>
    public sealed class DataAssistantConversationStore : IDataAssistantConversationStore
    {
        /// <summary>
        /// Life time of a conversation since its last message.
        /// </summary>
        public static readonly TimeSpan ConversationLifetime = TimeSpan.FromHours(2);

        private readonly IMemoryCache _cache;
        private readonly TimeSpan _lifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAssistantConversationStore"/> class.
        /// </summary>
        /// <param name="cache">The memory cache; its clock decides when a conversation expires.</param>
        /// <param name="lifetime">Life time of a conversation since its last message; two hours by default.</param>
        public DataAssistantConversationStore(IMemoryCache cache, TimeSpan? lifetime = null)
        {
            _cache = cache;
            _lifetime = lifetime ?? ConversationLifetime;
        }

        /// <inheritdoc />
        public DataAssistantConversation? Get(string conversationId, string userId) =>
            _cache.TryGetValue(BuildKey(conversationId), out DataAssistantConversation? conversation) && conversation is not null && conversation.UserId == userId
                ? conversation
                : null;

        /// <inheritdoc />
        public void Save(string conversationId, DataAssistantConversation conversation)
        {
            _cache.Set(BuildKey(conversationId), conversation, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = _lifetime });
        }

        private static string BuildKey(string conversationId) => "data-assistant-conversation:" + conversationId;
    }
}
