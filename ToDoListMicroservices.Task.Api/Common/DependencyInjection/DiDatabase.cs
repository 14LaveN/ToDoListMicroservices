using ToDoListMicroservices.Database.Common;
using ToDoListMicroservices.Database.MetricsAndRabbitMessages;
using ToDoListMicroservices.Identity.Database;
using ToDoListMicroservices.Task.Database;

namespace ToDoListMicroservices.Task.Api.Common.DependencyInjection;

public static class DiDatabase
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMongoDatabase(configuration);
        services.AddBaseDatabase(configuration);
        services.AddUserDatabase();
        services.AddTasksDatabase();
        
        //TODO string pathToFirebaseConfig = @"G:\DotNetProjects\BackingShop\firebase.json";
//TODO 
        //TODO FirebaseApp.Create(new AppOptions
        //TODO {
        //TODO     Credential = GoogleCredential.FromFile(pathToFirebaseConfig),
        //TODO });
        
        return services;
    }
}