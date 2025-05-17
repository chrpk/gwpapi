using System.Globalization;
using Galytix.WebApi.Domain.Entities;
using Galytix.WebApi.Feature.GrossWrittenPremium;

namespace Galytix.WebApi.Feature.DataImport.Mapping;

/// <summary>
/// GwpRecordMapper
/// </summary>
public static class GwpRecordMapper
{
    /// <summary>
    /// Map to GrossWrittenPremiumRecord
    /// </summary>
    /// <param name="gwpRecord"></param>
    /// <returns></returns>
    public static GrossWrittenPremiumRecord Map(this GwpRecord gwpRecord)
    {
        var result = new GrossWrittenPremiumRecord
        {
            Country = gwpRecord.Country,
            VariableId = gwpRecord.VariableId,
            VariableName = gwpRecord.VariableName,
            LineOfBusiness = gwpRecord.LineOfBusiness,
            YearlyValues = new Dictionary<int, decimal>
            {
                {
                    2008,
                    decimal.TryParse(gwpRecord.Y2008, NumberStyles.Number, CultureInfo.InvariantCulture, out var tmp)
                        ? tmp
                        : 0m
                },
                {
                    2009,
                    decimal.TryParse(gwpRecord.Y2009, NumberStyles.Number, CultureInfo.InvariantCulture, out tmp)
                        ? tmp
                        : 0m
                },
                {
                    2010,
                    decimal.TryParse(gwpRecord.Y2010, NumberStyles.Number, CultureInfo.InvariantCulture, out tmp)
                        ? tmp
                        : 0m
                },
                {
                    2011,
                    decimal.TryParse(gwpRecord.Y2011, NumberStyles.Number, CultureInfo.InvariantCulture, out tmp)
                        ? tmp
                        : 0m
                },
                {
                    2012,
                    decimal.TryParse(gwpRecord.Y2012, NumberStyles.Number, CultureInfo.InvariantCulture, out tmp)
                        ? tmp
                        : 0m
                },
                {
                    2013,
                    decimal.TryParse(gwpRecord.Y2013, NumberStyles.Number, CultureInfo.InvariantCulture, out tmp)
                        ? tmp
                        : 0m
                },
                {
                    2014,
                    decimal.TryParse(gwpRecord.Y2014, NumberStyles.Number, CultureInfo.InvariantCulture, out tmp)
                        ? tmp
                        : 0m
                },
                {
                    2015,
                    decimal.TryParse(gwpRecord.Y2015, NumberStyles.Number, CultureInfo.InvariantCulture, out tmp)
                        ? tmp
                        : 0m
                }
            }
        };

        // Note: The sum is calculated here because no need to calculate it in the database.
        // Not best practice to calculate in the mapping function, but for the sake of simplicity, I
        // do it here.
        result.Sum = result.YearlyValues.Values.Sum() / result.YearlyValues.Values.Count;

        return result;
    }
}