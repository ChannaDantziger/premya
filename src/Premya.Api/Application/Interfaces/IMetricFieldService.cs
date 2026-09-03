using Premya.Api.Contracts.MetricFields;

namespace Premya.Api.Application.Interfaces;

public interface IMetricFieldService
{
    Task<IReadOnlyList<MetricFieldResponse>> GetAllAsync(int metricId, CancellationToken cancellationToken);
    Task<(MetricFieldResponse? Result, string? Error)> CreateAsync(int metricId, CreateMetricFieldRequest request, CancellationToken cancellationToken);
    Task<(MetricFieldResponse? Result, string? Error)> UpdateAsync(int id, UpdateMetricFieldRequest request, CancellationToken cancellationToken);
}
