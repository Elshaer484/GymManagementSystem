using GymManagment.DAL.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GymManagment.DAL.Data.Configurations
{
    public class PlanConfigure : IEntityTypeConfiguration<Plan>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Plan> builder)
        {
            builder.Property(a => a.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(a => a.Description)
                   .HasMaxLength(200);


            builder.Property(x => x.Price)
                   .HasPrecision(10, 2);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("DurationCheck", "DurationDays Between 1 and 365");
            });
        }
    }
}
