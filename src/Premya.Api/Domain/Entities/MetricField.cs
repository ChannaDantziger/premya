namespace Premya.Api.Domain.Entities;

public class MetricField
{
    public int Id { get; set; }
    public int MetricId { get; set; }
    public required string FieldName { get; set; }
    public required string DataType { get; set; }
    public bool IsRelevant { get; set; } = true;
    public int DisplayOrder { get; set; }

    public Metric? Metric { get; set; }
    public ICollection<FileStructureField> FileStructureFields { get; set; } = [];
}
