namespace Premya.Api.Contracts.Metrics;

public record MetricResponse(
    int Id,
    int PremiumMethodId,
    string Name,
    string Description,
    string SourceType,
    string SourceName,
    string IngestionFrequency);
