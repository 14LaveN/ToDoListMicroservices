using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using ToDoListMicroservices.Database.Common;
using ToDoListMicroservices.Database.MetricsAndRabbitMessages;
using ToDoListMicroservices.Identity.Database;
using ToDoListMicroservices.Notification.Database;

namespace ToDoListMicroservices.Identity.Api.Common.DependencyInjection;

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
        
        services.AddBaseDatabase(configuration);
        services.AddUserDatabase();
        services.AddMongoDatabase(configuration);
        services.AddNotificationsDatabase();
        
        //TODO string pathToFirebaseConfig = @"G:\DotNetProjects\ToDoListMicroservices\firebase.json";
//TODO 
        //TODO FirebaseApp.Create(new AppOptions
        //TODO {
        //TODO     Credential = GoogleCredential.FromFile(pathToFirebaseConfig),
        //TODO });
        
        return services;
    }
}