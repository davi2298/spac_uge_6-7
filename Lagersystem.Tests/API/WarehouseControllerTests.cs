using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagersystem.API;
using Lagersystem.Entitys;
using Lagersystem.Tests.TestUtils;
using Lagersystem.Utilitys;
using Microsoft.EntityFrameworkCore;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace Lagersystem.Tests.API;
public class WarehouseControllerTests
{
    MockHttpMessageHandler httpMock;


    public WarehouseControllerTests()
    {
        httpMock = new MockHttpMessageHandler();


    }

    [Fact]
    public async Task DeleteWith0Items()
    {
        // Given
        var warehouseData = new List<Warehouse>{
            new Warehouse() { WarehouseId = "0",}
        }.AsQueryable();
        var mockWarehouse = DBMocker.CreateDBSet(warehouseData);
        var mockContext = new Mock<LagerContext>();
        mockContext.Setup(m => m.Warehouses).Returns(mockWarehouse.Object);
        var dbcontext = mockContext.Object;
        var controller = new WarehouseController(dbcontext);
        // When
        await controller.Delete("0");
        var tmp = dbcontext.Warehouses.First();
        // Then
        Assert.Empty(dbcontext.Warehouses);
    }
    [Fact]
    public async Task DeleteWithSomeItems()
    {
        // Given
        var itemsData = new List<Item> {
            new Item("Item 0") { ItemId = "0"},
            new Item("Item 1") { ItemId = "1"},
            new Item("Item 2") { ItemId = "2"},
            new Item("Item 3") { ItemId = "3"},
            new Item("Item 4") { ItemId = "4"},
            new Item("Item 5") { ItemId = "5"},
            new Item("Item 6") { ItemId = "6"},
            new Item("Item 7") { ItemId = "7"},
        }.AsQueryable();
        var locationsData = new List<Location> {
            new Location{LocationId = "0", Item = itemsData.Where(i => i.ItemId == "0").First(), Aisle = "aisle0", Bin = "bin"},
            new Location{LocationId = "1", Item = itemsData.Where(i => i.ItemId == "1").First(), Aisle = "aisle1", Bin = "bin"},
            new Location{LocationId = "2", Item = itemsData.Where(i => i.ItemId == "2").First(), Aisle = "aisle2", Bin = "bin"},
            new Location{LocationId = "3", Item = itemsData.Where(i => i.ItemId == "3").First(), Aisle = "aisle3", Bin = "bin"},
            new Location{LocationId = "4", Item = itemsData.Where(i => i.ItemId == "4").First(), Aisle = "aisle4", Bin = "bin"},
            new Location{LocationId = "5", Item = itemsData.Where(i => i.ItemId == "5").First(), Aisle = "aisle5", Bin = "bin"},
            new Location{LocationId = "6", Item = itemsData.Where(i => i.ItemId == "6").First(), Aisle = "aisle6", Bin = "bin"},
            new Location{LocationId = "7", Item = itemsData.Where(i => i.ItemId == "7").First(), Aisle = "aisle7", Bin = "bin"},
        }.AsQueryable();
        var warehouseData = new List<Warehouse>{
            new Warehouse() { WarehouseId = "0", ItemLocations = locationsData.ToList()}
        }.AsQueryable();
        var mockWarehouse = DBMocker.CreateDBSet(warehouseData);
        var mockItems = DBMocker.CreateDBSet(itemsData);
        var mockLocations = DBMocker.CreateDBSet(locationsData);
        var mockContext = new Mock<LagerContext>();
        mockContext.Setup(m => m.Warehouses).Returns(mockWarehouse.Object);
        mockContext.Setup(m => m.Items).Returns(mockItems.Object);
        var dbcontext = mockContext.Object;
        var controller = new WarehouseController(dbcontext);
        // When
        await controller.Delete("0");
        // Then
        Assert.Empty(dbcontext.Warehouses);
    }

    [Fact]
    public void AddNewLocationsWarehouse()
    {
        // Given
        var itemsData = new List<Item> {
            new Item("Item 0") { ItemId = "0"},
            new Item("Item 1") { ItemId = "1"},
            new Item("Item 2") { ItemId = "2"},
            new Item("Item 3") { ItemId = "3"},
            new Item("Item 4") { ItemId = "4"},
            new Item("Item 5") { ItemId = "5"},
            new Item("Item 6") { ItemId = "6"},
            new Item("Item 7") { ItemId = "7"},
        }.AsQueryable();
        var locationsData = new List<Location> {
            new Location{LocationId = "0", Item = itemsData.Where(i => i.ItemId == "0").First(), Aisle = "aisle0", Bin = "bin"},
            new Location{LocationId = "1", Item = itemsData.Where(i => i.ItemId == "1").First(), Aisle = "aisle1", Bin = "bin"},
            new Location{LocationId = "2", Item = itemsData.Where(i => i.ItemId == "2").First(), Aisle = "aisle2", Bin = "bin"},
            new Location{LocationId = "3", Item = itemsData.Where(i => i.ItemId == "3").First(), Aisle = "aisle3", Bin = "bin"},
            new Location{LocationId = "4", Item = itemsData.Where(i => i.ItemId == "4").First(), Aisle = "aisle4", Bin = "bin"},
            new Location{LocationId = "5", Item = itemsData.Where(i => i.ItemId == "5").First(), Aisle = "aisle5", Bin = "bin"},
            new Location{LocationId = "6", Item = itemsData.Where(i => i.ItemId == "6").First(), Aisle = "aisle6", Bin = "bin"},
            new Location{LocationId = "7", Item = itemsData.Where(i => i.ItemId == "7").First(), Aisle = "aisle7", Bin = "bin"},
        }.AsQueryable();
        var warehouseData = new List<Warehouse>{
            new Warehouse() { WarehouseId = "0",}
        }.AsQueryable();
        var mockWarehouse = DBMocker.CreateDBSet(warehouseData);
        var mockItems = DBMocker.CreateDBSet(itemsData);
        var mockLocations = DBMocker.CreateDBSet(locationsData);
        var mockContext = new Mock<LagerContext>();
        mockContext.Setup(m => m.Warehouses).Returns(mockWarehouse.Object);
        mockContext.Setup(m => m.Items).Returns(mockItems.Object);
        var dbcontext = mockContext.Object;
        var controller = new WarehouseController(dbcontext);

        // When
        foreach (var location in locationsData)
        {
            controller.AddItem(location, dbcontext.Warehouses.First().WarehouseId);
        }

        // Then
        Assert.Equal(locationsData.Count(), dbcontext.Warehouses.First().ItemLocations.Count);
    }

}
