using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagersystem.Utilitys;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Lagersystem.Tests.TestUtils;
public class DBMocker
{
    public static Mock<DbSet<T>> CreateDBSet<T>(IQueryable<T> itemData) where T : class
    {
        var mockItems = new Mock<DbSet<T>>();
        mockItems.As<IQueryable<T>>().Setup(m => m.Provider).Returns(itemData.Provider);
        mockItems.As<IQueryable<T>>().Setup(m => m.Expression).Returns(itemData.Expression);
        mockItems.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(itemData.ElementType);
        mockItems.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(itemData.GetEnumerator());
        return mockItems;
    }
}
