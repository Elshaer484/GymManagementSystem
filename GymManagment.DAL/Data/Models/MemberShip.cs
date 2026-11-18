using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Models
{
    public class MemberShip : BaseEntity
    {
        public Member member { get; set; } = default!;

        public int memberId { get; set; }

        public Plan plan { get; set; } = default!;

        public int planId { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive => EndDate > DateTime.Now;

        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
    }
}
