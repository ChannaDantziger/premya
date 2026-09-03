using Premya.Api.Domain.Entities;

namespace Premya.Api.Application.Interfaces;

public interface IMetricRepository
{
    Task<IReadOnlyList<Metric>> GetAllAsync(int premiumMethodId, CancellationToken cancellationToken);
    Task<Metric?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> PremiumMethodExistsAsync(int premiumMethodId, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(int premiumMethodId, string name, int? excludedId, CancellationToken cancellationToken);
    Task AddAsync(Metric metric, CancellationToken cancellationToken);
    void Update(Metric metric);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
