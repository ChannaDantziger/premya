using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.Imports;

namespace Premya.Api.Application.Services;

public class ImportService(IExcelReader excelReader, IImportRepository repository) : IImportService
{
    public Task<IReadOnlyList<ImportResponse>> GetHistoryAsync(int metricId, CancellationToken cancellationToken) =>
        repository.GetHistoryAsync(metricId, cancellationToken);

    public Task<ImportResponse?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        repository.GetByIdAsync(id, cancellationToken);

    public async Task<ImportResponse?> ImportAsync(
        int metricId,
        int dataYear,
        string calculationPeriod,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        var parsedExcel = await excelReader.ReadAsync(stream, cancellationToken);
        return await repository.ImportAsync(
            metricId,
            file.FileName,
            dataYear,
            calculationPeriod,
            parsedExcel,
            cancellationToken);
    }
}
