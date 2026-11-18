using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositorities.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext dbcontext;
        private readonly Dictionary<string, object> _repostories = [];
        public ISessionRepository SessionRepository { get; }
        public UnitOfWork(GymDbContext dbcontext , ISessionRepository SessionRepository)
        {
            this.dbcontext = dbcontext;
            this.SessionRepository = SessionRepository;
        }


        public IGenericRepository<TEntity> GetReposotries<TEntity>() where TEntity : BaseEntity, new()
        {
            var MyEntity = typeof(TEntity).Name;

            if(_repostories.TryGetValue(MyEntity , out object? value))
            {
                return (IGenericRepository<TEntity>)value;
            }
            else
            {
                var repo = new GenericRepository<TEntity>(dbcontext);
                _repostories.Add(MyEntity, repo);
                return repo;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await dbcontext.SaveChangesAsync(ct); 
        }
    }
}
