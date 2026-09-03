using Microsoft.AspNetCore.Mvc;
using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.MetricFields;
using Premya.Api.Contracts.Metrics;

namespace Premya.Api.Controllers;

[ApiController]
[Route("api/metrics")]
public class MetricsController(IMetricService metricService, IMetricFieldService fieldService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MetricResponse>>> GetAll(
        [FromQuery] int premiumMethodId,
        CancellationToken cancellationToken) =>
        Ok(await metricService.GetAllAsync(premiumMethodId, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MetricResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await metricService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MetricResponse>> Create(
        CreateMetricRequest request,
        CancellationToken cancellationToken)
    {
        var (result, error) = await metricService.CreateAsync(request, cancellationToken);
        return error is not null
            ? Conflict(new { message = error })
            : CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MetricResponse>> Update(
        int id,
        UpdateMetricRequest request,
        CancellationToken cancellationToken)
    {
        var (result, error) = await metricService.UpdateAsync(id, request, cancellationToken);
        if (result is null && error is null)
        {
            return NotFound();
        }

        return error is not null ? Conflict(new { message = error }) : Ok(result);
    }

    [HttpGet("{metricId:int}/fields")]
    public async Task<ActionResult<IReadOnlyList<MetricFieldResponse>>> GetFields(
        int metricId,
        CancellationToken cancellationToken) =>
        Ok(await fieldService.GetAllAsync(metricId, cancellationToken));

    [HttpPost("{metricId:int}/fields")]
    public async Task<ActionResult<MetricFieldResponse>> CreateField(
        int metricId,
        CreateMetricFieldRequest request,
        CancellationToken cancellationToken)
    {
        var (result, error) = await fieldService.CreateAsync(metricId, request, cancellationToken);
        return error is not null
            ? Conflict(new { message = error })
            : Created($"/api/metrics/{metricId}/fields/{result!.Id}", result);
    }

    [HttpPut("fields/{id:int}")]
    public async Task<ActionResult<MetricFieldResponse>> UpdateField(
        int id,
        UpdateMetricFieldRequest request,
        CancellationToken cancellationToken)
    {
        var (result, error) = await fieldService.UpdateAsync(id, request, cancellationToken);
        if (result is null && error is null)
        {
            return NotFound();
        }

        return error is not null ? Conflict(new { message = error }) : Ok(result);
    }
}
