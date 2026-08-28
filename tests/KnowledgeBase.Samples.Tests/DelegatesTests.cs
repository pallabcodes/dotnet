using KnowledgeBase.Samples.Delegates;

namespace KnowledgeBase.Samples.Tests;

public sealed class DelegatesTests
{
    [Fact]
    public void Subscribers_are_notified_on_publish()
    {
        var bus = new ChannelBus();
        var received = new List<string>();

        void Handler(object? sender, Message message) => received.Add(message.Content);

        bus.Published += Handler;
        bus.Publish("alerts", "disk 90% full");

        Assert.Equal(["disk 90% full"], received);
    }

    [Fact]
    public void Unsubscribed_handlers_are_not_raised()
    {
        var bus = new ChannelBus();
        var hits = 0;

        void Handler(object? sender, Message message) => hits++;

        bus.Published += Handler;
        bus.Publish("a", "one");
        bus.Published -= Handler;
        bus.Publish("a", "two");

        Assert.Equal(1, hits);
    }

    [Fact]
    public void Publishing_with_no_subscribers_does_not_throw()
    {
        var bus = new ChannelBus();
        bus.Publish("silent", "nobody is listening");
    }

    [Fact]
    public void Message_capitalizes_channel_and_content()
    {
        var bus = new ChannelBus();
        Message? captured = null;
        bus.Published += (_, message) => captured = message;

        bus.Publish("orders", "shipped");

        Assert.NotNull(captured);
        Assert.Equal("orders", captured.Channel);
        Assert.Equal("shipped", captured.Content);
    }
}