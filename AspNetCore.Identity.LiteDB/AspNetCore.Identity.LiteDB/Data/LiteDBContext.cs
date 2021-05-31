using System;
using System.Linq;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.Identity.LiteDB.Data
{
   public class LiteDbContext : ILiteDbContext
   {
      public LiteDatabase LiteDatabase { get; protected set; }

      public LiteDbContext(LiteDatabase liteDatabase)
      {
         LiteDatabase = liteDatabase;
      }

      public LiteDbContext(IConfiguration configuration)
      {
         try
         {
            string connectionString = configuration
               .GetSection("ConnectionStrings")
               .GetChildren()
               .FirstOrDefault()
               ?.Value;

            LiteDatabase = new LiteDatabase(connectionString);
         }
         catch (NullReferenceException)
         {
            throw new NullReferenceException("No connection string defined in appsettings.json");
         }
      }
   }
}