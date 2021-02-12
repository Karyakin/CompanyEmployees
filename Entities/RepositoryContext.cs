using Entities.Configuration;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositorys
{
   public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            :base(options)
        {
        }

        /// <summary>
        /// В данном меоде мы вызываем два собственных метода для настройки наших моделей
        /// подробнее о всех метадах Fluent API метанит
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());// метод из книжки
           // modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            new EmployeeConfiguration().Configure(modelBuilder.Entity<Employee>());// метод майков modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogEntityTypeConfiguration).Assembly);
        }


        DbSet<Company> Companies { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<User> Users { get; set; }

    }
}
