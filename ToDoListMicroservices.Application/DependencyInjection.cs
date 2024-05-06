using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.Application.Common;
using ToDoListMicroservices.Application.Core.Abstractions.Common;
using ToDoListMicroservices.Application.Core.Abstractions.Helpers.JWT;
using ToDoListMicroservices.Application.Core.Helpers.JWT;
using ToDoListMicroservices.Application.Core.Helpers.Metric;

namespace ToDoListMicroservices.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentException();

        services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();
        services.AddScoped<CreateMetricsHelper>();
        services.AddScoped<IDateTime, MachineDateTime>();
        
        return services;
    }
}