namespace Premya.Api.Contracts.MetricFields;

public record MetricFieldResponse(
    int Id,
    int MetricId,
    string FieldName,
    string DataType,
    bool IsRelevant,
    int DisplayOrder);
