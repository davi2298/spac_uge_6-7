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
    public DbSet<Supplier> Suppliers { get; private set; }
    public DbSet<Item> Items { get; private set; }
    public DbSet<Warehouse> Warehouses { get; private set; }
    public DbSet<Location> Locations { get; private set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Program.GetConnectionString();
        optionsBuilder.UseMySQL(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        setupKeys<Supplier>(modelBuilder);
        setupKeys<Warehouse>(modelBuilder);
        setupKeys<Item>(modelBuilder);
        setupKeys<Location>(modelBuilder);
        // modelBuilder.Entity<Warehouse>().HasMany(e => e.ItemLocations).WithOne(e => e.Warehouse);
        // modelBuilder.Entity<Location>().HasOne(e => e.WarehouseId).WithMany(w => w.ItemLocations).HasForeignKey(w).HasAlternateKey(["Warehouse_Id","Item_Id"]);
        // modelBuilder.Entity<Item>().HasOne(e => e.Dimensions).WithOne   (e => e.Item)
    }
    private void setupKeys<T>(ModelBuilder modelBuilder) where T : AEntity
    {
        modelBuilder.Entity<T>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(f => f.Id)
                        .ValueGeneratedOnAdd();
                });
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
