using Autofac;
using AutoMapper;

namespace DataService.AutofacModule
{
    public class AutoMapperModule : Autofac.Module
    {
        private MapperConfiguration Config
        {
            get;
            set;
        }

        public AutoMapperModule(MapperConfiguration config)
        {
            this.Config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            IMapper val = this.Config.CreateMapper();
            RegistrationExtensions.RegisterInstance<MapperConfiguration>(builder, this.Config).As<IConfigurationProvider>().SingleInstance();
            RegistrationExtensions.RegisterInstance<IMapper>(builder, val).As<IMapper>().SingleInstance();
        }
    }
}
