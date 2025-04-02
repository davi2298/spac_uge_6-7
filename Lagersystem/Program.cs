using DotEnv.Core;
using Lagersystem.Utilitys;
using Microsoft.EntityFrameworkCore;
public class Program
{
    public static void Main(string[] args)
    {
        // load .env
        var connectionString = GetConnectionString();
        // setting up servieces
        var services = new ServiceCollection();
        services.AddDbContext<SupplyerContext>(options => options.UseMySQL(connectionString: connectionString));
        // services.AddDbContext<WarehouseContext>(options => options.UseMySQL(connectionString)); // uncomment when implemented
        // services.AddDbContext<ItemContext>(options => options.UseMySQL(connectionString)); // uncomment when implemented

        var serviceProvider = services.BuildServiceProvider();
        var tmp = serviceProvider.GetRequiredService<SupplyerContext>();
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast");

        app.Run();


    }
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    public static string GetConnectionString()
    {
        // load .env
        new EnvLoader().Load();

        // generate connection strign given .env variables
        var dbuser = EnvReader.Instance["DBUSER"];
        var database = EnvReader.Instance["DBNAME"];
        var dbpassword = EnvReader.Instance["DBPASSWORD"];
        var connectionString = $"server=localhost;database={database};user={dbuser};password={dbpassword}";
        return connectionString;
    }
}