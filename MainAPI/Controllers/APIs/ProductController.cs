using DataService.Domain;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SkyConnect.API.Controllers.APIs
{

    public partial interface IProductController
    {

        HttpResponseMessage GetListProduct(int? id);
        HttpResponseMessage ChangeProductActiveState(int ProductId, bool active);
        HttpResponseMessage CreateProduct(ProductViewModel model);
        HttpResponseMessage UpdateProduct(ProductViewModel model);
    }

    [RoutePrefix("api/product")]
    public class ProductController : ApiController, IProductController
    {
        public HttpResponseMessage ChangeProductActiveState(int ProductId, bool active)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage CreateProduct(ProductViewModel model)
        {
            throw new NotImplementedException();
        }
        [Route]
        [HttpGet]
        public HttpResponseMessage GetListProduct(int? id = null)
        {
            var pDomain = new ProductDomain();
            var product = pDomain.GetList(id);
            var res = new BaseResponse<List<ProductAPIViewModel>>();

            if (product != null)
            {
                res = new BaseResponse<List<ProductAPIViewModel>>()
                {
                    Data = product,
                    Message = "Success",
                    Success = true,
                    ResultCode = (int)ResultEnum.Success
                };
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(res),
                    StatusCode = HttpStatusCode.OK
                };
            }
            res = new BaseResponse<List<ProductAPIViewModel>>()
            {
                Data = null,
                Message = "Failed",
                Success = false,
                ResultCode = (int)ResultEnum.Fail
            };
            return new HttpResponseMessage()
            {
                Content = new JsonContent(res),
                StatusCode = HttpStatusCode.NoContent
            };
        }
        
        public HttpResponseMessage UpdateProduct(ProductViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
