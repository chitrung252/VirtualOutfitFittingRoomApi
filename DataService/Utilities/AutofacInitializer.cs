using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using DataService.AutofacModule;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace DataService.Utilities
{
    public static class AutofacInitializer
    {
        public static void Initialize(Assembly assembly, Type dbContextType, MapperConfiguration autoMapperConfig = default(MapperConfiguration), params Autofac.Module[] additionalModules)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            Autofac.Integration.Mvc.RegistrationExtensions.RegisterControllers(containerBuilder, Assembly.GetExecutingAssembly()).PropertiesAutowired();
            ModuleRegistrationExtensions.RegisterModule(containerBuilder, new RepositoryModule(assembly));
            ModuleRegistrationExtensions.RegisterModule(containerBuilder, new ServiceModule(assembly));
            ModuleRegistrationExtensions.RegisterModule(containerBuilder, new EntityFrameworkModule(assembly, dbContextType));

            if (additionalModules != null)
            {
                foreach (var module in additionalModules)
                {
                    ModuleRegistrationExtensions.RegisterModule(containerBuilder, module);
                }
            }
            if (autoMapperConfig == default(IMapperConfigurationExpression))
            {
                autoMapperConfig = new MapperConfiguration(cfg => { cfg.CreateMissingTypeMaps = true; });
                IMapper mapper = autoMapperConfig.CreateMapper();
            }
            ModuleRegistrationExtensions.RegisterModule(containerBuilder, new AutoMapperModule(autoMapperConfig));
            var reslover = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(reslover));
        }
    }
}
