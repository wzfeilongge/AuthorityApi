﻿// <auto-generated />
using System;
using Authority.repository.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Authority.Web.Api.Migrations
{
    [DbContext(typeof(DbcontextRepository))]
    partial class DbcontextRepositoryModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Authority.Model.BankModel.BusinessMan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BusinessType");

                    b.Property<bool>("Istrue");

                    b.Property<int>("ResultName");

                    b.Property<DateTime>("TakeNumber");

                    b.HasKey("Id");

                    b.ToTable("BusinessMan");
                });

            modelBuilder.Entity("Authority.Model.Model.BankModel.CounterCuteGirl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CounterNumber");

                    b.Property<string>("EmployeeNumber")
                        .IsRequired();

                    b.Property<bool>("IsTrue");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CounterCuteGirl");
                });

            modelBuilder.Entity("Authority.Model.Model.Departments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count");

                    b.Property<string>("DepartmentName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Authority.Model.Model.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Authority.Model.Model.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Price");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<string>("SaleDate");

                    b.Property<double>("TotalPrice");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Sale");
                });

            modelBuilder.Entity("Authority.Model.Model.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminPassword");

                    b.Property<string>("Department");

                    b.Property<string>("EndTime");

                    b.Property<string>("HospitalName");

                    b.Property<string>("PassWord")
                        .IsRequired();

                    b.Property<string>("Remarks");

                    b.Property<string>("Role");

                    b.Property<string>("StartTime");

                    b.Property<int>("State");

                    b.Property<string>("Token");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
