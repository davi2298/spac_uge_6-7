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
        var connectionString = Program.GetConnectionString();
        optionsBuilder.UseMySQL(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    } // if special serialse/deserialse stuff needed

}
