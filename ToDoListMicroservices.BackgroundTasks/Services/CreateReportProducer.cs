using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ToDoListMicroservices.Application.Core.Helpers.CSV;
using ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Interfaces;
using ToDoListMicroservices.Domain.Entities;

namespace ToDoListMicroservices.BackgroundTasks.Services;

/// <summary>
/// Represents the create repor producer class.
/// </summary>
internal sealed class CreateReportProducer(
    IMetricsRepository metricsRepository,
    IMongoRepository<RabbitMessage> rabbitMessagesRepository,
    ILogger<CreateReportProducer> logger)
    : ICreateReportProducer
{
    private readonly FirebaseStorage _firebaseStorage = new("todolistmicroservices.appspot.com");
    
    /// <inheritdoc />
    public async System.Threading.Tasks.Task ProduceAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Request for create the report for metrics and rabbit messages - {DateTime.UtcNow}");

        var metricsRequestDuration =
            await metricsRepository.GetByTime(180, "ToDoListMicroservices_request_duration_seconds");

        if (metricsRequestDuration.Value.IsNullOrEmpty())
        {
            logger.LogWarning("Metrics request duration with the same time - 180 not found");
        }
        
        var metricsRequestsTotal =
            await metricsRepository.GetByTime(180, "ToDoListMicroservices_requests_total");
        
        if (metricsRequestsTotal.Value.IsNullOrEmpty())
        {
            logger.LogWarning("Metrics request total with the same time - 180 not found");
        }

        var rabbitMessages = await rabbitMessagesRepository.GetByTime(180);
        
        if (rabbitMessages.Value.IsNullOrEmpty())
        {
            logger.LogWarning("Rabbit messages with the same time - 180 not found");
        }

        if (!rabbitMessages.Value.IsNullOrEmpty())
        {
            var csvService = new CsvBaseService();
            var uploadFile =await csvService.UploadFile(rabbitMessages.Value);

            var fileName = $"Rabbit statistics for {DateTime.Now.ToLongDateString()}.csv";
            var path =
                $@"G:\DotNetProjects\ToDoListMicroservices\Data\{fileName}";
            
            var file = File.Create(path);

            await file.WriteAsync(uploadFile, stoppingToken);
            
            await _firebaseStorage.Child("uploads").Child(fileName).PutAsync(file);
            
            logger.LogInformation($"Load rabbit messages files in the firebase - {DateTime.UtcNow}");
        }

        if (!metricsRequestsTotal.Value.IsNullOrEmpty()
            || !metricsRequestDuration.Value.IsNullOrEmpty())
        {
            var metrics = metricsRequestsTotal.Value.Union(metricsRequestDuration.Value);
            
            var csvService = new CsvBaseService();
            var uploadFile = await csvService.UploadFile(metrics);

            var fileName = $"Metrics - statistics for {DateTime.Now.ToLongDateString()}.csv";
            var path =
                $@"G:\DotNetProjects\ToDoListMicroservices\Data\{fileName}";
            var file = File.Create(path);

            await file.WriteAsync(uploadFile, stoppingToken);
            await _firebaseStorage.Child("uploads").Child(fileName).PutAsync(file);
            
            logger.LogInformation($"Load metrics files in the firebase - {DateTime.UtcNow}");
        }
    }
}