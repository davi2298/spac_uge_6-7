using DotEnv.Core;
using Lagersystem.Entitys;
using Lagersystem.Utilitys;
using Microsoft.EntityFrameworkCore;
public class Program
{
    public static void Main(string[] args)
    {
        // load .env
        new EnvLoader().Load();

        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        // builder.Services.AddDbContext<LagerContext>(options => options.UseMySQL(connectionString: GetConnectionString()));

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        var app = builder.Build();
        // builder.Services.Configure<LagerContext>(options => options.)
        using (var dbContext = new LagerContext())
        {
            if (EnvReader.Instance.EnvBool("DBDEBUGGING"))
            {
                dbContext.Database.EnsureDeleted();
            }
            dbContext.Database.EnsureCreated();
            if (!dbContext.Items.Any())
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
        // return;
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
        var pathToData = TryGetSolutionDirectoryInfo() + "\\Lagersystem\\Data";
        AddFromSql(pathToData, "\\item.sql");
        AddFromSql(pathToData, "\\Supplier.sql");
        AddFromSql(pathToData, "\\Warehouse.sql");
        AddFromSql(pathToData, "\\Location.sql");


        context.Items.ToList()
            .ForEach(i =>
                i.Supplier = context.Suppliers.ToArray()[Random.Shared.Next(context.Suppliers.Count() - 1)]
            );

        while (!context.Warehouses.Any()) { }
        var joinedWarehouses = context.Warehouses.Join(context.Locations, w => w.Id, l => l.Warehouse.Id,(w,l) => new {Warehouse = w,Location =l} );
        foreach (var warehouse in joinedWarehouses)
        {
            var tmp = warehouse.Location.Item;
            tmp.Location = warehouse.Warehouse;
        }
        context.SaveChanges();


        static void AddFromSql(string pathToData, string file)
        {
            if (!File.Exists(pathToData + file))
                return;
            IEnumerable<string> itemLines = File.ReadAllLines(pathToData + file);

            foreach (var line in itemLines)
            {
                LagerContext.AddItemFromSQL(line, GetConnectionString());
            }
        }
    }
    
    /// <summary>
    /// This is for when debugging it returns to the folder with the sln and not the debug subfolder
    /// Taken from "https://stackoverflow.com/questions/19001423/getting-path-to-the-parent-folder-of-the-solution-file-using-c-sharp"
    /// </summary>
    /// <param name="currentPath"></param>
    /// <returns></returns>
    public static DirectoryInfo TryGetSolutionDirectoryInfo(string? currentPath = null)
    {
        var directory = new DirectoryInfo(
            currentPath ?? Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory!;
    }
}