using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagersystem.Entitys;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Lagersystem.Utilitys;

public class LagerContext : DbContext
{

    // set of entites
    public virtual DbSet<Supplier> Suppliers { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<Warehouse> Warehouses { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Program.GetConnectionString();
        optionsBuilder.UseMySQL(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.Entity<Warehouse>()
        //     .HasMany(x => x.ItemLocations)
        //     .WithOne(x => x.Warehouse)
        
    }


    /// <summary>
    /// Danger zone
    /// </summary>
    /// <param name="sql"></param>
    public static void AddItemFromSQL(string sql, string connectionString)
    {
        using var tmp = new MySqlConnection(connectionString);
        tmp.Open();
        var tmp2 = tmp.CreateCommand();
        tmp2.CommandText = sql;
        tmp2.ExecuteNonQuery();
        tmp.Close();
    }

}
