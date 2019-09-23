using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using System;
using System.Reflection;

namespace DataService.AutofacModule
{

    public sealed class RepositoryModule: Autofac.Module
    {
        private Assembly assembly;

        public RepositoryModule(Assembly assembly)
        {
            this.assembly = assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.assembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces();
        }
    }

}
