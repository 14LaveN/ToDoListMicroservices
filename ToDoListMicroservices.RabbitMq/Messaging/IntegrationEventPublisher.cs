using System.Text;
using System.Text.Json;
using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.RabbitMq.Messaging.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ToDoListMicroservices.RabbitMq.Messaging;

/// <summary>
/// Represents the integration event publisher.
/// </summary>
public sealed class IntegrationEventPublisher(IOptions<MessageBrokerSettings> messageBrokerSettingsOptions)
    : IIntegrationEventPublisher
{
    private readonly MessageBrokerSettings _messageBrokerSettings = messageBrokerSettingsOptions.Value;

    /// <summary>
    /// Initialize connection.
    /// </summary>
    /// <returns>Returns connection to RabbitMQ.</returns> 
    private static async Task<IConnection> CreateConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(MessageBrokerSettings.AmqpLink)
        };

        var connection = await connectionFactory.CreateConnectionAsync();

        return connection;
    }

    /// <inheritdoc />
    public async System.Threading.Tasks.Task Publish(IIntegrationEvent integrationEvent)
    {
        using var connection = await CreateConnection();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(_messageBrokerSettings.QueueName, false, false, false);

        await channel.ExchangeDeclareAsync(_messageBrokerSettings.QueueName + "Exchange", ExchangeType.Direct, durable: false);
        
        await channel.QueueBindAsync(_messageBrokerSettings.QueueName,
            exchange: _messageBrokerSettings.QueueName + "Exchange",
            routingKey: _messageBrokerSettings.QueueName);

        string payload = JsonSerializer.Serialize(integrationEvent, typeof(IIntegrationEvent));

        var body = Encoding.UTF8.GetBytes(payload);

        if (_messageBrokerSettings.QueueName is not null)
            await channel.BasicPublishAsync(_messageBrokerSettings.QueueName + "Exchange",
                _messageBrokerSettings.QueueName, body: body);
    }
}