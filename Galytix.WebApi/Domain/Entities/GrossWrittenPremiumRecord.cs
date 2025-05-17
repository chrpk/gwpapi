namespace Galytix.WebApi.Domain.Entities;

public class GrossWrittenPremiumRecord
{
    /// <summary>
    /// Country code
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Variable id
    /// </summary>
    public string VariableId { get; set; }

    /// <summary>
    /// Variable name
    /// </summary>
    public string VariableName { get; set; }

    /// <summary>
    /// Line of business
    /// </summary>
    public string LineOfBusiness { get; set; }

    /// <summary>
    /// Yearly values
    /// </summary>
    public Dictionary<int, decimal> YearlyValues { get; set; } = new();

    /// <summary>
    /// Sum of yearly values
    /// </summary>
    public decimal Sum { get; set; }
}