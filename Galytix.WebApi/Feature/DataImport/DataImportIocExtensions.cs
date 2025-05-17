using System.IO.Abstractions;
using Galytix.WebApi.Feature.DataImport.Abstractions;
using Galytix.WebApi.Feature.DataImport.Services;

namespace Galytix.WebApi.Feature.DataImport;

public static class DataImportIocExtensions
{
    public static IServiceCollection AddDataImportFeature(this IServiceCollection services)
    {
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<IDataImportService, DataImportService>();

        return services;
    }
}