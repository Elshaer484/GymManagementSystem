namespace GymManagment.DAL.Data.Models
{
    public class Plan : BaseEntity
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public int DurationDays { get; set; }

        public Decimal Price { get; set; }

        public bool IsActive { get; set; }

        public ICollection<MemberShip> MemberShips { get; set; } = default!;
    }
}
