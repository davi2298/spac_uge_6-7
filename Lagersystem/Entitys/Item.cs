using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lagersystem.Entitys;
public class Item : AEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), MaxLength(36)]
    public string? ItemId { get; init; }

    public string Name { get; init; }
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public Warehouse? Location { get; set; }
    [JsonIgnore]
    public Supplier? Supplier { get; set; }
    public float? Price { get; set; }
    public float? Cost { get; set; }
    public float? Weight { get; set; }
    public Dimensions? Dimensions { get; set; }
    public DateTime? Expiration_date { get; set; }
    public DateTime? Date_received { get; set; }
    public DateTime? Last_updated { get; set; }
    public string? Status { get; set; }
    public Item(string name)
    {
        Name = name;
    }
    public Item(string id, string name, string sku, string barcode, string category, string description, int quantity, Warehouse location, Supplier supplier, float price, float cost, float weight, Dimensions dimensions, DateTime expiration_date, DateTime date_received, DateTime last_updated, string status)
    {
        ItemId = id;
        Name = name;
        Sku = sku;
        Barcode = barcode;
        Category = category;
        Description = description;
        Quantity = quantity;
        Location = location;
        Supplier = supplier;
        Price = price;
        Cost = cost;
        Weight = weight;
        Dimensions = dimensions;
        Expiration_date = expiration_date;
        Date_received = date_received;
        Last_updated = last_updated;
        Status = status;
    }
}



public class Dimensions
{
    [ForeignKey("Id"), JsonIgnore]
    public Item Item { get; set; }

    public float Length { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public Dimensions() { }
    public Dimensions(Item item)
    {
        Item = item;
    }
    public Dimensions(Item item, float length, float width, float height)
    {
        Item = item;
        Length = length;
        Width = width;
        Height = height;
    }
}
