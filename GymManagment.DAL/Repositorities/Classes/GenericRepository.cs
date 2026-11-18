using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagment.DAL.Repositorities.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        public readonly GymDbContext _dbContext;
        public readonly DbSet<TEntity> set; 

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            set = dbContext.Set<TEntity>();
        }


        public void Add(TEntity entity)
        {
            set.Add(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
          return await set.AsNoTracking().AnyAsync(predicate, ct);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        {
            return predicate is null ? await set.AsNoTracking().CountAsync(ct) : await set.AsNoTracking().CountAsync(predicate, ct);
        }

        public void Delete(TEntity entity)
        {
            set.Remove(entity);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool Tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = Tracking ? set : set.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate);

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges, CancellationToken ct)
        {
            IQueryable<TEntity> query = trackChanges ? set : set.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct)
        {
           return await set.FindAsync(id,ct);
        }

        public void Update(TEntity entity)
        {
            set.Update(entity);  
        }


         
    }
}
