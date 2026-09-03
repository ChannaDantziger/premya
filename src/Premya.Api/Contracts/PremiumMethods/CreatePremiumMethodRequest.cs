namespace Premya.Api.Contracts.PremiumMethods;

public record CreatePremiumMethodRequest(
    string MethodNumber,
    string Description,
    decimal PremiumRate,
    string CalculationPeriod);
