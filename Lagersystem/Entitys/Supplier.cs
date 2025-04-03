namespace Lagersystem.Entitys;

public class Supplier : AEntity
{
    public string Name { get; set; }
    public string? Contact { get; set; }
    public ICollection<Item> Items{ get; set; } = new List<Item>();
    public Supplier(string name):base (null){
        Name = name;
    }
    public Supplier(string id, string name, string contact, ICollection<Item> items) : base(id)
    {
        Name = name;
        Contact = contact;
        Items = items;
    }
}
