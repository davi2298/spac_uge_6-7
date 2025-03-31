using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.Utilitys;

public class ExampleContext<T> : DbContext
{
    // Stinglton stuff
    private static ExampleContext<T>? instanse;
    public static ExampleContext<T> GetInstanse => instanse ??= new();

}
