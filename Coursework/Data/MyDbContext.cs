using Coursework.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Coursework.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<UserTable> Users { get; set; }
        public DbSet<OrderTypeModel> OrderTypes { get; set; }
        public DbSet<OrderInfoModel> OrderInfo { get; set; }
        public DbSet<AddressInfoModel> AdressInfo { get; set; }
        public DbSet<CarTable> CarTable { get; set; }
        public DbSet<DriverInfo> DriverInfo { get; set; }
        public DbSet<RolesModel> Roles { get; set; }

        // Other DbSet properties for your entities

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTable>().ToTable("UserTable", "dbo");
            modelBuilder.Entity<OrderTypeModel>().ToTable("OrderType", "dbo");
            modelBuilder.Entity<OrderInfoModel>().ToTable("OrderInfo", "dbo");
            modelBuilder.Entity<AddressInfoModel>().ToTable("AddressInfo", "dbo");
            modelBuilder.Entity<CarTable>().ToTable("CarTable", "dbo");
            modelBuilder.Entity<DriverInfo>().ToTable("DriverInfo", "dbo");

            modelBuilder.Entity<RolesModel>().ToTable("Roles", "dbo");
        }

        private readonly IConfiguration _configuration;

        public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

}
