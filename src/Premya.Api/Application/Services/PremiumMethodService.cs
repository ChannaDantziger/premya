using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.PremiumMethods;
using Premya.Api.Domain.Entities;

namespace Premya.Api.Application.Services;

public class PremiumMethodService(IPremiumMethodRepository repository) : IPremiumMethodService
{
    public async Task<IReadOnlyList<PremiumMethodResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var methods = await repository.GetAllAsync(cancellationToken);
        return methods.Select(Map).ToList();
    }

    public async Task<PremiumMethodResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var method = await repository.GetByIdAsync(id, cancellationToken);
        return method is null ? null : Map(method);
    }

    public async Task<(PremiumMethodResponse? Result, string? Conflict)> CreateAsync(
        CreatePremiumMethodRequest request,
        CancellationToken cancellationToken)
    {
        if (await repository.ExistsByMethodNumberAsync(request.MethodNumber, null, cancellationToken))
        {
            return (null, "A premium method with this method number already exists.");
        }

        var method = new PremiumMethod
        {
            MethodNumber = request.MethodNumber,
            Description = request.Description,
            PremiumRate = request.PremiumRate,
            CalculationPeriod = request.CalculationPeriod
        };

        await repository.AddAsync(method, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return (Map(method), null);
    }

    public async Task<(PremiumMethodResponse? Result, string? Conflict)> UpdateAsync(
        int id,
        UpdatePremiumMethodRequest request,
        CancellationToken cancellationToken)
    {
        var method = await repository.GetByIdAsync(id, cancellationToken);
        if (method is null)
        {
            return (null, null);
        }

        if (await repository.ExistsByMethodNumberAsync(request.MethodNumber, id, cancellationToken))
        {
            return (null, "A premium method with this method number already exists.");
        }

        method.MethodNumber = request.MethodNumber;
        method.Description = request.Description;
        method.PremiumRate = request.PremiumRate;
        method.CalculationPeriod = request.CalculationPeriod;

        repository.Update(method);
        await repository.SaveChangesAsync(cancellationToken);
        return (Map(method), null);
    }

    private static PremiumMethodResponse Map(PremiumMethod method) => new(
        method.Id,
        method.MethodNumber,
        method.Description,
        method.PremiumRate,
        method.CalculationPeriod);
}
