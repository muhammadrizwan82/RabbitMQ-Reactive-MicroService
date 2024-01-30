﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReactiveMicroService.CustomerService.API.Repository;

#nullable disable

namespace ReactiveMicroService.CustomerService.API.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ReactiveMicroService.CustomerService.API.Models.CustomerDevices", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("CreatedIP")
                        .HasColumnType("longtext");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("DeviceId")
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceToken")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedIP")
                        .HasColumnType("longtext");

                    b.Property<string>("UserAgent")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerDevices");
                });

            modelBuilder.Entity("ReactiveMicroService.CustomerService.API.Models.Customers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("CreatedIP")
                        .HasColumnType("longtext");

                    b.Property<int?>("DialCode")
                        .HasColumnType("int");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedIP")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ReactiveMicroService.CustomerService.API.Models.CustomerDevices", b =>
                {
                    b.HasOne("ReactiveMicroService.CustomerService.API.Models.Customers", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });
#pragma warning restore 612, 618
        }
    }
}
