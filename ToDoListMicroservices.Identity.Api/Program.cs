using System.Reflection;
using ToDoListMicroservices.Application;
using ToDoListMicroservices.Application.ApiHelpers.Configurations;
using ToDoListMicroservices.Application.ApiHelpers.Middlewares;
using ToDoListMicroservices.BackgroundTasks;
using ToDoListMicroservices.BackgroundTasks.QuartZ.Schedulers;
using ToDoListMicroservices.Email;
using ToDoListMicroservices.Identity.Api.Common.DependencyInjection;
using ToDoListMicroservices.RabbitMq;
using ToDoListMicroservices.RabbitMq.Messaging.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Prometheus;
using Prometheus.Client.AspNetCore;
using Prometheus.Client.HttpRequestDurations;
using ToDoListMicroservices.Application.Core.Settings;
using ToDoListMicroservices.Database.Common;
using ToDoListMicroservices.Identity.Application;
using ToDoListMicroservices.Identity.Domain.Entities;

#region BuilderRegion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Запуск OpenTelemetry
builder.Services.AddMetricsOpenTelemetry(builder.Logging);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Optimal;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.SmallestSize;
});

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection(MessageBrokerSettings.SettingsKey));

builder.Services.AddValidators();

builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddBackgroundTasks(builder.Configuration);

builder.Services.AddRabbitBackgroundTasks();

builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddMediatr();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddSwachbackleService(
    Assembly.GetExecutingAssembly(),
    Assembly.GetExecutingAssembly().GetName().Name!);

builder.Services.AddApplication();

builder.Services.AddCaching();

builder.Services.AddLoggingExtension(builder.Configuration);

builder.Services.AddAuthorizationExtension(builder.Configuration);

builder.Services.AddCors(options => options.AddDefaultPolicy(corsPolicyBuilder =>
    corsPolicyBuilder.WithOrigins("https://localhost:44460", "http://localhost:44460")
        .AllowAnyHeader()
        .AllowAnyMethod()));

#endregion

#region ApplicationRegion

var app = builder.Build();

var scheduler = new UserDbScheduler();
scheduler.Start(builder.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerApp();
    app.ApplyMigrations();
}

app.UseCors();

UseMetrics();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.MapControllers();

UseCustomMiddlewares();


app.Run();

#endregion

#region UseMiddlewaresRegion

void UseCustomMiddlewares()
{
    if (app is null)
        throw new ArgumentException();

    app.UseMiddleware<RequestLoggingMiddleware>(app.Logger);
    app.UseMiddleware<ResponseCachingMiddleware>();
}

void UseMetrics()
{
    if (app is null)
        throw new ArgumentException();
    
    app.UseMetricServer();
    app.UseHttpMetrics();
    app.UsePrometheusServer();
    app.UsePrometheusRequestDurations();
}

#endregion