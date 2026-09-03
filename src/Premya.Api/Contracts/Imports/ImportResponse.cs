namespace Premya.Api.Contracts.Imports;

public record ImportResponse(
    int Id,
    int MetricId,
    int FileStructureVersionId,
    string FileName,
    string Status,
    int RecordCount,
    string? ErrorMessage);
