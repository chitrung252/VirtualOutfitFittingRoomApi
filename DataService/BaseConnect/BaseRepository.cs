using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataService.BaseConnect
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>, IRepository where TEntity : class, IEntity
    {
        protected DbContext dbContext;

        protected DbSet dbSet;

        public BaseRepository(DbContext context)
        {
            this.dbContext = context;
            this.dbSet = this.dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get()
        {
            return Queryable.AsQueryable<TEntity>((IEnumerable<TEntity>)this.dbSet);
        }

        public virtual TEntity Get<TKey>(TKey id)
        {
            return (TEntity)this.dbSet.Find(new object[1] { id });
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Queryable.Where<TEntity>((IQueryable<TEntity>)this.dbSet, predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetActive()
        {
            if (typeof(IActivable).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> node = (TEntity q) => ((IActivable)q).Active;
                node = (Expression<Func<TEntity, bool>>)RemoveCastsVisitor.Visit(node);
                return Queryable.Where<TEntity>(this.Get(), node);
            }
            return this.Get();
        }

        public virtual IQueryable<TEntity> GetActive(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(IActivable).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> node = (TEntity q) => ((IActivable)q).Active;
                node = (Expression<Func<TEntity, bool>>)RemoveCastsVisitor.Visit(node);
                return Queryable.Where<TEntity>(Queryable.Where<TEntity>(this.Get(), node), predicate);
            }
            return this.Get(predicate);
        }

        public virtual TEntity FirstOrDefault()
        {
            return Queryable.FirstOrDefault<TEntity>((IQueryable<TEntity>)this.dbSet);
        }

        public virtual TEntity FirstOrDefaultActive()
        {
            if (typeof(IActivable).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> node = (TEntity q) => ((IActivable)q).Active;
                node = (Expression<Func<TEntity, bool>>)RemoveCastsVisitor.Visit(node);
                return Queryable.FirstOrDefault<TEntity>((IQueryable<TEntity>)this.dbSet, node);
            }
            return this.FirstOrDefault();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Queryable.FirstOrDefault<TEntity>((IQueryable<TEntity>)this.dbSet, predicate);
        }

        public virtual TEntity FirstOrDefaultActive(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(IActivable).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> node = (TEntity q) => ((IActivable)q).Active;
                node = (Expression<Func<TEntity, bool>>)RemoveCastsVisitor.Visit(node);
                return Queryable.FirstOrDefault<TEntity>(Queryable.Where<TEntity>((IQueryable<TEntity>)this.dbSet, predicate), node);
            }
            return this.FirstOrDefault(predicate);
        }

        public virtual void Add(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        public virtual void Edit(TEntity entity)
        {
            #region old
            var type = entity.GetType();
            object key;
            if (type.Name.Equals("Order"))
            {
                key = type.GetProperties().FirstOrDefault(p =>
               p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase)
               || p.Name.Equals("RentID", StringComparison.OrdinalIgnoreCase)).GetValue(entity, null);
            }
            else
            {
                key = type.GetProperties().FirstOrDefault(p =>
               p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase)
               || p.Name.Equals(type.Name + "ID", StringComparison.OrdinalIgnoreCase)).GetValue(entity, null);
            }

            var entityDb = this.dbContext.Set<TEntity>().Find(key);

            var listTypeE = entityDb.GetType().GetProperties().AsEnumerable();
            var listP = type.GetProperties();
            // Update special feild which modify
            foreach (PropertyInfo item in listP)
            {
                var value = item.GetValue(entity, null);
                //var check = (item.PropertyType != typeof(string)&&typeof(IEnumerable).IsAssignableFrom(item.PropertyType))||item.PropertyType.FullName.Contains("DataService") ;
                var check = item.PropertyType.FullName.Contains("Entities");
                //var check4 = item.PropertyType.GetInterfaces().Contains(typeof(IEnumerable));
                if (value != null && !value.ToString().Equals("") && listTypeE.Where(e => e.Name == item.Name) != null && check == false)
                {
                    this.dbContext.Entry(entityDb).Property(item.Name.ToString()).CurrentValue = value;
                }

            }
            // Update All feilds
            //this.dbContext.Entry(entityDb).CurrentValues.SetValues(entity);
            #endregion

            //this.dbContext.Entry(entity).State = EntityState.Modified;

        }

        public virtual void Activate(TEntity entity)
        {
            if (((object)entity) is IActivable)
            {
                ((IActivable)(object)entity).Active = true;
                return;
            }
            throw new NotSupportedException("TEntity must implement IActivable to use this method. TEntity: " + typeof(TEntity).FullName);
        }

        public virtual void Deactivate(TEntity entity)
        {
            if (((object)entity) is IActivable)
            {
                ((IActivable)(object)entity).Active = false;
                return;
            }
            throw new NotSupportedException("TEntity must implement IActivable to use this method. TEntity: " + typeof(TEntity).FullName);
        }

        public virtual void Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            this.dbContext.SaveChanges();
        }

        public virtual void Refresh(TEntity entity)
        {
            this.dbContext.Entry<TEntity>(entity).Reload();
        }

        public Task<TEntity> GetAsync<TKey>(TKey id)
        {
            return dbSet.Cast<TEntity>().FindAsync(id);
        }

        public Task<TEntity> FirstOrDefaultAsync()
        {
            return QueryableExtensions.FirstOrDefaultAsync<TEntity>((IQueryable<TEntity>)this.dbSet);
        }

        public Task<TEntity> FirstOrDefaultActiveAsync()
        {
            if (typeof(IActivable).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> node = (TEntity q) => ((IActivable)q).Active;
                node = (Expression<Func<TEntity, bool>>)RemoveCastsVisitor.Visit(node);
                return QueryableExtensions.FirstOrDefaultAsync<TEntity>((IQueryable<TEntity>)this.dbSet, node);
            }
            return this.FirstOrDefaultAsync();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return QueryableExtensions.FirstOrDefaultAsync<TEntity>((IQueryable<TEntity>)this.dbSet, predicate);
        }

        public Task<TEntity> FirstOrDefaultActiveAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(IActivable).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> node = (TEntity q) => ((IActivable)q).Active;
                node = (Expression<Func<TEntity, bool>>)RemoveCastsVisitor.Visit(node);
                return QueryableExtensions.FirstOrDefaultAsync<TEntity>(Queryable.Where<TEntity>((IQueryable<TEntity>)this.dbSet, predicate), node);
            }
            return this.FirstOrDefaultAsync(predicate);
        }

        public Task SaveAsync()
        {
            return this.SaveAsync();
        }
    }
}
