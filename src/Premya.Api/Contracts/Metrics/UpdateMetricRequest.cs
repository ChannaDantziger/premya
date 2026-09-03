namespace Premya.Api.Contracts.Metrics;

public record UpdateMetricRequest(
    string Name,
    string Description,
    string SourceType,
    string SourceName,
    string IngestionFrequency);
