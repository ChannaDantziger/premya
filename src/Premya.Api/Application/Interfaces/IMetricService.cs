using Premya.Api.Contracts.Metrics;

namespace Premya.Api.Application.Interfaces;

public interface IMetricService
{
    Task<IReadOnlyList<MetricResponse>> GetAllAsync(int premiumMethodId, CancellationToken cancellationToken);
    Task<MetricResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<(MetricResponse? Result, string? Error)> CreateAsync(CreateMetricRequest request, CancellationToken cancellationToken);
    Task<(MetricResponse? Result, string? Error)> UpdateAsync(int id, UpdateMetricRequest request, CancellationToken cancellationToken);
}
