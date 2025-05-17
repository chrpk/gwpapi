namespace Galytix.WebApi.Shared;

public class CsvDataLoader
{
    public static async Task<List<T>> LoadCsvDataAsync<T>(string filePath)
    {
        var csvData = new List<T>();

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvHelper.CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csvData = await csv.GetRecordsAsync<T>().ToListAsync();
        }

        return csvData;
    }
}