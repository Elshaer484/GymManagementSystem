using GymManagment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurations
{
    public class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x => x.createdAt)
                   .HasDefaultValueSql("GETDATE()")
                   .HasColumnName("JoinDate");

            base.Configure(builder);
        }
    }
}
