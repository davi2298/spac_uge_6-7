using DotEnv.Core;
using Lagersystem.Entitys;
using Lagersystem.Utilitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        // return;
        builder.Services.AddDbContext<LagerContext>();
        // builder.Services.AddCors();

        builder.Services.AddControllers();
        builder.Services.AddCors();
        // builder.Services.AddMvc().AddJsonOptions(options  );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // app.UseHttpsRedirection();
        // app.UseRouting();

        app.UseCors();
        // app.UseStaticFiles();
        app.MapControllers();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("", () => "Hellow World");//.RequireCors();
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
        app.MapGet("", () => "Hellow World").RequireCors();

        // return;

        app.Run();


    }
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<LagerContext>();
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
        var pathToData = Path.Combine([TryGetSolutionDirectoryInfo().ToString(), "Lagersystem", "Data"]);

        AddFromSql(pathToData, "item.sql");
        AddFromSql(pathToData, "Supplier.sql");
        AddFromSql(pathToData, "Warehouse.sql");
        AddFromSql(pathToData, "Location.sql");


        context.Items.ToList()
            .ForEach(i =>
                i.Supplier = context.Suppliers.ToArray()[Random.Shared.Next(context.Suppliers.Count() - 1)]
            );

        while (!context.Warehouses.Any())
        { // todo : do something here
        }
        var joinedWarehouses = context.Warehouses.Join(context.Locations, w => w.WarehouseId, l => l.Warehouse.WarehouseId, (w, l) => new { Warehouse = w, Location = l });
        foreach (var warehouse in joinedWarehouses)
        {
            var tmp = warehouse.Location.Item;
            tmp.Location = warehouse.Warehouse;
        }
        context.SaveChanges();


        static void AddFromSql(string pathToData, string file)
        {
            var path = Path.Combine(pathToData, file);
            if (!File.Exists(path))
                return;
            IEnumerable<string> itemLines = File.ReadAllLines(path);
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