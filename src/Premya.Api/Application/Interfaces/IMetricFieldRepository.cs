using Premya.Api.Domain.Entities;

namespace Premya.Api.Application.Interfaces;

public interface IMetricFieldRepository
{
    Task<IReadOnlyList<MetricField>> GetAllAsync(int metricId, CancellationToken cancellationToken);
    Task<MetricField?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> MetricExistsAsync(int metricId, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(int metricId, string fieldName, int? excludedId, CancellationToken cancellationToken);
    Task AddAsync(MetricField field, CancellationToken cancellationToken);
    void Update(MetricField field);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
