
using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.Entities.Services;
using DataService.Utilities;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public interface IProductCategoryDomain
    {
        ProductCategoryAPIViewModel GetCategoryByQueryParams(int catId, int brandId , int storeId , int saleMethod );
        List<ProductCategoryAPIViewModel> GetAllCategoryByQueryParams( int brandId, int storeId, int saleMethod);
        BaseResponse<ProductCategoryAPIViewModel> GetCategoryByExtraId(int extraID);
        List<ProductCategoryAPIViewModel> GetProductCategoriesByBrandId(int brand_id);
        BaseResponse<List<ProductCategoryAPIViewModel>> GetProductCategoriesByStoreId(int store_id);
        BaseResponse<List<ProductCategoryAPIViewModel>> GetProductCategoriesByRequest(CategoryRequest<string> request);
    }
    public class ProductCategoryDomain : BaseDomain, IProductCategoryDomain
    {
        public List<ProductCategoryAPIViewModel> GetAllCategoryByQueryParams(int brandId, int storeId, int saleMethod)
        {
            var categoryService = this.Service<IProductCategoryService>();
            return categoryService.GetAllCategoryByQueryParams(brandId, storeId, saleMethod);
        }

        public BaseResponse<ProductCategoryAPIViewModel> GetCategoryByExtraId(int extraID)
        {
            try
            {
                var categoryService = this.Service<IProductCategoryService>();
                var cate =  categoryService.GetCategoryById(extraID);
                if (cate == null)
                {
                    throw ApiException.Get(true, ConstantManager.MES_PRODUCTCATEGORY_NOT_FOUND, ResultEnum.RatingProductNotFound, HttpStatusCode.OK);
                }
              return  BaseResponse<ProductCategoryAPIViewModel>.Get(true, ConstantManager.MES_SUCCESS, cate, ResultEnum.Success);
            }
            catch (Exception ex)
            {
                if(ex is ApiException)
                {
                    throw ex;
                }
                else
                {
                    throw ApiException.Get(false, ex.ToString(), ResultEnum.InternalError, HttpStatusCode.InternalServerError);
                }
            }
           
        }

        //public IEnumerable<ProductCategoryApiViewModel> GetProductCategoryAPIByStoreId(int brandId)
        //{
        //    var productCategoryService = this.Service<IProductCategoryService>();
        //    return productCategoryService.GetProductCategoryAPIByParams(brandId);
        //}
        public ProductCategoryAPIViewModel GetCategoryByQueryParams(int catId, int brandId, int storeId, int saleMethod)
        {
            var categoryService = this.Service<IProductCategoryService>();
            return categoryService.GetCategoryByQueryParams(catId,brandId,storeId,saleMethod);
        }

        public List<ProductCategoryAPIViewModel> GetProductCategoriesByBrandId(int brand_id)
        {
            var categoryService = this.Service<IProductCategoryService>();
            return categoryService.GetProductCategoriesByBrandId(brand_id);
        }

        

        public BaseResponse<List<ProductCategoryAPIViewModel>> GetProductCategoriesByStoreId(int store_id)
        {
            var categoryService = this.Service<IProductCategoryService>();
            try
            {
                var cateVM = categoryService.GetProductCategoriesByStoreId(store_id);
                if (cateVM == null)
                {
                    throw ApiException.Get(true, ConstantManager.MES_PRODUCTCATEGORY_NOT_FOUND, ResultEnum.ProductCategoryNotFound, HttpStatusCode.OK);
                }
                return BaseResponse<List<ProductCategoryAPIViewModel>>.Get(true, ConstantManager.MES_SUCCESS, cateVM, ResultEnum.Success);
            }
            catch (Exception ex)
            {
                if(ex is ApiException)
                {
                    throw ex;
                }
                throw ApiException.Get(false, ex.ToString(), ResultEnum.InternalError, HttpStatusCode.InternalServerError);
            }
            
        }
        public BaseResponse<List<ProductCategoryAPIViewModel>> GetProductCategoriesByRequest(CategoryRequest<string> request)
        {
            var categoryService = this.Service<IProductCategoryService>();
            try
            {
                if (request.StoreId == null)
                {
                    throw ApiException.Get(false, "StoreId is required!", ResultEnum.StoreIdNotFound, HttpStatusCode.BadRequest);
                }
                var cateVM = categoryService.GetProductCategoriesByRequest(request);
                if (cateVM == null)
                {
                    throw ApiException.Get(true, ConstantManager.MES_PRODUCTCATEGORY_NOT_FOUND, ResultEnum.ProductCategoryNotFound, HttpStatusCode.OK);
                }
                return BaseResponse<List<ProductCategoryAPIViewModel>>.Get(true, ConstantManager.MES_SUCCESS, cateVM, ResultEnum.Success);
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    throw ex;
                }
                throw ApiException.Get(false, ex.ToString(), ResultEnum.InternalError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
