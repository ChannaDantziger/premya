namespace Premya.Api.Contracts.PremiumMethods;

public record UpdatePremiumMethodRequest(
    string MethodNumber,
    string Description,
    decimal PremiumRate,
    string CalculationPeriod);
