namespace Premya.Api.Domain.Entities;

public class Metric
{
    public int Id { get; set; }
    public int PremiumMethodId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string SourceType { get; set; }
    public required string SourceName { get; set; }
    public required string IngestionFrequency { get; set; }

    public PremiumMethod? PremiumMethod { get; set; }
    public ICollection<MetricField> MetricFields { get; set; } = [];
    public ICollection<FileStructureVersion> FileStructureVersions { get; set; } = [];
    public ICollection<ImportBatch> ImportBatches { get; set; } = [];
}
