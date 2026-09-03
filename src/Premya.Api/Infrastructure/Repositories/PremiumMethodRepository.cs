using Microsoft.EntityFrameworkCore;
using Premya.Api.Application.Interfaces;
using Premya.Api.Domain.Entities;
using Premya.Api.Infrastructure.Persistence;

namespace Premya.Api.Infrastructure.Repositories;

public class PremiumMethodRepository(PremyaDbContext dbContext) : IPremiumMethodRepository
{
    public async Task<IReadOnlyList<PremiumMethod>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.PremiumMethods
            .AsNoTracking()
            .OrderBy(method => method.MethodNumber)
            .ToListAsync(cancellationToken);

    public Task<PremiumMethod?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.PremiumMethods.FirstOrDefaultAsync(method => method.Id == id, cancellationToken);

    public Task<bool> ExistsByMethodNumberAsync(
        string methodNumber,
        int? excludedId,
        CancellationToken cancellationToken) =>
        dbContext.PremiumMethods.AnyAsync(
            method => method.MethodNumber == methodNumber &&
                      (!excludedId.HasValue || method.Id != excludedId.Value),
            cancellationToken);

    public Task AddAsync(PremiumMethod premiumMethod, CancellationToken cancellationToken) =>
        dbContext.PremiumMethods.AddAsync(premiumMethod, cancellationToken).AsTask();

    public void Update(PremiumMethod premiumMethod) => dbContext.PremiumMethods.Update(premiumMethod);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
