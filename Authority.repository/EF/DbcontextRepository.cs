using Authority.Common.Helper;
using Authority.Model.BankModel;
using Authority.Model.Model;
using Authority.Model.Model.BankModel;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

namespace Authority.repository.EF
{
    public class DbcontextRepository : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = new LoggerFactory(new[] {
            new DebugLoggerProvider()
        });
        public DbcontextRepository()
        {

        }
        public static DbcontextRepository Context
        {
            get
            {
                return new DbcontextRepository();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var sqltype = "Sql:sqlType";
            var sqlstr = "Sql:str";
            var sql = JsonHelper.GetDbConnection(sqltype, sqlstr);
            if (sql.Item1 == "1")
            {
                optionsBuilder.UseSqlServer(sql.Item2, b => b.MigrationsAssembly("Authority.Web.Api"));
            }
            else if (sql.Item1 == "2")
            {
                optionsBuilder.UseOracle(sql.Item2, b => b.MigrationsAssembly("Authority.Web.Api"));
            }
            else if (sql.Item1 == "3")
            {
                optionsBuilder.UseMySQL(sql.Item2, b => b.MigrationsAssembly("Authority.Web.Api"));
            }
            else if (sql.Item1 == "4")
            {
                optionsBuilder.UseSqlite(sql.Item2, b => b.MigrationsAssembly("Authority.Web.Api"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var tuser = modelBuilder.Entity<User>().ToTable("User");
            tuser.Property(e => e.Id).IsRequired().ValueGeneratedNever(); //用户表设置主键不自增
            tuser.Property(e => e.UserName).IsRequired();
            tuser.Property(e => e.PassWord).IsRequired();

            var uDepartments = modelBuilder.Entity<Departments>().ToTable("Departments");
            uDepartments.Property(e => e.Id).IsRequired();
            uDepartments.Property(e => e.DepartmentName).IsRequired();

            var uSale = modelBuilder.Entity<Sale>().ToTable("Sale");
            uSale.Property(e => e.Id).IsRequired();
            uSale.Property(e => e.ProductId).IsRequired();
            uSale.Property(e => e.Quantity).IsRequired();
            uSale.Property(e => e.UserId).IsRequired();
            uSale.Property(e => e.TotalPrice).IsRequired();

            var uProduct = modelBuilder.Entity<Product>().ToTable("Product");
            uProduct.Property(e => e.Id).IsRequired();
            uProduct.Property(e => e.ProductName).IsRequired();

            var uBusinessMan = modelBuilder.Entity<BusinessMan>().ToTable("BusinessMan");
            uBusinessMan.Property(e => e.Id).IsRequired();
            uBusinessMan.Property(e => e.Istrue).IsRequired();
            uBusinessMan.Property(e => e.TakeNumber).IsRequired();
            uBusinessMan.Property(e => e.ResultName).IsRequired();

            var uCounterCuteGirl = modelBuilder.Entity<CounterCuteGirl>().ToTable("CounterCuteGirl");
            uCounterCuteGirl.Property(e => e.CounterNumber).IsRequired();
            uCounterCuteGirl.Property(e => e.EmployeeNumber).IsRequired();
            uCounterCuteGirl.Property(e => e.IsTrue).IsRequired();
            uCounterCuteGirl.Property(e => e.Name).IsRequired();
            uCounterCuteGirl.Property(e => e.Id).IsRequired();
        }



        public DbSet<User> User { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<BusinessMan> BusinessMan { get; set; }
        public DbSet<CounterCuteGirl> CounterCuteGirl { get; set; }
    }
}
