// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using DotEnv.Core;
// using Lagersystem.Utilitys;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using MySqlX.XDevAPI.Common;


// public class LagerContextFactory : IDesignTimeDbContextFactory<LagerContext>
// {
//     public LagerContext CreateDbContext(string[] args)
//     {
//         new EnvLoader().Load();

//         var dbuser = EnvReader.Instance["DBUSER"];
//         var database = EnvReader.Instance["DBNAME"];
//         var dbpassword = EnvReader.Instance["DBPASSWORD"];
//         var connectionString = $"server=localhost;database={database};user={dbuser};password={dbpassword}";

//         var optionsBuilder = new DbContextOptionsBuilder<LagerContext>();
//         optionsBuilder.UseMySQL(connectionString: connectionString);
//         // optionsBuilder.UseMySQL(Result)

//         return new LagerContext();
//     }
// }
