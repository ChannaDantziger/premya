using Microsoft.AspNetCore.Mvc;
using Premya.Api.Application.Interfaces;
using Premya.Api.Contracts.PremiumMethods;

namespace Premya.Api.Controllers;

[ApiController]
[Route("api/premium-methods")]
public class PremiumMethodsController(IPremiumMethodService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PremiumMethodResponse>>> GetAll(
        CancellationToken cancellationToken) =>
        Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PremiumMethodResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PremiumMethodResponse>> Create(
        CreatePremiumMethodRequest request,
        CancellationToken cancellationToken)
    {
        var (result, conflict) = await service.CreateAsync(request, cancellationToken);
        if (conflict is not null)
        {
            return Conflict(new { message = conflict });
        }

        return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PremiumMethodResponse>> Update(
        int id,
        UpdatePremiumMethodRequest request,
        CancellationToken cancellationToken)
    {
        var (result, conflict) = await service.UpdateAsync(id, request, cancellationToken);
        if (conflict is not null)
        {
            return Conflict(new { message = conflict });
        }

        return result is null ? NotFound() : Ok(result);
    }
}
