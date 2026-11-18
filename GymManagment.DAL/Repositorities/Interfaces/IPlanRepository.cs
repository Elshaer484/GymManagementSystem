using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositorities.Interfaces
{
    public interface IPlanRepository : IGenericRepository<Plan>
    {
        Task<IEnumerable<Plan>> GetPlanWithMembers();

    }
}
