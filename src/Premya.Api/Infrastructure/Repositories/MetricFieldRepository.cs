using Microsoft.EntityFrameworkCore;
using Premya.Api.Application.Interfaces;
using Premya.Api.Domain.Entities;
using Premya.Api.Infrastructure.Persistence;

namespace Premya.Api.Infrastructure.Repositories;

public class MetricFieldRepository(PremyaDbContext dbContext) : IMetricFieldRepository
{
    public async Task<IReadOnlyList<MetricField>> GetAllAsync(int metricId, CancellationToken cancellationToken) =>
        await dbContext.MetricFields.AsNoTracking()
            .Where(field => field.MetricId == metricId)
            .OrderBy(field => field.DisplayOrder)
            .ThenBy(field => field.FieldName)
            .ToListAsync(cancellationToken);

    public Task<MetricField?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.MetricFields.FirstOrDefaultAsync(field => field.Id == id, cancellationToken);

    public Task<bool> MetricExistsAsync(int metricId, CancellationToken cancellationToken) =>
        dbContext.Metrics.AnyAsync(metric => metric.Id == metricId, cancellationToken);

    public Task<bool> ExistsByNameAsync(int metricId, string fieldName, int? excludedId, CancellationToken cancellationToken) =>
        dbContext.MetricFields.AnyAsync(field => field.MetricId == metricId &&
            field.FieldName == fieldName && (!excludedId.HasValue || field.Id != excludedId.Value), cancellationToken);

    public Task AddAsync(MetricField field, CancellationToken cancellationToken) =>
        dbContext.MetricFields.AddAsync(field, cancellationToken).AsTask();

    public void Update(MetricField field) => dbContext.MetricFields.Update(field);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
