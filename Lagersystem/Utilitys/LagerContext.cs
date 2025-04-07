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
