using LiteDB;
using LiteDB.Async;

namespace Galytix.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        
        builder.Services.AddSingleton<LiteDatabaseAsync>(sp =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Default");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'Default' not found.");
            }
            
            return new LiteDatabaseAsync(connectionString);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.Run();
    }
}