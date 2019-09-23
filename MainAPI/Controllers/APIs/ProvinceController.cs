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
    public interface IProvinceController
    {
        HttpResponseMessage GetProvince();
    }
    [RoutePrefix("api/province")]
    public class ProvinceController : ApiController, IProvinceController
    {
        [Route("")]
        public HttpResponseMessage GetProvince()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            BaseResponse<List<ProvinceAPIViewModel>> response = new BaseResponse<List<ProvinceAPIViewModel>>();
            try
            {
                ProvinceDomain provinceDomain = new ProvinceDomain();
                response = provinceDomain.GetProvince();
            }
            catch (ApiException e)
            {
                httpResponseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<List<ProvinceAPIViewModel>>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception ex)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<List<ProvinceAPIViewModel>>.Get(false, ex.ToString(), null, ResultEnum.InternalError);
                throw;
            }
            httpResponseMessage.Content = new JsonContent(response);
            return httpResponseMessage;
        }
    }
}
