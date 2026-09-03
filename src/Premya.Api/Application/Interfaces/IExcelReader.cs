using Premya.Api.Application.Imports;

namespace Premya.Api.Application.Interfaces;

public interface IExcelReader
{
    Task<ParsedExcel> ReadAsync(Stream stream, CancellationToken cancellationToken);
}
