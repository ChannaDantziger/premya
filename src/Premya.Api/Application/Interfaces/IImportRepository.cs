using Premya.Api.Application.Imports;
using Premya.Api.Contracts.Imports;

namespace Premya.Api.Application.Interfaces;

public interface IImportRepository
{
    Task<IReadOnlyList<ImportResponse>> GetHistoryAsync(int metricId, CancellationToken cancellationToken);
    Task<ImportResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ImportResponse?> ImportAsync(
        int metricId,
        string fileName,
        int dataYear,
        string calculationPeriod,
        ParsedExcel parsedExcel,
        CancellationToken cancellationToken);
}
