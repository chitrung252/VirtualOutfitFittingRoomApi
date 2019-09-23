using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.BaseConnect
{
    public interface IBaseRepository<TEntity> : IRepository where TEntity : class, IEntity
    {
        TEntity Get<TKey>(TKey id);

        IQueryable<TEntity> Get();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetActive();

        IQueryable<TEntity> GetActive(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault();

        TEntity FirstOrDefaultActive();

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefaultActive(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Activate(TEntity entity);

        void Deactivate(TEntity entity);

        void Edit(TEntity entity);

        void Delete(TEntity entity);

        void Save();

        void Refresh(TEntity entity);

        Task<TEntity> GetAsync<TKey>(TKey id);

        Task<TEntity> FirstOrDefaultAsync();

        Task<TEntity> FirstOrDefaultActiveAsync();

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultActiveAsync(Expression<Func<TEntity, bool>> predicate);

        Task SaveAsync();
    }
}
