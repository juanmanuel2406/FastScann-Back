using Microsoft.AspNetCore.Mvc;
namespace FastScan.Api.Controllers;
[ApiController, Route("api/health")]
public class HealthController : ControllerBase { [HttpGet] public IActionResult Get() => Ok(new { status = "ok", service = "FastScan API", utc = DateTime.UtcNow }); }
