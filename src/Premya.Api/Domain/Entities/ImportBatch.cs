namespace Premya.Api.Domain.Entities;

public class ImportBatch
{
    public int Id { get; set; }
    public int MetricId { get; set; }
    public int FileStructureVersionId { get; set; }
    public required string FileName { get; set; }
    public int DataYear { get; set; }
    public required string CalculationPeriod { get; set; }
    public DateTime ImportedAt { get; set; }
    public required string Status { get; set; }
    public string? ErrorMessage { get; set; }
    public int RecordCount { get; set; }

    public Metric? Metric { get; set; }
    public FileStructureVersion? FileStructureVersion { get; set; }
    public ICollection<DynamicRecord> DynamicRecords { get; set; } = [];
}
