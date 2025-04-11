using Lagersystem.API;
using Lagersystem.Entitys;
using Lagersystem.Utilitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Lagersystem.Tests.API
{
    public class SupplierControllerTests
    {
        private readonly Mock<LagerContext> _mockContext;
        private readonly SupplierController _controller;
        private readonly Mock<DbSet<Supplier>> _mockSuppliers;

        public SupplierControllerTests()
        {
            _mockContext = new Mock<LagerContext>();
            _mockSuppliers = new Mock<DbSet<Supplier>>();

            var suppliers = new List<Supplier>
            {
                new Supplier("1", "Supplier 1", "Contact 1", new List<Item>()),
                new Supplier("2", "Supplier 2", "Contact 2", new List<Item>())
            }.AsQueryable();

            _mockSuppliers.As<IQueryable<Supplier>>().Setup(m => m.Provider).Returns(suppliers.Provider);
            _mockSuppliers.As<IQueryable<Supplier>>().Setup(m => m.Expression).Returns(suppliers.Expression);
            _mockSuppliers.As<IQueryable<Supplier>>().Setup(m => m.ElementType).Returns(suppliers.ElementType);
            _mockSuppliers.As<IQueryable<Supplier>>().Setup(m => m.GetEnumerator()).Returns(suppliers.GetEnumerator());

            _mockContext.Setup(c => c.Suppliers).Returns(_mockSuppliers.Object);

            _controller = new SupplierController(_mockContext.Object);
        }

        [Fact]
        public void GetAll_ReturnsAllSuppliers()
        {
            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Get_ReturnsSupplier_WhenSupplierExists()
        {
            // Act
            var result = _controller.Get("1") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Supplier>(result.Value);
            Assert.Equal("Supplier 1", ((Supplier)result.Value).Name);
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenSupplierDoesNotExist()
        {
            // Act
            var result = _controller.Get("3");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_AddsSupplier_WhenSupplierIsValid()
        {
            // Arrange
            var newSupplier = new Supplier("3", "Supplier 3", "Contact 3", new List<Item>());

            // Act
            var result = await _controller.Create(newSupplier);

            // Assert
            _mockSuppliers.Verify(m => m.Add(It.IsAny<Supplier>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Update_UpdatesSupplier_WhenSupplierExists()
        {
            // Arrange
            var updatedSupplier = new Supplier("1", "Updated Supplier", "Updated Contact", new List<Item>());

            // Act
            var result = await _controller.Update(updatedSupplier, "1");

            // Assert
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_RemovesSupplier_WhenSupplierExists()
        {
            // Act
            var result = await _controller.Delete("1");

            // Assert
            _mockSuppliers.Verify(m => m.Remove(It.IsAny<Supplier>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
            Assert.IsType<OkResult>(result);
        }
    }
}
