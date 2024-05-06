using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ToDoListMicroservices.Email.Emails;
using ToDoListMicroservices.Email.Emails.Settings;

namespace ToDoListMicroservices.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
            throw new ArgumentException();
    
        //services.Configure<MailSettings>(configuration.GetSection(MailSettings.SettingsKey));
        
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
        
        return services;
    }
}