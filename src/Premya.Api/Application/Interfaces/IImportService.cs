using Microsoft.AspNetCore.Http;
using Premya.Api.Contracts.Imports;

namespace Premya.Api.Application.Interfaces;

public interface IImportService
{
    Task<IReadOnlyList<ImportResponse>> GetHistoryAsync(int metricId, CancellationToken cancellationToken);
    Task<ImportResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ImportResponse?> ImportAsync(
        int metricId,
        int dataYear,
        string calculationPeriod,
        IFormFile file,
        CancellationToken cancellationToken);
}
