namespace Lagersystem.Entitys;

public class Supplier : AEntity
{
    public string Name { get; set; }
    public string Contact { get; set; }
    public ICollection<Item> Items{ get; set; }
    public Supplier(string id, string name, string contact, ICollection<Item> items) : base(id)
    {
        Name = name;
        Contact = contact;
        Items = items;
    }
}
