using GymManagment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(x =>
            {
                x.HasCheckConstraint("CK_Session_Capacity", "Capacity Between 1 and 25");
                x.HasCheckConstraint("CK_Session_StartDate", "StartDate < EndDate");
            });


        }
    }
}
