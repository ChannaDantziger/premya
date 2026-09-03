using Premya.Api.Contracts.Imports;

namespace Premya.Api.Application.Interfaces;

public interface IDynamicDataRepository
{
    Task<ImportDataResponse?> GetAsync(
        int metricId,
        int? importBatchId,
        string? fieldName,
        string? search,
        string? sortBy,
        bool descending,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
