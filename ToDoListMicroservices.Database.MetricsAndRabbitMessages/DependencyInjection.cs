using ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Interfaces;
using ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.Application.Core.Settings;
using ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Interfaces;
using ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Repositories;
using ToDoListMicroservices.Domain.Entities;

namespace ToDoListMicroservices.Database.MetricsAndRabbitMessages;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.Configure<MongoSettings>(configuration.GetSection(MongoSettings.MongoSettingsKey));
        
        services.AddOptions<MongoSettings>()
            .BindConfiguration(MongoSettings.MongoSettingsKey)
            .ValidateOnStart();

        services.AddSingleton<IMetricsRepository, MetricsRepository>();
        services.AddSingleton<IMongoRepository<RabbitMessage>, RabbitMessagesRepository>();
        services.AddSingleton<ICommonMongoDbContext, CommonMongoDbContext>();
        
        services.AddTransient<MongoSettings>();
        
        services.AddHealthChecks()
            .AddMongoDb(configuration.GetConnectionString("MongoConnection")!);

        return services;
    }
}