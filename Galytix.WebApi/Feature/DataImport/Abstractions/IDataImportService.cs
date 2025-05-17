namespace Galytix.WebApi.Feature.DataImport.Abstractions;

public interface IDataImportService
{
    Task LoadAsync(string filePath);
}