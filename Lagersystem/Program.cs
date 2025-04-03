using DotEnv.Core;
using Lagersystem.Entitys;
using Lagersystem.Utilitys;
using Microsoft.EntityFrameworkCore;
public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        // builder.Services.AddDbContext<LagerContext>(options => options.UseMySQL(connectionString: GetConnectionString()));

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        var app = builder.Build();
        // builder.Services.Configure<LagerContext>(options => options.)
        using (var dbContext = new LagerContext())
        {
            dbContext.Database.EnsureCreated();
            if (!dbContext.Warehouses.Any())
            {

                Setup(dbContext);
            }
        }

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

    private static void Setup(LagerContext context)
    {
        var supplyerNames = new[]
        {
            "Ana", "Bob", "Carl", "Dennis", "Erik"
        };
        var itemNames = new[]
        {
            "Alpha", "Beta", "Gamma", "Delta"
        };
        IList<Supplier> supplyers = [];
        IList<Item> items = [];
        foreach (var name in supplyerNames)
        {
            supplyers.Add(new Supplier(name));
        }
        var wareHouse = new Warehouse();
        foreach (var itemName in itemNames)
        {
            var item = new Item(itemName);
            var itemSupplier = supplyers[Random.Shared.Next(supplyers.Count-1)];
            item.Supplier = itemSupplier;
            item.Dimensions = new Dimensions(item, Random.Shared.Next(10, 100), Random.Shared.Next(10, 100), Random.Shared.Next(10, 100));
            itemSupplier.Items.Add(item);
            wareHouse.ItemLocations.Add(new Location(item, "1", $"{Random.Shared.Next(100)}", $"{itemName}"));
        }

        context.Warehouses.Add(wareHouse);
        context.Items.AddRange(items);
        context.Suppliers.AddRange(supplyers);
        context.SaveChanges();
    }
}