using CsvHelper.Configuration.Attributes;

namespace Galytix.WebApi.Feature.GrossWrittenPremium;

/// <summary>
/// Gross Written Premium CSV model
/// </summary>
public class GwpRecord
{
    [Name("country")] public string Country { get; set; }

    [Name("variableId")] public string VariableId { get; set; }

    [Name("variableName")] public string VariableName { get; set; }

    [Name("lineOfBusiness")] public string LineOfBusiness { get; set; }

    [Name("Y2000")] public string Y2000 { get; set; }

    [Name("Y2001")] public string Y2001 { get; set; }

    [Name("Y2002")] public string Y2002 { get; set; }

    [Name("Y2003")] public string Y2003 { get; set; }

    [Name("Y2004")] public string Y2004 { get; set; }

    [Name("Y2005")] public string Y2005 { get; set; }

    [Name("Y2006")] public string Y2006 { get; set; }

    [Name("Y2007")] public string Y2007 { get; set; }

    [Name("Y2008")] public string Y2008 { get; set; }

    [Name("Y2009")] public string Y2009 { get; set; }

    [Name("Y2010")] public string Y2010 { get; set; }

    [Name("Y2011")] public string Y2011 { get; set; }

    [Name("Y2012")] public string Y2012 { get; set; }

    [Name("Y2013")] public string Y2013 { get; set; }

    [Name("Y2014")] public string Y2014 { get; set; }

    [Name("Y2015")] public string Y2015 { get; set; }
}