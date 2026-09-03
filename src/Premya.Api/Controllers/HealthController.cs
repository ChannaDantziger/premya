using Microsoft.AspNetCore.Mvc;

namespace Premya.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> Get() => Ok(new { status = "ok" });
}
