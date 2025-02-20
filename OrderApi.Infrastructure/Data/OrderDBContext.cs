using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using Mysqlx.Crud;
using Microsoft.EntityFrameworkCore;


namespace OrderApi.Infrastructure.Data
{
   
  public class OrderDBContext(DbContextOptions<OrderDBContext> options) :DbContext (options)

    {
      public DbSet<Domain.Entities.Order> Orders { get; set; }
     }

         
    
}
