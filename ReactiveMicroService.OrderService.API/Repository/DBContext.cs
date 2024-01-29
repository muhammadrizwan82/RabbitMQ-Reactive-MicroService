using Microsoft.EntityFrameworkCore;
using ReactiveMicroService.OrderService.API.Models;

namespace ReactiveMicroService.OrderService.API.Repository
{
    public class DBContext : GenericDBContext<DBContext>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Orders> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
