using Microsoft.AspNetCore.Mvc;

namespace MyTestApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new 
        { 
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            service = "MyTestApp.Api"
        });
    }

    [HttpGet("version")]
    public IActionResult GetVersion()
    {
        return Ok(new 
        { 
            version = "1.0.0",
            buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER") ?? "local"
        });
    }

    /// <summary>
    /// Readiness probe for deployment - lightweight check
    /// </summary>
    [HttpGet("/api/ready")]
    public IActionResult ReadinessCheck()
    {
        return Ok(new { ready = true, timestamp = DateTime.UtcNow });
    }

    /// <summary>
    /// Liveness probe - checks if app is running
    /// </summary>
    [HttpGet("/api/alive")]
    public IActionResult LivenessCheck()
    {
        return Ok(new { alive = true, version = "1.0", timestamp = DateTime.UtcNow });
    }
}
