namespace Premya.Api.Contracts.MetricFields;

public record CreateMetricFieldRequest(
    string FieldName,
    string DataType,
    bool IsRelevant,
    int DisplayOrder);
