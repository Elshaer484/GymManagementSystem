using GymManagment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(x => x.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.HasIndex(x => x.Email)
                   .IsUnique();

            builder.HasIndex(x => x.phone)
                   .IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_GymUser_Phone", "phone LIKE '010%' or phone LIKE '011%' or phone LIKE '012%' or phone LIKE '015%'");
                tb.HasCheckConstraint("Email_check", "Email LIKE '_%@_%.%'");
            });

            builder.OwnsOne(x => x.address, address =>  
            {
                address.Property(x => x.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);

                address.Property(x => x.City)
                       .HasColumnName("City")
                       .HasColumnType("varchar")
                       .HasMaxLength(50 );
            });

        }
    }
}
