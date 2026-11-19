using GSBaseAPI.GPTModule.Models;
using Microsoft.Extensions.Caching.Memory;

public sealed class TutorConversationState
{
    public string ConversationId { get; set; } = "";
    public string ResponseLang { get; set; } = "es";
    public string GradeLevel { get; set; } = "";

    // Cache the lesson context you already built (so no re-search)
    public LessonRow Lesson { get; set; } = new LessonRow();

    // Rolling chat history
    public List<ChatTurn> History { get; set; } = new();
}

public sealed class ChatTurn
{
    public string Role { get; init; } = ""; // "user" | "assistant"
    public string Content { get; init; } = "";
    public DateTimeOffset At { get; init; } = DateTimeOffset.UtcNow;
}

public interface ITutorConversationStore
{
    Task<TutorConversationState?> GetAsync(string conversationId);
    Task UpsertAsync(TutorConversationState state, TimeSpan? ttl = null);
    Task ClearAsync(string conversationId);
}

public sealed class MemoryTutorConversationStore : ITutorConversationStore
{
    private readonly IMemoryCache _cache;
    private static string Key(string conversationId) => $"tutor:conv:{conversationId}";

    public MemoryTutorConversationStore(IMemoryCache cache) => _cache = cache;

    public Task<TutorConversationState?> GetAsync(string conversationId)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
            return Task.FromResult<TutorConversationState?>(null);

        _cache.TryGetValue(Key(conversationId), out TutorConversationState? state);
        return Task.FromResult(state);
    }

    public Task UpsertAsync(TutorConversationState state, TimeSpan? ttl = null)
    {
        if (state == null || string.IsNullOrWhiteSpace(state.ConversationId))
            return Task.CompletedTask;

        _cache.Set(Key(state.ConversationId), state, ttl ?? TimeSpan.FromHours(6));
        return Task.CompletedTask;
    }

    public Task ClearAsync(string conversationId)
    {
        if (!string.IsNullOrWhiteSpace(conversationId))
            _cache.Remove(Key(conversationId));
        return Task.CompletedTask;
    }
}