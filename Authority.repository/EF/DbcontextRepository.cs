using Authority.Model.BankModel;
using Authority.Model.Model;
using Authority.Model.Model.BankModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Authority.repository.EF
{
    public class DbcontextRepository : DbContext
    {
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
            //this.Database.EnsureCreated();
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var sqlType = config["Sql:sqlType"];
            var sqlstr = config["Sql:str"];
            if (sqlType == "1")
            {
                optionsBuilder.UseSqlServer(sqlstr, b => b.UseRowNumberForPaging());
                return;
            }
            else if (sqlType == "2")
            {
                optionsBuilder.UseOracle(sqlstr);
                return;
            }
            else if (sqlType == "3")
            {
                optionsBuilder.UseMySQL(sqlstr);
                return;
            }


        }

        public DbSet<User> User { get; set; }


        public DbSet<Departments> Departments { get; set; }

        public DbSet<Sale> Sale { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<BusinessMan> BusinessMan { get; set; }

        public DbSet<CounterCuteGirl> CounterCuteGirl { get; set; }


    }
}
