using Microsoft.AspNetCore.Mvc;
using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.Imports;

namespace Premya.Api.Controllers;

[ApiController]
[Route("api/imports")]
public class ImportsController(IImportService importService, IDynamicDataRepository dataRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ImportResponse>>> GetHistory(
        [FromQuery] int metricId,
        CancellationToken cancellationToken) =>
        Ok(await importService.GetHistoryAsync(metricId, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ImportResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await importService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [RequestSizeLimit(20_000_000)]
    public async Task<ActionResult<ImportResponse>> Import(
        [FromForm] int metricId,
        [FromForm] int dataYear,
        [FromForm] string calculationPeriod,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { message = "An Excel file is required." });
        }

        try
        {
            var result = await importService.ImportAsync(metricId, dataYear, calculationPeriod, file, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidDataException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpGet("/api/metrics/{metricId:int}/data")]
    public async Task<ActionResult<ImportDataResponse>> GetData(
        int metricId,
        [FromQuery] int? importBatchId,
        [FromQuery] string? fieldName,
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] bool descending = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var result = await dataRepository.GetAsync(metricId, importBatchId, fieldName, search, sortBy, descending, page, pageSize, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
