using Premya.Api.Contracts.PremiumMethods;

namespace Premya.Api.Application.Interfaces;

public interface IPremiumMethodService
{
    Task<IReadOnlyList<PremiumMethodResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<PremiumMethodResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<(PremiumMethodResponse? Result, string? Conflict)> CreateAsync(
        CreatePremiumMethodRequest request,
        CancellationToken cancellationToken);
    Task<(PremiumMethodResponse? Result, string? Conflict)> UpdateAsync(
        int id,
        UpdatePremiumMethodRequest request,
        CancellationToken cancellationToken);
}
