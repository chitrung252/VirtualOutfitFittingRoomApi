using DataService.APIViewModels;
using DataService.Domain;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Utilities;
using SkyConnect.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace SkyConnect.API.Controllers.APIs
{
    [RoutePrefix("api/category")]
    public class ProductCategoryController : ApiController
    {
        #region old
        //[Route("get-categories/{storeId}/{SaleMethod}")]
        //public HttpResponseMessage GetCategoryList(int storeId, int SaleMethod, string catId)
        //{
        //    ProductCategoryDomain categoryDomain = new ProductCategoryDomain();
        //    List<ProductCategoryAPIViewModel> listCat = new List<ProductCategoryAPIViewModel>();
        //    StoreDomain storeDomain = new StoreDomain();
        //    var brandId = storeDomain.GetStoreByStoreId(storeId).BrandId.Value;
        //    var check = Regex.IsMatch(catId, "\\d");
        //    if (check)
        //    {
        //        string[] catList = catId.Split(',');
        //        //string test = " storeId: " + storeId + " SaleMethod: " + SaleMethod
        //        foreach (string item in catList)
        //        {
        //            var catID = Int32.Parse(item);

        //            var listCategory = categoryDomain.GetCategoryByQueryParams(catID, brandId, storeId, SaleMethod);
        //            if (listCategory != null)
        //            {
        //                listCat.Add(listCategory);
        //            }
        //            else
        //            {
        //                return new HttpResponseMessage()
        //                {
        //                    StatusCode = HttpStatusCode.OK,
        //                    Content = new JsonContent(new BaseResponse<List<ProductCategoryAPIViewModel>>()
        //                    {
        //                        ResultCode = (int)DataService.Models.ResultEnum.ProductCategoryNotFound,
        //                        Message = "Không tìm thấy sản phẩm",
        //                        Success = false

        //                    })
        //                };
        //            }
        //        }

        //    }
        //    else
        //    {
        //        return new HttpResponseMessage()
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new JsonContent(new BaseResponse<string>()
        //            {
        //                ResultCode = (int)DataService.Models.ResultEnum.InternalError,
        //                Message = "CatId is Wrong",
        //                Success = false,

        //            })
        //        };

        //    }
        //    return new HttpResponseMessage()
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Content = new JsonContent(new BaseResponse<List<ProductCategoryAPIViewModel>>()
        //        {
        //            ResultCode = (int)DataService.Models.ResultEnum.Success,
        //            Message = "Lấy sản phẩm thành công",
        //            Success = true,
        //            Data = listCat,

        //        })
        //    };


        //}
        //[Route("get-categories/{storeId}/{SaleMethod}")]
        //public HttpResponseMessage GetAllCategoryList(int storeId, int SaleMethod)
        //{
        //    try
        //    {
        //        ProductCategoryDomain categoryDomain = new ProductCategoryDomain();
        //        List<ProductCategoryAPIViewModel> listCat = new List<ProductCategoryAPIViewModel>();
        //        StoreDomain storeDomain = new StoreDomain();
        //        var brandId = storeDomain.GetStoreByStoreId(storeId).BrandId.Value;
        //        listCat = categoryDomain.GetAllCategoryByQueryParams(brandId, storeId, SaleMethod);
        //        return new HttpResponseMessage()
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new JsonContent(new BaseResponse<List<ProductCategoryAPIViewModel>>()
        //            {
        //                ResultCode = (int)DataService.Models.ResultEnum.Success,
        //                Message = "Lấy sản phẩm thành công",
        //                Success = true,
        //                Data = listCat,

        //            })
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpResponseMessage()
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new JsonContent(new BaseResponse<string>()
        //            {
        //                ResultCode = (int)DataService.Models.ResultEnum.InternalError,
        //                Message = "ProductCategoryError: " + ex,
        //                Success = false
        //            })
        //        };
        //    }

        //}
        #endregion

        //[Route("get-category-extra/{catId}")]
        [Route("get-category-extra")]
        public HttpResponseMessage GetListCategoryExtra(int catId,int storeId)
        {
            CategoryExtraMappingDomain categoryExtraMappingDomain = new CategoryExtraMappingDomain();
            ProductCategoryDomain categoryDomain = new ProductCategoryDomain();
            ProductCategoryAPIViewModel Cat = new ProductCategoryAPIViewModel();
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            BaseResponse<ProductCategoryAPIViewModel> response = new BaseResponse<ProductCategoryAPIViewModel>();
            try
            {
                // get Extra mapping by PrimaryCategoryId 
                var extra = categoryExtraMappingDomain.GetCategoryExtraMapping(catId,storeId);
                // get Category by ExtraCategoryId
                response =  categoryDomain.GetCategoryByExtraId(extra.Data.ExtraCategoryId,storeId);
            }
            catch(ApiException e)
            {
                responseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<ProductCategoryAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<ProductCategoryAPIViewModel>.Get(false, "ProductCategoryError : " + ex.ToString(), null, ResultEnum.InternalError);
            }
            responseMessage.Content = new JsonContent(response);
            return responseMessage;
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetProductCategoriesByStoreId(CategoryRequest<string> request)
        {
            List<ProductCategoryAPIViewModel> productCategories = new List<ProductCategoryAPIViewModel>();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            BaseResponse<List<ProductCategoryAPIViewModel>> response = new BaseResponse<List<ProductCategoryAPIViewModel>>();
            try
            {
                ProductCategoryDomain domain = new ProductCategoryDomain();
                response = domain.GetProductCategoriesByRequest(request);
                
            }
            catch(ApiException e)
            {
                httpResponseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<List<ProductCategoryAPIViewModel>>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<List<ProductCategoryAPIViewModel>>.Get(false, "ProductCategoryException: " + ex.ToString(), null, ResultEnum.InternalError);
            }
            httpResponseMessage.Content = new JsonContent(response);
            return httpResponseMessage;
          
        }
    }
}
