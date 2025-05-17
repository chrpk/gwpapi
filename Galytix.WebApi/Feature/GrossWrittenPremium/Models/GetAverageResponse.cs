namespace Galytix.WebApi.Feature.GrossWrittenPremium;

/// <summary>
/// GetAverageResponse
/// </summary>
public class GetAverageResponse
{
    /// <summary>
    /// Average gross written for transport
    /// </summary>
    public decimal Transport { get; set; }

    /// <summary>
    /// Average gross written for liability
    /// </summary>
    public decimal Liability { get; set; }
}