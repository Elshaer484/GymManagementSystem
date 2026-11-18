using GymManagment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurations
{
    public class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.createdAt)
                   .HasColumnName("StartDate")
                   .HasDefaultValueSql("GETDATE()");








        }
    }
}
