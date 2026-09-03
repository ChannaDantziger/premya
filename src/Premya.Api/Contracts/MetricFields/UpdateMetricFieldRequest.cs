namespace Premya.Api.Contracts.MetricFields;

public record UpdateMetricFieldRequest(
    string FieldName,
    string DataType,
    bool IsRelevant,
    int DisplayOrder);
