using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Galytix.WebApi.Feature.GrossWrittenPremium;

[ApiController]
[Route("server/api/gwp")]
public class GrossWrittenPremiumController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GrossWrittenPremiumController> _logger;

    public GrossWrittenPremiumController(
        IMediator mediator,
        ILogger<GrossWrittenPremiumController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get average gross written premium
    /// </summary>
    /// <param name="request">Get average gross written premium request</param>
    /// <returns>Average gross written premium</returns>
    [HttpPost("avg")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Dictionary<string, double>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Dictionary<string, double>>> GetAverage([FromBody] GetAverageRequest request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }
}