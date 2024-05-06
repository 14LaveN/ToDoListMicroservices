using System.Diagnostics;
using System.Globalization;
using ToDoListMicroservices.Cache.Service;
using Microsoft.Extensions.Caching.Distributed;
using Prometheus;

namespace ToDoListMicroservices.Application.Core.Helpers.Metric;

/// <summary>
/// Represents the create metrics helper.
/// </summary>
public sealed class CreateMetricsHelper
{
    private static readonly Counter RequestCounter =
        Metrics.CreateCounter("ToDoListMicroservices_requests_total", "Total number of requests.");
    
    private readonly IDistributedCache _distributedCache;

    /// <summary>
    /// Initialize a new instance of the <see cref="CreateMetricsHelper"/>
    /// </summary>
    /// <param name="distributedCache">The distributed cache.</param>
    public CreateMetricsHelper (IDistributedCache distributedCache) =>
        _distributedCache = distributedCache;

    /// <summary>
    /// Login metrics method.
    /// </summary>
    /// <param name="stopwatch">The <see cref="Stopwatch"/> class.</param>
    public async System.Threading.Tasks.Task CreateMetrics(Stopwatch stopwatch)
    {
        RequestCounter.Inc();

        Metrics.CreateHistogram("ToDoListMicroservices_request_duration_seconds", "Request duration in seconds.")
            .Observe(stopwatch.Elapsed.TotalMilliseconds);

        Dictionary<string, string> counter = new Dictionary<string, string>()
        {
            {"Name", RequestCounter.Name},
            {"Value",RequestCounter.Value.ToString()}
        };
        
        await _distributedCache.SetRecordAsync(
            "metrics_counter-key",
            counter,
            TimeSpan.FromMinutes(6),
            TimeSpan.FromMinutes(6));

        await _distributedCache.SetRecordAsync(
            "metrics_request_duration_seconds-key",
            stopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.CurrentCulture),
            TimeSpan.FromMinutes(6),
            TimeSpan.FromMinutes(6));
    }
}