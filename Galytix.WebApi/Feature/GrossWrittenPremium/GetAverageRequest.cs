namespace Galytix.WebApi.Feature.GrossWrittenPremium;

/// <summary>
/// Get average gross written premium request model
/// </summary>
public class GetAverageRequest
{
    /// <summary>
    /// Country code (2 letters). For example, "GB" for Great Britain.
    /// Case-insensitive.
    /// </summary>
    public required string Country { get; set; }
    
    /// <summary>
    /// Lines of business (LOB) codes. For example, "property" for property lines.
    /// </summary>
    public string[] Lob { get; set; } = [];
}