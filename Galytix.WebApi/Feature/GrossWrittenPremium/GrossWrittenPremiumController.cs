
using Microsoft.AspNetCore.Mvc;

namespace Galytix.WebApi.Feature.GrossWrittenPremium;


[ApiController]
[Route("server/api/gwp")]
public class GrossWrittenPremiumController : ControllerBase
{
    private readonly ILogger<GrossWrittenPremiumController> _logger;

    public GrossWrittenPremiumController(ILogger<GrossWrittenPremiumController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get average gross written premium
    /// </summary>
    /// <param name="request">Get average gross written premium request</param>
    /// <returns>Average gross written premium</returns>
    [HttpPost("avg")]
    public IActionResult GetAverage([FromBody] GetAverageRequest request)
    {
        var response = new GetAverageResponse
        {
            Transport = 1000,
            Liability = 2000
        };

        return Ok(response);
    }
}