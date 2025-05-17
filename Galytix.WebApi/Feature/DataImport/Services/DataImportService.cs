using System.Globalization;
using System.IO.Abstractions;
using CsvHelper;
using CsvHelper.Configuration;
using Galytix.WebApi.Domain.Entities;
using Galytix.WebApi.Feature.DataImport.Abstractions;
using Galytix.WebApi.Feature.DataImport.Mapping;
using Galytix.WebApi.Feature.GrossWrittenPremium;
using LiteDB.Async;

namespace Galytix.WebApi.Feature.DataImport.Services;

/// <summary>
/// DataImportService
/// </summary>
public class DataImportService : IDataImportService
{
    private readonly ILiteDatabaseAsync _db;
    private readonly IFileSystem _fs;
    private readonly ILogger<DataImportService> _logger;

    private static readonly CsvConfiguration _csvConfiguration = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        HeaderValidated = null,
        MissingFieldFound = null
    };

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="db"></param>
    /// <param name="fs"></param>
    public DataImportService(
        ILiteDatabaseAsync db,
        IFileSystem fs,
        ILogger<DataImportService> logger)
    {
        _db = db;
        _fs = fs;
        _logger = logger;
    }

    public async Task LoadAsync(string filePath)
    {
        var collection = _db.GetCollection<GrossWrittenPremiumRecord>();

        var isCollectionExist = await collection.CountAsync() > 0;

        if (isCollectionExist)
        {
            _logger.LogInformation("The collection already exists. No need to load the data again.");
            // If the collection already exists, we don't need to load the data again.
            return;
        }

        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

        if (!_fs.File.Exists(filePath)) throw new FileNotFoundException($"The file '{filePath}' does not exist.");

        await collection.EnsureIndexAsync(x => x.Country);
        await collection.EnsureIndexAsync(x => x.LineOfBusiness);

        // Load the CSV file
        using var reader = _fs.File.OpenText(filePath);
        using var csv = new CsvReader(reader, _csvConfiguration);

        // Read the records from the CSV file
        // and map them to the GrossWrittenPremiumRecord
        var records = csv.GetRecords<GwpRecord>();

        // Map the records to the GrossWrittenPremiumRecord
        var mappedRecords = records
            .Select(record => record.Map())
            .ToArray();

        // Insert the records into the database
        await collection.InsertBulkAsync(mappedRecords);

        _logger.LogInformation($"Inserted {mappedRecords.Length} records into the database.");
    }
}