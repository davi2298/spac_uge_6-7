using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.Entitys;

public class Warehouse : AEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string WarehouseId { get; init; }

    [InverseProperty("Warehouse")]
    public ICollection<Location> ItemLocations { get; set; } = new List<Location>();
    public Warehouse()  { }
    public Warehouse(string id) { 
        WarehouseId = id;
    }
    public Warehouse(string id, ICollection<Location> itemLocations) : this(id)
    {
        ItemLocations = itemLocations;
    }


}

public class Location
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string LocationId { get; init; }

    [NotNull]
    [ForeignKey("ItemId")]
    public Item Item { get; set; }
    [ForeignKey("WarehouseId"),NotNull]
    public Warehouse Warehouse { get; set; }
    public string? Aisle { get; set; }
    public string? Shelf { get; set; }
    public string? Bin { get; set; }
}
