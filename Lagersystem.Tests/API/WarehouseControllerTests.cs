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
    public LagerContext setupContext()
    {
        var warehouseData = new List<Warehouse>{
            new Warehouse() { WarehouseId = "0",}
        }.AsQueryable();
        var mockWarehouse = DBMocker.CreateDBSet(warehouseData);
        var mockContext = new Mock<LagerContext>();
        mockContext.Setup(m => m.Warehouses).Returns(mockWarehouse.Object);
        return mockContext.Object;

    }
    [Fact]
    public void DeleteWith0Items()
    {
        var dbcontext = setupContext();
        var controller = new WarehouseController(dbcontext);
        controller.Delete("0");
        Assert.True(true);
    }


}
