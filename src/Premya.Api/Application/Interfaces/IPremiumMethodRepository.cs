using Premya.Api.Domain.Entities;

namespace Premya.Api.Application.Interfaces;

public interface IPremiumMethodRepository
{
    Task<IReadOnlyList<PremiumMethod>> GetAllAsync(CancellationToken cancellationToken);
    Task<PremiumMethod?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsByMethodNumberAsync(string methodNumber, int? excludedId, CancellationToken cancellationToken);
    Task AddAsync(PremiumMethod premiumMethod, CancellationToken cancellationToken);
    void Update(PremiumMethod premiumMethod);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
