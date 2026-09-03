namespace Premya.Api.Contracts.Imports;

public record ImportDataResponse(
    IReadOnlyList<string> Fields,
    IReadOnlyList<IReadOnlyDictionary<string, object?>> Records,
    int Page,
    int PageSize,
    int TotalRecords);
