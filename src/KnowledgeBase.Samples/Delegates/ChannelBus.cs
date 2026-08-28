namespace KnowledgeBase.Samples.Delegates;

/// <summary>A message published on a channel.</summary>
public sealed record Message(string Channel, string Content, DateTimeOffset PublishedAt);

/// <summary>
/// Demonstrates the event pattern: a contract for notifying one-or-many,
/// opt-in subscribers without the publisher knowing who (if anyone) listens.
/// Events are multicast delegates with add/remove semantics.
/// </summary>
public sealed class ChannelBus
{
    public event EventHandler<Message>? Published;

    public void Publish(string channel, string content)
    {
        var message = new Message(channel, content, DateTimeOffset.UtcNow);
        Published?.Invoke(this, message);
    }
}