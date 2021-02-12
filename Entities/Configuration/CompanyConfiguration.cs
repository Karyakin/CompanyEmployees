using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
   public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        /// <summary>
        /// подробнее о всех метадах Fluent API метанит
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(x => x.Country).HasMaxLength(20);
            builder.Property(x => x.Address).HasMaxLength(50);

            builder.HasData(
                new Company
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "IT_Solutions Ltd",
                    Address = "583 Wall Dr. Gwynn Oak, MD 21207",
                    Country = "USA"
                },
                new Company
                {
                    Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                    Name = "Admin_Solutions Ltd",
                    Address = "312 Forest Avenue, BF 923",
                    Country = "USA"
                });
        }
    }
}
