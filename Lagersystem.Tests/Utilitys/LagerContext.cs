using Lagersystem.Entitys;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Lagersystem.Utilitys.Tests;

public class LagerContextTests //: IDisposable
{
    public LagerContextTests()
    {
    }
    // public void Dispose()
    // {

    // }

    [Fact]
    public void GetItemDimenssions()
    {
        // Given
        var dimension0 = new Dimensions { Width = 10, Height = 10, Length = 10 };
        var dimension1 = new Dimensions { Width = 15, Height = 15, Length = 15 };
        var dimension2 = new Dimensions { Width = 100, Height = 100, Length = 100 };
        var itemData = new List<Item> {
            new Item("Item 1") { Id = "0", Dimensions = dimension0},
            new Item("Item 2") { Id = "1", Dimensions = dimension1},
            new Item("Item 3") { Id = "2", Dimensions = dimension2},
        }.AsQueryable();
        Mock<DbSet<Item>> mockItems = createDBSet<Item>(itemData);
        var mockContext = new Mock<LagerContext>();
        mockContext.Setup(m => m.Items).Returns(mockItems.Object);
        var context = mockContext.Object;

        // When
        var item0 = context.Items.Where(i => i.Id == "0").First();
        var item1 = context.Items.Where(i => i.Id == "1").First();
        var item2 = context.Items.Where(i => i.Id == "2").First();

        // Then

        Assert.Equal(dimension0, item0.Dimensions);
        Assert.Equal(dimension1, item1.Dimensions);
        Assert.Equal(dimension2, item2.Dimensions);
    }


    [Fact]
    public void warehouseSavesNewLocation()
    {
        // Given
        var itemData = new List<Item> {
            new Item("Item 0") { Id = "0"},
        }.AsQueryable();
        var warehouseData = new List<Warehouse>{
            new Warehouse() { Id = "0",}
        }.AsQueryable();

        Mock<DbSet<Item>> mockItems = createDBSet(itemData);
        Mock<DbSet<Warehouse>> mockWarehous = createDBSet(warehouseData);
        var mockContext = new Mock<LagerContext>();
        mockContext.Setup(m => m.Items).Returns(mockItems.Object);
        mockContext.Setup(m => m.Warehouses).Returns(mockWarehous.Object);
        var context = mockContext.Object;

        // When
        var warehouse = context.Warehouses.First();
        var item = context.Items.First();
        warehouse.AddItemLocation(item, "Aisle", "Shelf", "Bin");
        var expectedLocation = warehouse.ItemLocations.First();
        context.SaveChanges();
        var warehouseActual = context.Warehouses.First();


        // Then
        var actualLocation = warehouseActual.ItemLocations.First();
        Assert.Single(warehouseActual.ItemLocations);
        Assert.Equal(expectedLocation, actualLocation);
    }
    private static Mock<DbSet<T>> createDBSet<T>(IQueryable<T> itemData) where T : class
    {
        var mockItems = new Mock<DbSet<T>>();
        mockItems.As<IQueryable<T>>().Setup(m => m.Provider).Returns(itemData.Provider);
        mockItems.As<IQueryable<T>>().Setup(m => m.Expression).Returns(itemData.Expression);
        mockItems.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(itemData.ElementType);
        mockItems.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(itemData.GetEnumerator());
        return mockItems;
    }
}
