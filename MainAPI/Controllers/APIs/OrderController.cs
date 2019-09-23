using DataService.APIViewModels;
using DataService.Domain;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Utilities;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyConnect.API.Identities;
using SkyConnect.API.Models;
using SkyConnect.API.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SkyConnect.API.Controllers.APIs
{
    [Authorize]
    [RoutePrefix("api/order")]
    [MobileAuthentication]
    public class OrderController : ApiController
    {
        private DataService.Models.Identities.ApplicationUserManager _userManager;
        public DataService.Models.Identities.ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<DataService.Models.Identities.ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [HttpPost]
        [Route("calculate")]
        public HttpResponseMessage CalculateFeeDelivery(OrderAPIViewModel order)
        {
            var customerDomain = new CustomerDomain();
            BaseResponse<OrderAPIViewModel> response = new BaseResponse<OrderAPIViewModel>();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            //var customerId = customerDomain.getCustomerIdFromToken(RequestContext);
            //if (customerId != 0)
            //{
            //    order.CustomerID = customerId;
            //}
            IOrderDomain domain = new OrderDomain();
            try
            {
                var customerID = customerDomain.getCustomerIdFromToken(RequestContext);
                if(customerID != null)
                {
                    order.CustomerID = customerID;
                }
                response.Data = domain.GetDeliveryFee(order);
                response.Message = ConstantManager.MES_SUCCESS;
                response.ResultCode = (int)ResultEnum.Success;
                response.Success = true;
            }
            catch (DataService.Utilities.ApiException e)
            {
                httpResponseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<OrderAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception e)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<OrderAPIViewModel>.Get(false, e.ToString(), null, ResultEnum.InternalError);
            }
            httpResponseMessage.Content = new JsonContent(response);
            return httpResponseMessage;
        }
        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreateOrder(OrderAPIViewModel order)
        {
            var customerDomain = new CustomerDomain();
            BaseResponse<OrderHistoryAPIViewModel> response = new BaseResponse<OrderHistoryAPIViewModel>();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            #region get CustomerId and EmployeeId from Token
            var customerId = customerDomain.getCustomerIdFromToken(RequestContext);
            if (customerId != null)
            {
                order.CustomerID = customerId;
            }
            var claimPrincipal = (ClaimsPrincipal)RequestContext.Principal;
            var employeeId = claimPrincipal.Claims.Where(c => c.Type == "EmployeeId").Select(c => c.Value).SingleOrDefault();
            var userName = claimPrincipal.Claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            Int32 employeeID = 0;
            Int32.TryParse(employeeId, out employeeID);
            if(employeeID != 0)
            {
                order.EmployeeId = employeeID;
                order.CheckInPerson = userName;
            }
            #endregion

            IOrderDomain domain = new OrderDomain();
            try
            {
                
                response = domain.AddOrderFromMobile(order);
            }
            catch (DataService.Utilities.ApiException e)
            {
                httpResponseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<OrderHistoryAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception e)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<OrderHistoryAPIViewModel>.Get(false, e.ToString(), null, ResultEnum.InternalError);
            }
            httpResponseMessage.Content = new JsonContent(response);
            return httpResponseMessage;
        }
        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetHistoryOrder(OrderRequest<string> request)
        {
            BaseResponse<List<OrderHistoryAPIViewModel>> response = new BaseResponse<List<OrderHistoryAPIViewModel>>();
            var domain = new OrderDomain();
            var customerDomain = new CustomerDomain();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            var customerId = customerDomain.getCustomerIdFromToken(RequestContext);
            var claimPrincipal = (ClaimsPrincipal)RequestContext.Principal;
            var employeeId = claimPrincipal.Claims.Where(c => c.Type == "EmployeeId").Select(c => c.Value).SingleOrDefault();
            var userName = claimPrincipal.Claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            Int32 employeeID = 0;
            Int32.TryParse(employeeId, out employeeID);
            if (request.CreateAtMax.HasValue)
            {
                request.CreateAtMax = request.CreateAtMax.Value.GetEndOfDate();
            }
            if (request.CreateAtMin.HasValue)
            {
                request.CreateAtMin = request.CreateAtMin.Value.GetEndOfDate();
            }
            
            if (customerId != null)
            {
                request.CustomerId = customerId.Value;
            }
            if(employeeID != 0)
            {
                request.UserName = userName;
            }
            try
            {
                response = domain.GetOrderHistoryByRequest(request);

            }
            catch (ApiException e)
            {
                httpResponseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<List<OrderHistoryAPIViewModel>>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception e)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<List<OrderHistoryAPIViewModel>>.Get(false, e.ToString(), null, ResultEnum.InternalError);
            }
            httpResponseMessage.Content = new JsonContent(response);
            return httpResponseMessage;
        }
        //[Route("")]
        //[HttpPut]
        private HttpResponseMessage UpdateOrder(OrderAPIViewModel order)
        {
            var customerDomain = new CustomerDomain();
            BaseResponse<OrderHistoryAPIViewModel> response = new BaseResponse<OrderHistoryAPIViewModel>();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            var customerId = customerDomain.getCustomerIdFromToken(RequestContext);
            if(customerId != null)
            {
                order.CustomerID = customerId;
            }
            var domain = new OrderDomain();
            try
            {
                response = domain.UpdateOrder(order,customerId.Value);
            }
            catch (ApiException e)
            {
                httpResponseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<OrderHistoryAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception e)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<OrderHistoryAPIViewModel>.Get(false, e.ToString(), null, ResultEnum.InternalError);
            }
            httpResponseMessage.Content = new JsonContent(response);
            return httpResponseMessage;
        }
        [Route("")]
        [HttpPatch]
        public HttpResponseMessage UpdateTableCode(OrderAPIViewModel order)
        {
            var customerDomain = new CustomerDomain();
            BaseResponse<OrderHistoryAPIViewModel> response = new BaseResponse<OrderHistoryAPIViewModel>();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            var customerId = customerDomain.getCustomerIdFromToken(RequestContext);
            if (customerId != null)
            {
                order.CustomerID = customerId;
            }
            var domain = new OrderDomain();
            try
            {
                response = domain.UpdateTableCode(order, customerId.Value);
            }
            catch (ApiException e)
            {
                httpResponseMessage.StatusCode = e.StatusCode;
                response = BaseResponse<OrderHistoryAPIViewModel>.Get(e.Success, e.ErrorMessage, null, e.ErrorStatus);
            }
            catch (Exception e)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                response = BaseResponse<OrderHistoryAPIViewModel>.Get(false, e.ToString(), null, ResultEnum.InternalError);
            }
            httpResponseMessage.Content = new JsonContent(response);
            return httpResponseMessage;
        }
    }
}
