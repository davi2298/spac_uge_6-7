namespace Lagersystem.Entitys;

public class Warehouse : AEntity
{
    public IDictionary<Item, Location> ItemLocations;
    public Warehouse(string id, Dictionary<Item, Location> itemLocations) : base(id)
    {
        ItemLocations = itemLocations;
    }


}
    public record Location(string aisle, string shelf, string bin)
    {
        public string Aisle = aisle;
        public string Shelf = shelf;
        public string Bin = bin;
    }
