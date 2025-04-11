using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.Entitys;

public class Warehouse : AEntity<Warehouse>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), MaxLength(36)]
    public string WarehouseId { get; init; }

    [InverseProperty("Warehouse")]
    public ICollection<Location> ItemLocations { get; set; } = new List<Location>();
    public Warehouse() { }
    public Warehouse(string id)
    {
        WarehouseId = id;
    }
    public Warehouse(string id, ICollection<Location> itemLocations) : this(id)
    {
        ItemLocations = itemLocations;
    }

    public void AddItemLocation(Item item, string aisle, string shelf, string bin)
    {
        var newLocation = new Location();
        newLocation.Warehouse = this;
        newLocation.Item = item;
        newLocation.Aisle = aisle;
        newLocation.Shelf = shelf;
        newLocation.Bin = bin;
        ItemLocations.Add(newLocation);
    }

    public void Update(Warehouse entity)
    {
        throw new NotSupportedException();
    }
}

public class Location
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), MaxLength(36)]
    public string LocationId { get; init; }

    [NotNull]
    [ForeignKey("ItemId"), JsonIgnore, CascadingParameter]
    public Item Item { get; set; }

    [ForeignKey("WarehouseId"), NotNull, JsonIgnore, CascadingParameter]

    public Warehouse Warehouse { get; set; }
    public string? Aisle { get; set; }
    public string? Shelf { get; set; }
    public string? Bin { get; set; }
}
