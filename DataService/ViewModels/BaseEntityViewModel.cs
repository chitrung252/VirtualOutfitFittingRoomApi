using AutoMapper;
using DataService.BaseConnect;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModels
{
    public abstract class BaseEntityViewModel<T> where T : IEntity, new()
    {
        protected Type SelfType
        {
            get;
            set;
        }

        protected Type EntityType
        {
            get;
            set;
        }

        public BaseEntityViewModel()
        {
            this.SelfType = base.GetType();
            this.EntityType = typeof(T);
        }

        public BaseEntityViewModel(T entity)
            : this()
        {
            this.CopyFromEntity(entity);
        }

        public virtual void CopyFromEntity(T entity)
        {
            DependencyUtils.Resolve<IMapper>().Map((object)entity, (object)this, this.EntityType, this.SelfType);
        }

        public virtual void CopyToEntity(T entity)
        {
            DependencyUtils.Resolve<IMapper>().Map((object)this, (object)entity, this.SelfType, this.EntityType);
        }

        public T ToEntity()
        {

            T val = new T();
            this.CopyToEntity(val);
            return val;
        }
    }
}
