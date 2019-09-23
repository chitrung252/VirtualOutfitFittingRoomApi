using Autofac;
using Autofac.Builder;
using DataService.BaseConnect;
using System;
using System.Data.Entity;
using System.Reflection;

namespace DataService.AutofacModule
{
    public sealed class EntityFrameworkModule : Autofac.Module
    {
        private Assembly assembly;

        private Type dbContextType;

        public EntityFrameworkModule(Assembly assembly, Type dbContextType)
        {
            this.assembly = assembly;
            this.dbContextType = dbContextType;
        }

        protected override void Load(ContainerBuilder builder)
        {
            ModuleRegistrationExtensions.RegisterModule(builder, new RepositoryModule(this.assembly));
            Autofac.RegistrationExtensions.InstancePerRequest<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(Autofac.RegistrationExtensions.RegisterType(builder, this.dbContextType).As(new Type[1]
            {
            typeof(DbContext)
            }), new object[0]);
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
        }
    }
}
