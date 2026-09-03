using Microsoft.EntityFrameworkCore;
using Premya.Api.Application.Interfaces;
using Premya.Api.Domain.Entities;
using Premya.Api.Infrastructure.Persistence;

namespace Premya.Api.Infrastructure.Repositories;

public class MetricRepository(PremyaDbContext dbContext) : IMetricRepository
{
    public async Task<IReadOnlyList<Metric>> GetAllAsync(int premiumMethodId, CancellationToken cancellationToken) =>
        await dbContext.Metrics.AsNoTracking()
            .Where(metric => metric.PremiumMethodId == premiumMethodId)
            .OrderBy(metric => metric.Name)
            .ToListAsync(cancellationToken);

    public Task<Metric?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Metrics.FirstOrDefaultAsync(metric => metric.Id == id, cancellationToken);

    public Task<bool> PremiumMethodExistsAsync(int premiumMethodId, CancellationToken cancellationToken) =>
        dbContext.PremiumMethods.AnyAsync(method => method.Id == premiumMethodId, cancellationToken);

    public Task<bool> ExistsByNameAsync(int premiumMethodId, string name, int? excludedId, CancellationToken cancellationToken) =>
        dbContext.Metrics.AnyAsync(metric => metric.PremiumMethodId == premiumMethodId &&
            metric.Name == name && (!excludedId.HasValue || metric.Id != excludedId.Value), cancellationToken);

    public Task AddAsync(Metric metric, CancellationToken cancellationToken) =>
        dbContext.Metrics.AddAsync(metric, cancellationToken).AsTask();

    public void Update(Metric metric) => dbContext.Metrics.Update(metric);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
