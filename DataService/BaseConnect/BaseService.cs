using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataService.Models.Entities;
using DataService.Utilities;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.BaseConnect
{
    public abstract class BaseService
    {

        public IConfigurationProvider AutoMapperConfig
        {
            get
            {
                return this.Service<IConfigurationProvider>();
            }
        }

        public T Service<T>() where T : class
        {
            return DependencyUtils.Resolve<T>();
        }

    }
    public abstract class BaseService<TEntity, TViewModel> :BaseService, IBaseService<TEntity, TViewModel>, IService where TEntity : class, IEntity, new()
        where TViewModel : BaseEntityViewModel<TEntity>, new()
    {
        protected IUnitOfWork unitOfWork;

        protected IBaseRepository<TEntity> repository;

        public IUnitOfWork UnitOfWork { get { return unitOfWork; } }

        public IBaseRepository<TEntity> Repository { get { return repository; } }
        public BaseService(IUnitOfWork unitOfWork, IBaseRepository<TEntity> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public BaseService() { }

        public virtual TEntity Get<TKey>(TKey id)
        {
            return this.repository.Get(id);
        }

        public virtual IQueryable<TViewModel> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.repository.Get(predicate).ProjectTo<TViewModel>(this.AutoMapperConfig);
        }
        public virtual IQueryable<TViewModel> GetActive()
        {
            return this.repository.GetActive().ProjectTo<TViewModel>(this.AutoMapperConfig);
        }

        public IQueryable<TViewModel> GetActive(Expression<Func<TEntity, bool>> predicate)
        {
            return this.repository.GetActive(predicate).ProjectTo<TViewModel>(this.AutoMapperConfig);
        }

        public TViewModel FirstOrDefault()
        {
            return this.repository.FirstOrDefault().ToViewModel<TEntity, TViewModel>();
        }

        public TViewModel FirstOrDefaultActive()
        {
            return this.repository.FirstOrDefaultActive().ToViewModel<TEntity, TViewModel>();
        }

        public TViewModel FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.repository.FirstOrDefault(predicate).ToViewModel<TEntity, TViewModel>();
        }

        public TViewModel FirstOrDefaultActive(Expression<Func<TEntity, bool>> predicate)
        {
            return this.repository.FirstOrDefaultActive(predicate).ToViewModel<TEntity, TViewModel>();
        }
        /// <summary>
        /// Create Entity 
        /// </summary>
        /// <param name="model">TViewModel</param>
        /// <param name="keyName"></param>
        public virtual void Create(TViewModel model, string keyName)
        {
            var entity = model.ToEntity();
            Create(entity);

            var entityKeyProperty = entity.GetType().GetProperty(keyName);
            var modelKeyProperty = model.GetType().GetProperty(keyName);

            modelKeyProperty.SetValue(model, entityKeyProperty.GetValue(entity));
        }
        public virtual void Create(TEntity entity)
        {
            this.OnCreate(entity);
            this.Save();
        }

        /// <summary>
        /// Eidit Entity 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keyValue"></param>
        /// <param name="model">TViewModel</param>
        public virtual void Edit<TKey>(TKey keyValue, TViewModel model)
        {
            var entity = Get(keyValue);
            model.CopyToEntity(entity);
            Update(entity);
        }

        public virtual void Update(TEntity entity)
        {
            this.OnUpdate(entity);
            this.Save();
        }

        public virtual void Delete<TKey>(TKey keyValue)
        {
            var entity = Get(keyValue);
            Delete(entity);
        }
        public virtual void Delete(TEntity entity)
        {
            this.OnDelete(entity);
            this.Save();
        }

        public virtual IQueryable<TViewModel> Get()
        {
            return this.repository.Get().ProjectTo<TViewModel>(this.AutoMapperConfig);
        }

        public virtual void Activate<TKey>(TKey keyValue)
        {
            var entity = Get(keyValue);
            Activate(entity);
        }

        public virtual void Deactivate<TKey>(TKey keyValue)
        {
            var entity = Get(keyValue);
            Deactivate(entity);
        }
        public virtual void Activate(TEntity entity)
        {
            this.OnActivate(entity);
            this.Save();
        }

        public virtual void Deactivate(TEntity entity)
        {
            this.OnDeactivate(entity);
            this.Save();
        }

        public virtual void Save()
        {
            this.unitOfWork.Commit();
        }

        public virtual void Refresh(TEntity entity)
        {
            this.repository.Refresh(entity);
        }

        public async Task<TViewModel> GetAsync<TKey>(TKey id)
        {
            return (await this.repository.GetAsync(id))?.ToViewModel<TEntity, TViewModel>();
        }

        public async virtual Task<List<TViewModel>> GetAsync()
        {
            return await Get()
                .ProjectTo<TViewModel>(this.AutoMapperConfig)
                .ToListAsync();
        }

        public async virtual Task<List<TViewModel>> GetActiveAsync()
        {
            return await GetActive()
                .ProjectTo<TViewModel>(this.AutoMapperConfig)
                .ToListAsync();
        }


        public async Task<TViewModel> FirstOrDefaultAsync()
        {
            return (await this.repository.FirstOrDefaultAsync())?.ToViewModel<TEntity, TViewModel>();
        }

        public async Task<TViewModel> FirstOrDefaultActiveAsync()
        {
            return (await this.repository.FirstOrDefaultActiveAsync())?.ToViewModel<TEntity, TViewModel>();
        }

        public async Task<TViewModel> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await this.repository.FirstOrDefaultAsync(predicate))?.ToViewModel<TEntity, TViewModel>();
        }

        public async Task<TViewModel> FirstOrDefaultActiveAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await this.repository.FirstOrDefaultActiveAsync(predicate))?.ToViewModel<TEntity, TViewModel>();
        }

        public async virtual Task CreateAsync(TViewModel model)
        {
            var entity = model.ToEntity();
            await CreateAsync(entity);
        }

        public async virtual Task CreateAsync(TViewModel model, string keyName)
        {
            var entity = model.ToEntity();
            await CreateAsync(entity);

            var entityKeyProperty = entity.GetType().GetProperty(keyName);
            var modelKeyProperty = model.GetType().GetProperty(keyName);

            modelKeyProperty.SetValue(model, entityKeyProperty.GetValue(entity));
        }

        public virtual async Task ActivateAsync<TKey>(TKey keyValue)
        {
            var entity = Get(keyValue);
            await ActivateAsync(entity);
        }

        public Task SaveAsync()
        {
            return this.unitOfWork.CommitAsync();
        }

        public Task CreateAsync(TEntity entity)
        {
            this.OnCreate(entity);
            return this.SaveAsync();
        }

        public Task ActivateAsync(TEntity entity)
        {
            this.OnActivate(entity);
            return this.SaveAsync();
        }

        public Task DeactivateAsync(TEntity entity)
        {
            this.OnDeactivate(entity);
            return this.SaveAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            this.OnUpdate(entity);
            return this.SaveAsync();
        }

        public Task DeleteAsync(TEntity entity)
        {
            this.OnDelete(entity);
            return this.SaveAsync();
        }

        protected virtual void OnCreate(TEntity entity)
        {
            if (((object)entity) is IAuditable)
            {
                IAuditable obj = ((object)entity) as IAuditable;
                DateTime dateTime2 = obj.CreatedTime = (obj.UpdatedTime = DateTime.UtcNow);
            }
            if (((object)entity) is IActivable)
            {
                ((IActivable)(object)entity).Active = true;
            }
            this.repository.Add(entity);
        }

        protected virtual void OnUpdate(TEntity entity)
        {
            if (((object)entity) is IAuditable)
            {
                (((object)entity) as IAuditable).UpdatedTime = DateTime.UtcNow;
            }
            this.repository.Edit(entity);
        }

        protected virtual void OnActivate(TEntity entity)
        {
            this.repository.Activate(entity);
        }

        protected virtual void OnDeactivate(TEntity entity)
        {
            this.repository.Deactivate(entity);
        }

        protected virtual void OnDelete(TEntity entity)
        {
            this.repository.Delete(entity);
        }
    }
}
