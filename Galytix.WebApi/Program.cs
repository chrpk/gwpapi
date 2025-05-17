using Galytix.WebApi.Feature.DataImport;
using Galytix.WebApi.Feature.DataImport.Abstractions;
using Galytix.WebApi.Feature.GrossWrittenPremium;
using LiteDB.Async;

namespace Galytix.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container;
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        builder.Services.AddSingleton<ILiteDatabaseAsync, LiteDatabaseAsync>(sp =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Default");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'Default' not found.");

            return new LiteDatabaseAsync(connectionString);
        });

        // Register features
        builder.Services.AddDataImportFeature();
        builder.Services.AddGrossWrittenPremiumFeature();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "Galytix API V1"); });
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        // Load the data into the database
        var csvFilePath = builder.Configuration["CsvFilePath"]
                          ?? throw new ArgumentNullException("CsvFilePath not found in configuration.");
        await app.Services.GetService<IDataImportService>().LoadAsync(csvFilePath);

        await app.RunAsync();
    }
}