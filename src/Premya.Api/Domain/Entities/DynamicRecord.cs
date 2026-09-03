namespace Premya.Api.Domain.Entities;

public class DynamicRecord
{
    public int Id { get; set; }
    public int ImportBatchId { get; set; }
    public int RowNumber { get; set; }

    public ImportBatch? ImportBatch { get; set; }
    public ICollection<DynamicValue> DynamicValues { get; set; } = [];
}
