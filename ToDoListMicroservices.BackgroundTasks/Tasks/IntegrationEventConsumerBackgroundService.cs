﻿using System.Text;
using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.BackgroundTasks.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ToDoListMicroservices.BackgroundTasks.Services;
using ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Interfaces;
using ToDoListMicroservices.Domain.Entities;
using ToDoListMicroservices.RabbitMq.Messaging.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ToDoListMicroservices.RabbitMq.Abstractions;

namespace ToDoListMicroservices.BackgroundTasks.Tasks;

internal  sealed class IntegrationEventConsumerBackgroundService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChannel _channel;
    private readonly IConnection _connection;
    private readonly MessageBrokerSettings _messageBrokerSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEventConsumerBackgroundService"/>
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="messageBrokerSettingsOptions">The message broker settings options.</param>
    public IntegrationEventConsumerBackgroundService(
        ILogger<IntegrationEventConsumerBackgroundService> logger,
        IServiceProvider serviceProvider,
        IOptions<MessageBrokerSettings> messageBrokerSettingsOptions)
    {
        _serviceProvider = serviceProvider;

        _messageBrokerSettings = messageBrokerSettingsOptions.Value;
        
        var factory = new ConnectionFactory
        {
            Uri = new Uri(MessageBrokerSettings.AmqpLink)
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();

        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.QueueDeclareAsync(_messageBrokerSettings.QueueName, false, false, false);

        try
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += OnIntegrationEventReceived!;

            _channel.BasicConsumeAsync(_messageBrokerSettings.QueueName, false, consumer);
        }
        catch (Exception e)
        {
            logger.LogCritical($"ERROR: Failed to process the integration events: {e.Message}", e.Message);
        }
    }

    /// <inheritdoc />
    public System.Threading.Tasks.Task StartAsync(CancellationToken cancellationToken) => System.Threading.Tasks.Task.CompletedTask;

    /// <inheritdoc />
    public System.Threading.Tasks.Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();

        return System.Threading.Tasks.Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _channel.CloseAsync();

        _connection.CloseAsync();
    }

    /// <summary>
    /// Processes the integration event received from the message queue.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The event arguments.</param>
    /// <returns>The completed taskEntity.</returns>
    private void OnIntegrationEventReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        string body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        
        var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(body, new JsonSerializerSettings
        {
            Converters = { new IntegrationEventConverter() },
            TypeNameHandling = TypeNameHandling.Auto
        });

        using IServiceScope scope = _serviceProvider.CreateScope();

        var integrationEventConsumer = scope.ServiceProvider.GetRequiredService<IIntegrationEventConsumer>();
        var rabbitRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<RabbitMessage>>();

        integrationEventConsumer.Consume(integrationEvent);
        rabbitRepository.InsertAsync(body);

        _channel.BasicAckAsync(eventArgs.DeliveryTag, false).GetAwaiter().GetResult();
    }
}