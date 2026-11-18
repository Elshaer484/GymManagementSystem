using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositorities.Interfaces
{
      public interface ISessionRepository : IGenericRepository<Session>
      {
            Task<bool> HasSessionNowById(int trainerId, CancellationToken ct);

            Task<IEnumerable<Session>> GetAllSessionsWithTrainersAndCatergryAsync(CancellationToken ct);

            Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default);

            Task<Session?> GetByIdSessionWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct = default);
      }
}
