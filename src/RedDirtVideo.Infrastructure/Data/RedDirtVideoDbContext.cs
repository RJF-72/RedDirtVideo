using Microsoft.AspNetCore.Mvc;

namespace RedDirtVideo.API.Controllers;

[ApiController]
[Route("api/v1")]
public class HealthController : ControllerBase
{
    [HttpGet("health")]
    public ActionResult<object> GetHealth()
    {
        return Ok(new { status = "ok", service = "Red Dirt Video API" });
    }
}
