namespace Premya.Api.Contracts.Metrics;

public record CreateMetricRequest(
    int PremiumMethodId,
    string Name,
    string Description,
    string SourceType,
    string SourceName,
    string IngestionFrequency);
