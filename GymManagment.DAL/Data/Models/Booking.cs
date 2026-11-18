using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Models
{
    public class Booking : BaseEntity
    {
        public Member member { get; set; } = default!;

        public int memberId { get; set; }

        public Session session { get; set; } = default!;

        public int sessionId { get; set; }

        public bool IsAttended { get; set; }



    }
}
