using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.Metrics;
using Premya.Api.Domain.Entities;

namespace Premya.Api.Application.Services;

public class MetricService(IMetricRepository repository) : IMetricService
{
    public async Task<IReadOnlyList<MetricResponse>> GetAllAsync(int premiumMethodId, CancellationToken cancellationToken) =>
        (await repository.GetAllAsync(premiumMethodId, cancellationToken)).Select(Map).ToList();

    public async Task<MetricResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var metric = await repository.GetByIdAsync(id, cancellationToken);
        return metric is null ? null : Map(metric);
    }

    public async Task<(MetricResponse? Result, string? Error)> CreateAsync(
        CreateMetricRequest request,
        CancellationToken cancellationToken)
    {
        if (!await repository.PremiumMethodExistsAsync(request.PremiumMethodId, cancellationToken))
        {
            return (null, "Premium method was not found.");
        }

        if (await repository.ExistsByNameAsync(request.PremiumMethodId, request.Name, null, cancellationToken))
        {
            return (null, "A metric with this name already exists for the selected premium method.");
        }

        var metric = new Metric
        {
            PremiumMethodId = request.PremiumMethodId,
            Name = request.Name,
            Description = request.Description,
            SourceType = request.SourceType,
            SourceName = request.SourceName,
            IngestionFrequency = request.IngestionFrequency
        };

        await repository.AddAsync(metric, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return (Map(metric), null);
    }

    public async Task<(MetricResponse? Result, string? Error)> UpdateAsync(
        int id,
        UpdateMetricRequest request,
        CancellationToken cancellationToken)
    {
        var metric = await repository.GetByIdAsync(id, cancellationToken);
        if (metric is null)
        {
            return (null, null);
        }

        if (await repository.ExistsByNameAsync(metric.PremiumMethodId, request.Name, id, cancellationToken))
        {
            return (null, "A metric with this name already exists for the selected premium method.");
        }

        metric.Name = request.Name;
        metric.Description = request.Description;
        metric.SourceType = request.SourceType;
        metric.SourceName = request.SourceName;
        metric.IngestionFrequency = request.IngestionFrequency;
        repository.Update(metric);
        await repository.SaveChangesAsync(cancellationToken);
        return (Map(metric), null);
    }

    private static MetricResponse Map(Metric metric) => new(
        metric.Id,
        metric.PremiumMethodId,
        metric.Name,
        metric.Description,
        metric.SourceType,
        metric.SourceName,
        metric.IngestionFrequency);
}
