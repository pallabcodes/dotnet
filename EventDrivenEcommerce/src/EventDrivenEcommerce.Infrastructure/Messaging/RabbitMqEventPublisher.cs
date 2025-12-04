using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Application.IntegrationEvents;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EventDrivenEcommerce.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ implementation of IEventPublisher.
/// </summary>
public sealed class RabbitMqEventPublisher : IEventPublisher, IDisposable
{
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqEventPublisher(RabbitMqSettings settings, ILogger<RabbitMqEventPublisher> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password,
            VirtualHost = settings.VirtualHost,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Ensure exchange exists
        _channel.ExchangeDeclare(
            exchange: settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);
    }

    public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var routingKey = GetRoutingKey(integrationEvent);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(integrationEvent));

        var properties = _channel.CreateBasicProperties();
        properties.MessageId = integrationEvent.Id.ToString();
        properties.Timestamp = new AmqpTimestamp((long)(integrationEvent.OccurredOn - DateTime.UnixEpoch).TotalSeconds);
        properties.Type = integrationEvent.GetType().Name;
        properties.Persistent = true;

        await Task.Run(() =>
        {
            _channel.BasicPublish(
                exchange: "ecommerce-events",
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body);
        }, cancellationToken);

        _logger.LogInformation("Published event {EventType} with ID {EventId} to routing key {RoutingKey}",
            integrationEvent.GetType().Name, integrationEvent.Id, routingKey);
    }

    private static string GetRoutingKey(IIntegrationEvent integrationEvent)
    {
        return integrationEvent switch
        {
            OrderPlacedIntegrationEvent => "order.placed",
            OrderConfirmedIntegrationEvent => "order.confirmed",
            _ => "unknown"
        };
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}

/// <summary>
/// Configuration settings for RabbitMQ.
/// </summary>
public sealed class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "ecommerce-events";
}

