using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataService.Models.Entities;
using DataService.Models.Entities.Services;
using DataService.ViewModels;
using Autofac;
using DataService.Utilities;
using System.Reflection;
//using DataService.APIViewModels;

namespace DataService
{
    public static class ApiEndpoint
    {
        public static void Entry(Action<IMapperConfigurationExpression> additionalMapperConfig, params Autofac.Module[] additionalModules)
        {
            Action<IMapperConfigurationExpression> fullMapperConfig = q =>
            {
                ApiEndpoint.ConfigAutoMapper(q);

                if (additionalMapperConfig != null)
                {
                    additionalMapperConfig(q);
                }
            };

            AutofacInitializer.Initialize(
                Assembly.GetExecutingAssembly(),
                typeof(DataEntities),
                new MapperConfiguration(fullMapperConfig),
                additionalModules);
        }
        private static void ConfigAutoMapper(IMapperConfigurationExpression config)
        {
            config.CreateMissingTypeMaps = true;
            config.AllowNullDestinationValues = false;
            //config.CreateMap<Order, OrderAPIViewModel>();
            //config.CreateMap<OrderAPIViewModel, Order>();

            //config.CreateMap<OrderDetail, OrderDetailAPIViewModel>();
            //config.CreateMap<OrderDetailAPIViewModel, OrderDetail>();

            //config.CreateMap<OrderPromotionMapping, OrderPromotionMappingAPIViewModel>();
            //config.CreateMap<OrderPromotionMappingAPIViewModel, OrderPromotionMapping>();

            //config.CreateMap<Payment, PaymentAPIViewModel>();
            //config.CreateMap<PaymentAPIViewModel, Payment>();

            //config.CreateMap<OrderDetailPromotionMapping, OrderDetailPromotionMappingAPIViewModel>();
            //config.CreateMap<OrderDetailPromotionMappingAPIViewModel, OrderDetailPromotionMapping>();
        }
    }
}
