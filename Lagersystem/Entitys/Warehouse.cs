using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.Entitys;

public class Warehouse : AEntity
{

    public ICollection<Location> ItemLocations { get; set; } = new List<Location>();
    public Warehouse() : base(null) { }
    public Warehouse(string id) : base(id) { }
    public Warehouse(string id, ICollection<Location> itemLocations) : base(id)
    {
        ItemLocations = itemLocations;
    }


}
public class Location : AEntity
{


    [ForeignKey("Item_Id")]
    public Item Item { get; set; }
    [ForeignKey("Warehouse_Id")]
    public Warehouse Warehouse { get; set; }
    public string? Aisle { get; set; }
    public string? Shelf { get; set; }
    public string? Bin { get; set; }
    public Location() : base(null)
    { }

    public Location(string id, Item item) : base(id)
    {
        Item = item;
    }
    public Location(string id, Item item, string aisle, string shelf, string bin) : base(id)
    {
        Item = item;
        Aisle = aisle;
        Shelf = shelf;
        Bin = bin;
    }
}
