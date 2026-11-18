using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositorities.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetReposotries<TEntity>() where TEntity : BaseEntity, new();

        Task<int> SaveChangesAsync(CancellationToken ct); 


        public ISessionRepository SessionRepository { get; }
    }
}
