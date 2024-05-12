using System.Text;
using System.Text.Json;
using ToDoListMicroservices.Database.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ToDoListMicroservices.Identity.Application.Core.Settings.User;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Identity.Api.Common.DependencyInjection;

public static class DiAuthorization
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddAuthorizationExtension(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<UserManager<User>>();
        
        services.AddHttpContextAccessor();
        
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<BaseDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = ["https://localhost:7028", configuration["Jwt:ValidIssuers"]],
                    ValidAudiences = new List<string>(){"https://localhost:7028", configuration["Jwt:ValidAudiences"]!},
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                
                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";
                
                        if (context.AuthenticateFailure == null ||
                            context.AuthenticateFailure.GetType() != typeof(SecurityTokenExpiredException))
                            return context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = context.Error,
                                error_description = context.ErrorDescription
                            }));
                        var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                        context.Response.Headers.Append("x-token-expired", authenticationException?.Expires.ToString("o"));
                        context.ErrorDescription =
                            $"The token expired on {authenticationException?.Expires:o}";
                
                        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = context.Error,
                            error_description = context.ErrorDescription
                        }));
                    }
                };
            });
        
        services.AddIdentityServer()
            .AddAspNetIdentity<User>()
            .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
            .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
            .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
            .AddInMemoryClients(IdentityConfiguration.Clients)
            .AddDeveloperSigningCredential();
        
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        
        services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();
        
        services.AddAuthorization();
        
        services.ConfigureApplicationCookie(config =>
        {
            config.Cookie.Name = "ToDoListMicroservices.Identity.Api.Cookie";
            config.LoginPath = "/Auth/Login";
            config.LogoutPath = "/Auth/Logout";
        });
        
        return services;
    }
}