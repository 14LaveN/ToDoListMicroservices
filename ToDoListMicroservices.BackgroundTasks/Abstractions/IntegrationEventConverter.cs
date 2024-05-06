using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.RabbitMq.Messaging.User.Events.UserCreated;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDoListMicroservices.RabbitMq.Messaging.User.Events.PasswordChanged;

namespace ToDoListMicroservices.BackgroundTasks.Abstractions;

/// <summary>
/// Represents the integration event JSON converter class.
/// </summary>
internal class IntegrationEventConverter : JsonConverter<IIntegrationEvent>
{
    /// <inheritdoc />
    public override IIntegrationEvent ReadJson(JsonReader reader, Type objectType, IIntegrationEvent existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        if (jsonObject.ToString() != "{}")
        {// You can determine the concrete type of the integration event based on a property in the JSON object
            var eventType = jsonObject.Value<string?>();

            // Login an instance of the concrete type based on the eventType
            IIntegrationEvent integrationEvent = eventType switch
            {
                "UserPasswordChangedIntegrationEvent" => jsonObject.ToObject<UserPasswordChangedIntegrationEvent>(),
                "UserCreatedIntegrationEvent" => jsonObject.ToObject<UserCreatedIntegrationEvent>(),
                // Add more cases for other concrete types if needed
                _ => throw new NotSupportedException($"Unsupported integration event type: {eventType}")
            };

            return integrationEvent;
        }

        return default;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, IIntegrationEvent value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}