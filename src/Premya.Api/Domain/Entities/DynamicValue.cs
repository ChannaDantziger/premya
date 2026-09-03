namespace Premya.Api.Domain.Entities;

public class DynamicValue
{
    public int Id { get; set; }
    public int DynamicRecordId { get; set; }
    public int FileStructureFieldId { get; set; }
    public string? ValueText { get; set; }
    public decimal? ValueNumber { get; set; }
    public DateTime? ValueDate { get; set; }
    public bool? ValueBoolean { get; set; }

    public DynamicRecord? DynamicRecord { get; set; }
    public FileStructureField? FileStructureField { get; set; }
}
