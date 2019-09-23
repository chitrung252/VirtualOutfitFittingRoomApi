using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.BaseConnect
{
    public interface IBaseService<TEntity, TViewModel> : IService where TEntity : class, IEntity, new()
        where TViewModel : BaseEntityViewModel<TEntity>
    {
        TEntity Get<TKey>(TKey id);

        IQueryable<TViewModel> Get();

        IQueryable<TViewModel> Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TViewModel> GetActive();

        IQueryable<TViewModel> GetActive(Expression<Func<TEntity, bool>> predicate);

        TViewModel FirstOrDefault();

        TViewModel FirstOrDefaultActive();

        TViewModel FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TViewModel FirstOrDefaultActive(Expression<Func<TEntity, bool>> predicate);

        void Create(TViewModel model, string keyName);

        void Edit<TKey>(TKey keyValue, TViewModel model);

        void Delete<TKey>(TKey keyValue);

        void Deactivate<TKey>(TKey keyValue);

        void Activate<TKey>(TKey keyValue);

         void Create(TEntity entity);

        void Activate(TEntity entity);

        void Deactivate(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Save();

        void Refresh(TEntity entity);

        Task<TViewModel> GetAsync<TKey>(TKey id);

        Task<List<TViewModel>> GetAsync();

        Task<List<TViewModel>> GetActiveAsync();

        Task CreateAsync(TViewModel model);

        Task CreateAsync(TViewModel model, string keyName);

        Task ActivateAsync<TKey>(TKey keyValue);

        Task<TViewModel> FirstOrDefaultAsync();

        Task<TViewModel> FirstOrDefaultActiveAsync();

        Task<TViewModel> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TViewModel> FirstOrDefaultActiveAsync(Expression<Func<TEntity, bool>> predicate);

        Task CreateAsync(TEntity entity);

        Task ActivateAsync(TEntity entity);

        Task DeactivateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task SaveAsync();
    }
}
