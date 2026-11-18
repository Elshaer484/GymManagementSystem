using GymManagment.DAL.Data.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Data.Models
{
    public class Trainer : GymUser
    {
        public Special specialization { get; set; }

        public ICollection<Session> Sessions { get; set; } = default!;
    }
}
