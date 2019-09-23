using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
//using DataService.APIViewModels;
using DataService.Models.Entities;
using DataService.ViewModels;

namespace SkyConnect.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            DataService.ApiEndpoint.Entry(this.AdditionalMapperConfig);
        }

        private void AdditionalMapperConfig(IMapperConfigurationExpression obj)
        {
            obj.CreateMap<Product, ProductAPIViewModel>().ReverseMap();
            //obj.CreateMap<Card, CardAPIViewModel>();
            //obj.CreateMap<CardAPIViewModel, Card>();
            //obj.CreateMap<Membership, MembershipAPIViewModel>();
            //obj.CreateMap<MembershipAPIViewModel, Membership>();
            //obj.CreateMap<MembershipType, MembershipTypeAPIViewModel>();
            //obj.CreateMap<Account, AccountAPIViewModel>();
            //obj.CreateMap<AccountAPIViewModel, Account>();
            //obj.CreateMap<Customer, CustomerAPIViewModel>();
            //obj.CreateMap<CustomerAPIViewModel, Customer>();
            //obj.CreateMap<Product, ProductAPIViewModel>();
            //obj.CreateMap<ProductCategory, ProductCategoryAPIViewModel>();
            //obj.CreateMap<CategoryExtraMapping, CategoryExtraMappingAPIViewModel>();
            //obj.CreateMap<DeliveryAPIViewModel, DeliveryInformation>();
            //obj.CreateMap<DeliveryInformation, DeliveryAPIViewModel>();
            //obj.CreateMap<CategoryExtraMapping, CategoryExtraMappingAPIViewModel>();
            //// new
            //obj.CreateMap<ProductBrandAPIViewModel, ProductBrand>();
            //obj.CreateMap<ProductBrand, ProductBrandAPIViewModel>();
            //obj.CreateMap<RatingAPIViewModel, Rating>();
            //obj.CreateMap<Rating, RatingAPIViewModel>();
            //obj.CreateMap<Promotion, PromotionAPIViewModel>().ReverseMap();
            //obj.CreateMap<Voucher, VoucherAPIViewModel>()
            //    .ForMember(v => v.PromotionCode, opt =>
            //    {
            //        opt.MapFrom(v => v.Promotion.PromotionCode);
            //    }).ReverseMap();
            //obj.CreateMap<VoucherAPIViewModel, Voucher>();
            //obj.CreateMap<OrderDetail, OrderDetailsHistoryAPIViewModel>();
            //obj.CreateMap<OrderDetailsHistoryAPIViewModel, OrderDetail>();
            //obj.CreateMap<Order, OrderHistoryAPIViewModel>();
            //obj.CreateMap<OrderHistoryAPIViewModel, Order>();
            //obj.CreateMap<ProductDetailMapping, ProductDetailMappingAPIViewModel>();
            //obj.CreateMap<ProductDetailMappingAPIViewModel, ProductDetailMapping>();
            //obj.CreateMap<ProvinceAPIViewModel, Province>();
            //obj.CreateMap<Province, ProvinceAPIViewModel>();
            //obj.CreateMap<District, DistrictAPIViewModel>();
            //obj.CreateMap<DistrictAPIViewModel, District>();
            //obj.CreateMap<EmployeeAPIViewModel, Employee>();
            //obj.CreateMap<Employee, EmployeeAPIViewModel>();
        }
    }
}
