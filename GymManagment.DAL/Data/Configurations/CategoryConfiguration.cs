using GymManagment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.CategoryName)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(x=>x.createdAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasData(
                    new Category { Id = 1, CategoryName = "Cardio" },
                    new Category { Id = 2, CategoryName = "Strength" },
                    new Category { Id = 3, CategoryName = "Yoga" },
                    new Category { Id = 4, CategoryName = "Boxing" },
                    new Category { Id = 5, CategoryName = "CrossFit" }
            );
        }
    }
}
