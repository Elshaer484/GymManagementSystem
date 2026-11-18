using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace GymManagment.DAL.Repositorities.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false, CancellationToken ct = default);

        public Task<TEntity?> GetByIdAsync(int id, CancellationToken ct);

        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void  Delete(TEntity entity);

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate  , CancellationToken ct = default);

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate ,bool Tracking = false, CancellationToken ct = default);

        public Task<int> CountAsync(Expression<Func<TEntity , bool>>? predicate = null , CancellationToken ct = default);
    }
}
