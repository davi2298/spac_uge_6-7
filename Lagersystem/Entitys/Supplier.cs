using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagersystem.Entitys;

public class Supplier : AEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? SupplierId { get; init; }

    public string Name { get; set; }
    public string? Contact { get; set; }
    public ICollection<Item> Items { get; set; } = new List<Item>();
    public Supplier(string name)
    {
        Name = name;
    }
    public Supplier(string id, string name, string contact, ICollection<Item> items)
    {
        SupplierId = id;
        Name = name;
        Contact = contact;
        Items = items;
    }
}
