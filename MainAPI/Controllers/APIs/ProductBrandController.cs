using DataService.APIViewModels;
using DataService.Domain;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SkyConnect.API.Controllers.APIs
{
    public interface IProductBrandController
    {
        HttpResponseMessage GetProductBrands(ProductBrandRequest<string> request);
    }
    [RoutePrefix("api/product-brand")]
    public class ProductBrandController : ApiController, IProductBrandController
    {
        [Route("")]
        public HttpResponseMessage GetProductBrands(ProductBrandRequest<string> request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            BaseResponse<List<ProductBrandAPIViewModel>> response = new BaseResponse<List<ProductBrandAPIViewModel>>();
            try
            {
                // Domain
                var domain = new ProductBrandDomain();
                response = domain.GetProductBrand(request);
               
            }
            catch(ApiException e)
            {
                // catch ApiException
                responseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<List<ProductBrandAPIViewModel>>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch(Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<List<ProductBrandAPIViewModel>>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
            }

            //return 
            responseMessage.Content = new JsonContent(response);
            return responseMessage;
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreatProductBrand(ProductBrandAPIViewModel request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            BaseResponse<ProductBrandAPIViewModel> response = new BaseResponse<ProductBrandAPIViewModel>();
            try
            {
                // Domain
                var domain = new ProductBrandDomain();
                response = domain.CreateProductBrand(request);

            }
            catch (ApiException e)
            {
                // catch ApiException
                responseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<ProductBrandAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<ProductBrandAPIViewModel>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
            }

            //return 
            responseMessage.Content = new JsonContent(response);
            return responseMessage;
            throw new NotImplementedException();
        }
        [HttpPut]
        [Route("")]
        public HttpResponseMessage UpdateProductBrand(ProductBrandAPIViewModel request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            BaseResponse<ProductBrandAPIViewModel> response = new BaseResponse<ProductBrandAPIViewModel>();
            try
            {
                // Domain
                var domain = new ProductBrandDomain();
                response = domain.UpdateProductBrand(request);

            }
            catch (ApiException e)
            {
                // catch ApiException
                responseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<ProductBrandAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<ProductBrandAPIViewModel>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
            }

            //return 
            responseMessage.Content = new JsonContent(response);
            return responseMessage;
            throw new NotImplementedException();
        }
        
    }
}
