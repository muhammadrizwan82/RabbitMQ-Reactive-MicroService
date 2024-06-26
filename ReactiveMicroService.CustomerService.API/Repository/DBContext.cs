﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactiveMicroService.CustomerService.API.Models;
using System.Reflection.Metadata;

namespace ReactiveMicroService.CustomerService.API.Repository
{
    public class DBContext : GenericDBContext<DBContext>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<CustomerDevices> CustomerDevices { get; set; }
        public DbSet<CustomerAddresses> CustomerAddresses { get; set; }

   
    }
}
