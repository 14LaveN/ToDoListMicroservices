using Microsoft.AspNetCore.Mvc;

namespace ToDoListMicroservices.BackgroundTasks.Services;

/// <summary>
/// Represents the create report producer interface.
/// </summary>
internal interface ICreateReportProducer
{
    /// <summary>
    /// Produce.
    /// </summary>
    System.Threading.Tasks.Task ProduceAsync(CancellationToken stoppingToken);
}