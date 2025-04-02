using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagersystem.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.Utilitys;

public class SupplyerContext : DbContext
{
    public SupplyerContext(DbContextOptions<SupplyerContext> options) : base(options)
    {
    }

    // set of supplyers
    public DbSet<Supplier> Suppliers { get; private set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
        // var database = EnvReader.GetStringValue("DATABASE");
        // var dbuser = EnvReader.GetStringValue("DBUSER");
        // var dbpassword = EnvReader.GetStringValue("DBPASSWORD"); ;
        // var connectionString = $"server=localhost;database={database};user={dbuser};password={dbpassword}";
        var connectionString = $"server=localhost;database=LagerSystem;user=root;password=Velkommen25";
        optionsBuilder.UseMySQL(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    } // if special serialse/deserialse stuff needed

}
