using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagersystem.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.Utilitys;

public class LagerContext : DbContext
{

    // set of entites
    public DbSet<Supplier> Suppliers { get; private set; }
    public DbSet<Item> Items { get; private set; }
    public DbSet<Warehouse> Warehouses { get; private set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Program.GetConnectionString();
        optionsBuilder.UseMySQL(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        // modelBuilder.Entity<Item>().HasOne(e => e.Dimensions).WithOne   (e => e.Item)
    }

}
