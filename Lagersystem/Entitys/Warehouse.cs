using System.ComponentModel.DataAnnotations.Schema;

namespace Lagersystem.Entitys;

public class Warehouse : AEntity
{
    public ICollection<Location> ItemLocations { get; set; } = new List<Location>();
    public Warehouse() : base(null) { }
    public Warehouse(string id, ICollection<Location> itemLocations) : base(id)
    {
        ItemLocations = itemLocations;
    }


}
public class Location
{
    [ForeignKey("Id")]
    public Item Item { get; set; }
    public string? Aisle { get; set; }
    public string? Shelf { get; set; }
    public string? Bin { get; set; }
    public Location(){}

    public Location(Item item)
    {
        Item = item;
    }
    public Location(Item item, string aisle, string shelf, string bin)
    {
        Item = item;
        Aisle = aisle;
        Shelf = shelf;
        Bin = bin;
    }
}
