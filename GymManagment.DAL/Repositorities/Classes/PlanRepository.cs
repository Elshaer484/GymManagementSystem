using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositorities.Classes
{
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {
        public PlanRepository(GymDbContext db) : base(db)
        {
        }
        public Task<IEnumerable<Plan>> GetPlanWithMembers()
        {
            throw new NotImplementedException();
        }
    }
}
