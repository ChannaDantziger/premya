namespace Premya.Api.Domain.Entities;

public class FileStructureVersion
{
    public int Id { get; set; }
    public int MetricId { get; set; }
    public int VersionNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public Metric? Metric { get; set; }
    public ICollection<FileStructureField> Fields { get; set; } = [];
    public ICollection<ImportBatch> ImportBatches { get; set; } = [];
}
