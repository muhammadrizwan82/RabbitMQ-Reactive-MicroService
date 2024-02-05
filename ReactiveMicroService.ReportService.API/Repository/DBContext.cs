using Microsoft.EntityFrameworkCore;
using ReactiveMicroService.ReportService.API.Models;

namespace ReactiveMicroService.ReportService.API.Repository
{
    public class DBContext : GenericDBContext<DBContext>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<CustomerDevices> CustomerDevices { get; set; }
        public DbSet<CustomerAddresses> CustomerAddresses { get; set; }
        public DbSet<Orders> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
