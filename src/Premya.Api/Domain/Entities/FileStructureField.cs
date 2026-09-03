namespace Premya.Api.Domain.Entities;

public class FileStructureField
{
    public int Id { get; set; }
    public int FileStructureVersionId { get; set; }
    public int? MetricFieldId { get; set; }
    public required string FieldName { get; set; }
    public required string DataType { get; set; }
    public bool IsRelevant { get; set; } = true;
    public int DisplayOrder { get; set; }

    public FileStructureVersion? FileStructureVersion { get; set; }
    public MetricField? MetricField { get; set; }
    public ICollection<DynamicValue> DynamicValues { get; set; } = [];
}
