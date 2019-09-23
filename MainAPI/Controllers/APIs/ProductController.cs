using DataService.Domain;
using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
//using HmsService.Sdk;
//using HmsService.ViewModels;
using SkyConnect.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataService.Utilities;
using Newtonsoft.Json;

namespace SkyConnect.API.Controllers.APIs
{
    public interface IProductController
    {
        HttpResponseMessage GetProducts(ProductRequest<string> request);
        HttpResponseMessage GetCountProduct(ProductRequest<string> request);
        HttpResponseMessage GetProductById(int product_id, ProductRequest<string> request);
        HttpResponseMessage CreateProduct(ProductRequest<ProductAPIViewModel> request);
        HttpResponseMessage UpdateProduct(int product_id, ProductRequest<ProductAPIViewModel> request);
    }
    //[Authorize]
    [RoutePrefix("api/product")]
    public class ProductController : ApiController, IProductController
    {
        // GET: ProductApi
        [Route("get-products/{terminalId}/{saleMethodEnum}")]
        public HttpResponseMessage GetProductList(int terminalId, int saleMethodEnum)
        {
            
            var productList = new List<ProductAPIViewModel>();
            try
            {
                #region old
                //var productApi = new ProductApi();
                //var storeApi = new StoreApi();
                //var productDetailMappingApi = new ProductDetailMappingApi();
                //var store = storeApi.GetStoreByIdSync(terminalId);
                //var productDetailApi = new ProductDetailMappingApi();

                //var listP = productDetailMappingApi
                //    .GetProductAPIByStoreID(terminalId, store.BrandId.Value)
                //    .Where(p => (saleMethodEnum == 0 || (p.Product.SaleMethodEnum.Value & saleMethodEnum) != 0)).Take(10);

                //foreach (var item in listP)
                //{
                //    if (item.Product != null)
                //    {
                //        var p = new ProductAPIViewModel();
                //        p = item.Product;

                //var p = new ProductApiViewModel()
                //{
                //    ProductId = item.Product.ProductID,
                //    ProductName = item.Product.ProductName,
                //    ShortName = null,
                //    Code = item.Product.Code,
                //    PicURL = item.Product.PicURL,
                //    Price = item.Product.Price,
                //    DiscountPercent = item.Product.DiscountPercent,
                //    DiscountPrice = item.Product.DiscountPrice,
                //    CatID = item.Product.CatID,
                //    ProductType = item.Product.ProductType,
                //    DisplayOrder = item.Product.DisplayOrder,
                //    IsMenuDisplay = true,
                //    IsAvailable = item.Product.IsAvailable,
                //    PosX = (int)item.Product.PosX.GetValueOrDefault(),
                //    PosY = (int)item.Product.PosY.GetValueOrDefault(),
                //    ColorGroup = item.Product.ColorGroup,
                //    Group = (int)item.Product.Group.GetValueOrDefault(),
                //    GeneralProductId = item.Product.GeneralProductId,
                //    Att1 = item.Product.Att1,
                //    Att2 = item.Product.Att2,
                //    Att3 = item.Product.Att3,
                //    MaxExtra = 0,
                //    IsUsed = true,
                //    HasExtra = false,
                //    IsFixedPrice = item.Product.IsFixedPrice,
                //    IsDefaultChildProduct = item.Product.IsDefaultChildProduct == (int)SaleTypeEnum.DefaultNothing ? false : true,
                //    IsMostOrder = item.Product.IsMostOrdered,
                //    Category = new ProductCategoryApiViewModel()
                //    {
                //        Code = item.Product.CatID,
                //        Name = item.Product.ProductCategory.CateName,
                //        Type = item.Product.ProductCategory.Type,
                //        DisplayOrder = item.Product.ProductCategory.DisplayOrder,
                //        IsExtra = item.Product.ProductCategory.IsExtra ? 1 : 0,
                //        IsDisplayed = item.Product.ProductCategory.IsDisplayed,
                //        IsUsed = true,
                //        AdjustmentNote = item.Product.ProductCategory.AdjustmentNote,
                //        ImageFontAwsomeCss = item.Product.ProductCategory.ImageFontAwsomeCss,
                //        ParentCateId = item.Product.ProductCategory.ParentCateId
                //    }
                //};
                //if (p.IsFixedPrice == false)
                //{
                //    var priceProduct = productDetailApi.GetPriceByStore(terminalId, item.Product.ProductID);
                //    if (priceProduct != 0)
                //    {
                //        p.Price = priceProduct;
                //    }
                //    else
                //    {
                //        p.Price = item.Product.Price;
                //    }

                //    var discountProduct = productDetailApi.GetDiscountByStore(terminalId, item.Product.ProductID);
                //    if (priceProduct != 0)
                //    {
                //        p.DiscountPercent = discountProduct;
                //    }
                //    else
                //    {
                //        p.DiscountPercent = item.Product.DiscountPercent;
                //    }

                //}
                //productList.Add(p);
                //    }
                //}
                #endregion
                // Call ProductDomail to get List
                ProductDomain domain = new ProductDomain();
                productList = domain.GetProductListByTerminalIdAndSaleMethod(terminalId, saleMethodEnum);

            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new JsonContent(BaseResponse<string>.GetInternalServerError(e, "POS-ProductAPI-Error: "))
                };
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new BaseResponse<List<ProductAPIViewModel>>()
                {
                    ResultCode = (int)DataService.Models.ResultEnum.Success,
                    Message = "Lấy sản phẩm thành công",
                    Success = true,
                    Data = productList,

                })
            };
        }

        // GET: ProductApi
        [Route("get-product-detail/{store_id}/{product_id}")]
        public HttpResponseMessage GetProductDetail(int store_id, int product_id)
        {
            ProductAPIViewModel product;
            ProductDomain domain = new ProductDomain();
            try
            {
                // Call ProductDomail to get product detail
                product = domain.GetProductFromProductDetailMapping(store_id, product_id);
                if (product == null)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Content = new JsonContent(new BaseResponse<string>()
                        {
                            ResultCode = (int)DataService.Models.ResultEnum.ProductDetailNotFound,
                            Message = "Không có sản phẩm",
                            Success = false

                        })
                    };
                }
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new JsonContent(BaseResponse<string>.GetInternalServerError(e, "POS-ProductAPI-Error: "))
                };
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new BaseResponse<ProductAPIViewModel>()
                {
                    ResultCode = (int)DataService.Models.ResultEnum.Success,
                    Message = "Lấy sản phẩm thành công",
                    Success = true,
                    Data = product,

                })
            };
        }
        [Route("")]
        public HttpResponseMessage GetProducts(ProductRequest<string> request)
        {

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            BaseResponse<List<ProductAPIViewModel>> response = new BaseResponse<List<ProductAPIViewModel>>();
            try
            {
                
                ProductDomain productDomain = new ProductDomain();
                var listProduct = new List<ProductAPIViewModel>();
                response = productDomain.GetProductByRequest(request);
                
            }
            catch (ApiException e)
            {
                httpResponseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<List<ProductAPIViewModel>>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<List<ProductAPIViewModel>>.Get(false, "Product-Error: " + ex.ToString(), null, ResultEnum.InternalError);
            }
            httpResponseMessage.Content = new JsonContent(response);
            return httpResponseMessage;
        }


        
        [Route("count")]
        public HttpResponseMessage GetCountProduct([FromUri]ProductRequest<string> request)
        {
            ProductDomain productDomain = new ProductDomain();
            var count = productDomain.getCount(request);
            if (count > 0)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new BaseResponse<string>()
                    {
                        ResultCode = (int)DataService.Models.ResultEnum.Success,
                        Success = true,
                        Message = ConstantManager.MES_SUCCESS,
                        Data = count.ToString()
                    })
                };
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new JsonContent(new BaseResponse<string>()
                {
                    ResultCode = (int)DataService.Models.ResultEnum.ProductNotFound,
                    Success = true,
                    Message = ConstantManager.MES_PRODUCT_NOTFOUND
                })
            };
        }
        [Route("{product_id}")]
        public HttpResponseMessage GetProductById(int product_id, [FromUri]ProductRequest<string> request)
        {
            ProductDomain productDomain = new ProductDomain();
            var product = productDomain.GetProductById(product_id);
            if (product != null)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new BaseResponse<ProductAPIViewModel>()
                    {
                        ResultCode = (int)DataService.Models.ResultEnum.Success,
                        Success = true,
                        Message = ConstantManager.MES_SUCCESS,
                        Data = product
                    })
                };
            }
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new JsonContent(new BaseResponse<string>()
                {
                    ResultCode = (int)DataService.Models.ResultEnum.ProductNotFound,
                    Success = true,
                    Message = ConstantManager.MES_PRODUCT_NOTFOUND
                })
            };

        }
        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreateProduct(ProductRequest<ProductAPIViewModel> request)
        {
            ProductDomain productDomain = new ProductDomain();
            var product = productDomain.CreateProduct(request.Data);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new BaseResponse<ProductAPIViewModel>()
                {
                    ResultCode = (int)DataService.Models.ResultEnum.Success,
                    Success = true,
                    Message = ConstantManager.MES_SUCCESS,
                    Data = product
                })
            };
        }
        [HttpPut]
        [Route("{product_id}")]
        public HttpResponseMessage UpdateProduct([FromUri]int product_id, [FromBody] ProductRequest<ProductAPIViewModel> request)
        {
            ProductDomain productDomain = new ProductDomain();
            var product = productDomain.UpdateProduct(request.Data);
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new BaseResponse<ProductAPIViewModel>()
                {
                    ResultCode = (int)DataService.Models.ResultEnum.Success,
                    Success = true,
                    Message = ConstantManager.MES_SUCCESS,
                    Data = product
                })
            };
        }
    }
}
