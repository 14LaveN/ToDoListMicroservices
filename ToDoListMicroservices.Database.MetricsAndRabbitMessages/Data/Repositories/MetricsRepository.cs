using ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ToDoListMicroservices.Application.Core.Settings;
using ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Interfaces;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Maybe;
using ToDoListMicroservices.Domain.Common.Entities;

namespace ToDoListMicroservices.Database.MetricsAndRabbitMessages.Data.Repositories;

/// <summary>
/// Represents the generic metrics repository class.
/// </summary>
internal sealed class MetricsRepository
    : IMongoRepository<MetricEntity>, IMetricsRepository
{
    private readonly ICommonMongoDbContext _commonMongoDbContext;

    /// <summary>
    /// Login new instance of metrics repository.
    /// </summary>
    /// <param name="commonMongoDbContext">The common mongo db context.</param>
    public MetricsRepository(
        ICommonMongoDbContext commonMongoDbContext)
    {
        _commonMongoDbContext = commonMongoDbContext;
    }

    /// <inheritdoc />
    public async Task<List<MetricEntity>> GetAllAsync() =>
        await _commonMongoDbContext.Metrics.Find(_ => true).ToListAsync();

    /// <inheritdoc />
    public async System.Threading.Tasks.Task InsertAsync(MetricEntity type) =>
        await _commonMongoDbContext.Metrics.InsertOneAsync(type);

    /// <inheritdoc />
    public async Task<Maybe<List<MetricEntity>>> GetByTime(int time, string metricName)
    {
        var metrics = await _commonMongoDbContext.Metrics.FindAsync(x=>x.CreatedAt.Day == DateTime.Today.Day 
            && x.CreatedAt.Month == DateTime.Today.Month
            && x.CreatedAt.Year == DateTime.Today.Year
            && x.Name == metricName).Result.ToListAsync();

        return metrics;
    }

    /// <inheritdoc />
    public async System.Threading.Tasks.Task InsertRangeAsync(IEnumerable<MetricEntity> types) =>
        await _commonMongoDbContext.Metrics.InsertManyAsync(types);

    /// <inheritdoc />
    public async System.Threading.Tasks.Task RemoveAsync(string id) =>
        await _commonMongoDbContext.Metrics.DeleteOneAsync(x => x.Id == id);
}