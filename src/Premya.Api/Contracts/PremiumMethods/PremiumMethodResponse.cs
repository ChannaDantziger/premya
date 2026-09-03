namespace Premya.Api.Contracts.PremiumMethods;

public record PremiumMethodResponse(
    int Id,
    string MethodNumber,
    string Description,
    decimal PremiumRate,
    string CalculationPeriod);
