using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.Entities;
using DataService.Models.Entities.Services;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public  interface IProductBrandDomain
    {
        BaseResponse<List<ProductBrandAPIViewModel>> GetProductBrand(ProductBrandRequest<string> request);
        BaseResponse<ProductBrandAPIViewModel> CreateProductBrand(ProductBrandAPIViewModel productBrand);
        BaseResponse<ProductBrandAPIViewModel> UpdateProductBrand(ProductBrandAPIViewModel productBrand);
    }
    public class ProductBrandDomain : BaseDomain, IProductBrandDomain
    {
        public BaseResponse<ProductBrandAPIViewModel> CreateProductBrand(ProductBrandAPIViewModel productBrand)
        {
            var service = this.Service<IProductBrandService>();
            var productBrandViewModel = service.CreateProductBrand(productBrand);
            if (productBrandViewModel == null)
            {
                throw ApiException.Get(false, ConstantManager.PRODUCT_BRAND_CREATE_FAIL, ResultEnum.ProductBrandCreateFail, HttpStatusCode.BadRequest);

            }
            return BaseResponse<ProductBrandAPIViewModel>.Get(true, ConstantManager.MES_CREATE_PRODUCT_BRAND_SUCCESS, productBrandViewModel, ResultEnum.Success);

        }

        public BaseResponse<List<ProductBrandAPIViewModel>> GetProductBrand(ProductBrandRequest<string> request)
        {
            var service = this.Service<IProductBrandService>();
            var list = service.GetProductBrand(request);
            if(list == null)
            {
                throw ApiException.Get(false, ConstantManager.MES_PRODUCTBRAND_NOTFOUND, ResultEnum.ProductBrandNotFound, HttpStatusCode.BadRequest);
            }
            return BaseResponse<List<ProductBrandAPIViewModel>>.Get(true, ConstantManager.MES_PRODUCTBRAND_OK, list, ResultEnum.Success);
        }

        public BaseResponse<ProductBrandAPIViewModel> UpdateProductBrand(ProductBrandAPIViewModel productBrand)
        {
            var service = this.Service<IProductBrandService>();
            var productBrandViewModel = service.UpdateProductBrand(productBrand);
            if (productBrandViewModel == null)
            {
                throw ApiException.Get(false, ConstantManager.PRODUCT_BRAND_UPDATE_FAIL, ResultEnum.ProductBrandUpdateFail, HttpStatusCode.BadRequest);

            }
            return BaseResponse<ProductBrandAPIViewModel>.Get(true, ConstantManager.MES_UPDATE_PRODUCT_BRAND_SUCCESS, productBrandViewModel, ResultEnum.Success);
        }
    }
}
