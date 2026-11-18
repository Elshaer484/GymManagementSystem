using GymManagment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(x => x.Id);

            builder.Property(x => x.createdAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => new { x.sessionId, x.memberId });
        }
    }
}
