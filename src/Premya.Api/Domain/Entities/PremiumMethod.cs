namespace Premya.Api.Domain.Entities;

public class PremiumMethod
{
    public int Id { get; set; }
    public required string MethodNumber { get; set; }
    public required string Description { get; set; }
    public decimal PremiumRate { get; set; }
    public required string CalculationPeriod { get; set; }

    public ICollection<Metric> Metrics { get; set; } = [];
}
