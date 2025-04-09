using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagersystem.Utilitys;
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
    public void setupContext(){
        var context = new LagerContext();
        
    }
    [Fact]
    public void Delete()
    {
        httpMock.When(HttpMethod.Delete, "");
            Assert.True(true);
    }
}
