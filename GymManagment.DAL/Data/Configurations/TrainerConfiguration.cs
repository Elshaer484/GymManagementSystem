using GymManagment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurations
{
    public class TrainerConfiguration : GymUserConfiguration<Trainer> , IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(x => x.createdAt)
                   .HasColumnName("HireDate")
                   .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);
        }
    }
}
