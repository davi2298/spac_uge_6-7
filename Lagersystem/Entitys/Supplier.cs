using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lagersystem.Entitys;

public class Supplier : AEntity<Supplier>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), MaxLength(36)]
    public string? SupplierId { get; init; }

    public string Name { get; set; }
    public string? Contact { get; set; }
    [JsonIgnore]
    public ICollection<Item> Items { get; set; } = new List<Item>();
    [JsonConstructor]
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

    public void Update(Supplier entity)
    {
        Contact = entity.Contact ?? Contact;
        Items = entity.Items ?? Items;
    }
}
