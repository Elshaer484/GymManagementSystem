using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositorities.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainersAndCatergryAsync(CancellationToken ct)
        {
            var query = dbContext.sessions.AsNoTracking().Include(m => m.trainer).Include(m => m.category);

            return await query.ToListAsync(ct);
        }


        public async Task<bool> HasSessionNowById(int TrainerId , CancellationToken ct)
        {
            var query =  dbContext.trainers.AsNoTracking().Include(m => m.Sessions).Where(m=> m.Id == TrainerId);

            return query.Any() ? false : true;
        }

        public async Task<Session?> GetByIdSessionWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct = default)
        {
            return await dbContext.sessions.AsNoTracking().Include(x => x.trainer).Include(x => x.category).FirstOrDefaultAsync(n => n.Id == SessionId);
        }

        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return await dbContext.bookings.CountAsync(b => b.sessionId == sessionId);
        }
    }
}
