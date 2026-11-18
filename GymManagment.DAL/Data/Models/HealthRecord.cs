using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal height { get; set; }

        public decimal weight { get; set; }

        public string BloodType { get; set; }
        
        public string? Note { get; set; }

        #region RelationShips

        public Member member { get; set; }
        
        public int memberId { get; set; }

        #endregion

    }
}
