using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }


        #region RelationShips

        
        public HealthRecord healthRecord { get; set; } = default!;

        public ICollection<MemberShip> memberShips { get; set; } = default!;

        public ICollection<Booking> MemberSessions { get; set; } = default!;

        #endregion
    }
}
